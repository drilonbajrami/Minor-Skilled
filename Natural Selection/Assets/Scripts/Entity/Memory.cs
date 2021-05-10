using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
	Dictionary<int, MemoryData> _resourcesInMemory;

	private int _memoryCapacity;

	//private float _currentMemorySpan;
	//private float _memorySpan = 5.0f;

	private float _updateInterval = 2f;
	private float _interval;

	private void Start()
	{
		_resourcesInMemory = new Dictionary<int, MemoryData>();
		_interval = _updateInterval;
		_memoryCapacity = 20;
	}

	private void Update()
	{
		_interval -= Time.deltaTime;
	}

	/// <summary>
	/// Depending on which type to look for, the entity
	/// will find the closest resource of that type
	/// </summary>
	/// <param name="resourceType"></param>
	/// <returns></returns>
	public GameObject FindClosestResource(ResourceType resourceType)
	{
		GameObject closestResource = null;
		float closestDistance = Mathf.Infinity;

		if (_resourcesInMemory.Count != 0)
		{
			foreach (KeyValuePair<int, MemoryData> resource in _resourcesInMemory)
			{
				if (resource.Value.ObjectInMemory.GetComponent<Resource>().GetResourceType() == resourceType)
				{
					float distance = Vector3.SqrMagnitude(gameObject.transform.position - resource.Value.LastKnownPosition);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestResource = resource.Value.ObjectInMemory;
					}
				}
			}
		}

		return closestResource;
	}

	/// <summary>
	/// Entity will register resource to its memory
	/// </summary>
	/// <param name="gameObject"></param>
	public void RegisterResourceToMemory(GameObject gameObject)
	{
		if (gameObject != null)
			if (!gameObject.GetComponent<Resource>().IsConsumed())
				if (!RemembersResource(gameObject))
					if (_resourcesInMemory.Count <= _memoryCapacity)
						_resourcesInMemory.Add(gameObject.GetInstanceID(), new MemoryData(gameObject));
	}

	/// <summary>
	/// Checks if the entity remembers the resource or not
	/// </summary>
	/// <param name="resource"></param>
	/// <returns></returns>
	private bool RemembersResource(GameObject resource)
	{
		if (_resourcesInMemory.Count == 0)
			return false;
		else
			return _resourcesInMemory.ContainsKey(resource.GetInstanceID());
	}

	/// <summary>
	/// If resource is already consumed then forget,
	/// otherwise forget after (_memorySpan) time.
	/// </summary>
	//private void ForgetWithTime()
	//{
	//	_currentMemorySpan -= Time.deltaTime;
	//	if (_currentMemorySpan <= 0.0f)
	//	{
	//		if (_resources.Count != 0)
	//			_resources.RemoveAt(0);
	//		_currentMemorySpan = _memorySpan;
	//	}
	//}

	/// <summary>
	/// If resource has already been consumed then forget about it.
	/// </summary>
	/// <param name="gameObject"></param>
	public void ForgetResource(GameObject gameObject)
	{
		if (gameObject != null && _resourcesInMemory.ContainsKey(gameObject.GetInstanceID()))
				_resourcesInMemory.Remove(gameObject.GetInstanceID());
	}

	public bool KnowsAboutResource(ResourceType resourceType)
	{
		return _resourcesInMemory.Any(o => o.Value.ObjectInMemory.GetComponent<Resource>().GetResourceType() == resourceType);
	}

	//private void UpdateResources()
	//{
	//	for (int i = 0; i < _resources.Count; i++)
	//	{
	//		if (_resources[i].GetComponent<Resource>().IsConsumed())
	//			_resources.RemoveAt(i);
	//	}
	//}
}