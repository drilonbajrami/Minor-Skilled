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
	/// Get a random point within a radius of given position
	/// </summary>
	/// <param name="from"></param>
	/// <param name="radius"></param>
	/// <returns></returns>
	public static Vector3 RandomTarget(Vector3 from, float radius)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += from;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}

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

	/// <summary>
	/// Check if distance between 'a' and 'b' is smaller than the 'difference'
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="difference"></param>
	/// <returns></returns>
	public static bool CheckIfClose(Transform a, Transform b, float difference)
	{
		return Mathf.Abs(a.position.x - b.position.x) < difference && Mathf.Abs(a.position.z - b.position.z) < difference;
	}

	/// <summary>
	/// Returns closest object to 'from' object
	/// </summary>
	/// <param name="from"></param>
	/// <param name="toA"></param>
	/// <param name="toB"></param>
	/// <returns></returns>
	public static GameObject ClosestTo(GameObject from, GameObject toA, GameObject toB)
	{
		float lengthA = (from.transform.position - toA.transform.position).sqrMagnitude;
		float lengthB = (from.transform.position - toB.transform.position).sqrMagnitude;
		return lengthA < lengthB ? toA : toB;
	}

	public static float GetAngle(Vector3 from, Vector3 to, Vector3 axis)
	{
		float signedAngle = Vector3.SignedAngle(from, to, axis);
		return (signedAngle > 0.0f) ? signedAngle : 360.0f + signedAngle;
	}
}
