using UnityEngine;

[CreateAssetMenu(fileName = "Color Allele", menuName = "Allele/Color", order = 4)]
public class ColorAlleleSO : ScriptableObject
{
    public string Name;
    public ColorAllele Allele;

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