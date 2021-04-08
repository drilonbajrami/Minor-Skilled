using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] List<GameObject> _sightZone;

    void Start()
    {
        _sightZone = new List<GameObject>();
    }

	private void Update()
	{
		
	}

	public List<GameObject> CanSee()
    {
        return _sightZone;
    }

	public bool CanSee(GameObject gameObject)
	{
		return _sightZone.Any(a => a.GetInstanceID() == gameObject.GetInstanceID());
	}

    public void See(GameObject pObject)
    {
        if (_sightZone.Count == 0 && pObject != null)
            _sightZone.Add(pObject);
        else if (!_sightZone.Contains(pObject) && pObject != null)
            _sightZone.Add(pObject);
    }

    public void Unsee(GameObject pObject)
    {
        if (_sightZone.Count != 0)
            if (_sightZone.Contains(pObject))
                _sightZone.Remove(pObject);
            else if (pObject == null)
                _sightZone.Remove(pObject);
    }
}
