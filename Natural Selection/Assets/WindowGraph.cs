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
	[SerializeField] private GameObject pointPrefab;
	[SerializeField] private GameObject textPrefab;
	[SerializeField] private GameObject markPrefab;

	public float padding;
	private float yMaxValue;
	private float xDivisionLength;

	private GameObject xAxis;
	private GameObject yAxis;

	private List<TextMeshProUGUI> yAxisValues;

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
		ShowGraph(Counter.herbivoreCounts, Color.red);
		ShowGraph(Counter.carnivoreCounts, Color.blue);
	}

	private void OnDisable()
	{
		Clear();
	}

	private void Clear()
	{
		yAxisValues.Clear();
		for (int i = graphAxes.childCount - 1; i >= 0; i--)
			Destroy(graphAxes.GetChild(i).gameObject);			
	}

	private GameObject CreatePoint(Vector2 anchoredPosition)
	{
		GameObject point = Instantiate(pointPrefab);
		point.transform.SetParent(graphContainer, false);
		RectTransform rectTransform = point.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
		return point;
	}

	private void ShowGraph(List<int> valueList, Color color)
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
			GameObject point = CreatePoint(new Vector2(xPos, yPos));
			point.transform.SetParent(graphAxes, false);
			point.GetComponent<Image>().color = color;

			if (lastPoint != null)
				AddPointConnection(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition, color);

			lastPoint = point;
		}

		for (int i = 1; i < 21; i++)
		{
			yAxisValues[i - 1].text = (i * (yMaxValue / 20.0f)).ToString("F0");
		}
	}

	private void AddPointConnection(Vector2 pointPosA, Vector2 pointPosB, Color color)
	{
		GameObject connection = new GameObject("PointConnection", typeof(Image));
		connection.transform.SetParent(graphAxes, false);
		connection.GetComponent<Image>().color = color;
		RectTransform rectTransform = connection.GetComponent<RectTransform>();
		Vector2 dir = (pointPosB - pointPosA).normalized;
		float distance = Vector2.Distance(pointPosA, pointPosB);
		rectTransform.anchorMin = new Vector2(0, 0);
		rectTransform.anchorMax = new Vector2(0, 0);
		rectTransform.sizeDelta = new Vector2(distance, 3f);
		rectTransform.anchoredPosition = pointPosA + dir * distance * 0.5f;
		rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
	}

	private void DrawAxes(float padding, int numberOfCycles)
	{
		// X Axis
		xAxis = new GameObject("Axis", typeof(Image));
		xAxis.transform.SetParent(graphAxes, false);
		xAxis.GetComponent<Image>().color = new Color(1, 1, 1, 1);
		RectTransform rectXAxis = xAxis.GetComponent<RectTransform>();
		rectXAxis.anchorMin = Vector2.zero;
		rectXAxis.anchorMax = Vector2.zero;
		rectXAxis.pivot = Vector2.zero;
		rectXAxis.anchoredPosition = new Vector2(padding, padding);
		rectXAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.x - (padding * 2), 3.0f);
		rectXAxis.localEulerAngles = new Vector3(0, 0, 0);

		// Y Axis
		yAxis = new GameObject("Axis", typeof(Image));
		yAxis.transform.SetParent(graphAxes, false);
		yAxis.GetComponent<Image>().color = new Color(1, 1, 1, 1);
		RectTransform rectYAxis = yAxis.GetComponent<RectTransform>();
		rectYAxis.anchorMin = Vector2.zero;
		rectYAxis.anchorMax = Vector2.zero;
		rectYAxis.pivot = Vector2.zero;
		rectYAxis.anchoredPosition = new Vector2(padding, padding);
		rectYAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.y - (padding * 2), 3.0f);
		rectYAxis.localEulerAngles = new Vector3(0, 0, 90);

		DrawMarks(padding, numberOfCycles);
	}

	public void DrawMarks(float padding, int numberOfCycles)
	{
		float xAxisLength = xAxis.GetComponent<RectTransform>().sizeDelta.x;
		float yAxisLength = yAxis.GetComponent<RectTransform>().sizeDelta.x;

		int cycleMarks = 5 + (numberOfCycles / 5) * 5;
		xDivisionLength = xAxisLength / cycleMarks;
		float yDivisionLength = yAxisLength / 20;

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
			RectTransform textRect = text.GetComponent<RectTransform>();
			textRect.anchoredPosition = new Vector2(xDivisionLength * i + padding, 35.0f);
		}

		// Draw the marks and text marks on Y axis
		for (int i = 1; i < 21; i++)
		{
			// Mark
			GameObject mark = Instantiate(markPrefab, graphAxes, false);
			RectTransform rectTransform = mark.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(padding - 5.0f, yDivisionLength * i + padding);

			// Text Mark
			GameObject text = Instantiate(textPrefab, graphAxes, false);
			yAxisValues.Add(text.GetComponent<TextMeshProUGUI>());
			RectTransform textRect = text.GetComponent<RectTransform>();
			textRect.anchoredPosition = new Vector2(35.0f, yDivisionLength * i + padding);
		}
	}

	public float GetAngleFromVectorFloat(Vector3 dir)
	{
		dir = dir.normalized;
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;
		return n;
	}
}