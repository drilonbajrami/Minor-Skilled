using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
	[SerializeField] private Color carnivore;
	[SerializeField] private Color herbivore;

	public static Color CARNIVORE;
	public static Color HERBIVORE;

	private void Awake()
	{
		CARNIVORE = carnivore;
		HERBIVORE = herbivore;
	}
}
