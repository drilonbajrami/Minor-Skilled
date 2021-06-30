using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour, IPooledObject
{
	#region Data
	// State
	private State _state = null;
	[SerializeField] private string stateName;

	// Agent and Transform
	private NavMeshAgent agent;
	public Transform Transform { get; private set; }

	// Genome
	[SerializeField] private Genome genome;
	public Genome Genome { get { return genome; } set { genome = value; } }

	// Vitals
	public Vitals _vitals = new Vitals();
	public Vitals Vitals { get { return _vitals; } set { _vitals = value; } }

	[SerializeField] public Speed Speed;
	[SerializeField] public Behavior Behavior;

	[SerializeField] private Order order;
	[SerializeField] private Sex sex;

	// Senses
	public Memory Memory { get; private set; }
	public Sight Sight { get; private set; }
	public Smell Smell { get; private set; }
	[HideInInspector] public int SenseRefreshInterval = 5;
	[HideInInspector] public float SightRadius { get; set; }

	private const float MIN_SPEED = 0.2f;
	#endregion

	public float Fitness;

	public bool debugOn = false;

	void Awake()
	{
		Transform = gameObject.GetComponent<Transform>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		Memory = gameObject.GetComponent<Memory>();
		Sight = gameObject.GetComponentInChildren<Sight>();
		Smell = gameObject.GetComponentInChildren<Smell>();
	}

	void OnEnable()
	{
		Vitals.Reset();
		ChangeState(new PrimaryState());
		Cycle.CycleStart += OnCycleStart;
		Cycle.CycleEnd += OnCycleEnd;
	}

	void Update()
	{
		if (agent.isActiveAndEnabled)
		{
			HandleState();
			Vitals.ConsumeEnergy(0.8f);
			Fitness += 0.01f;
		}

		Vitals.ConsumeEnergy(agent.velocity.magnitude * GetHeight() * 0.001f);
		if (Vitals.IsStarving())
		{
			Die();
		}
	}

	#region State
	// Change entity's state from outside this class
	public void ChangeState(State state)
	{
		_state = state;
		stateName = _state.GetStateName();
	}

	// Handles the state the entity is on
	public void HandleState()
	{
		_state.HandleState(this);
	}
	#endregion

	#region CycleEventMethods
	public void OnCycleStart(object sender, EventArgs eventArgs)
	{
		Walk();
		agent.enabled = true;
		Vitals.Reset();
	}

	public void OnCycleEnd(object sender, EventArgs eventArgs)
	{
		_state = new PrimaryState();
		agent.enabled = false;
	}

	public void OnObjectSpawn()
	{
		gameObject.SetActive(true);
		Fitness = 0.0f;
	}
	#endregion

	#region AgentMethods
	public void SetDestination(Vector3 target)
	{
		agent.SetDestination(target);
	}

	public bool HasPath()
	{
		return agent.hasPath;
	}

	public void ClearPath()
	{
		agent.ResetPath();
	}

	public void Stop()
	{
		agent.isStopped = true;
		agent.ResetPath();
	}

	public bool IsStopped()
	{
		return agent.isStopped || agent.velocity.magnitude < MIN_SPEED;
	}

	public float CurrentSpeed()
	{
		return agent.speed;
	}

	public float GetHeight()
	{
		return gameObject.transform.localScale.y;
	}

	public void Run()
	{
		agent.speed = Speed.Running;
	}

	public void Walk()
	{
		agent.speed = Speed.Walking;
	}
	#endregion

	#region SetTraitsMethods
	public void ExpressGenome()
	{
		genome.ExpressGenome(this);
	}

	public void SetColor(Color color)
	{
		gameObject.GetComponent<Renderer>().material.color = color;
	}

	public void SetHeight(float height)
	{
		Transform.localScale = new Vector3(1, height, 1);
		SightRadius = 5 + height * 2.5f;
		Sight.gameObject.GetComponent<SphereCollider>().radius = SightRadius;
		Speed.Running -= height * 5;
	}

	public void SetSpeed(float pWalking, float pRunning, float pAngular, float pAcceleration)
	{
		Speed = new Speed(pWalking, pRunning, pAngular, pAcceleration);
	}

	public void SetSex(Sex pSex)
	{
		sex = pSex;
	}

	public void SetOrder(Order pOrder)
	{
		order = pOrder;
	}

	public void SetBehavior(float pPeerUtility, float pOpponentUtility, float pSocializingChance)
	{
		Behavior = new Behavior(pPeerUtility, pOpponentUtility, pSocializingChance);
	}
	#endregion

	#region IsMethods
	public bool NeedsToEat()
	{
		return Vitals.NeedsToEat();
	}

	public bool IsHerbivore()
	{
		return order == Order.HERBIVORE;
	}

	public bool IsCarnivore()
	{
		return order == Order.CARNIVORE;
	}

	public bool IsMale()
	{
		return sex == Sex.MALE;
	}

	public bool IsFemale()
	{
		return sex == Sex.FEMALE;
	}

	public bool IsRunning()
	{
		return agent.speed == Speed.Running;
	}

	public bool IsWalking()
	{
		return agent.speed == Speed.Walking;
	}
	#endregion

	#region TransformMethods
	/// <summary>
	/// Get angle between <strong>this.forward</strong> and <strong>directionToObject</strong>.
	/// </summary>
	/// <returns> Angle in range from <strong>0</strong> to <strong>360</strong> degrees. </returns>
	public float GetAngleTo(Vector3 directionToObject)
	{
		float signedAngle = Vector3.SignedAngle(gameObject.transform.forward, directionToObject, Transform.up);
		return (signedAngle >= 0.0f) ? signedAngle : 360.0f + signedAngle;
	}

	/// <summary>
	/// Checks if distance to the object <strong>B</strong> is smaller than the <strong>difference</strong>.
	/// </summary>
	public bool CheckIfClose(GameObject b, float difference)
	{
		return Mathf.Abs(Transform.localPosition.x - b.transform.localPosition.x) < difference && Mathf.Abs(Transform.localPosition.z - b.transform.localPosition.z) < difference;
	}

	/// <summary>
	/// Returns the closest object out of <strong>A</strong> and <strong>B</strong>.
	/// </summary>
	public GameObject ClosestTo(GameObject A, GameObject B)
	{
		float lengthA = (Transform.localPosition - A.transform.localPosition).sqrMagnitude;
		float lengthB = (Transform.localPosition - B.transform.localPosition).sqrMagnitude;
		return lengthA < lengthB ? A : B;
	}

	/// <summary> Returns a random direction depending on the given condition of <strong>fullRandom</strong>. </summary>
	/// <returns> 
	/// <para> If 'fullRandom == false': Random direction within bounds of the ideal direction ± half the angle of a sigth section </para>
	/// <para> If 'fullRandom == true' : Random direction within bounds of forward direction ± 45 degrees </para>
	/// </returns>
	public Vector3 GetIdealRandomDestination(bool fullRandom = false)
	{
		float angle;
		if (!fullRandom)
			angle = Sight.SightArea.GetIdealDirectionAngle();
		else
		{
			if (Random.Range(0, 2) == 0)
				angle = Random.Range(0, 45);
			else
				angle = Random.Range(315, 360);
		}

		Vector3 randomPoint = Quaternion.AngleAxis(angle, Vector3.up) * Transform.forward * SightRadius + Transform.position;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, SightRadius, 1))
		{
			finalPosition = hit.position;
		}

		if (finalPosition == Vector3.zero)
			finalPosition = Random.insideUnitSphere * SightRadius;

		return finalPosition;
	}
	#endregion

	public void Die()
	{
		Cycle.CycleStart -= OnCycleStart;
		Cycle.CycleEnd -= OnCycleEnd;

		if (IsHerbivore())
			Counter.Instance.RemoveHerbivoreAlive();
		else
			Counter.Instance.RemoveCarnivoreAlive();

		EntitySpawner.entityPooler.PoolObject("Entity", gameObject);
		gameObject.SetActive(false);
	}
}