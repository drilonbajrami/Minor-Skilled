using UnityEngine;

/// <summary>
/// Allele is the fundamental part that contains genetic information for a specific trait
/// </summary>
[System.Serializable]
public abstract class Allele<T> where T : Allele<T>
{
	[SerializeField] private Dominance dominance;
	public Dominance Dominance { get { return dominance; } protected set { dominance = value; } }

	/// <summary>
	/// Constructor with dominance parameter
	/// </summary>
	/// <param name="pDominance"></param>
	public Allele(Dominance pDominance)
	{
		dominance = pDominance;
	}

	/// <summary>
	/// Returns a copy of this allele
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public abstract T GetCopy(float mutationFactor = 0, float mutationChance = 0);

	/// <summary>
	/// Performs mutation on the allele depending on the mutationFactor and mutationChance
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	public virtual void Mutate(float mutationFactor, float mutationChance) { }
}

/// <summary>
/// Allele dominance type
/// </summary>
public enum Dominance
{
	DOMINANT,
	SEMIDOMINANT,
	RECESSIVE
}