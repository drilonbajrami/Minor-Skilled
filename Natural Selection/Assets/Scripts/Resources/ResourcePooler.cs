using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

	private void Start()
	{
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.transform.parent = gameObject.transform;
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}

	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		if (!poolDictionary[tag].Peek().activeSelf)
		{
			GameObject objectToSpawn = poolDictionary[tag].Dequeue();
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
			objectToSpawn.transform.parent = gameObject.transform;

			IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

			if (pooledObject != null)
				pooledObject.OnObjectSpawn();

			poolDictionary[tag].Enqueue(objectToSpawn);

			return objectToSpawn;
		}
		else
			return null;
	}
}
