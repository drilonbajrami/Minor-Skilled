using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
	public TMP_Text herbivoreCount;
	public TMP_Text carnivoreCount;
	public TMP_Text herbivoreAliveCount;
	public TMP_Text carnivoreAliveCount;

	private static int herbivoreTotal = 0;
	private static int carnivoreTotal = 0;

	public static int herbivoreAlive = 0;
	public static int carnivoreAlive = 0;

	public static List<int> herbivoreCounts = new List<int>();
	public static List<int> carnivoreCounts = new List<int>();

	private void Start()
	{
		ResetCounter();
	}

	public void Update()
	{
		herbivoreCount.text = $"Herbivore Total Count: {herbivoreTotal}";
		carnivoreCount.text = $"Carnivore Total Count: {carnivoreTotal}";
		herbivoreAliveCount.text = $"Herbivores Alive: {herbivoreAlive}";
		carnivoreAliveCount.text = $"Carnivores Alive: {carnivoreAlive}";
	}

	public static void ResetCounter()
	{
		herbivoreTotal = 0;
		carnivoreTotal = 0;
		herbivoreAlive = 0;
		carnivoreAlive = 0;
	}

	public static void AddCountPerCycle()
	{
		herbivoreCounts.Add(herbivoreAlive);
		carnivoreCounts.Add(carnivoreAlive);
	}

	public static void IncrementHerbivoreTotal()
	{
		herbivoreTotal++;
	}
	public static void IncrementCarnivoreTotal()
	{
		carnivoreTotal++;
	}

	public static void DecrementHerbivoreTotal()
	{
		herbivoreTotal--;
	}

	public static void DecrementCarnivoreTotal()
	{
		carnivoreTotal--;
	}

	public static void IncrementHerbivoreAlive()
	{
		herbivoreAlive++;
	}

	public static void IncrementCarnivoreAlive()
	{
		carnivoreAlive++;
	}

	public static void DecrementHerbivoreAlive()
	{
		herbivoreAlive--;
		herbivoreAlive = Mathf.Clamp(herbivoreAlive, 0, 10000);
	}

	public static void DecrementCarnivoreAlive()
	{
		carnivoreAlive--;
		carnivoreAlive = Mathf.Clamp(carnivoreAlive, 0, 10000);
	}
}
