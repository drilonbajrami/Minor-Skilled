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
            ColorGene color = new ColorGene(new ColorAllele(CLeft.colorAllele.Dominance, CLeft.colorAllele.Color), new ColorAllele(CRight.colorAllele.Dominance, CRight.colorAllele.Color));
            SizeGene size = new SizeGene(new SizeAllele(SLeft.sizeAllele.Dominance, SLeft.sizeAllele.Size), new SizeAllele(SRight.sizeAllele.Dominance, SRight.sizeAllele.Size));
            genome = new Genome(color, size);
            genome.ExpressGenome(this);
            colorLeft = new Color((genome.Color.AlleleLeft as ColorAllele).Color.r, (genome.Color.AlleleLeft as ColorAllele).Color.g, (genome.Color.AlleleLeft as ColorAllele).Color.b);
            colorRight = new Color((genome.Color.AlleleRight as ColorAllele).Color.r, (genome.Color.AlleleRight as ColorAllele).Color.g, (genome.Color.AlleleRight as ColorAllele).Color.b);
            sizeLeft = (genome.Size.AlleleLeft as SizeAllele).Size;
            sizeRight = (genome.Size.AlleleRight as SizeAllele).Size;
        }
    }

	public void UpdateGenomeInfo()
	{
        colorLeft = new Color((genome.Color.AlleleLeft as ColorAllele).Color.r, (genome.Color.AlleleLeft as ColorAllele).Color.g, (genome.Color.AlleleLeft as ColorAllele).Color.b);
        colorRight = new Color((genome.Color.AlleleRight as ColorAllele).Color.r, (genome.Color.AlleleRight as ColorAllele).Color.g, (genome.Color.AlleleRight as ColorAllele).Color.b);
        sizeLeft = (genome.Size.AlleleLeft as SizeAllele).Size;
        sizeRight = (genome.Size.AlleleRight as SizeAllele).Size;
	}
}