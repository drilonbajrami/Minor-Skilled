using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for visualizing the utility sections of an entity
/// </summary>
public class PieGraph : MonoBehaviour
{  
    [SerializeField] private Image wedgePrefab;
    [SerializeField] private Gradient riskColor;

    private Image[] wedges;
    private Transform entityTransform;

	public void CreatePieGraph(SightArea sightArea, Transform pTransform)
    {
        entityTransform = pTransform;
        float zRotation = 0.0f;
        wedges = new Image[sightArea.sections.Length];

        for (int i = 0; i < wedges.Length; i++)
        {
            wedges[i] = Instantiate(wedgePrefab) as Image;
            wedges[i].transform.SetParent(transform, false);
            wedges[i].color = riskColor.Evaluate(Mathf.InverseLerp(-1, 1, sightArea.sections[i].UtilityValue));
            wedges[i].fillAmount = Mathf.InverseLerp(0, 360, 360/wedges.Length);
            wedges[i].transform.rotation = Quaternion.Euler(new Vector3(90, 0, zRotation));
            zRotation -= wedges[i].fillAmount * 360.0f;
        }
    }

    public void UpdateSectionUtilityColor(int index, float utilityValue)
    {
        wedges[index].color = riskColor.Evaluate(Mathf.InverseLerp(-1, 1, utilityValue));
        wedges[index].gameObject.transform.GetChild(0).GetComponent<Text>().text = utilityValue.ToString("F2");
        wedges[index].gameObject.transform.GetChild(1).GetComponent<Text>().text = (index + 1).ToString();
        transform.eulerAngles = new Vector3(90, 0, -entityTransform.eulerAngles.y);
    }

	private void Update()
	{
        if (entityTransform != null)
        {
            transform.position = entityTransform.position;
        }
	}
}
