using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    // Cache the bounds of the ground plane
    private Bounds bounds;

    [SerializeField] private float spawnInterval = 5.0f;
    private float spawnRate;

    public static ObjectPooler resourcePooler;

    void Start()
    {
        bounds = GameObject.FindGameObjectWithTag("Ground").gameObject.GetComponent<MeshCollider>().bounds;
        resourcePooler = gameObject.GetComponent<ObjectPooler>();
        spawnRate = spawnInterval;
    }

    void Update()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate < 0.0f)
        {
            resourcePooler.SpawnFromPool("Food", GetRandomPosition(10.0f), Quaternion.identity);
            spawnRate = spawnInterval; // Reset spawn timer
        }
    }

    /// <summary>
    /// Returns a random position within the bounds of the ground
    /// </summary>
    /// <param name="margin"></param>
    /// <returns></returns>
	private Vector3 GetRandomPosition(float margin)
    {
        float xRandom = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        float zRandom = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        return new Vector3(xRandom, 0.5f, zRandom);
    }
}