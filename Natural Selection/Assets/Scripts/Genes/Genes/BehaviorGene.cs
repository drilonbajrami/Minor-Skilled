[System.Serializable]
public class BehaviorGene : Gene<BehaviorGene, BehaviorAllele>
{
	public BehaviorAllele IdealAllele;

	public BehaviorGene(BehaviorAllele pAlleleA, BehaviorAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override BehaviorGene CrossGene(BehaviorGene other, float mutationFactor = 0, float mutationChance = 0)
	{
		return new BehaviorGene(IdealAllele.GetCopy(), other.IdealAllele.GetCopy());
	}

	public override void CompleteDominance(Entity entity)
	{
		if (AlleleA.OverallUtility >= AlleleB.OverallUtility)
			IdealAllele = AlleleA;
		else
			IdealAllele = AlleleB;

		entity.SetBehavior(IdealAllele.PeerUtility, IdealAllele.OpponentUtility, IdealAllele.SocializingChance);
	}

	public override void IncompleteDominance(Entity entity) { }

	public override void CoDominance(Entity entity) { }
}
