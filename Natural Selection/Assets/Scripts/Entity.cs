using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
	private State _state = null;
	[SerializeField] private string _stateName;

	public float velocity;
	public float hungriness = 0;
	public float thirstiness = 0;
	public float FOV = 90.0f;

	public float minVelocity = 0.2f;

	public GameObject waterToDrink = null;
	public GameObject foodToEat = null;
	public NavMeshAgent agent;

	public GameObject beacon;

	LineRenderer lineRenderer;

	void Start()
	{
		agent = this.gameObject.GetComponent<NavMeshAgent>();
		_state = new PrimaryState();
		_state.SetEntity(this);

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.positionCount = 6;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			lineRenderer.enabled = !lineRenderer.enabled;

		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.forward * 10 + transform.position);

		lineRenderer.SetPosition(2, transform.position);
		lineRenderer.SetPosition(3, Quaternion.AngleAxis(FOV/2, transform.up) * transform.forward * 10 + transform.position);

		lineRenderer.SetPosition(4, transform.position);
		lineRenderer.SetPosition(5, Quaternion.AngleAxis(-FOV/2, transform.up) * transform.forward * 10 + transform.position);

		_stateName = _state.GetStateName();
		velocity = agent.velocity.magnitude;
		
		if (thirstiness >= 100.0f || hungriness >= 100.0f)
			Destroy(this.gameObject);

		// Fix this shit
		if (foodToEat == null)
			foodToEat = null;

		if (waterToDrink == null)
			waterToDrink = null;
	}

	private void FixedUpdate()
	{
		HandleState();
	}

	// Rarely used since states of an entity can be changed within those states
	public void ChangeState(State state)
	{
		this._state = state;
		this._state.SetEntity(this);
	}

	// Handles the state this entity is on
	public void HandleState()
	{
		_state.HandleState();
	}

	//____________________________________________________________Entity functions_______________________________________//

	public void SetDestination(Vector3 target)
	{
		this.agent.SetDestination(target);
	}

	public Transform GetTransform()
	{
		return this.gameObject.transform;
	}

	public Vector3 GetPosition()
	{
		return this.gameObject.transform.position;
	}

	public bool HasPath()
	{
		return agent.hasPath;
	}

	public float Velocity()
	{
		return this.agent.velocity.magnitude;
	}

	public void TurnOnSense()
	{
		this.gameObject.GetComponent<SphereCollider>().enabled = true;
	}

	public void TurnOffSense()
	{
		this.gameObject.GetComponent<SphereCollider>().enabled = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if (waterToDrink == null && other.tag == "Water")
			waterToDrink = other.gameObject;

		if (foodToEat == null && other.tag == "Plant")
			foodToEat = other.gameObject;
	}

	// Deprecated Behavior Tree pattern, will keep here just in case
	private void ConstructBehavioralTree()
	{
		IsHungryOrThirstyNode isHungryOrThirstyNode = new IsHungryOrThirstyNode(this);
		IsMovingNode isMovingNode = new IsMovingNode(this, 0.1f);
		SetDestinationNode setDestinationNode = new SetDestinationNode(this);

		ResourcesNearNode resourcesNearNode = new ResourcesNearNode(this);
		ConsumeResourceNode consumeResourceNode = new ConsumeResourceNode(this);

		Sequence Roam = new Sequence(new List<Node> { isHungryOrThirstyNode, isMovingNode, setDestinationNode });
		Sequence HungryOrThirsty = new Sequence(new List<Node> { resourcesNearNode, consumeResourceNode });


		// Should this be used, declare the variable below
		//topNode = new Selector(new List<Node> { Roam, HungryOrThirsty });

		//private Node topNode; // declare it as a variable of this class
		//topNode.Evaluate(); // add this line on the Update function
	}
}