using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Smell : MonoBehaviour
{
    Dictionary<int, MemoryData> _smellZone;
    Dictionary<int, MemoryData> _smellZonePrey;

    private float refreshTimer;
    private float refreshInterval = 5.0f;

    void Start()
    {     
        _smellZone = new Dictionary<int, MemoryData>();
        _smellZonePrey = new Dictionary<int, MemoryData>();
    }

	private void Update()
	{
        RefreshSmell();
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

    public bool HasPartnersAround()
    {
        return _smellZone.Count > 0;
    }

    public bool HasPreyAround()
    {
        return _smellZonePrey.Count > 0;
    }

    private void RefreshSmell()
    {
        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0.0f)
        {
            List<int> toRemove1 = new List<int>();
            List<int> toRemove2 = new List<int>();

            foreach (KeyValuePair<int, MemoryData> o in _smellZone)
                if (o.Value.IsObjectMissing() || !o.Value.Object.activeSelf)
                    toRemove1.Add(o.Key);

            foreach (KeyValuePair<int, MemoryData> o in _smellZonePrey)
                if (o.Value.IsObjectMissing() || !o.Value.Object.activeSelf)
                    toRemove2.Add(o.Key);

            for (int i = 0; i < toRemove1.Count; i++)
                _smellZone.Remove(toRemove1[i]);

            for (int i = 0; i < toRemove2.Count; i++)
                _smellZonePrey.Remove(toRemove2[i]);

            refreshTimer = refreshInterval;
        }
    }

    //====================================================================================================
    //											TRIGGERS
    //====================================================================================================
    private void OnTriggerEnter(Collider other)
	{
        Entity a = other.gameObject.GetComponent<Entity>();

        if (a != null)
        {
            if (a.gender != gameObject.GetComponentInParent<Entity>().gender && !a.isOnReproducingState && a.order == gameObject.GetComponentInParent<Entity>().order)
            {
                _smellZone.Add(other.gameObject.GetInstanceID(), new MemoryData(other.gameObject));
            }

            if (gameObject.GetComponentInParent<Entity>().order == Order.CARNIVORE && a.order == Order.HERBIVORE)
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

            if (a.order == Order.HERBIVORE && gameObject.GetComponentInParent<Entity>().order == Order.CARNIVORE && _smellZonePrey.ContainsKey(other.gameObject.GetInstanceID()))
            {
                _smellZonePrey.Remove(other.gameObject.GetInstanceID());
            }
        }
	}
}
