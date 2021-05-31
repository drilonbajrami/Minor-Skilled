using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour, IPooledObject
{
	public bool isRunning = false;
	public bool isDone = false;

	public bool debugOn = false;
	public float predatorU = 0.0f;
	public float preyU = 0.0f;
	public float social = 0.0f;

	public float Fitness;

	// Event handler 
	public event EventHandler<Entity> Death;

	private State _state = null;
	[SerializeField] private string _stateName;

	/*[HideInInspector]*/ public float hungriness = 0.0f;
	[HideInInspector] public float thirstiness = 0.0f;
	private float hungerThreshold = 50.0f;
	private float thirstThreshold = 50.0f;

	

	[HideInInspector] public float sightRadius;
	[HideInInspector] public float smellRadius;

	private readonly float minVelocity = 0.2f;
	private float defaultSpeed;

	public Genome genome;
	[SerializeField] public Gender gender;
	[SerializeField] public Order order;
	[HideInInspector] public Memory Memory	{ get; private set; }
	[HideInInspector] public Sight	Sight	{ get; private set; }
	[HideInInspector] public Smell	Smell	{ get; private set; }
	[Range(5, 20)] public int SenseRefreshInterval = 5;

	private NavMeshAgent agent;
	public Transform Transform { get; private set; }

	void OnEnable()
	{
		// Cache the transform
		Transform = gameObject.GetComponent<Transform>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		Memory = gameObject.GetComponent<Memory>();
		Sight = gameObject.GetComponentInChildren<Sight>();
		Smell = gameObject.GetComponentInChildren<Smell>();

		sightRadius = Sight.gameObject.GetComponent<SphereCollider>().radius;
		smellRadius = Smell.gameObject.GetComponent<SphereCollider>().radius;
		Fitness = 0.0f;

		defaultSpeed = agent.speed;

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

		if (genome != null)
		{
			predatorU = genome.Behavior.IdealAllele.OpponentUtility;
			preyU = genome.Behavior.IdealAllele.PeerUtility;
			social = genome.Behavior.IdealAllele.SocializingChance;
		}
		//// Testing
		//if (pU != predatorU && order == Order.HERBIVORE)
		//{
		//	pU = predatorU;
		//	IncreaseMaxSpeed();
		//	gameObject.GetComponent<Renderer>().material.color = Colors.FITCOLOR.Evaluate(Mathf.InverseLerp(0, -1, pU));
		//}

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

	public void OnCycleStart(object sender, EventArgs eventArgs)
	{
		hungriness = 0.0f;
		DecreaseMaxSpeed();
		agent.enabled = true;
		isRunning = true;
	}

	public void OnCycleEnd(object sender, EventArgs eventArgs)
	{
		_state = new PrimaryState();
		isRunning = false;
		isDone = false;
	}
	//====================================================================================================
	//											Entity Agent Methods
	//====================================================================================================

	public void SetDestination(Vector3 target)
	{
		agent.isStopped = false;
		this.agent.SetDestination(target);
	}

	public Vector3 GetDestination()
	{
		return agent.destination;
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

	public void IncreaseMaxSpeed()
	{
		agent.speed = defaultSpeed * 1.5f;
	}

	public void IncreaseMaxSpeed(float scale)
	{
		if (scale < 0.2) return;
		agent.speed = defaultSpeed * scale;
	}

	public float DecreaseMaxSpeed()
	{
		return agent.speed = defaultSpeed;
	}

	public bool IsStopped()
	{
		return agent.isStopped;
	}

	public bool IsStuck()
	{
		return agent.velocity.magnitude < minVelocity;
	}

	public float Velocity()
	{
		return agent.velocity.magnitude;
	}

	//====================================================================================================
	//											Entity Other Methods
	//====================================================================================================
	/// <summary>
	/// Death publisher method
	/// </summary>
	/// <param name="entity"></param>
	protected virtual void OnDeath(Entity entity)
	{
		Death?.Invoke(this, entity);
	}

	public void Die()
	{
		Cycle.CycleStart -= OnCycleStart;
		Cycle.CycleEnd -= OnCycleEnd;
		EntitySpawner.hCount--;
		OnDeath(this);
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
	}

	public void DieFromHungerAndThirst()
	{
		if (order == Order.HERBIVORE)
			EntitySpawner.hCount--;
		else
			EntitySpawner.cCount--;

		gameObject.SetActive(false);
	}

	// For others (Subscriber method to run)
	public void OnOtherDeath(object source, Entity entity)
	{
		//Debug.Log($"Observer {this.gameObject.name}: {entity.gameObject.name} Died");
		if (order == Order.HERBIVORE)
		{
			genome.Behavior.IdealAllele.ChangeOpponentUtility(-0.2f);
			genome.Behavior.IdealAllele.ChangePeerUtility(0.1f);
			genome.Behavior.IdealAllele.ChangeSocializingChance(Random.Range(-2.0f, 3.0f));
			Fitness += 0.01f;
		}

		entity.Death -= OnOtherDeath;
	}

	public bool IsHungry()
	{
		return hungriness > hungerThreshold;
	}

	public bool IsThirsty()
	{
		return thirstiness > thirstThreshold;
	}

	/// <summary>
	/// Returns the angle between entity's forward direction and the direction towards the object
	/// Angle is in range from 0 to 360 degrees (exclusive)
	/// </summary>
	/// <param name="directionToObject"> The direction from entity towards the object.</param>
	/// <returns></returns>
	public float GetAngleTo(Vector3 directionToObject)
	{
		float signedAngle = Vector3.SignedAngle(Transform.forward, directionToObject, Transform.up);
		return (signedAngle >= 0.0f) ? signedAngle : 360.0f + signedAngle;
	}

	/// <summary>
	/// Checks if distance between the entity and the object is smaller than the difference.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="difference"></param>
	/// <returns></returns>
	public bool CheckIfClose(GameObject b, float difference)
	{
		return Mathf.Abs(Transform.localPosition.x - b.transform.localPosition.x) < difference && Mathf.Abs(Transform.localPosition.z - b.transform.localPosition.z) < difference;
	}

	/// <summary>
	/// Returns the closest object to the entity out of two given ones.
	/// </summary>
	/// <param name="toA"></param>
	/// <param name="toB"></param>
	/// <returns></returns>
	public GameObject ClosestTo(GameObject toA, GameObject toB)
	{
		float lengthA = (Transform.localPosition - toA.transform.localPosition).sqrMagnitude;
		float lengthB = (Transform.localPosition - toB.transform.localPosition).sqrMagnitude;
		return lengthA < lengthB ? toA : toB;
	}

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
}

public enum Gender { MALE, FEMALE }
public enum Order { HERBIVORE, CARNIVORE }

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