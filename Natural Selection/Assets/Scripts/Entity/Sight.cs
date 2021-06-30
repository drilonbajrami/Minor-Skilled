using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
	private Entity entity;
	Dictionary<int, MemoryData> _objectsInSight = new Dictionary<int, MemoryData>();

	public SightArea SightArea;
	public PieGraph pieGraph;

	private void OnEnable()
	{
		entity = gameObject.GetComponentInParent<Entity>();
		_objectsInSight.Clear();
		SightArea = new SightArea(12);
		StartCoroutine(RefreshSightCoroutine());

		if (pieGraph != null)
			pieGraph.CreatePieGraph(SightArea, entity);
	}

	/// <summary>
	/// Assess all sight area sections utility values
	/// </summary>
	/// <returns></returns>
	public void AssessUtilities()
	{
		if (_objectsInSight.Count == 0)
			return;

		SightArea.AssessUtilityValues(entity, _objectsInSight);

		if (pieGraph != null)
		{
			for (int i = 0; i < SightArea.sections.Length; i++)
			{
				pieGraph.UpdateSectionUtilityColor(i, SightArea.sections[i].UtilityValue);
			}
			pieGraph.UpdateRotation();
		}
	}

	/// <summary>
	/// Returns true if the object is in sight otherwise false if not.
	/// </summary>
	/// <param name="gameObject"></param>
	/// <returns></returns>
	public bool CanSee(GameObject gameObject)
	{
		if (gameObject == null)	return false;
		else return _objectsInSight.ContainsKey(gameObject.GetInstanceID());
	}

	/// <summary>
	/// Registers object to entity's sight.
	/// </summary>
	/// <param name="gameObject"></param>
	public void See(GameObject gameObject)
	{
		if (gameObject == null) return;
		else if (!_objectsInSight.ContainsKey(gameObject.GetInstanceID()))
			_objectsInSight.Add(gameObject.GetInstanceID(), new MemoryData(gameObject));
	}

	/// <summary>
	/// Deregisters object from entity's sight.
	/// </summary>
	/// <param name="gameObject"></param>
	public void Unsee(GameObject gameObject)
	{
		if (gameObject == null) return;
		else if (_objectsInSight.Count != 0) _objectsInSight.Remove(gameObject.GetInstanceID());
	}

	/// <summary>
	/// Sight refresh coroutine
	/// </summary>
	/// <returns></returns>
	private IEnumerator RefreshSightCoroutine()
	{
		while (true) {
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
		foreach (KeyValuePair<int, MemoryData> o in _objectsInSight)
			if (o.Value.ObjectNoLongerExists() || !o.Value.Object.activeSelf)
				toRemove.Add(o.Key);

		// Remove all pairs whose value contains a null reference
		for (int i = 0; i < toRemove.Count; i++)
			_objectsInSight.Remove(toRemove[i]);
	}

	//====================================================================================================
	//											TRIGGERS
	//====================================================================================================
	private void OnTriggerEnter(Collider other)
	{
		if (entity.Memory != null)
		{
			if (other.gameObject.CompareTag("Food"))
			{
				entity.Memory.MemorizeResource(other.gameObject);
				See(other.gameObject);
			}

			if (other.gameObject.CompareTag("Entity"))
			{
				See(other.gameObject);

				if (other.gameObject.GetComponent<Entity>().IsCarnivore() && entity.IsHerbivore())
					AssessUtilities();
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

			if (other.gameObject.GetComponent<Entity>().IsCarnivore() && entity.IsHerbivore())
				AssessUtilities();
		}
	}
}
