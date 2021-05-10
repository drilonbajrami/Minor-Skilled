using UnityEngine;
using Random = UnityEngine.Random;

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

	public override ColorAllele GetCopy(float mutationFactor, float mutationChance)
	{
        ColorAllele copy = new ColorAllele(Dominance, new Color(_color.r, _color.g, _color.b, 1));
        copy.Mutate(mutationFactor, mutationChance);
        return copy;
	}

    public override void Mutate(float mutationFactor, float mutationChance)
	{
		if (monochromaticMutation)
			MonochromaticMutation(mutationFactor, mutationChance);
		else
			PolychromaticMutation(mutationFactor, mutationChance);
	}

	/// <summary>
	/// Mutations affects every color channel the same way
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	private void MonochromaticMutation(float mutationFactor, float mutationChance)
	{
		// RGB Channel
		if (Random.Range(0.0f, 100.0f) <= mutationChance)
		{
			float edit = Random.Range(-mutationFactor, mutationFactor);
			_color.r += edit;
			_color.g += edit;
			_color.b += edit;

			_color.r = Mathf.Clamp(_color.r, 0.0f, 1.0f);
			_color.g = Mathf.Clamp(_color.g, 0.0f, 1.0f);
			_color.b = Mathf.Clamp(_color.b, 0.0f, 1.0f);
		}
	}

	/// <summary>
	/// Mutations affects each channel indivudally and differently 
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
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
}