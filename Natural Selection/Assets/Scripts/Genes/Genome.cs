using UnityEngine;

/// <summary>
/// Genome contains all the genes that an entity has
/// </summary>
[System.Serializable]
public class Genome
{
	[SerializeField] private SexGene sex;
	[SerializeField] private ColorGene color;
	[SerializeField] private HeightGene height;
	[SerializeField] private SpeedGene speed;
	[SerializeField] private BehaviorGene behavior;

	public SexGene		Sex		 { get { return sex;	  } }
	public ColorGene	Color	 { get { return color;	  } }
	public HeightGene	Height	 { get { return height;	  } }
	public SpeedGene	Speed	 { get { return speed;	  } }
	public BehaviorGene Behavior { get { return behavior; } }

	public Genome(SexGene pSex, ColorGene pColor, HeightGene pHeight, SpeedGene pSpeed, BehaviorGene pBehavior)
	{
		sex = pSex;
		color = pColor;
		height = pHeight;
		speed = pSpeed;
		behavior = pBehavior;
	}

	/// <summary>
	/// Express the whole genome by expressing each gene indiviudally
	/// </summary>
	/// <param name="entity"></param>
	public void ExpressGenome(Entity entity)
	{
		sex.ExpressGene(entity);
		color.ExpressGene(entity);
		height.ExpressGene(entity);
		speed.ExpressGene(entity);
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
		SexGene newSex = sex.CrossGene(otherGenome.Sex);
		ColorGene newColor = color.CrossGene(otherGenome.Color, mutationFactor, mutationChance);
		HeightGene newSize = height.CrossGene(otherGenome.Height, mutationFactor, mutationChance);
		SpeedGene newSpeed = speed.CrossGene(otherGenome.Speed, mutationFactor, mutationChance);
		BehaviorGene newBehavior = behavior.CrossGene(otherGenome.Behavior, mutationFactor, mutationChance);
		return new Genome(newSex, newColor, newSize, newSpeed, newBehavior);
	}
}