using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGenderColor : MonoBehaviour
{
	//[SerializeField] private Color male;
	//[SerializeField] private Color female;
	//[SerializeField] private Color reproducing;
	//[SerializeField] private Color offspring;

	//[SerializeField] private Color carnivore;
	//[SerializeField] private Color herbivore;

	[SerializeField] private Color fleeing;
	[SerializeField] private Color chasing;

	//public static Color MALE;
	//public static Color FEMALE;
	//public static Color REPRODUCING;
	//public static Color OFFSPRING;

	//public static Color CARNIVORE;
	//public static Color HERBIVORE;

	public static Color FLEEING;
	public static Color CHASING;

	[SerializeField] private Color maleHerbivore;
	[SerializeField] private Color femaleHerbivore;
	[SerializeField] private Color maleCarnivore;
	[SerializeField] private Color femaleCarnivore;
	[SerializeField] private Color herbivoreReproducing;
	[SerializeField] private Color carnivoreReproducing;

	public static Color HMALE;
	public static Color HFEMALE;
	public static Color CMALE;
	public static Color CFEMALE;
	public static Color HREPRODUCE;
	public static Color CREPRODUCE;

	private void Awake()
	{
		//MALE = male;
		//FEMALE = female;
		//REPRODUCING = reproducing;
		//OFFSPRING = offspring;

		//CARNIVORE = carnivore;
		//HERBIVORE = herbivore;

		FLEEING = fleeing;
		CHASING = chasing;

		HMALE = maleHerbivore;
		HFEMALE = femaleHerbivore;
		CMALE = maleCarnivore;
		CFEMALE = femaleCarnivore;
		HREPRODUCE = herbivoreReproducing;
		CREPRODUCE = carnivoreReproducing;
	}
}
