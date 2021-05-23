using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
	public float predatorU = -0.5f;
	public float waterU = 0.5f;
	public float foodU = 0.5f;
	public float preyU = 0.1f;
	//public Genome genome;

	// Event handler 
	public event EventHandler<Entity> Death;

	// Publish method
	protected virtual void OnDeath(Entity entity)
	{
		if (Death != null)
		{
			Death(this, entity);
		}
	}

	public void Die()
	{
		HerbivoreCounter.counter--;
		OnDeath(this);
		Destroy(this.gameObject);
	}

	// For others (Subscriber method to run)
	public void OnOtherDeath(object source, Entity entity)
	{
		//Debug.Log($"Observer {this.gameObject.name}: {entity.gameObject.name} Died");
		if (order == Order.HERBIVORE)
		{
			predatorU -= 0.25f;
		}

		entity.Death -= OnOtherDeath;
	}

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

	[HideInInspector] public Memory Memory	{ get; private set; }
	[HideInInspector] public Sight	Sight	{ get; private set; }
	[HideInInspector] public Smell	Smell	{ get; private set; }
	[Range(5, 20)] public int SenseRefreshInterval = 5;

	private NavMeshAgent agent;
	public Transform Transform { get; private set; }

	LineRenderer lineRenderer;


	void Awake()
	{
		// Cache the transform
		Transform = gameObject.GetComponent<Transform>();

		//genome = GetComponent<Genome>();
		agent = gameObject.GetComponent<NavMeshAgent>();
		Memory = gameObject.GetComponent<Memory>();
		Sight = gameObject.GetComponentInChildren<Sight>();
		Smell = gameObject.GetComponentInChildren<Smell>();


		sightRadius = Sight.gameObject.GetComponent<SphereCollider>().radius;
		//smellRadius = Smell.gameObject.GetComponent<SphereCollider>().radius;
		maxSpeed = agent.speed;

		// Do not touch
		_state = new PrimaryState();
		_stateName = _state.GetStateName();

		// Has to be sent somehwere else
		hungriness = Random.Range(40.0f, 60.0f);
		thirstiness = Random.Range(40.0f, 60.0f);
		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			gestationDuration = 0.0f;
			maleReproductionDuration = 0.0f;
		}
		else
		{
			gestationDuration = Random.Range(30.0f, 50.0f);
			maleReproductionDuration = Random.Range(30.0f, 50.0f);
		}
		

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.positionCount = 6;
	}

	private void Start()
	{
		if (order == Order.CARNIVORE)
			gameObject.GetComponent<Renderer>().material.color = Colors.CARNIVORE;
		else
			gameObject.GetComponent<Renderer>().material.color = Colors.HERBIVORE;
	}

	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Space))
		//	lineRenderer.enabled = !lineRenderer.enabled;
		//DrawFieldOFView();

		if (thirstiness >= 100.0f || hungriness >= 100.0f)
			//Destroy(this.gameObject);

		gestationDuration -= Time.deltaTime;
		maleReproductionDuration -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		HandleState();
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