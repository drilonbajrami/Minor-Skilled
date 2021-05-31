using UnityEngine;

/// <summary>
/// Genome contains all the genes that an entity has
/// </summary>
public class Genome
{
	[SerializeField] private ColorGene color;
	[SerializeField] private SizeGene size;
	[SerializeField] private SexGene sex;
	[SerializeField] private BehaviorGene behavior;

	public ColorGene Color { get { return color; } set { color = value; } }
	public SizeGene Size { get { return size; } set { size = value; } }
	public SexGene Sex { get { return sex; } set { sex = value; } }
	public BehaviorGene Behavior { get { return behavior; } set { behavior = value; } }

	public Genome(ColorGene pColor, SizeGene pSize, SexGene pSex, BehaviorGene pBehavior)
	{
		color = pColor;
		size = pSize;
		sex = pSex;
		behavior = pBehavior;
	}

	/// <summary>
	/// Express the whole genome by expressing each gene indiviudally
	/// </summary>
	/// <param name="entity"></param>
	public void ExpressGenome(Entity entity)
	{
		color.ExpressGene(entity);
		size.ExpressGene(entity);
		sex.ExpressGene(entity);
		behavior.ExpressGene(entity);
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
		SexGene newSex = sex.CrossGene(otherGenome.sex, mutationFactor, mutationChance);
		BehaviorGene newBehavior = behavior.CrossGene(otherGenome.behavior, mutationFactor, mutationChance);
		return new Genome(newColor, newSize, newSex, newBehavior);
	}
}