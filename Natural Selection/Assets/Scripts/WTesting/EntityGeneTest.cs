using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityGeneTest : MonoBehaviour
{
    [SerializeField] ColorAlleleSO CLeft;
    [SerializeField] ColorAlleleSO CRight;
    [SerializeField] SizeAlleleSO SLeft;
    [SerializeField] SizeAlleleSO SRight;

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
            ColorGene color = new ColorGene(new ColorAllele(CLeft.colorAllele.Dominance, CLeft.colorAllele.Color), new ColorAllele(CRight.colorAllele.Dominance, CRight.colorAllele.Color));
            SizeGene size = new SizeGene(new SizeAllele(SLeft.sizeAllele.Dominance, SLeft.sizeAllele.Size), new SizeAllele(SRight.sizeAllele.Dominance, SRight.sizeAllele.Size));

            // Create the new genome with the given genes
            //genome = new Genome(color, size);
            //genome.ExpressGenome(this);

            // This is for debugging in inspector
			colorLeft = new Color(genome.Color.AlleleA.Color.r, genome.Color.AlleleA.Color.g, genome.Color.AlleleA.Color.b);
			colorRight = new Color(genome.Color.AlleleB.Color.r, genome.Color.AlleleB.Color.g, genome.Color.AlleleB.Color.b);
			sizeLeft = genome.Size.AlleleA.Size;
			sizeRight = genome.Size.AlleleB.Size;
		}
    }

	public void UpdateGenomeInfo()
	{
        // Used for debugging in inspector
		colorLeft = new Color(genome.Color.AlleleA.Color.r, genome.Color.AlleleA.Color.g, genome.Color.AlleleA.Color.b);
		colorRight = new Color(genome.Color.AlleleB.Color.r, genome.Color.AlleleB.Color.g, genome.Color.AlleleB.Color.b);
		sizeLeft = genome.Size.AlleleA.Size;
		sizeRight = genome.Size.AlleleB.Size;
	}
}