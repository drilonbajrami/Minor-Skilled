using UnityEngine;

[System.Serializable]
public class SpeedAllele : Allele<SpeedAllele>
{
	private const float MIN_WALKING		  = 2.0f; // 5
	private const float MAX_WALKING		  = 3.0f; // 8
	private const float MIN_RUNNING		  = 5.0f; // 20
	private const float MAX_RUNNING		  = 12.0f; // 35
	private const float MIN_ANGULAR		  = 90.0f;
	private const float MAX_ANGULAR		  = 180.0f;
	private const float MIN_ACCELERATION  = 5.0f;
	private const float MAX_ACCELERATION  = 15.0f;

	private const float WALKING_DIFFERENCE		 = MAX_WALKING - MIN_WALKING;
	private const float RUNNING_DIFFERENCE		 = MAX_RUNNING - MIN_RUNNING;
	private const float ANGULAR_DIFFERENCE		 = MAX_ANGULAR - MIN_ANGULAR;
	private const float ACCELERATION_DIFFERENCE  = MAX_ACCELERATION - MIN_ACCELERATION;

	[Range(MIN_WALKING, MAX_WALKING)]			 [SerializeField] private float _walking;
	[Range(MIN_RUNNING, MAX_RUNNING)]			 [SerializeField] private float _running;
	[Range(MIN_ANGULAR, MAX_ANGULAR)]			 [SerializeField] private float _angular;
	[Range(MIN_ACCELERATION,  MAX_ACCELERATION)] [SerializeField] private float _acceleration;
	public float Walking => _walking;
	public float Running => _running;
	public float Angular => _angular;
	public float Acceleration => _acceleration;

	public SpeedAllele(Dominance pDominance, float pWalking, float pRunning, float pAngular, float pAcceleration) : base(pDominance)
	{
		_walking = pWalking;
		_running = pRunning;
		_angular = pAngular;
		_acceleration = pAcceleration;
	}

	public override SpeedAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
		SpeedAllele copy = new SpeedAllele(Dominance, _walking, _running, _angular, _acceleration);
		copy.Mutate(mutationFactor, mutationChance);
		return copy;
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (mutationFactor == 0) return;

		// Walking Speed
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * WALKING_DIFFERENCE;
			_walking += Random.Range(-mutation, mutation);
			_walking = Mathf.Clamp(_walking, MIN_WALKING, MAX_WALKING);
		}

		// Running Speed
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * RUNNING_DIFFERENCE;
			_running += Random.Range(-mutation, mutation);
			_running = Mathf.Clamp(_running, MIN_RUNNING, MAX_RUNNING);
		}

		// Angular Speed
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * ANGULAR_DIFFERENCE;
			_angular += Random.Range(-mutation, mutation);
			_angular = Mathf.Clamp(_angular, MIN_ANGULAR, MAX_ANGULAR);
		}

		// Acceleration
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * ACCELERATION_DIFFERENCE;
			_acceleration += Random.Range(-mutation, mutation);
			_acceleration = Mathf.Clamp(_acceleration, MIN_ACCELERATION, MAX_ACCELERATION);
		}
	}
}