using UnityEngine;

[System.Serializable]
public class SexGene : Gene<SexGene, SexAllele>
{
	public SexGene(SexAllele pAlleleA, SexAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override SexGene CrossGene(SexGene other, float mutationFactor, float mutationChance)
	{
		return new SexGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CompleteDominance(Entity entity)
	{
		if (!AlleleA.IsFemale || !AlleleB.IsFemale)
			entity.gender = Gender.MALE;
		else
			entity.gender = Gender.FEMALE;
	}

	public override void IncompleteDominance(Entity entity)
	{
		return; // For now the sex gene can only express itself into female or male traits at a time.
	}

	public override void CoDominance(Entity entity)
	{
		return; // For now the sex gene can only express itself into female or male traits at a time.
	}	
}
