using UnityEngine;

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

	public void ExpressGenome(EntityGeneTest entity)
	{
		color.ExpressGene(entity);
		size.ExpressGene(entity);
	}

	public Genome CrossGenome(Genome otherGenome, float mutationFactor, float mutationChance)
	{
		ColorGene newColor = CrossColorGenes(otherGenome, mutationFactor, mutationChance);
		//ColorGene newColor = (ColorGene)color.CrossGene(otherGenome.color, mutationFactor, mutationChance);
		SizeGene newSize = CrossSizeGenes(otherGenome, mutationFactor, mutationChance);
		return new Genome(newColor, newSize);
	}

	private ColorGene CrossColorGenes(Genome otherGenome, float mutationFactor, float mutationChance)
	{
		return new ColorGene(this.color.GetRandomAlleleCopy(mutationFactor, mutationChance) as ColorAllele, 
							 otherGenome.color.GetRandomAlleleCopy(mutationFactor, mutationChance) as ColorAllele);
	}

	private SizeGene CrossSizeGenes(Genome otherGenome, float mutationFactor, float mutationChance)
	{
		return new SizeGene(this.size.GetRandomAlleleCopy(mutationFactor, mutationChance) as SizeAllele,
							otherGenome.size.GetRandomAlleleCopy(mutationFactor, mutationChance) as SizeAllele);
	}
}