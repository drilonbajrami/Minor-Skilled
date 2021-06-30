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
	public float PeerUtility;
	public float OpponentUtility;
	public float SocializingChance;

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

	public float Energy { get { return _energy; } }

	public Vitals()
	{
		_energy = 100.0f;
	}

	public void ConsumeEnergy(float rate = 1)
	{
		_energy -= Time.deltaTime * rate;
		_energy = Mathf.Clamp(_energy, 0.0f, 100.0f);
	}

	public void RecoverEnergy(float amount)
	{
		_energy += amount;
		_energy = Mathf.Clamp(_energy, 0.0f, 100.0f);
	}

	public bool NeedsToEat()
	{
		return _energy < 75.0f;
	}

	public bool IsStarving()
	{
		return _energy <= 0.0f;
	}

	public void Reset()
	{
		_energy = 100.0f;
	}
}