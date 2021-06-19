using UnityEngine;

[System.Serializable]
public class HeightAllele : Allele<HeightAllele>
{
	private const float MIN_HEIGHT = 0.3f;
	private const float MAX_HEIGHT = 2.0f;
	private const float HEIGHT_DIFF = MAX_HEIGHT - MIN_HEIGHT;

	[Range(MIN_HEIGHT, MAX_HEIGHT)] [SerializeField] private float _height;
	public float Height => _height;

	public HeightAllele(Dominance pDominance, float pHeight) : base(pDominance)
	{
		_height = pHeight;
	}

	public override HeightAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
		HeightAllele copy = new HeightAllele(Dominance, _height);
		copy.Mutate(mutationFactor, mutationChance);
		return copy;
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (mutationFactor == 0) return;

		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = mutationFactor * HEIGHT_DIFF;
			_height += Random.Range(-mutation, mutation);
			_height = Mathf.Clamp(_height, MIN_HEIGHT, MAX_HEIGHT);
		}	
	}
}