using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedGene : Gene<SpeedGene, SpeedAllele>
{
	public SpeedGene(SpeedAllele pAlleleA, SpeedAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override SpeedGene CrossGene(SpeedGene other, float mutationFactor, float mutationChance)
	{
		return new SpeedGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CompleteDominance(Entity entity)
	{
		//If both are dominant or recessive, then choose randomly
		if (AlleleA.Dominance == AlleleB.Dominance)
		{
			if (Random.Range(0, 2) == 0)
				entity.SetSpeed(AlleleA.Walking, AlleleA.Running, AlleleA.Angular, AlleleA.Acceleration);
			else
				entity.SetSpeed(AlleleB.Walking, AlleleB.Running, AlleleB.Angular, AlleleB.Acceleration);
		}
		else if (AlleleA.Dominance == Dominance.DOMINANT && AlleleB.Dominance != AlleleA.Dominance)
			entity.SetSpeed(AlleleA.Walking, AlleleA.Running, AlleleA.Angular, AlleleA.Acceleration);
		else
			entity.SetSpeed(AlleleB.Walking, AlleleB.Running, AlleleB.Angular, AlleleB.Acceleration);
	}

	public override void IncompleteDominance(Entity entity)
	{
		entity.SetSpeed((AlleleA.Walking + AlleleB.Walking) / 2, (AlleleA.Running + AlleleB.Running) / 2,
						(AlleleA.Angular + AlleleB.Angular) / 2, (AlleleA.Acceleration + AlleleB.Acceleration) / 2);
	}

	public override void CoDominance(Entity entity) { }
}
