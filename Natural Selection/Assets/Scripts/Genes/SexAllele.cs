using UnityEngine;

[System.Serializable]
public class SexAllele : Allele<SexAllele>
{
	[Header("Dominance is dependent on the 'Is Female' variable.")]
	[SerializeField] private bool _isFemale;
	public bool IsFemale => _isFemale;

	public SexAllele(bool pIsFemale) : base()
	{
		_isFemale = pIsFemale;
		Dominance = pIsFemale ? Dominance.RECESSIVE : Dominance.DOMINANT;
	}

	public override SexAllele GetCopy(float mutationFactor, float mutationChance)
	{
		return new SexAllele(_isFemale);
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		return; // Mutation not implemented yet for genders since it is a complicated issue
	}
}