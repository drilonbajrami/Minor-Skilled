using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Specie", menuName = "ScriptableObjects/Specie", order = 1)]
public class Specie : ScriptableObject
{
    public string Name;
    public Order Order;
    public int instanceNumber;

    //public List<Gene> colorGenes;

    private void OnValidate()
    {
        if (Name != "")
        {
            string thisFileNewName = Name;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, thisFileNewName);
        }
    }
}