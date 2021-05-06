using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MemoryData
{
	private Vector3 lastKnownPosition;
	private GameObject objectToRemember;

	public Vector3 LastKnownPosition => lastKnownPosition;
	public GameObject Object => objectToRemember;

	public MemoryData(GameObject pObjectToRemember)
	{
		objectToRemember = pObjectToRemember;
		lastKnownPosition = pObjectToRemember.transform.position;
	}

	public bool IsObjectMissing()
	{
		return objectToRemember == null;
	}
}
