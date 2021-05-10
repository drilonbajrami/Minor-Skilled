using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SizeAllele : Allele<SizeAllele>
{
	[SerializeField] private float _size;
	public float Size => _size;

	public SizeAllele(Dominance pDominance, float pSize) : base(pDominance)
	{
		_size = pSize;
	}

	public override SizeAllele GetCopy(float mutationFactor, float mutationChance)
	{
		SizeAllele copy = new SizeAllele(Dominance, _size);
		copy.Mutate(mutationFactor, mutationChance);
		return copy;
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			_size += Random.Range(-mutationFactor, mutationFactor);
			_size = Mathf.Clamp(_size, 0.1f, 1.0f);
		}	
	}
}
