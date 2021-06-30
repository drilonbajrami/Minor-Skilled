using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
	// Singleton
	public static Counter Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{		
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public TMP_Text herbivoreCount;
	public TMP_Text carnivoreCount;
	public TMP_Text herbivoreAliveCount;
	public TMP_Text carnivoreAliveCount;

	// Counts
	private int herbivoreTotal = 0;
	private int carnivoreTotal = 0;
	private int herbivoreAlive = 0;
	private int carnivoreAlive = 0;
	private int peakAliveCount = 0;

	// List of counts per cycle
	private List<int> herbivoreCounts = new List<int>();
	private List<int> carnivoreCounts = new List<int>();
	public List<int> HerbivoreCounts { get { return herbivoreCounts; } }
	public List<int> CarnivoreCounts { get { return carnivoreCounts; } }
	public int PeakAliveCount { get { return peakAliveCount; } }

	private List<float> averageSpeedHerbivore = new List<float>();
	public List<float> AverageSpeedHerbivore { get { return averageSpeedHerbivore; } }
	private float peakSpeedAverage = 0.01f;
	public float PeakSpeedAverage { get { return peakSpeedAverage; } }

	private List<float> averageHeightHerbivore = new List<float>();
	public List<float> AverageHeightHerbivore { get { return averageHeightHerbivore; } }

	private List<float> averagePeerUtilityHerbivore = new List<float>();
	public List<float> AveragePeerUtilityHerbivore { get { return averagePeerUtilityHerbivore; } }

	private List<float> averageOpponentUtilityHerbivore = new List<float>();
	public List<float> AverageOpponentUtilityHerbivore { get { return averageOpponentUtilityHerbivore; } }

	private List<float> averageSocializingChanceHerbivore = new List<float>();
	public List<float> AverageSocializingChanceHerbivore { get { return averageSocializingChanceHerbivore; } }

	private void Start()
	{
		ResetCounter();
	}

	public void Update()
	{
		herbivoreCount.text		 = $"Herbivore Total: {herbivoreTotal}";
		carnivoreCount.text		 = $"Carnivore Total: {carnivoreTotal}";
		herbivoreAliveCount.text = $"Herbivores Alive: {herbivoreAlive}";
		carnivoreAliveCount.text = $"Carnivores Alive: {carnivoreAlive}";
	}

	public void ResetCounter()
	{
		herbivoreTotal = 0;
		carnivoreTotal = 0;
		herbivoreAlive = 0;
		carnivoreAlive = 0;
		peakAliveCount = 0;
		herbivoreCounts.Clear();
		carnivoreCounts.Clear();
	}

	public bool AllSpeciesExtinct()
	{
		return herbivoreAlive == 0 && carnivoreAlive == 0;
	}

	public bool HerbivoresExtinct()
	{
		return herbivoreAlive == 0;
	}

	public void AddCountPerCycle()
	{
		herbivoreCounts.Add(herbivoreAlive);
		carnivoreCounts.Add(carnivoreAlive);
	}

	// Herbivore Counts
	public void AddHerbivoreTotal() => herbivoreTotal++;
	public void RemoveHerbivoreTotal() => herbivoreTotal--;
	public void AddHerbivoreAlive()
	{
		herbivoreAlive++;
		UpdatePeakAliveCounter(herbivoreAlive);
	}
	public void RemoveHerbivoreAlive()
	{
		herbivoreAlive--;
		herbivoreAlive = Mathf.Clamp(herbivoreAlive, 0, 10000);
	}

	// Carnivore Counts
	public void AddCarnivoreTotal() => carnivoreTotal++;
	public void RemoveCarnivoreTotal() => carnivoreTotal--;
	public void AddCarnivoreAlive()
	{
		carnivoreAlive++;
		UpdatePeakAliveCounter(carnivoreAlive);
	}
	public void RemoveCarnivoreAlive()
	{
		carnivoreAlive--;
		carnivoreAlive = Mathf.Clamp(carnivoreAlive, 0, 10000);
	}

	private void UpdatePeakAliveCounter(int newValue)
	{
		if (peakAliveCount < newValue)
			peakAliveCount = (int)(newValue + 0.2f * newValue);
	}

	// Herbivore Speed
	public void AddSpeedAverageOnCycle(float average)
	{
		averageSpeedHerbivore.Add(average);
		UpdatePeakSpeed(average);
	}
	private void UpdatePeakSpeed(float newValue)
	{
		if (peakSpeedAverage < newValue)
			peakSpeedAverage = newValue + 0.2f * newValue;
	}

	// Herbivore Height
	public void AddHeightAverageOnCycle(float average)
	{
		averageHeightHerbivore.Add(average);
	}

	// Herbivore Behavior
	public void AddPeerUtilityAverageOnCycle(float average)
	{
		averagePeerUtilityHerbivore.Add(average);
	}

	public void AddOpponentUtilityAverageOnCycle(float average)
	{
		averageOpponentUtilityHerbivore.Add(average);
	}

	public void AddSocializingChanceAverageOnCycle(float average)
	{
		averageSocializingChanceHerbivore.Add(average);
	}
}
