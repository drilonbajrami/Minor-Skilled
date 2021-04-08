using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    /*[SerializeField]*/ private GameObject ground;
    private MeshCollider groundCollider;

    [SerializeField] private float spawnInterval = 5.0f;
    private float spawnRate;

    private ResourcePooler resourcePooler;

    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        groundCollider = ground.GetComponent<MeshCollider>();
        resourcePooler = gameObject.GetComponent<ResourcePooler>();
        spawnRate = spawnInterval;
    }

    void Update()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate < 0.0f)
        {
            int i = Random.Range(0, 2);
            if(i == 0)
                resourcePooler.SpawnFromPool("Water", GetRandomPosition(), Quaternion.identity);
            else
                resourcePooler.SpawnFromPool("Plant", GetRandomPosition(), Quaternion.identity);
        }
    }

	private Vector3 GetRandomPosition()
    {
        float xRandom = Random.Range(groundCollider.bounds.min.x + 10.0f, groundCollider.bounds.max.x - 10.0f);
        float zRandom = Random.Range(groundCollider.bounds.min.z + 10.0f, groundCollider.bounds.max.z - 10.0f);
        return new Vector3(xRandom, 0.5f, zRandom);
    }
}
