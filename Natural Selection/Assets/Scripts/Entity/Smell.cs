using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{
    [SerializeField] List<GameObject> _smellZone;
    [SerializeField] List<GameObject> _smellZonePrey;

    void Start()
    {
        _smellZone = new List<GameObject>();
        _smellZonePrey = new List<GameObject>();
    }

	public GameObject ChoosePartner()
    {
        if (_smellZone.Count == 0)
            return null;
        else
        {
            int i = Random.Range(0, _smellZone.Count);
            return _smellZone[i];
        }
    }

    public GameObject ChoosePrey()
    {
        if (_smellZonePrey.Count == 0)
            return null;
        else
        {
            int i = Random.Range(0, _smellZonePrey.Count);
            return _smellZonePrey[i];
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

	private void OnTriggerEnter(Collider other)
	{
        Entity a = other.gameObject.GetComponent<Entity>();
        if (a != null)
        {
            if (a.gender != gameObject.GetComponentInParent<Entity>().gender && a.isOnReproducingState && a.order == gameObject.GetComponentInParent<Entity>().order)
                _smellZone.Add(other.gameObject);

            if (gameObject.GetComponentInParent<Entity>().order == Order.CARNIVORE && a.order == Order.HERBIVORE)
                if (!_smellZonePrey.Contains(other.gameObject))
                    _smellZonePrey.Add(other.gameObject);
        }
	}

	private void OnTriggerExit(Collider other)
	{
        Entity a = other.gameObject.GetComponent<Entity>();
        if (a != null)
        {
            if (_smellZone.Contains(other.gameObject))
                _smellZone.Remove(other.gameObject);

            if (a.order == Order.HERBIVORE && gameObject.GetComponentInParent<Entity>().order == Order.CARNIVORE && _smellZonePrey.Contains(other.gameObject))
                _smellZonePrey.Remove(other.gameObject);
        }
	}

	private void OnTriggerStay(Collider other)
	{
        Entity a = other.gameObject.GetComponent<Entity>();
        if (a != null)
        {
            if (a.gender != gameObject.GetComponentInParent<Entity>().gender && !_smellZone.Contains(other.gameObject) && a.isOnReproducingState)
                _smellZone.Add(other.gameObject);

            if (gameObject.GetComponentInParent<Entity>().order == Order.CARNIVORE && a.order == Order.HERBIVORE)
                if (!_smellZonePrey.Contains(other.gameObject))
                    _smellZonePrey.Add(other.gameObject);
        }
	}
}
