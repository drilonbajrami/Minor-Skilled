using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
	public GameObject partner = null;
	public bool isMating = false;
	public bool offspring = false;
	public bool isOnReproducingState = false;

	public float gestationDuration;
	public float maleReproductionDuration;

	public GameObject predator = null;
	public bool fleeing = false;

	private State _state = null;
	[SerializeField] private string _stateName;

	public float hungriness = 0.0f;
	public float thirstiness = 0.0f;
	private float hungerThreshold = 50.0f;
	private float thirstThreshold = 50.0f;

	private float maxSpeed;

	[SerializeField] public Gender gender;
	[SerializeField] public Order order;

	// Field Of View
	[HideInInspector] public float FOV = 90.0f;
	[HideInInspector] public float sightRadius;
	[HideInInspector] public float smellRadius;

	private float minVelocity = 0.2f;

	[HideInInspector] public Memory Memory { get; set; }
	[HideInInspector] public Sight Sight { get; set; }
	[HideInInspector] public Smell Smell { get; set; }

	private NavMeshAgent agent;

	LineRenderer lineRenderer;

	void Start()
	{
		int i = Random.Range(0, 2);
		if (i == 0)
		{
			gender = Gender.MALE;
			//order = Order.HERBIVORE;
			//gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.HERBIVORE;
		}
		else
		{
			gender = Gender.FEMALE;
			//order = Order.CARNIVORE;
			//gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.CARNIVORE;
		}

		//int j = Random.Range(0, 2);
		//if (j == 0)
		//{
		//	order = Order.HERBIVORE;
		//	gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.HERBIVORE;
		//}
		//else
		//{
		//	order = Order.CARNIVORE;
		//	gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.CARNIVORE;
		//}

		if (order == Order.CARNIVORE)
			gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.CARNIVORE;
		else
			gameObject.GetComponent<Renderer>().material.color = EntityGenderColor.HERBIVORE;

		agent = gameObject.GetComponent<NavMeshAgent>();
		Memory = gameObject.GetComponent<Memory>();
		Sight = gameObject.GetComponent<Sight>();
		Smell = gameObject.GetComponentInChildren<Smell>();
		sightRadius = gameObject.GetComponent<SphereCollider>().radius;
		smellRadius = Smell.GetComponent<SphereCollider>().radius;
		maxSpeed = agent.speed;
		_state = new PrimaryState(this);
		_stateName = _state.GetStateName();
		//_state = new ReproduceState(this);

		hungriness = Random.Range(20.0f, 60.0f);
		thirstiness = Random.Range(20.0f, 60.0f);
		gestationDuration = Random.Range(10.0f, 50.0f);
		maleReproductionDuration = Random.Range(10.0f, 50.0f);

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.positionCount = 6;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			lineRenderer.enabled = !lineRenderer.enabled;
		DrawFieldOFView();

		//if (Input.GetKeyDown(KeyCode.P))
		//	Smell.gameObject.SetActive(true);

		if (thirstiness >= 100.0f || hungriness >= 100.0f)
			Destroy(this.gameObject);

		gestationDuration -= Time.deltaTime;
		maleReproductionDuration -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		HandleState();
	}

	// Rarely used since states of an entity can be changed within those states
	public void ChangeState(State state)
	{
		_state = state;
		_state.SetEntity(this);
		_stateName = _state.GetStateName();
	}

	// Handles the state the entity is on
	public void HandleState()
	{
		_state.HandleState();
	}

	//====================================================================================================
	//											Entity Agent Methods
	//====================================================================================================
	public Transform GetTransform()
	{
		return this.gameObject.transform;
	}

	public Vector3 GetPosition()
	{
		return gameObject.transform.position;
	}

	//====================================================================================================
	//											Entity Transform Methods
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
		agent.speed = maxSpeed * 1.5f;
	}

	public float DecreaseMaxSpeed()
	{
		return agent.speed = maxSpeed;
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
	public bool IsHungry()
	{
		return hungriness > hungerThreshold;
	}

	public bool IsThirsty()
	{
		return thirstiness > thirstThreshold;
	}

	public void SetSkinColor(Color color)
	{
		gameObject.GetComponent<Renderer>().material.color = color;
	}

	public void SetMatingPartner(GameObject pObject)
	{
		partner = pObject;
		isMating = true;
	}

	public void DiscardMatingPartner()
	{
		partner = null;
		isMating = false;
	}

	public void SetPredator(GameObject pObject)
	{
		predator = pObject;
	}

	public void DiscardPredator()
	{
		predator = null;
	}

	//====================================================================================================
	//											TRIGGERS
	//====================================================================================================
	private void OnTriggerEnter(Collider other)
	{
		if (Memory != null && Sight != null)
		{
			if ((other.gameObject.tag == "Water" || other.gameObject.tag == "Plant"))
			{
				Memory.RegisterResourceToMemory(other.gameObject);
				Sight.See(other.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (Sight != null)
		{
			if (other.gameObject != null)
				Sight.Unsee(other.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (Sight != null)
		{
			Resource a = other.gameObject.GetComponent<Resource>();
			if (a != null)
				if (a.IsConsumed())
					Sight.Unsee(other.gameObject);
		}
	}
	//====================================================================================================


	private void DrawFieldOFView()
	{
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.forward * 10 + transform.position);

		lineRenderer.SetPosition(2, transform.position);
		lineRenderer.SetPosition(3, Quaternion.AngleAxis(FOV / 2, transform.up) * transform.forward * 10 + transform.position);

		lineRenderer.SetPosition(4, transform.position);
		lineRenderer.SetPosition(5, Quaternion.AngleAxis(-FOV / 2, transform.up) * transform.forward * 10 + transform.position);
	}
}

public enum Gender { MALE, FEMALE }
public enum Order { HERBIVORE, CARNIVORE }