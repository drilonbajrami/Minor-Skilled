using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MemoryData
{
	private Vector3 lastKnownPosition;
	private GameObject objectToRemember;

	public MemoryData(GameObject pObjectToRemember)
	{
		objectToRemember = pObjectToRemember;
		lastKnownPosition = pObjectToRemember.transform.position;
	}

	public Vector3 LastKnownPosition => lastKnownPosition;
	public GameObject Object => objectToRemember;

	public bool isObjectMissing()
	{
		return objectToRemember == null;
	}
}
