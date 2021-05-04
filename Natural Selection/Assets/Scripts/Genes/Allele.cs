using UnityEngine;

/// <summary>
/// Allele is the fundamental part that contains genetic information for a specific trait
/// </summary>
[System.Serializable]
public abstract class Allele
{
	private string name;
	public string Name => name;

	[SerializeField] private Dominance dominance;
	public Dominance Dominance { get { return dominance; } }

	public Allele(Dominance pDominance)
	{
		dominance = pDominance;
	}

	public void SetName(string pName)
	{
		name = pName;
	}

	/// <summary>
	/// Returns a copy of this allele
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	/// <returns></returns>
	public abstract Allele GetCopy(float mutationFactor, float mutationChance);

	/// <summary>
	/// Performs mutation on the allele depending on the mutationFactor and mutationChance
	/// </summary>
	/// <param name="mutationFactor"></param>
	/// <param name="mutationChance"></param>
	public abstract void Mutate(float mutationFactor, float mutationChance);
}