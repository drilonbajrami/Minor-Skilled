using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
	private Entity entity;
	Dictionary<int, MemoryData> _objectsWithinSight;

	public SightArea sightArea;
	public PieGraph pieGraph;

	void Start()
	{
		entity = gameObject.GetComponentInParent<Entity>();
		_objectsWithinSight = new Dictionary<int, MemoryData>();
		sightArea = new SightArea(12);
		StartCoroutine(RefreshSightCoroutine());

		if (pieGraph != null)
			pieGraph.CreatePieGraph(sightArea, gameObject.transform.parent.transform);
	}

	public GameObject ChoosePrey()
	{
		if (_objectsWithinSight.Count == 0)
			return null;

		GameObject toReturn = null;
		foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
		{
			if (o.Value.ObjectNoLongerExists())
				continue;

			Entity e = o.Value.ObjectInMemory.GetComponent<Entity>();
			if (e.order == Order.CARNIVORE)
				continue;
			else
			{
				toReturn = o.Value.ObjectInMemory;
			}
		}

		return toReturn;
	}

	public float EvaluateUtilities()
	{
		// Get list of gameObjects
		List<GameObject> list = new List<GameObject>();
		foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
		{
			if (o.Value.ObjectInMemory == null)
				continue;

			list.Add(o.Value.ObjectInMemory);
		}

		if (list.Count == 0)
			return 0.0f;

		float bestAngle = sightArea.CalculateUtilityValues(gameObject.transform.parent.gameObject.GetComponent<Entity>(), list);

		if (pieGraph != null)
		{
			for (int i = 0; i < sightArea.sections.Length; i++)
			{
				pieGraph.UpdateSectionUtilityColor(i, sightArea.sections[i].UtilityValue);
			}
		}

		return bestAngle;
	}

	public bool CanSee(GameObject gameObject)
	{
		if (gameObject != null)
			return _objectsWithinSight.ContainsKey(gameObject.GetInstanceID());
		else
			return false;
	}

	public void See(GameObject gameObject)
	{
		if (gameObject == null)
			return;
		else if (!_objectsWithinSight.ContainsKey(gameObject.GetInstanceID()))
			_objectsWithinSight.Add(gameObject.GetInstanceID(), new MemoryData(gameObject));
	}

	public void Unsee(GameObject gameObject)
	{
		if (gameObject == null)
			return;
		else if (_objectsWithinSight.Count != 0)
			_objectsWithinSight.Remove(gameObject.GetInstanceID());
	}

	/// <summary>
	/// Sight refresh coroutine
	/// </summary>
	/// <returns></returns>
	private IEnumerator RefreshSightCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(entity.SenseRefreshInterval);
			RefreshSight();
		}
	}

	/// <summary>
	/// Clean sight of missing objects
	/// </summary>
	private void RefreshSight()
	{
		List<int> toRemove = new List<int>();

		// Store keys of pairs whose value's object is missing
		foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
			if (o.Value.ObjectNoLongerExists() || !o.Value.ObjectInMemory.activeSelf)
				toRemove.Add(o.Key);

		// Remove all pairs whose value contains a null reference
		for (int i = 0; i < toRemove.Count; i++)
			_objectsWithinSight.Remove(toRemove[i]);
	}

	//====================================================================================================
	//											TRIGGERS
	//====================================================================================================
	private void OnTriggerEnter(Collider other)
	{
		if (entity.Memory != null)
		{
			if (other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Plant"))
			{
				entity.Memory.MemorizeResource(other.gameObject);
				See(other.gameObject);
			}

			if (other.gameObject.CompareTag("Entity"))
			{
				See(other.gameObject);
				other.gameObject.GetComponent<Entity>().Death += gameObject.transform.parent.gameObject.GetComponent<Entity>().OnOtherDeath;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != null)
		{
			Unsee(other.gameObject);
		}

		if (other.gameObject.CompareTag("Entity"))
		{
			Unsee(other.gameObject);
			other.gameObject.GetComponent<Entity>().Death -= gameObject.transform.parent.gameObject.GetComponent<Entity>().OnOtherDeath;
		}
	}
}
