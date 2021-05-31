using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Utility class contains all functions that deal with:
/// distance, random position, transform
/// </summary>
public static class TransformUtils
{
	/// <summary>
	/// Get a random point within a give radius and FOV of given transform
	/// </summary>
	/// <param name="from"></param>
	/// <param name="radius"></param>
	/// <returns></returns>
	public static Vector3 RandomTarget(Transform from, float radius, float FOV)
	{
		Vector3 randomPoint = RandomPositionWithinFOV(from, radius, FOV);

		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, 1))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	/// <summary>
	/// Returns a random position within the FOV of give transform
	/// </summary>
	/// <param name="a"></param>
	/// <param name="radius"></param>
	/// <returns></returns>
	private static Vector3 RandomPositionWithinFOV(Transform a, float radius, float FOV)
	{
		// Get random point within transform 'a'
		Vector3 randomPoint = Random.insideUnitSphere * radius + a.position;

		// Get direction from position of transform 'a' to the random point
		Vector3 directionToRandomPoint = randomPoint - a.position;

		// Find angle between two directions
		float angle = Vector3.Angle(a.forward, directionToRandomPoint);

		if (Mathf.Abs(angle) < FOV/2)
			return randomPoint;
		else
			return RandomPositionWithinFOV(a, radius, FOV);
	}

}
