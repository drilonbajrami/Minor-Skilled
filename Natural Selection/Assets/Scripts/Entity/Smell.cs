using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Smell : MonoBehaviour
{
	private Entity entity;
	Dictionary<int, MemoryData> _smellZone = new Dictionary<int, MemoryData>();
	Dictionary<int, MemoryData> _smellZonePrey = new Dictionary<int, MemoryData>();

	void OnEnable()
	{
		entity = gameObject.GetComponentInParent<Entity>();
		_smellZone.Clear();
		_smellZonePrey.Clear();
		StartCoroutine(RefreshSmellCoroutine());
	}

	public GameObject ChoosePrey()
	{
		// This should be changed at some point as well, there should be selections to choose prey
		if (_smellZonePrey.Count == 0)
			return null;
		else
		{
			int i = Random.Range(0, _smellZonePrey.Count);
			return _smellZonePrey.Values.ElementAt(i).Object;
		}
	}

	public void RemoveFromSmellZone(int id)
	{
		_smellZonePrey.Remove(id);
	}

	public bool HasPreyAround()
	{
		return _smellZonePrey.Count > 0;
	}

	/// <summary>
	/// Smell refresh coroutine
	/// </summary>
	/// <returns></returns>
	private IEnumerator RefreshSmellCoroutine()
	{
		while (true) {
			yield return new WaitForSeconds(entity.SenseRefreshInterval);
			RefreshSmell();
		}
	}

	/// <summary>
	/// Clean smell of missing objects
	/// </summary>
	private void RefreshSmell()
	{
		List<int> toRemove1 = new List<int>();
		List<int> toRemove2 = new List<int>();

		foreach (KeyValuePair<int, MemoryData> o in _smellZone)
			if (o.Value.ObjectNoLongerExists() || !o.Value.Object.activeSelf)
				toRemove1.Add(o.Key);

		foreach (KeyValuePair<int, MemoryData> o in _smellZonePrey)
			if (o.Value.ObjectNoLongerExists() || !o.Value.Object.activeSelf)
				toRemove2.Add(o.Key);

		foreach (int i in toRemove1)
			_smellZone.Remove(i);

		foreach (int i in toRemove2)
			_smellZonePrey.Remove(i);
	}

	#region Not used
	public bool HasPartnersAround()
	{
		return _smellZone.Count > 0;
	}

	public GameObject ChoosePartner()
	{
		// This should be changed at some point, the choice is based on fitness values 
		if (_smellZone.Count == 0)
			return null;
		else
		{
			int i = Random.Range(0, _smellZone.Count);
			return _smellZone.Values.ElementAt(i).Object;
		}
	}
	#endregion

	//====================================================================================================
	//											TRIGGERS
	//====================================================================================================
	private void OnTriggerEnter(Collider other)
	{
		Entity a = other.gameObject.GetComponent<Entity>();

		if (a != null)
		{
			//if (a.gender != gameObject.GetComponentInParent<Entity>().gender && !a.isOnReproducingState && a.order == gameObject.GetComponentInParent<Entity>().order)
			//{
			//	_smellZone.Add(other.gameObject.GetInstanceID(), new MemoryData(other.gameObject));
			//}

			if (entity.IsCarnivore() && a.IsHerbivore())
			{
				if (!_smellZonePrey.ContainsKey(other.gameObject.GetInstanceID()))
					_smellZonePrey.Add(other.gameObject.GetInstanceID(), new MemoryData(other.gameObject));
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Entity a = other.gameObject.GetComponent<Entity>();

		if (a != null)
		{
			if (_smellZone.ContainsKey(other.gameObject.GetInstanceID()))
			{
				_smellZone.Remove(other.gameObject.GetInstanceID());
			}

			if (a.IsHerbivore() && entity.IsCarnivore() && _smellZonePrey.ContainsKey(other.gameObject.GetInstanceID()))
			{
				_smellZonePrey.Remove(other.gameObject.GetInstanceID());
			}
		}
	}
}
