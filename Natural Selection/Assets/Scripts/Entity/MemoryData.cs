using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct that holds a reference to an object and its last know position
/// </summary>
public struct MemoryData
{
	public GameObject ObjectInMemory { get; }
	public Vector3 LastKnownPosition { get; set; }
	
	public MemoryData(GameObject pObjectToRemember)
	{
		ObjectInMemory = pObjectToRemember;
		LastKnownPosition = pObjectToRemember.transform.position;
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
