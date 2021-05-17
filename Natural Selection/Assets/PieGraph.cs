using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieGraph : MonoBehaviour
{
    public Image wedgePrefab;
    public Gradient riskColor;
    private Image[] wedges;
    private Transform entityTransform;

	public void CreatePieGraph(SightArea sightArea, Transform pTransform)
    {
        entityTransform = pTransform;
        float zRotation = 0.0f;
        wedges = new Image[sightArea.sightSections.Length];

        for (int i = 0; i < wedges.Length; i++)
        {
            wedges[i] = Instantiate(wedgePrefab) as Image;
            wedges[i].transform.SetParent(transform, false);
            wedges[i].color = riskColor.Evaluate(Mathf.InverseLerp(-1, 1, sightArea.sightSections[i].utilityValue));
            wedges[i].fillAmount = Mathf.InverseLerp(0, 360, 360/wedges.Length);
            wedges[i].transform.rotation = Quaternion.Euler(new Vector3(90, 0, zRotation));
            zRotation -= wedges[i].fillAmount * 360.0f;
        }
    }

    public void ChangeUtilityColor(int index, float utilityValue)
    {
        wedges[index].color = riskColor.Evaluate(Mathf.InverseLerp(-1, 1, utilityValue));
    }

	private void Update()
	{
        if (entityTransform != null)
        {
            transform.position = entityTransform.position;
            transform.eulerAngles = new Vector3(90, 0, -entityTransform.eulerAngles.y);
        }
	}
}
