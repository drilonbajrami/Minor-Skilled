using UnityEngine;

/// <summary>
/// A gene consist of two alleles and it expresses the traits affected by the alleles
/// based on their dominance and values
/// </summary>
public abstract class Gene<T, A>
	where T : Gene<T, A>
	where A : Allele<A>
{
	// A gene consist of two alleles, one allele is inherited from each parent
	[SerializeField] private A _alleleA;
	[SerializeField] private A _alleleB;

	public A AlleleA => _alleleA;
	public A AlleleB => _alleleB;
    

	public Gene(A pAlleleA, A pAlleleB)
	{
		_alleleA = pAlleleA;
		_alleleB = pAlleleB;	
	}

	/// <summary>
	/// Returns a copy of one of the gene's alleles (Randomly selected).
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public A GetRandomAlleleCopy(float mutationFactor, float mutationChance)
	{
		return Random.Range(0, 2) == 0 ? _alleleA.GetCopy(mutationFactor, mutationChance) : _alleleB.GetCopy(mutationFactor, mutationChance);
	}

	/// <summary>
	/// Returns a new combined gene from both parent genes
	/// </summary>
	/// <param name="otherParentGene"></param>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public abstract T CrossGene(T otherParentGene, float mutationFactor, float mutationChance);

	/// <summary>
	/// Expresses the gene through entity's trait/feature based on its allele's types of dominance and values
	/// </summary>
	/// <param name="entity"></param>
	public void ExpressGene(EntityGeneTest entity)
	{
		if (_alleleA.Dominance == Dominance.DOMINANT || 
			_alleleB.Dominance == Dominance.DOMINANT || 
			(_alleleA.Dominance == Dominance.RECESSIVE && _alleleA.Dominance == _alleleB.Dominance))
			CompleteDominance(entity);
		else if (_alleleA.Dominance == Dominance.SEMIDOMINANT && _alleleB.Dominance == Dominance.SEMIDOMINANT)
			CoDominance(entity);
		else
			IncompleteDominance(entity);
	}

	/// <summary>
	/// Complete dominance occurs when one allele is fully dominant over the other.
	/// The trait is determined by the dominant allele only.
	/// </summary>
	/// <param name="entity"></param>
	public abstract void CompleteDominance(EntityGeneTest entity);

	/// <summary>
	/// Incomplete dominance occurs when one allele is not fully dominant over the other allele.
	/// The trait is a blend between two alleles.
	/// </summary>
	/// <param name="entity"></param>
	public abstract void IncompleteDominance(EntityGeneTest entity);

	/// <summary>
	/// CoDominance occrus when two alleles are semi-dominant over one another.
	/// The trait is a mixture between both alleles.
	/// </summary>
	/// <param name="entity"></param>
	public abstract void CoDominance(EntityGeneTest entity);
}
