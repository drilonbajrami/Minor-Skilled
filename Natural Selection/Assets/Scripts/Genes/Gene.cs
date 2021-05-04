using UnityEngine;

/// <summary>
/// A gene consist of two alleles and it expresses the traits affected by the alleles
/// based on their dominance and values
/// </summary>
public abstract class Gene
{
	[SerializeField] private Allele _alleleLeft;
	public Allele AlleleLeft => _alleleLeft;
	[SerializeField] private Allele _alleleRight;
	public Allele AlleleRight => _alleleRight;
    

	public Gene(Allele pAlleleLeft, Allele pAlleleRight)
	{
		_alleleLeft = pAlleleLeft;
		_alleleRight = pAlleleRight;	
	}

	/// <summary>
	/// Returns a copy of one of the gene's alleles (Randomly selected).
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public Allele GetRandomAlleleCopy(float mutationFactor, float mutationChance)
	{
		int i = Random.Range(0, 2);
		if (i == 0)
			return _alleleLeft.GetCopy(mutationFactor, mutationChance);
		else
			return _alleleRight.GetCopy(mutationFactor, mutationChance);
	}

	/// <summary>
	/// Expresses the gene through entity's trait/feature based on its allele's types of dominance and values
	/// </summary>
	/// <param name="entity"></param>
	public void ExpressGene(EntityGeneTest entity)
	{
		if (_alleleLeft.Dominance == Dominance.DOMINANT || 
			_alleleRight.Dominance == Dominance.DOMINANT || 
			(_alleleLeft.Dominance == Dominance.RECESSIVE && _alleleLeft.Dominance == _alleleRight.Dominance))
			CompleteDominance(entity);
		else if (_alleleLeft.Dominance == Dominance.SEMIDOMINANT && _alleleRight.Dominance == Dominance.SEMIDOMINANT)
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
	/// The trait is a mixture between two alleles.
	/// </summary>
	/// <param name="entity"></param>
	public abstract void CoDominance(EntityGeneTest entity);
}
