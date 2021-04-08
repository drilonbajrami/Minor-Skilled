using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
	[SerializeField] List<GameObject> _resources;

	private int _memoryCapacity;

	private float _currentMemorySpan;
	private float _memorySpan = 5.0f;

	private float _updateInterval = 2f;
	private float _interval;

	private void Start()
	{
		_resources = new List<GameObject>();
		_currentMemorySpan = _memorySpan;
		_interval = _updateInterval;
		_memoryCapacity = 20;
	}

	private void Update()
	{
		//ForgetResource();	

		_interval -= Time.deltaTime;

		ForgetWithTime();

		//if (_interval <= 0.0f)
		//{
		//	UpdateResources();
		//	_interval = _updateInterval;
		//}
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

		if (_resources.Count != 0)
		{
			foreach (GameObject resource in _resources)
			{
				if (/*resource != null && */resource.GetComponent<Resource>().GetResourceType() == resourceType)
				{
					float distance = Vector3.SqrMagnitude(gameObject.transform.position - resource.gameObject.transform.position);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestResource = resource;
					}
				}
			}
		}

		return closestResource;
	}

	/// <summary>
	/// Entity will register resource to its memory
	/// </summary>
	/// <param name="resource"></param>
	public void RegisterResourceToMemory(GameObject resource)
	{
		if (resource != null)
		{
			if (!resource.GetComponent<Resource>().IsConsumed())
			{
				if (!RemembersResource(resource))
				{
					if (_resources.Count >= _memoryCapacity)
					{
						_resources.RemoveAt(0);
						_resources.Add(resource);
					}
					else
					{
						_resources.Add(resource);
					}
				}
			}
		}
	}

	/// <summary>
	/// Checks if the entity remembers the resource or not
	/// </summary>
	/// <param name="resource"></param>
	/// <returns></returns>
	private bool RemembersResource(GameObject resource)
	{
		if (_resources.Count == 0)
			return false;
		else 
			return _resources.Any(r => r.GetInstanceID() == resource.GetInstanceID());
	}

	/// <summary>
	/// If resource is already consumed then forget,
	/// otherwise forget after (_memorySpan) time.
	/// </summary>
	private void ForgetWithTime()
	{
		_currentMemorySpan -= Time.deltaTime;
		if (_currentMemorySpan <= 0.0f)
		{
			if (_resources.Count != 0)
				_resources.RemoveAt(0);
			_currentMemorySpan = _memorySpan;
		}
	}

	/// <summary>
	/// If resource has already been consumed then forget about it.
	/// </summary>
	/// <param name="pResource"></param>
	public void ForgetResource(GameObject pResource)
	{
		int index = _resources.FindIndex(r => r.GetInstanceID() == pResource.GetInstanceID());
		if (index != -1) _resources.RemoveAt(index);
	}

	public bool KnowsAboutResource(ResourceType resourceType)
	{
		return _resources.Any(r => r.GetComponent<Resource>().GetResourceType() == resourceType);
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