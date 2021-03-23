using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{


	public List<GameObject> tiles;
	public GameObject Grass;
	public GameObject Water;
	public List<GameObject> borderTiles;

	public int mapWidth = 10;
	float tileOffsetX;
	float tileOffsetZ;
	float mapOffsetX;
	float mapOffsetZ;

	[Range(5.0f, 25.0f)]
	public float water;

	public void Generate()
	{
		if (tiles.Count != 0)
			ClearTiles();
		tiles = new List<GameObject>();
		tileOffsetX = Grass.transform.localScale.x;
		tileOffsetZ = tileOffsetX / Mathf.Sqrt(3) * 1.5f;

		mapOffsetX = -tileOffsetX * (mapWidth - 1) / 2;
		mapOffsetZ = -tileOffsetZ * (mapWidth - 1) / 2;
		Vector3 position;

		for (int x = 0; x < mapWidth; x++)
		{
			for (int z = 0; z < mapWidth; z++)
			{
				if (z % 2 == 0)
					position = new Vector3(x * tileOffsetX + mapOffsetX, 0, z * tileOffsetZ + mapOffsetZ);
				else
					position = new Vector3(x * tileOffsetX + tileOffsetX / 2 + mapOffsetX, 0, z * tileOffsetZ + mapOffsetZ);

				if (Vector3.Distance(Vector3.zero, position) < 80)
				{
					if (Random.Range(0.0f, 1.0f) > water / 100.0f)
						CreateTile(Grass, position, Quaternion.identity, transform);
					else
						CreateTile(Water, position, Quaternion.identity, transform);
				}
				else if (Vector3.Distance(Vector3.zero, position) < 100)
				{
					CreateTile(borderTiles[Random.Range(0, borderTiles.Count)], new Vector3(position.x, -1.8f, position.z), Quaternion.identity, transform);
				}
				else
					continue;
			}
		}

		//CombineMesh();
	}

	public void CreateTile(GameObject tileType, Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject temp = Instantiate(tileType);
		temp.transform.position = position;
		temp.transform.rotation = rotation;
		temp.transform.parent = parent;
		tiles.Add(temp);
	}

	public void ClearTiles()
	{
		foreach (GameObject o in tiles)
		{
			DestroyImmediate(o, true);
		}

		tiles.Clear();
	}
}
