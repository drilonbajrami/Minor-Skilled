using UnityEngine;

[System.Serializable]
public class ColorAllele : Allele<ColorAllele>
{
	[SerializeField] private Color _color;
	public Color Color => _color;

	[Tooltip("Mutations affect all color channels the same if set to true")]
	[SerializeField] private bool monochromaticMutation = true;

	public ColorAllele(Dominance pDominance, Color pColor) : base(pDominance)
	{
		_color = pColor;
	}

	public override ColorAllele GetCopy(float mutationFactor = 0, float mutationChance = 0)
	{
        ColorAllele copy = new ColorAllele(Dominance, new Color(_color.r, _color.g, _color.b, 1));
        copy.Mutate(mutationFactor, mutationChance);
        return copy;
	}

	public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (mutationFactor == 0) return;

		if (monochromaticMutation)
			MonochromaticMutation(mutationFactor, mutationChance);
		else
			PolychromaticMutation(mutationFactor, mutationChance);
	}

	/// <summary>
	/// Mutation affects all channels equally
	/// </summary>
	private void MonochromaticMutation(float mutationFactor, float mutationChance)
	{
		// RGB Channels
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float mutation = Random.Range(-mutationFactor, mutationFactor);
			_color.r += mutation;
			_color.g += mutation;
			_color.b += mutation;

			_color.r = Mathf.Clamp(_color.r, 0.0f, 1.0f);
			_color.g = Mathf.Clamp(_color.g, 0.0f, 1.0f);
			_color.b = Mathf.Clamp(_color.b, 0.0f, 1.0f);
		}
	}

	/// <summary>
	/// Mutations affects each channel differently 
	/// </summary>
	private void PolychromaticMutation(float mutationFactor, float mutationChance)
	{
		// Red Channel
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			_color.r += Random.Range(-mutationFactor, +mutationFactor);
			_color.r = Mathf.Clamp(_color.r, 0.0f, 1.0f);
		}

		// Green Channel
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			_color.g += Random.Range(-mutationFactor, +mutationFactor);
			_color.g = Mathf.Clamp(_color.g, 0.0f, 1.0f);
		}

		// Blue Channel
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			_color.b += Random.Range(-mutationFactor, +mutationFactor);
			_color.b = Mathf.Clamp(_color.b, 0.0f, 1.0f);
		}
	}

	public Color GetColor()
	{
		return new Color(_color.r, _color.g, _color.b, 1);
	}
}