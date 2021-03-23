using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    private MeshCollider groundCollider;

    [SerializeField] private float spawnInterval = 5.0f;
    private float spawnRate;

    [Space(20)]
    [SerializeField] private List<GameObject> resources = new List<GameObject>();
    
    void Start()
    {
        groundCollider = ground.GetComponent<MeshCollider>();
        spawnRate = spawnInterval;

        Time.timeScale = 2.0f;
    }

    void Update()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate < 0.0f)
        {
            GameObject temp = Instantiate(resources[Random.Range(0, resources.Count)]);

            Vector3 position = GetRandomPosition();
            position.y = temp.transform.localScale.y / 2;

            temp.transform.position = position;
            temp.transform.SetParent(transform);
            spawnRate = spawnInterval;
        }

    }

	private Vector3 GetRandomPosition()
    {
        float xRandom = Random.Range(groundCollider.bounds.min.x + 10.0f, groundCollider.bounds.max.x - 10.0f);
        float zRandom = Random.Range(groundCollider.bounds.min.z + 10.0f, groundCollider.bounds.max.z - 10.0f);
        return new Vector3(xRandom, 0.0f, zRandom); ;
    }
}
