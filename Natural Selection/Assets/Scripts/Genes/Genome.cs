using UnityEngine;

/// <summary>
/// Genome contains all the genes that an entity has
/// </summary>
public class Genome
{
	[SerializeField] private ColorGene color;
	[SerializeField] private SizeGene size;

	public ColorGene Color { get { return color; } set { color = value; } }
	public SizeGene Size { get { return size; } set { size = value; } }

	public Genome(ColorGene pColor, SizeGene pSize)
	{
		color = pColor;
		size = pSize;
	}

	/// <summary>
	/// Express the whole genome by expressing each gene indiviudally
	/// </summary>
	/// <param name="entity"></param>
	public void ExpressGenome(EntityGeneTest entity)
	{
		color.ExpressGene(entity);
		size.ExpressGene(entity);
	}

	/// <summary>
	/// Returns a new mutated copy genome that is a mix of both parents genome
	/// </summary>
	/// <param name="otherGenome"></param>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public Genome CrossGenome(Genome otherGenome, float mutationFactor, float mutationChance)
	{
		ColorGene newColor = color.CrossGene(otherGenome.color, mutationFactor, mutationChance);
		SizeGene newSize = size.CrossGene(otherGenome.size, mutationFactor, mutationChance);
		return new Genome(newColor, newSize);
	}
}