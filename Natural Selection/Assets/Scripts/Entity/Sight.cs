using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : MonoBehaviour
{
	Dictionary<int, MemoryData> _objectsWithinSight;

	private Memory memory;
	private float refreshTimer;
	private float refreshInterval = 5.0f;

	void Start()
	{
		_objectsWithinSight = new Dictionary<int, MemoryData>();
		memory = gameObject.GetComponentInParent<Memory>();
		refreshTimer = refreshInterval;
	}

	private void Update()
	{
		RefreshSight();
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

	private void RefreshSight()
	{
		refreshTimer -= Time.deltaTime;
		if (refreshTimer <= 0.0f)
		{
			List<int> toRemove = new List<int>();

			// Store keys of pairs whose value's object is missing
			foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
				if (o.Value.IsObjectMissing() || !o.Value.Object.activeSelf)
					toRemove.Add(o.Key);

			// Remove all pairs whose value contains a null reference
			for (int i = 0; i < toRemove.Count; i++)
				_objectsWithinSight.Remove(toRemove[i]);
			
			refreshTimer = refreshInterval;
		}
	}

	//====================================================================================================
	//											TRIGGERS
	//====================================================================================================
	private void OnTriggerEnter(Collider other)
	{
		if (memory != null)
		{
			if (other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Plant"))
			{
				memory.RegisterResourceToMemory(other.gameObject);
				See(other.gameObject);
			}

			if (other.gameObject.CompareTag("Entity"))
			{
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
			other.gameObject.GetComponent<Entity>().Death -= gameObject.transform.parent.gameObject.GetComponent<Entity>().OnOtherDeath;
		}
	}
}
