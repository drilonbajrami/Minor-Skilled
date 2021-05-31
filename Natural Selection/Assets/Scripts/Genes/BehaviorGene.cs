[System.Serializable]
public class BehaviorGene : Gene<BehaviorGene, BehaviorAllele>
{
	public BehaviorAllele IdealAllele;

	public BehaviorGene(BehaviorAllele pAlleleA, BehaviorAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override BehaviorGene CrossGene(BehaviorGene other, float mutationFactor, float mutationChance)
	{
		return new BehaviorGene(new BehaviorAllele(IdealAllele.PeerUtility, IdealAllele.OpponentUtility, IdealAllele.SocializingChance),
								new BehaviorAllele(other.IdealAllele.PeerUtility, other.IdealAllele.OpponentUtility, other.IdealAllele.SocializingChance));
	}

	public override void CompleteDominance(Entity entity)
	{
		if (AlleleA.OverallUtility >= AlleleB.OverallUtility)
			IdealAllele = AlleleA;
		else
			IdealAllele = AlleleB;
	}

	public override void IncompleteDominance(Entity entity)
	{
		throw new System.NotImplementedException();
	}

	public override void CoDominance(Entity entity)
	{
		throw new System.NotImplementedException();
	}
}
