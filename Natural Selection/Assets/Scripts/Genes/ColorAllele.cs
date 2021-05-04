using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ColorAllele : Allele
{
	[SerializeField] private Color _color;
	public Color Color => _color;

	public ColorAllele(Dominance pDominance, Color pColor) : base(pDominance)
	{
		_color = pColor;
	}

	public override Allele GetCopy(float mutationFactor, float mutationChance)
	{
        ColorAllele copy = new ColorAllele(this.Dominance, new Color(_color.r, _color.g, _color.b));
        copy.Mutate(mutationFactor, mutationChance);
        return copy;
	}

    public override void Mutate(float mutationFactor, float mutationChance)
    {
		//// Mutate the red channel
		//if (Random.Range(0.0f, 100.0f) <= mutationChance)
		//{
		//	_color.r += Random.Range(-mutationFactor, +mutationFactor);
		//	_color.r = Mathf.Clamp(_color.r, 0.0f, 1.0f);
		//}

		//// Mutate the red channel
		//if (Random.Range(0.0f, 100.0f) <= mutationChance)
		//{
		//	_color.g += Random.Range(-mutationFactor, +mutationFactor);
		//	_color.g = Mathf.Clamp(_color.g, 0.0f, 1.0f);
		//}

		//// Mutate the red channel
		//if (Random.Range(0.0f, 100.0f) <= mutationChance)
		//{
		//	_color.b += Random.Range(-mutationFactor, +mutationFactor);
		//	_color.b = Mathf.Clamp(_color.b, 0.0f, 1.0f);
		//}

		// Monochromatic mutation 
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
}