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
    [SerializeField] ResourceType type;
    private bool _isConsumed;
    private float onConsumeTimer = 10.0f;

    public void OnObjectSpawn()
    {
        gameObject.SetActive(true);
        _isConsumed = false;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
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
        _isConsumed = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

	public void Update()
	{
		if (_isConsumed)
		{
			onConsumeTimer -= Time.deltaTime;
			if (onConsumeTimer <= 0.0f)
				gameObject.SetActive(false);
		}
	}
}
