using UnityEngine;

[CreateAssetMenu(fileName = "Speed Allele", menuName = "Allele/Speed", order = 6)]
public class SpeedAlleleSO : ScriptableObject
{
    public string Name;
    public SpeedAllele Allele;

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