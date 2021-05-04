using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combiner : MonoBehaviour
{
    [SerializeField] GameObject parentA;
    [SerializeField] GameObject parentB;

    public int counter;
	private int col;
	private int row;
	[SerializeField] public int MAXCOL = 2;

	[Range(0.01f, 1f)]
	public float mutationFactor;

	[Range(0.0f, 100.0f)]
	public float mutationChance;

    public List<GameObject> offsprings;

	private void Start()
	{
		offsprings = new List<GameObject>();
		col = 0;
		row = 0;
	}

	void Update()
    {
		if (Input.GetKey(KeyCode.Space))
		{
			Genome father = parentA.GetComponent<EntityGeneTest>().genome;
			Genome mother = parentB.GetComponent<EntityGeneTest>().genome;

			GameObject offspring = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			EntityGeneTest a = offspring.AddComponent<EntityGeneTest>();

			a.genome = mother.CrossGenome(father, mutationFactor, mutationChance);
			a.genome.ExpressGenome(a);

			// Show values in inspector
			a.colorLeft = (a.genome.Color.AlleleLeft as ColorAllele).Color;
			a.colorRight = (a.genome.Color.AlleleRight as ColorAllele).Color;
			a.sizeLeft = (a.genome.Size.AlleleLeft as SizeAllele).Size;
			a.sizeRight = (a.genome.Size.AlleleRight as SizeAllele).Size;

			a.UpdateGenomeInfo();
			offsprings.Add(offspring);
			counter++;
			Place(offspring);
		}

		//if (counter > 0 && counter % 2 == 0)
		//{
		//	int a = counter;
		//	parentA = offsprings[a - 2];
		//	parentB = offsprings[a - 1];
		//}
	}

	public void Place(GameObject o)
	{
		if (col == MAXCOL)
		{
			row++;
			col = 0;
		}

		o.transform.position = new Vector3(-MAXCOL + col * 2, o.transform.localScale.y, parentA.transform.position.z - 2 - row * 2);
		col++;
	}

	public void PlaceCouple(GameObject o)
	{
		o.transform.position = new Vector3(-1 + col * 2, 1, parentA.transform.position.z - 2);
		col++;

		if (col == MAXCOL)
		{
			row++;
			col -= MAXCOL;
		}
	}
}
