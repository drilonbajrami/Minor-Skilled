using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStartSpawner : MonoBehaviour
{
    public GameObject entityPrefab;
    private List<GameObject> entities;

    private Bounds bounds;
    private List<Vector3> usedPositions;

    void Start()
    {
        bounds = GameObject.FindGameObjectWithTag("Ground").gameObject.GetComponent<MeshCollider>().bounds;
        entities = new List<GameObject>();
        usedPositions = new List<Vector3>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
            entities.Add(gameObject.transform.GetChild(i).gameObject);

        foreach (GameObject e in entities)
        {
            e.transform.position = GetRandomPosition(10.0f);
            e.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f);
        }
    }

    public GameObject CreateNewEntity(Order order)
    {
        if (order == Order.CARNIVORE)
            entityPrefab.GetComponent<Entity>().order = Order.CARNIVORE;
        else
            entityPrefab.GetComponent<Entity>().order = Order.HERBIVORE;

        GameObject entity = Instantiate(entityPrefab);
        return entity;
    }

    private Vector3 GetRandomPosition(float margin)
    {
        Vector3 position = new Vector3();
        position.x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
        position.z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
        position.y = 0.5f;

        if (usedPositions.Count != 0)
        {
            foreach (Vector3 usedPos in usedPositions)
            {
                if (Vector3.Distance(usedPos, position) < 2.0f)
                {
                    position = GetRandomPosition(margin);
                }
            }
        }

        return position;
    }
}
