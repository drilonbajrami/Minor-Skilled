using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public GameObject other;

    public int sectionCount;
    SightArea sightArea;
    public PieGraph pieGraph;

    public float radius = 5;
    public float angle = 90.0f;

	private void Start()
	{
        //sightArea = new SightArea(sectionCount);
        //pieGraph.CreatePieGraph(sightArea, gameObject.transform);
	}

	void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Debug.Log($"Angle is: {GetAngle()}");
        //    Vector3 toOther = other.transform.position - gameObject.transform.position;
        //    Debug.Log($"Angle is: {TransformUtils.GetAngle(gameObject.transform.forward, toOther, gameObject.transform.up)}");
        //    sightArea.CalculateUtilityValues(gameObject.transform, other);

        //    for (int i = 0; i < sightArea.sightSections.Length; i++)
        //        pieGraph.ChangeUtilityColor(i, sightArea.sightSections[i].utilityValue);
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log(TransformUtils.UtilityRandomPosition(gameObject.transform, 10, 200, sightArea.halfAngle));
        //}

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(gameObject.transform.forward);
        }
	}

    public float GetAngle()
    {
        Vector3 toOther = other.transform.position - gameObject.transform.position;
        return Vector3.SignedAngle(gameObject.transform.forward, toOther, gameObject.transform.up);
    }
}
