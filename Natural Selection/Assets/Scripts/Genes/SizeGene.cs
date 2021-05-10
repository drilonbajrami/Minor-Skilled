using UnityEngine;

[System.Serializable]
public class SizeGene : Gene<SizeGene, SizeAllele>
{
	public SizeGene(SizeAllele pAlleleLeft, SizeAllele pAlleleRight) : base(pAlleleLeft, pAlleleRight) { }

	public override SizeGene CrossGene(SizeGene other, float mutationFactor, float mutationChance)
	{
		return new SizeGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CoDominance(EntityGeneTest entity)
	{
		// This type of gene does not have the possibility of CoDominance
		// since size is just a scalar
	}

	public override void CompleteDominance(EntityGeneTest entity)
	{
		if (AlleleA.Dominance == Dominance.DOMINANT)
			entity.gameObject.transform.localScale = new Vector3(/*AlleleA.Size*/1, AlleleA.Size, /*AlleleA.Size*/1);
		else
			entity.gameObject.transform.localScale = new Vector3(/*AlleleB.Size*/1, AlleleB.Size, /*AlleleB.Size*/1);
	}

	public override void IncompleteDominance(EntityGeneTest entity)
	{
		float size = (AlleleA.Size + AlleleB.Size) / 2;
		entity.gameObject.transform.localScale = new Vector3(1, size, 1);
	}
}