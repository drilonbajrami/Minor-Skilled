// Object Pooling taken from Brackeys on YouTube: https://www.youtube.com/watch?v=tdSmKaJvCoA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses object pooling for spawning objects
/// </summary>
public class ObjectPooler : MonoBehaviour
{
	/// <summary>
	/// Pool of objects
	/// </summary>
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

	/// <summary>
	/// Create the pool of objects on start
	/// </summary>
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

	/// <summary>
	/// Spawns an object from the object pool depending on the given tag, positoin and rotation
	/// </summary>
	/// <param name="tag"></param>
	/// <param name="position"></param>
	/// <param name="rotation"></param>
	/// <returns></returns>
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		if (poolDictionary[tag].Count != 0 && !poolDictionary[tag].Peek().activeSelf)
		{
			GameObject objectToSpawn = poolDictionary[tag].Dequeue();
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
			objectToSpawn.transform.parent = gameObject.transform;

			IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

			if (pooledObject != null)
				pooledObject.OnObjectSpawn();

			//poolDictionary[tag].Enqueue(objectToSpawn);
			return objectToSpawn;
		}
		else
		{
			return null;
		}
	}

	public void PoolObject(string tag, GameObject item)
	{
		poolDictionary[tag].Enqueue(item);
	}
}