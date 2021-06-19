using UnityEngine;

public enum Sex { 
	MALE, 
	FEMALE 
}

public enum Order { 
	HERBIVORE, 
	CARNIVORE 
}

[System.Serializable]
public struct Speed
{
	public float Walking;
	public float Running;
	public float Angular;
	public float Acceleration;

	public Speed(float pWalking, float pRunning, float pAngular, float pAcceleration)
	{
		Walking = pWalking;
		Running = pRunning;
		Angular = pAngular;
		Acceleration = pAcceleration;
	}
}

[System.Serializable]
public struct Behavior
{
	public readonly float PeerUtility;
	public readonly float OpponentUtility;
	public readonly float SocializingChance;

	public Behavior(float pPeerUtility, float pOpponentUtility, float pSocializingChance)
	{
		PeerUtility = pPeerUtility;
		OpponentUtility = pOpponentUtility;
		SocializingChance = pSocializingChance;
	}
}

[System.Serializable]
public class Vitals
{
	[SerializeField] private float _energy;
	[SerializeField] private float _resources;

	public float Energy { get { return _energy; } }
	public float Resources { get { return _resources; } }

	public Vitals()
	{
		_energy = 100.0f;
		_resources = 0.0f;
	}

	public void DepleteEnergy(float rate = 1)
	{
		_energy -= Time.deltaTime * rate;
		_energy = Mathf.Clamp(_energy, 0.0f, 100.0f);
	}

	public void ConvertResourcesToEnergy()
	{
		if (_energy == 0.0f && _resources > 0.0f)
		{
			_energy = _resources;
			_resources = 0.0f;
		}
	}

	public void ReplenishResources(float amount)
	{
		_resources += amount;
		_resources = Mathf.Clamp(_resources, 0.0f, 100.0f);
	}

	public bool IsStarving()
	{
		return _energy <= 0.0f;
	}

	public void Reset()
	{
		_energy = 100.0f;
		_resources = 0.0f;
	}
}