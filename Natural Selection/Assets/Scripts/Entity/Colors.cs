using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
	[SerializeField] private Color carnivore;
	[SerializeField] private Color herbivore;
	[SerializeField] private Gradient fitnessColor;

	public static Color CARNIVORE;
	public static Color HERBIVORE;
	public static Gradient FITCOLOR;

	private void Awake()
	{
		CARNIVORE = carnivore;
		HERBIVORE = herbivore;
		FITCOLOR = fitnessColor;
	}
}
