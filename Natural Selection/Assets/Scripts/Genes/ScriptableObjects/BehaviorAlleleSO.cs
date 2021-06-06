using UnityEngine;

[CreateAssetMenu(fileName = "Behavior Allele", menuName = "Allele/Behavior", order = 3)]
public class BehaviorAlleleSO : ScriptableObject
{
    public string Name;
    public BehaviorAllele Allele	;

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
