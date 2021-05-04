using UnityEngine;

[System.Serializable]
public class SizeGene : Gene
{
	public SizeGene(SizeAllele pAlleleLeft, SizeAllele pAlleleRight) : base(pAlleleLeft, pAlleleRight) { }

	public override void CoDominance(EntityGeneTest entity)
	{
		
	}

	public override void CompleteDominance(EntityGeneTest entity)
	{
		if (AlleleLeft.Dominance == Dominance.DOMINANT)
			entity.gameObject.transform.localScale = new Vector3((AlleleLeft as SizeAllele).Size, (AlleleLeft as SizeAllele).Size, (AlleleLeft as SizeAllele).Size);
		else
			entity.gameObject.transform.localScale = new Vector3((AlleleRight as SizeAllele).Size, (AlleleRight as SizeAllele).Size, (AlleleRight as SizeAllele).Size);
	}

	public override void IncompleteDominance(EntityGeneTest entity)
	{
		float size = ((AlleleLeft as SizeAllele).Size + (AlleleRight as SizeAllele).Size) / 2;
		entity.gameObject.transform.localScale = new Vector3(size, size, size);
	}
}
