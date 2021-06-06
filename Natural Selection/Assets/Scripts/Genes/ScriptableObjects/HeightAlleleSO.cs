using UnityEngine;

[CreateAssetMenu(fileName = "Height Allele", menuName = "Allele/Height", order = 5)]
public class HeightAlleleSO : ScriptableObject
{
    public string Name;
    public HeightAllele Allele;

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