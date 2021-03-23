using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		TerrainGenerator terrainGen = (TerrainGenerator)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			terrainGen.Generate();
		}
	}
}
