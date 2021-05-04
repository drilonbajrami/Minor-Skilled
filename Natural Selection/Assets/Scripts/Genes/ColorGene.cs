using UnityEngine;

[System.Serializable]
public class ColorGene : Gene
{
	public ColorGene(ColorAllele pAlleleLeft, ColorAllele pAlleleRight) : base(pAlleleLeft, pAlleleRight) { }

	public override void CoDominance(EntityGeneTest entity)
	{
		Renderer rend = entity.gameObject.GetComponent<Renderer>();
		rend.material = new Material(Shader.Find("Shader Graphs/Skin"));
		Color left = new Color((AlleleLeft as ColorAllele).Color.r, (AlleleLeft as ColorAllele).Color.g, (AlleleLeft as ColorAllele).Color.b);
		Color right = new Color((AlleleRight as ColorAllele).Color.r, (AlleleRight as ColorAllele).Color.g, (AlleleRight as ColorAllele).Color.b);
		rend.material.SetColor("Color1", left);
		rend.material.SetColor("Color2", right);
	}

	public override void CompleteDominance(EntityGeneTest entity)
	{
		if (AlleleLeft.Dominance == Dominance.DOMINANT)
			entity.gameObject.GetComponent<Renderer>().material.color = new Color((AlleleLeft as ColorAllele).Color.r, (AlleleLeft as ColorAllele).Color.g, (AlleleLeft as ColorAllele).Color.b);
		else
			entity.gameObject.GetComponent<Renderer>().material.color = new Color((AlleleRight as ColorAllele).Color.r, (AlleleRight as ColorAllele).Color.g, (AlleleRight as ColorAllele).Color.b);
	}

	public override void IncompleteDominance(EntityGeneTest entity)
	{
		Color left = new Color((AlleleLeft as ColorAllele).Color.r, (AlleleLeft as ColorAllele).Color.g, (AlleleLeft as ColorAllele).Color.b);
		Color right = new Color((AlleleRight as ColorAllele).Color.r, (AlleleRight as ColorAllele).Color.g, (AlleleRight as ColorAllele).Color.b);
		entity.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(left, right, 0.5f);
	}
}