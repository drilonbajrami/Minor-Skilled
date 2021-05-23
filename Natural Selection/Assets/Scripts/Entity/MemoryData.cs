using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a reference to an object and its last know position
/// </summary>
public class MemoryData
{
	public GameObject ObjectInMemory { get; }
	public Vector3 LastKnownPosition { get; set; }
	
	public MemoryData(GameObject objectToRemember)
	{
		ObjectInMemory = objectToRemember;
		LastKnownPosition = objectToRemember.transform.position;
	}

	public bool ObjectNoLongerExists()
	{
		return ObjectInMemory == null;
	}

	public void UpdateLastKnownPosition()
	{
		LastKnownPosition = ObjectInMemory.transform.position;
	}
}
