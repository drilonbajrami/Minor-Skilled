using UnityEngine;

[System.Serializable]
public class SexGene : Gene<SexGene, SexAllele>
{
	public SexGene(SexAllele pAlleleA, SexAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override SexGene CrossGene(SexGene other, float mutationFactor = 0, float mutationChance = 0)
	{
		return new SexGene(GetRandomAlleleCopy(), other.GetRandomAlleleCopy());
	}

	public override void CompleteDominance(Entity entity)
	{
		if (!AlleleA.IsFemale || !AlleleB.IsFemale) // If one of alleles is a Y allele then we assign the MALE gender
			entity.SetSex(Sex.MALE);
		else                                        // Else if both alleses are X then we assign the FEMALE gender
			entity.SetSex(Sex.FEMALE);
	}

	public override void IncompleteDominance(Entity entity) { }

	public override void CoDominance(Entity entity) { }	
}