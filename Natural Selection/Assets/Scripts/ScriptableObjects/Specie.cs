using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Specie", menuName = "ScriptableObjects/Specie", order = 1)]
public class Specie : ScriptableObject
{
    public string Name;

    public Order Order;
    [HideInInspector] public int instanceNumber;
    [HideInInspector] public int numberOfColorAlleles;
    [HideInInspector] public int numberOfSexAlleles;
    [HideInInspector] public int numberOfSizeAlleles;
    [HideInInspector] public int numberOfBehaviorAlleles;

    [Header("Number of entities")]
    public int numberOfEntities;
    [Space(10.0f)]
    public int numberOfFemales;
    public int numberOfMales;

    [Header("Color Alleles")]
    public List<ColorAlleleSO> colorAlleles;

    [Header("Sex Alleles")]
    public List<SexAlleleSO> sexAlleles;

    [Header("Size Alleles")]
    public List<SizeAlleleSO> sizeAlleles;

    [Header("Behavior Alleles")]
    public List<BehaviorAlleleSO> behaviorAlleles;

    private int numE;
    private int numF;
    private int numM;

    private void OnValidate()
    {
        numberOfColorAlleles = colorAlleles.Count;
        numberOfSexAlleles = sexAlleles.Count;
        numberOfSizeAlleles = sizeAlleles.Count;
        numberOfBehaviorAlleles = behaviorAlleles.Count;

        if (numberOfEntities != numE)
        {
            numberOfFemales = numberOfEntities / 2;
            numberOfMales = numberOfEntities - numberOfFemales;
            numF = numberOfFemales;
            numM = numberOfMales;
        }
        else if (numberOfFemales != numF)
        {
            numberOfFemales = Mathf.Clamp(numberOfFemales, 0, numberOfEntities);
            numberOfMales = Mathf.Clamp(numberOfMales, 0, numberOfEntities);
            numberOfMales = numberOfEntities - numberOfFemales;
            numM = numberOfMales;
        }
        else if (numberOfMales != numM)
        {
            numberOfFemales = Mathf.Clamp(numberOfFemales, 0, numberOfEntities);
            numberOfMales = Mathf.Clamp(numberOfMales, 0, numberOfEntities);
            numberOfFemales = numberOfEntities - numberOfMales;
            numF = numberOfFemales;
        }


        numE = numberOfEntities;
        numF = numberOfFemales;
        numM = numberOfMales;

        if (Name != "")
        {
            string thisFileNewName = Name;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, thisFileNewName);
        }
    }
}