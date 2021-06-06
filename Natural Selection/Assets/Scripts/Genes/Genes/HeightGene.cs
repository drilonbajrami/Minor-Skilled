using UnityEngine;

[System.Serializable]
public class HeightGene : Gene<HeightGene, HeightAllele>
{
	public HeightGene(HeightAllele pAlleleA, HeightAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override HeightGene CrossGene(HeightGene other, float mutationFactor, float mutationChance)
	{
		return new HeightGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CompleteDominance(Entity entity)
	{
		// If both are dominant or recessive, then choose randomly 
		if (AlleleA.Dominance == AlleleB.Dominance)
		{
			if (Random.Range(0, 2) == 0)
				entity.SetHeight(AlleleA.Height);
			else
				entity.SetHeight(AlleleB.Height);
		}
		else if (AlleleA.Dominance == Dominance.DOMINANT && AlleleB.Dominance != AlleleA.Dominance)
			entity.SetHeight(AlleleA.Height);
		else
			entity.SetHeight(AlleleB.Height);
	}

	public override void IncompleteDominance(Entity entity)
	{
		float height = (AlleleA.Height + AlleleB.Height) / 2;
		entity.SetHeight(height);
	}

	public override void CoDominance(Entity entity) { }
}