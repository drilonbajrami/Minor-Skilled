using UnityEngine;

/// <summary>
/// Holds a reference to an object and its last know position
/// </summary>
public class MemoryData
{
	public GameObject Object { get; }
	public Vector3 LastKnownPosition { get; set; }
	
	public MemoryData(GameObject itemToRemember)
	{
		Object = itemToRemember;
		LastKnownPosition = itemToRemember.transform.position;
	}

	public bool ObjectNoLongerExists()
	{
		return Object == null;
	}

	public void UpdateLastKnownPosition()
	{
		LastKnownPosition = Object.transform.position;
	}
}
