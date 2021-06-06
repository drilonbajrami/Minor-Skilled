using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Specie", menuName = "Specie", order = 1)]
public class Specie : ScriptableObject
{
    public string Name;
    public Order Order;

    [Header("Number of entities")]
    public int total;
    public int females;
    public int males;

    [Header("Alleles")]
    public List<SexAlleleSO>      sexAlleles;
    public List<ColorAlleleSO>    colorAlleles;
    public List<HeightAlleleSO>   heightAlleles;
    public List<SpeedAlleleSO>    speedAlleles;
    public List<BehaviorAlleleSO> behaviorAlleles;

    private int numE;
    private int numF;
    private int numM;

    [HideInInspector] public int numberOfSexAlleles;
    [HideInInspector] public int numberOfColorAlleles;
    [HideInInspector] public int numberOfHeightAlleles;
    [HideInInspector] public int numberOfSpeedAlleles;
    [HideInInspector] public int numberOfBehaviorAlleles;

    private void OnValidate()
    {
        numberOfColorAlleles = colorAlleles.Count;
        numberOfSexAlleles = sexAlleles.Count;
        numberOfHeightAlleles = heightAlleles.Count;
        numberOfBehaviorAlleles = behaviorAlleles.Count;

        RecalculateSexRatios();

        if (Name != "")
        {
            string thisFileNewName = Name;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, thisFileNewName);
        }
    }

    private void RecalculateSexRatios()
    {
        if (total != numE)
        {
            females = total / 2;
            males = total - females;
            numE = total;
            numF = females;
            numM = males;
        }
        else if (females != numF)
        {
            females = Mathf.Clamp(females, 0, total);
            males = total - females;
            numM = males;
        }
        else if (males != numM)
        {
            males = Mathf.Clamp(males, 0, total);
            females = total - males;
            numF = females;
        }
    }
}