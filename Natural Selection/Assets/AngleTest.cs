using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public GameObject other;

    public int sectionCount;
    SightArea sightArea;
    PieGraph pieGraph;

	private void Start()
	{
        sightArea = new SightArea(sectionCount);
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Angle is: {GetAngle()}");
        }
	}

    public float GetAngle()
    {
        Vector3 toOther = other.transform.position - gameObject.transform.position;
        return Vector3.SignedAngle(gameObject.transform.forward, toOther, gameObject.transform.up);
    }
}
