using UnityEngine;

[CreateAssetMenu(fileName = "BehaviorAllele", menuName = "ScriptableObjects/BehaviorAllele", order = 1)]
public class BehaviorAlleleSO : ScriptableObject
{
    public string Name;
    public BehaviorAllele behaviorAllele;

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
