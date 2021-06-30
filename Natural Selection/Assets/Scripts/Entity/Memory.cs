using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
	private Entity entity;
	private Dictionary<int, MemoryData> _resources;
	private int _memoryCapacity = 20;

	public void OnEnable()
	{
		entity = gameObject.GetComponent<Entity>();
		_resources = new Dictionary<int, MemoryData>();
		StartCoroutine(RefreshMemoryCoroutine());
	}

	/// <summary>
	/// Depending on which type to look for, the entity will find the closest resource of that type
	/// </summary>
	/// <param name="resourceType"></param>
	/// <returns></returns>
	public GameObject FindClosestResource(ResourceType resourceType)
	{
		if (_resources.Count == 0)
			return null;

		GameObject closestResource = null;
		float closestDistance = Mathf.Infinity;

		foreach (KeyValuePair<int, MemoryData> resource in _resources)
		{
			if (resource.Value.Object.GetComponent<Resource>().GetResourceType() == resourceType)
			{
				float distance = Vector3.SqrMagnitude(gameObject.transform.position - resource.Value.LastKnownPosition);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestResource = resource.Value.Object;
				}
			}
		}

		return closestResource;
	}

	/// <summary>
	/// Entity will register resource to its memory
	/// </summary>
	/// <param name="gameObject"></param>
	public void MemorizeResource(GameObject gameObject)
	{
		if (gameObject == null || !gameObject.activeSelf)
			return;
		else if (!RemembersResource(gameObject) && gameObject.activeSelf && _resources.Count <= _memoryCapacity)
			_resources.Add(gameObject.GetInstanceID(), new MemoryData(gameObject));
	}

	/// <summary>
	/// Checks if the entity remembers the resource or not
	/// </summary>
	/// <param name="resource"></param>
	/// <returns></returns>
	private bool RemembersResource(GameObject resource)
	{
		if (resource == null) return false;
		return _resources.ContainsKey(resource.GetInstanceID());
	}

	/// <summary>
	/// If resource has already been consumed then forget about it.
	/// </summary>
	/// <param name="gameObject"></param>
	public void ForgetResource(GameObject gameObject)
	{
		if (gameObject != null && _resources.ContainsKey(gameObject.GetInstanceID()))
			_resources.Remove(gameObject.GetInstanceID());
	}

	public bool KnowsAboutResource(ResourceType resourceType)
	{
		return _resources.Any(o => o.Value.Object.GetComponent<Resource>().GetResourceType() == resourceType);
	}

	/// <summary>
	/// Memory refresh coroutine
	/// </summary>
	/// <returns></returns>
	private IEnumerator RefreshMemoryCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(entity.SenseRefreshInterval);
			RefreshMemory();
		}
	}

	/// <summary>
	/// Forget oldest memory and clean any memories that point to non-existing objects
	/// </summary>
	private void RefreshMemory()
	{
		if (_resources.Count == 0)
			return;

		// Forget oldest memory
		_resources.Remove(_resources.First().Key);

		List<int> toRemove = new List<int>();
		// Store keys of pairs whose value's object is missing
		foreach (KeyValuePair<int, MemoryData> o in _resources)
			if (o.Value.ObjectNoLongerExists() || !o.Value.Object.activeSelf)
				toRemove.Add(o.Key);

		// Remove all pairs whose value contains a null reference
		foreach (int i in toRemove)
			_resources.Remove(i);
	}
}