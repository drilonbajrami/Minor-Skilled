using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    WATER,
    FOOD
}

public class Resource : MonoBehaviour, IPooledObject
{
    public ResourceType type;
    private bool _isConsumed;

    /// <summary>
    /// When dequeued from the object pool for spawning, reset everything for this object
    /// </summary>
    public void OnObjectSpawn()
    {
        _isConsumed = false;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.SetActive(true);
    }

	public ResourceType GetResourceType()
    {
        return type;
    }

    public bool IsConsumed()
    {
        return _isConsumed;
    }

    public void Consume()
    {
        gameObject.SetActive(false);
        ResourceSpawner.resourcePooler.PoolObject("Food", gameObject); 
    }
}
