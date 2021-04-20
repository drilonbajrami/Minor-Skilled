using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : MonoBehaviour
{
	Dictionary<int, MemoryData> _objectsWithinSight;
	[SerializeField] List<GameObject> _objects;
	private Memory memory;
	private float refreshTimer;
	private float refreshInterval = 5.0f;

	void Start()
	{
		_objectsWithinSight = new Dictionary<int, MemoryData>();
		_objects = new List<GameObject>();
		memory = gameObject.GetComponentInParent<Memory>();
		refreshTimer = refreshInterval;
	}

	private void Update()
	{
		_objects.Clear();
		foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
			_objects.Add(o.Value.Object);

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
		if (_objectsWithinSight.Count == 0 || (gameObject != null && !_objectsWithinSight.ContainsKey(gameObject.GetInstanceID())))
			_objectsWithinSight.Add(gameObject.GetInstanceID(), new MemoryData(gameObject));
	}

	public void Unsee(GameObject gameObject)
	{
		if (_objectsWithinSight.Count != 0 && gameObject != null)
			_objectsWithinSight.Remove(gameObject.GetInstanceID());
	}

	private void RefreshSight()
	{
		refreshTimer -= Time.deltaTime;
		if (refreshTimer <= 0.0f)
		{
			List<int> toRemove = new List<int>();
			foreach (KeyValuePair<int, MemoryData> o in _objectsWithinSight)
				if (o.Value.isObjectMissing() || !o.Value.Object.activeSelf)
					toRemove.Add(o.Key);

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
			if ((other.gameObject.tag == "Water" || other.gameObject.tag == "Plant"))
			{
				memory.RegisterResourceToMemory(other.gameObject);
				See(other.gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != null)
			Unsee(other.gameObject);
	}

	//private void OnTriggerStay(Collider other)
	//{
	//	Resource rsc = other.gameObject.GetComponent<Resource>();
	//	if (rsc != null)
	//		if (rsc.IsConsumed())
	//			Unsee(other.gameObject);
	//}
}
