using UnityEngine;

[CreateAssetMenu(fileName = "ColorAllele", menuName = "ScriptableObjects/ColorAllele", order = 1)]
public class ColorAlleleSO : ScriptableObject
{
    public string Name;
    public ColorAllele colorAllele;

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