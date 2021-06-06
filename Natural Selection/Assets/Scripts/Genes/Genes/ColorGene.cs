using UnityEngine;

[System.Serializable]
public class ColorGene : Gene<ColorGene, ColorAllele>
{
	public ColorGene(ColorAllele pAlleleA, ColorAllele pAlleleB) : base(pAlleleA, pAlleleB) { }

	public override ColorGene CrossGene(ColorGene other, float mutationFactor, float mutationChance)
	{
		return new ColorGene(GetRandomAlleleCopy(mutationFactor, mutationChance), other.GetRandomAlleleCopy(mutationFactor, mutationChance));
	}

	public override void CompleteDominance(Entity entity)
	{
		// If both are dominant or recessive, then choose randomly 
		if (AlleleA.Dominance == AlleleB.Dominance)
		{
			if (Random.Range(0, 2) == 0)
				entity.SetColor(AlleleA.GetColor());
			else
				entity.SetColor(AlleleB.GetColor());
		}
		else if (AlleleA.Dominance == Dominance.DOMINANT && AlleleB.Dominance != AlleleA.Dominance)
			entity.SetColor(AlleleA.GetColor());
		else
			entity.SetColor(AlleleB.GetColor());
	}

	public override void IncompleteDominance(Entity entity)
	{
		Color left = AlleleA.GetColor();
		Color right = AlleleB.GetColor();
		entity.SetColor(Color.Lerp(left, right, 0.5f));
	}

	public override void CoDominance(Entity entity)
	{
		Renderer rend = entity.gameObject.GetComponent<Renderer>();
		rend.material = new Material(Shader.Find("Shader Graphs/Skin"));
		rend.material.SetColor("Color1", AlleleA.GetColor());
		rend.material.SetColor("Color2", AlleleB.GetColor());
	}
}