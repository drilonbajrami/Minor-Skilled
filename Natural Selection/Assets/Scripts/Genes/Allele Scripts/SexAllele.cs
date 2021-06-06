using UnityEngine;

[System.Serializable]
public class SexAllele : Allele<SexAllele>
{
	[Header("X alleles are recessive meanwhile the Y alleles are dominant.")]
	[SerializeField] private bool _isFemale;
	public bool IsFemale => _isFemale;

	public SexAllele(bool pIsFemale) : base(pIsFemale ? Dominance.RECESSIVE : Dominance.DOMINANT)
	{
		_isFemale = pIsFemale;
	}

	public override SexAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
		return new SexAllele(_isFemale);
	}
}