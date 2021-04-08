using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
	private List<Gene> _genes;

	public Genome()
	{
		_genes = new List<Gene>();
	}

	public Gene GetGene(GeneType type)
	{
		return _genes.Find(gene => gene.GeneType == type);
	}

	public void AddGene(Gene gene)
	{
		_genes.Add(gene);
	}
}
