using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public GameObject other;
	public GameObject pos;

    public int sectionCount;
    SightArea sightArea;
    public PieGraph pieGraph;

	private void Start()
	{
        sightArea = new SightArea(sectionCount);
        //pieGraph.CreatePieGraph(sightArea, gameObject.transform);
	}

	void Update()
    {
		if (Input.GetKey(KeyCode.Space))
		{
			//Vector3 toOther = other.transform.position - gameObject.transform.position;
			////sightArea.AssessUtilityValuesO(gameObject, other, 0);

			//for (int i = 0; i < sightArea.sections.Length; i++)
			//	pieGraph.UpdateSectionUtilityColor(i, sightArea.sections[i].UtilityValue);
			//Debug.Log($"Ideal Direction Angle: {sightArea.IdealDirectionAngle}");
			//float angle = sightArea.IdealDirectionAngle;
			//if (angle == 0)
			//	angle = Random.Range(0, 360);

			//pos.transform.position = TransformUtils.UtilityRandomPosition(this.transform, 5, angle, sightArea.halfAngle);
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			transform.rotation = Quaternion.AngleAxis(60.0f, transform.up);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			transform.rotation = Quaternion.AngleAxis(-60.0f, Vector3.up);
		}
		
	}

    public float GetAngle()
    {
        Vector3 toOther = other.transform.position - gameObject.transform.position;
        return Vector3.SignedAngle(gameObject.transform.forward, toOther, gameObject.transform.up);
    }
}
