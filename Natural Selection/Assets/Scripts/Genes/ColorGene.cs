using UnityEngine;

/// <summary>
/// Holds genetic information about the color trait
/// </summary>
[System.Serializable]
public class ColorGene : Gene<ColorGene, ColorAllele>
{
	public ColorGene(ColorAllele pAlleleA, ColorAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override ColorGene CrossGene(ColorGene other, float mutationFactor, float mutationChance)
	{
		return new ColorGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CompleteDominance(EntityGeneTest entity)
	{
		if (AlleleA.Dominance == Dominance.DOMINANT)
			entity.gameObject.GetComponent<Renderer>().material.color = new Color(AlleleA.Color.r, AlleleA.Color.g, AlleleA.Color.b);
		else
			entity.gameObject.GetComponent<Renderer>().material.color = new Color(AlleleB.Color.r, AlleleB.Color.g, AlleleB.Color.b);
	}

	public override void IncompleteDominance(EntityGeneTest entity)
	{
		Color left = new Color(AlleleA.Color.r, AlleleA.Color.g, AlleleA.Color.b);
		Color right = new Color(AlleleB.Color.r, AlleleB.Color.g, AlleleB.Color.b);
		entity.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(left, right, 0.5f);
	}

	public override void CoDominance(EntityGeneTest entity)
	{
		Renderer rend = entity.gameObject.GetComponent<Renderer>();
		rend.material = new Material(Shader.Find("Shader Graphs/Skin"));
		Color a = new Color(AlleleA.Color.r, AlleleA.Color.g, AlleleA.Color.b);
		Color b = new Color(AlleleB.Color.r, AlleleB.Color.g, AlleleB.Color.b);
		rend.material.SetColor("Color1", a);
		rend.material.SetColor("Color2", b);
	}
}