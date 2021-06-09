using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour, IPooledObject
{
	private const float MIN_SPEED = 0.2f;

	public bool isRunning = false;
	public bool isDone = false;

	public float opponentUtility = 0.0f;
	public float peerUtility = 0.0f;
	public float socializingChance = 0.0f;

	public float Fitness;
	public float hungriness = 0.0f;
	public float thirstiness = 0.0f;
	private float hungerThreshold = 50.0f;
	private float thirstThreshold = 50.0f;

	// Event handler 
	public event EventHandler<Entity> Death;

	private State _state = null;
	[SerializeField] private string _stateName;
	[SerializeField] public Genome genome;
	private Sex sex;
	private Order order;
	private Speed speed;
	public Memory Memory { get; private set; }
	public Sight  Sight	 { get; private set; }
	public Smell  Smell	 { get; private set; }

	[HideInInspector] public int SenseRefreshInterval = 5;
	[HideInInspector] public float sightRadius;

	private NavMeshAgent agent;
	public Transform Transform { get; private set; }

	void OnEnable()
	{
		Transform = gameObject.GetComponent<Transform>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		Memory = gameObject.GetComponent<Memory>();
		Sight = gameObject.GetComponentInChildren<Sight>();
		Smell = gameObject.GetComponentInChildren<Smell>();
		sightRadius = Sight.gameObject.GetComponent<SphereCollider>().radius;

		Fitness = 0.0f;
		// Do not touch
		_state = new PrimaryState();
		_stateName = _state.GetStateName();
		Cycle.CycleStart += OnCycleStart;
		Cycle.CycleEnd += OnCycleEnd;
	}

	void Update()
	{
		if (isRunning)
		{
			HandleState();
			hungriness += Time.deltaTime / 1.5f;
			Fitness += 0.0001f;
		}

		if (!isRunning && !isDone)
		{
			if (!agent.hasPath)
			{
				isDone = true;
				EntitySpawner.ActiveEntities++;
			}
		}

		if (!EntitySpawner.EntitiesNotReachedDestinationsYet && EntitySpawner.HasCycleEnded)
		{
			agent.enabled = false;
		}

		if (hungriness >= 100.0f)
			DieFromHungerAndThirst();
	}

	public void ResetEntity()
	{
		_state = new PrimaryState();
		hungriness = 0.0f;
	}

	public void ExpressGenome()
	{
		genome.ExpressGenome(this);
	}

	public void OnObjectSpawn()
	{
		Fitness = 0.0f;
		hungriness = 0.0f;
		gameObject.SetActive(true);
	}

	// Change entity's state from outside this class
	public void ChangeState(State state)
	{
		_state = state;
		_stateName = _state.GetStateName();
	}

	// Handles the state the entity is on
	public void HandleState()
	{
		_state.HandleState(this);
	}

	#region CycleEventMethods
	public void OnCycleStart(object sender, EventArgs eventArgs)
	{
		hungriness = 0.0f;
		Walk();
		agent.enabled = true;
		isRunning = true;
	}

	public void OnCycleEnd(object sender, EventArgs eventArgs)
	{
		_state = new PrimaryState();
		isRunning = false;
		isDone = false;
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

	public void Run()
	{
		agent.speed = speed.running;
	}

	public float Walk()
	{
		return agent.speed = speed.walking;
	}
	#endregion

	#region SetTraitsMethods
	public void SetColor(Color color)
	{
		gameObject.GetComponent<Renderer>().material.color = color;
	}

	public void SetHeight(float height)
	{
		Transform.localScale = new Vector3(1, height, 1);
	}

	public void SetSpeed(float pWalking, float pRunning, float pAngular, float pAcceleration)
	{
		speed = new Speed(pWalking, pRunning, pAngular, pAcceleration);
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
		peerUtility = pPeerUtility;
		opponentUtility = pOpponentUtility;
		socializingChance = pSocializingChance;
	}

	private void EditPeerUtility(float value)
	{
		peerUtility += value;
		peerUtility = Mathf.Clamp(peerUtility, -1.0f, 1.0f);
		genome.Behavior.IdealAllele.ChangePeerUtility(value);
	}

	private void EditOpponentUtility(float value)
	{
		opponentUtility += value;
		opponentUtility = Mathf.Clamp(opponentUtility, -1.0f, 1.0f);
		genome.Behavior.IdealAllele.ChangeOpponentUtility(value);
	}

	private void EditSocializingChance(float value)
	{
		socializingChance += value;
		socializingChance = Mathf.Clamp(socializingChance, 0.0f, 100.0f);
		genome.Behavior.IdealAllele.ChangeSocializingChance(value);
	}
	#endregion

	#region EventMethods
	protected virtual void OnDeath(Entity entity)
	{
		Death?.Invoke(this, entity);
	}

	public void Die()
	{
		Cycle.CycleStart -= OnCycleStart;
		Cycle.CycleEnd -= OnCycleEnd;
		Counter.DecrementHerbivoreAlive();
		OnDeath(this);
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
	}

	public void DieFromHungerAndThirst()
	{
		if (order == Order.HERBIVORE)
			Counter.DecrementHerbivoreAlive();
		else
			Counter.DecrementCarnivoreAlive();

		gameObject.SetActive(false);
	}

	// For others (Subscriber method to run)
	public void OnOtherDeath(object source, Entity entity)
	{
		if (order == Order.HERBIVORE)
		{
			EditOpponentUtility(-0.2f);
			EditPeerUtility(0.1f);
			EditSocializingChance(Random.Range(-2.0f, 3.0f));
			Fitness += 0.01f;
		}

		entity.Death -= OnOtherDeath;
	}
	#endregion

	#region IsMethods
	public bool IsHungry()
	{
		return hungriness > hungerThreshold;
	}

	public bool IsThirsty()
	{
		return thirstiness > thirstThreshold;
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
	#endregion

	#region TransformMethods
	/// <summary>
	/// Get angle between <strong>this.forward</strong> and <strong>directionToObject</strong>.
	/// </summary>
	/// <returns> Angle in range from <strong>0</strong> to <strong>360</strong> degrees. </returns>
	public float GetAngleTo(Vector3 directionToObject)
	{
		float signedAngle = Vector3.SignedAngle(Transform.forward, directionToObject, Transform.up);
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

		Vector3 randomPoint = Quaternion.AngleAxis(angle, Vector3.up) * Transform.forward * sightRadius + Transform.position;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sightRadius, 1))
		{
			finalPosition = hit.position;
		}

		if (finalPosition == Vector3.zero)
			finalPosition = Random.insideUnitSphere * sightRadius;

		return finalPosition;
	}
	#endregion
}

public enum Sex { MALE, FEMALE }
public enum Order { HERBIVORE, CARNIVORE, OMNIVORE }

public struct Speed
{
	public readonly float walking;
	public readonly float running;
	public readonly float angular;
	public readonly float acceleration;

	public Speed(float pWalking, float pRunning, float pAngular, float pAcceleration)
	{
		walking = pWalking;
		running = pRunning;
		angular = pAngular;
		acceleration = pAcceleration;
	}
}

// Unused Code
//LineRenderer lineRenderer;

//lineRenderer = gameObject.GetComponent<LineRenderer>();
//lineRenderer.positionCount = 6;

//private void DrawFieldOFView()
//{
//	lineRenderer.SetPosition(0, transform.position);
//	lineRenderer.SetPosition(1, transform.forward * 10 + transform.position);

//	lineRenderer.SetPosition(2, transform.position);
//	lineRenderer.SetPosition(3, Quaternion.AngleAxis(FOV / 2, transform.up) * transform.forward * 10 + transform.position);

//	lineRenderer.SetPosition(4, transform.position);
//	lineRenderer.SetPosition(5, Quaternion.AngleAxis(-FOV / 2, transform.up) * transform.forward * 10 + transform.position);
//}