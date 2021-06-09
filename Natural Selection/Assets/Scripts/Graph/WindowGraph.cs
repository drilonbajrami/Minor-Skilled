using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WindowGraph : MonoBehaviour
{
	[SerializeField] private RectTransform graphContainer;
	[SerializeField] private RectTransform graphAxes;

	// Prefabs for drawing graphs
	[SerializeField] private GameObject axisPrefab;
	[SerializeField] private GameObject pointPrefab;
	[SerializeField] private GameObject connectionPrefab;
	[SerializeField] private GameObject markPrefab;
	[SerializeField] private GameObject textPrefab;

	/* The amount we want to shift the graph
	 * from the bottom left corner, applies shifting
	 * on both axes evenly
	 */
	public float padding;

	/* The highest value that we need to show on the Y axis
	 * It is 20% bigger than the highest value we get from
	 * the input data we give to show the graph
	 */
	private float yMaxValue;

	/* The division length on the X axis
	 * Used for spreding cycle marks evenly
	 * depending on the number of cycles given
	 */
	private float xDivisionLength;

	private GameObject xAxis;
	private GameObject yAxis;

	/* Keep track of text marks on the Y axis
	 * So we can change the mark amount depending
	 * on the data's amount that we want to show
	 */
	private List<TextMeshProUGUI> yAxisValues;

	private bool herbivoreOn = false;
	private bool carnivoreOn = false;
	private bool herbivoreSpeedOn = false;

	private void Awake()
	{
		yAxisValues = new List<TextMeshProUGUI>();
		//DrawAxes(padding, Cycle.cycleCount);
	}

	//public void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.D))
	//	{
	//		Clear();
	//		DrawAxes(padding, Cycle.cycleCount);
	//	}
	//}

	private void OnEnable()
	{
		DrawAxes(padding, Cycle.cycleCount);
		//ShowGraph(Counter.herbivoreCounts, Color.red);
		//ShowGraph(Counter.carnivoreCounts, Color.blue);
	}

	private void OnDisable()
	{
		//Clear();
	}

	public void Clear()
	{
		if (herbivoreOn)
			ClearHerbivoreData();

		if (carnivoreOn)
			ClearCarnivoreData();

		yAxisValues.Clear();
		ClearGraphAxes();
		this.gameObject.SetActive(false);
	}

	public void ClearGraphAxes()
	{
		for (int i = graphAxes.childCount - 1; i >= 0; i--)
			Destroy(graphAxes.GetChild(i).gameObject);
	}

	public void ShowHerbivoreCount()
	{
		herbivoreOn = true;
		ShowGraph(Counter.herbivoreCounts, new Color(255f/255f, 50f/255f, 50f/255f), CreateDataHolder("Herbivore Count"));
	}

	public void ShowCarnivoreCount()
	{
		carnivoreOn = true;
		ShowGraph(Counter.carnivoreCounts, new Color(50f/255f, 50f/255f, 255f/255f), CreateDataHolder("Carnivore Count"));
	}

	public void ClearHerbivoreData()
	{
		herbivoreOn = false;
		Destroy(GameObject.Find("Herbivore Count").gameObject);
	}

	public void ClearCarnivoreData()
	{
		carnivoreOn = false;
		Destroy(GameObject.Find("Carnivore Count").gameObject);
	}

	public void ToggleHerbivoreCount()
	{
		if (!herbivoreOn)
			ShowHerbivoreCount();
		else
			ClearHerbivoreData();
	}

	public void ToggleCarnivoreCount()
	{
		if (!carnivoreOn)
			ShowCarnivoreCount();
		else
			ClearCarnivoreData();
	}

	public void ToggleHerbivoreSpeed()
	{
		if (!herbivoreSpeedOn)
			ShowHerbivoreSpeed();
		else
			ClearHerbivoreSpeedData();
	}

	public void ShowHerbivoreSpeed()
	{

	}

	public void ClearHerbivoreSpeedData()
	{

	}

	private void ShowGraph(List<int> valueList, Color color, RectTransform parent)
	{
		float graphHeight = yAxis.GetComponent<RectTransform>().sizeDelta.x;
		float maxVal = valueList.Max() + 20.0f / valueList.Max() * 100;
		if (yMaxValue < maxVal)
			yMaxValue = maxVal;

		GameObject lastPoint = null;

		for (int i = 0; i < valueList.Count; i++)
		{
			float xPos = (i * xDivisionLength) + padding;
			float yPos = (valueList[i] / yMaxValue) * graphHeight + padding;
			GameObject point = DrawPoint(new Vector2(xPos, yPos), color, parent);

			if (lastPoint != null)
				DrawConnection(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition, color, parent);

			lastPoint = point;
		}

		for (int i = 1; i < 21; i++)
		{
			yAxisValues[i - 1].text = (i * (yMaxValue / 20.0f)).ToString("F0");
		}
	}

	private RectTransform CreateDataHolder(string name)
	{
		GameObject dataHolder = new GameObject(name, typeof(RectTransform));
		dataHolder.transform.SetParent(graphContainer, false);
		RectTransform d = dataHolder.GetComponent<RectTransform>();
		d.anchorMin = new Vector2(0, 0);
		d.anchorMax = new Vector2(0, 0);
		d.pivot = new Vector2(0, 0);
		d.sizeDelta.Set(graphContainer.sizeDelta.x, graphContainer.sizeDelta.y);
		return d;
	}

	#region Drawing Functions

	private void DrawAxes(float padding, int numberOfCycles)
	{
		// X Axis
		xAxis = Instantiate(axisPrefab, graphAxes, false);
		RectTransform rectXAxis = xAxis.GetComponent<RectTransform>();
		rectXAxis.anchoredPosition = new Vector2(padding, padding);
		rectXAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.x - (padding * 2), 3.0f);

		// Y Axis
		yAxis = Instantiate(axisPrefab, graphAxes, false);
		RectTransform rectYAxis = yAxis.GetComponent<RectTransform>();
		rectYAxis.anchoredPosition = new Vector2(padding, padding);
		rectYAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.y - (padding * 2), 3.0f);
		rectYAxis.localEulerAngles = new Vector3(0, 0, 90);

		DrawMarks(padding, numberOfCycles);
	}

	public void DrawMarks(float padding, int numberOfCycles)
	{
		int cycleMarks = 5 + (numberOfCycles / 5) * 5;
		xDivisionLength = xAxis.GetComponent<RectTransform>().sizeDelta.x / cycleMarks;
		float yDivisionLength = yAxis.GetComponent<RectTransform>().sizeDelta.x / 20;

		// Draw the marks and text marks on X axis
		for (int i = 1; i < cycleMarks + 1; i++)
		{
			// Mark
			GameObject mark = Instantiate(markPrefab, graphAxes, false);
			RectTransform rectTransform = mark.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(xDivisionLength * i + padding, padding - 5.0f);
			rectTransform.localEulerAngles = new Vector3(0, 0, 90);

			// Text Mark
			GameObject text = Instantiate(textPrefab, graphAxes, false);
			text.GetComponent<TextMeshProUGUI>().text = i.ToString();
			text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
			text.GetComponent<RectTransform>().anchoredPosition = new Vector2(xDivisionLength * i + padding, 35.0f);
		}

		// Draw the marks and text marks on Y axis
		for (int i = 1; i < 21; i++)
		{
			// Mark
			GameObject mark = Instantiate(markPrefab, graphAxes, false);
			mark.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding - 5.0f, yDivisionLength * i + padding);

			// Text Mark
			GameObject text = Instantiate(textPrefab, graphAxes, false);
			yAxisValues.Add(text.GetComponent<TextMeshProUGUI>());
			text.GetComponent<RectTransform>().anchoredPosition = new Vector2(35.0f, yDivisionLength * i + padding);
		}
	}

	private GameObject DrawPoint(Vector2 anchoredPosition, Color color, RectTransform parent)
	{
		GameObject point = Instantiate(pointPrefab, parent, false);
		point.GetComponent<Image>().color = color;
		point.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
		return point;
	}

	private void DrawConnection(Vector2 pointA, Vector2 pointB, Color color, RectTransform parent)
	{
		GameObject connection = Instantiate(connectionPrefab, parent, false);
		connection.GetComponent<Image>().color = color;
		RectTransform c = connection.GetComponent<RectTransform>();
		Vector2 dir = (pointB - pointA).normalized;
		float distance = Vector2.Distance(pointA, pointB);
		c.sizeDelta = new Vector2(distance, 3f);
		c.anchoredPosition = pointA + dir * distance * 0.5f;
		c.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
	}

	public float GetAngleFromVectorFloat(Vector3 dir)
	{
		dir = dir.normalized;
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;
		return n;
	}

	#endregion
}