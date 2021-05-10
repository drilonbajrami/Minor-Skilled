using UnityEngine;

[CreateAssetMenu(fileName = "SizeAllele", menuName = "ScriptableObjects/SizeAllele", order = 1)]
public class SizeAlleleSO : ScriptableObject
{
    public string Name;
    public SizeAllele sizeAllele;

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