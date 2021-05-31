using UnityEngine;

[System.Serializable]
public class SizeGene : Gene<SizeGene, SizeAllele>
{
	public SizeGene(SizeAllele pAlleleA, SizeAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override SizeGene CrossGene(SizeGene other, float mutationFactor, float mutationChance)
	{
		return new SizeGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CoDominance(Entity entity)
	{
		// This type of gene does not have the possibility of CoDominance
		// since size is just a scalar
		// Later on I might try to implement size in a different matter or split it into length, height which make more sense
		// but for now I am going to keep it simple. 
	}

	public override void CompleteDominance(Entity entity)
	{
		if (AlleleA.Dominance == Dominance.DOMINANT)
			entity.gameObject.transform.localScale = new Vector3(/*AlleleA.Size*/1, AlleleA.Size, /*AlleleA.Size*/1);
		else
			entity.gameObject.transform.localScale = new Vector3(/*AlleleB.Size*/1, AlleleB.Size, /*AlleleB.Size*/1);
	}

	public override void IncompleteDominance(Entity entity)
	{
		float size = (AlleleA.Size + AlleleB.Size) / 2;
		entity.gameObject.transform.localScale = new Vector3(1, size, 1);
	}
}