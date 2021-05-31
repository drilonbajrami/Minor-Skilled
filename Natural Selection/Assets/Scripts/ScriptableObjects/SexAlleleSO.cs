using UnityEngine;

[CreateAssetMenu(fileName = "SexAllele", menuName = "ScriptableObjects/SexAllele", order = 1)]
public class SexAlleleSO : ScriptableObject
{
    public string Name;
    public SexAllele sexAllele;

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
