using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPosition : MonoBehaviour
{
    private List<GameObject> entities;

    private GameObject ground;
    private MeshCollider groundCollider;

    List<Vector3> usedPositions;

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        groundCollider = ground.GetComponent<MeshCollider>();
        entities = new List<GameObject>();
        usedPositions = new List<Vector3>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
            entities.Add(gameObject.transform.GetChild(i).gameObject);

        foreach (GameObject e in entities)
            e.transform.position = GetRandomPosition();
    }

    public GameObject CreateNewEntity(Order order)
    {
        if (order == Order.CARNIVORE)
            prefab.GetComponent<Entity>().order = Order.CARNIVORE;
        else
            prefab.GetComponent<Entity>().order = Order.HERBIVORE;

        GameObject entity = Instantiate(prefab);
        return entity;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3();
        position.x = Random.Range(groundCollider.bounds.min.x + 10.0f, groundCollider.bounds.max.x - 10.0f);
        position.z = Random.Range(groundCollider.bounds.min.z + 10.0f, groundCollider.bounds.max.z - 10.0f);
        position.y = 0.5f;

        if (usedPositions.Count != 0)
        {
            foreach (Vector3 usedPos in usedPositions)
            {
                if (Vector3.Distance(usedPos, position) < 2.0f)
                {
                    position = GetRandomPosition();
                }
            }
        }

        return position;
    }
}
