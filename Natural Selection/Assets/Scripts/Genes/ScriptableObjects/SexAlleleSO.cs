using UnityEngine;

[CreateAssetMenu(fileName = "Sex Allele", menuName = "Allele/Sex", order = 2)]
public class SexAlleleSO : ScriptableObject
{
    public string Name;
    public SexAllele Allele;

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
