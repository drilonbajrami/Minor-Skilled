using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityGeneTest : MonoBehaviour
{
    [SerializeField] ColorAlleleSO CLeft;
    [SerializeField] ColorAlleleSO CRight;
    [SerializeField] HeightAlleleSO SLeft;
    [SerializeField] HeightAlleleSO SRight;

    public Color colorLeft;
    public Color colorRight;
    public float sizeLeft;
    public float sizeRight;

    public Genome genome;

    void Start()
    {
        if (CLeft != null && CRight != null)
        {
            // Create the genes for the first generation
            ColorGene color = new ColorGene(new ColorAllele(CLeft.Allele.Dominance, CLeft.Allele.Color), new ColorAllele(CRight.Allele.Dominance, CRight.Allele.Color));
            HeightGene size = new HeightGene(new HeightAllele(SLeft.Allele.Dominance, SLeft.Allele.Height), new HeightAllele(SRight.Allele.Dominance, SRight.Allele.Height));

            // Create the new genome with the given genes
            //genome = new Genome(color, size);
            //genome.ExpressGenome(this);

            // This is for debugging in inspector
			colorLeft = new Color(genome.Color.AlleleA.Color.r, genome.Color.AlleleA.Color.g, genome.Color.AlleleA.Color.b);
			colorRight = new Color(genome.Color.AlleleB.Color.r, genome.Color.AlleleB.Color.g, genome.Color.AlleleB.Color.b);
			sizeLeft = genome.Height.AlleleA.Height;
			sizeRight = genome.Height.AlleleB.Height;
		}
    }

	public void UpdateGenomeInfo()
	{
        // Used for debugging in inspector
		colorLeft = new Color(genome.Color.AlleleA.Color.r, genome.Color.AlleleA.Color.g, genome.Color.AlleleA.Color.b);
		colorRight = new Color(genome.Color.AlleleB.Color.r, genome.Color.AlleleB.Color.g, genome.Color.AlleleB.Color.b);
		sizeLeft = genome.Height.AlleleA.Height;
		sizeRight = genome.Height.AlleleB.Height;
	}
}