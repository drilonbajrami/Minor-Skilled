using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Graph : MonoBehaviour
{
	[SerializeField] private RectTransform graphContainer;
	[SerializeField] private RectTransform graphAxesAndMarkings;

	// Prefabs for drawing graphs
	[SerializeField] private GameObject axisPrefab;
	[SerializeField] private GameObject pointPrefab;
	[SerializeField] private GameObject connectionPrefab;
	[SerializeField] private GameObject markPrefab;
	[SerializeField] private GameObject textPrefab;

	/* Keep track of currently shown graphs so we can delete them on toggling other options
	 */
	private List<GameObject> _currentShownGraphs = new List<GameObject>();

	/* The amount we want to shift the graph
	 * from the bottom left corner, applies shifting
	 * on both axes evenly
	 */
	public float padding;

	/* The highest value that we need to show on the Y axis
	 * It is 20% bigger than the highest value we get from
	 * the input data we give to show the graph
	 */
	private float yMaxValue = 0;

	/* The division length on the X axis
	 * Used for spreding cycle marks evenly
	 * depending on the number of cycles given
	 */
	private float xDivisionLength;

	/*
	 * Lengths of both axes
	 */
	private float xAxisLength;
	private float yAxisLength;

	/* Keep track of text marks on the Y axis
	 * So we can change the mark amount depending
	 * on the data's amount that we want to show
	 */
	private List<TextMeshProUGUI> yAxisValues;

	private void Awake()
	{
		yAxisValues = new List<TextMeshProUGUI>();
		_currentShownGraphs = new List<GameObject>();
	}

	private void OnEnable()
	{
		DrawAxes(padding, Cycle.cycleCount);
	}

	public void ClearAll()
	{
		yMaxValue = 0;
		ClearHerbivoreCountData();
		ClearCarnivoreCountData();
		ClearHerbivoreSpeedData();
		ClearHerbivoreHeightData();
		ClearHerbivorePeerUtilityData();
		ClearHerbivoreOpponentUtilityData();
		yAxisValues.Clear();
		ClearGraphAxes();
		this.gameObject.SetActive(false);
	}

	public void ClearGraph(bool value)
	{
		if (value)
		{
			ClearHerbivoreCountData();
			ClearCarnivoreCountData();
			ClearHerbivoreSpeedData();
			ClearHerbivoreHeightData();
			ClearHerbivorePeerUtilityData();
			ClearHerbivoreOpponentUtilityData();
			ClearHerbivoreSocializingChanceData();
			yMaxValue = 0;
		}
	}

	public void ClearGraphAxes()
	{
		for (int i = graphAxesAndMarkings.childCount - 1; i >= 0; i--)
			Destroy(graphAxesAndMarkings.GetChild(i).gameObject);
	}

	// Herbivore Count Graph
	public void ToggleHerbivoreCount(bool value)
	{
		if (value)
			ShowHerbivoreCount();
		else
			ClearHerbivoreCountData();
	}
	private void ShowHerbivoreCount()
	{
		RectTransform holder = CreateDataHolder("Herbivore Count");
		ShowGraph(Counter.Instance.HerbivoreCounts, Counter.Instance.PeakAliveCount, 0.0f, new Color(255f/255f, 50f/255f, 50f/255f), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivoreCountData()
	{
		GameObject a = _currentShownGraphs.Find(o => o.name == "Herbivore Count");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}
	
	// Carnivore Count Graph
	public void ToggleCarnivoreCount(bool value)
	{
		if (value)
			ShowCarnivoreCount();
		else
			ClearCarnivoreCountData();
	}
	private void ShowCarnivoreCount()
	{
		RectTransform holder = CreateDataHolder("Carnivore Count");
		ShowGraph(Counter.Instance.CarnivoreCounts, Counter.Instance.PeakAliveCount, 0.0f, new Color(50f/255f, 50f/255f, 255f/255f), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearCarnivoreCountData()
	{
		GameObject a = _currentShownGraphs.Find(o => o.name == "Carnivore Count");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}
	
	// Heribvore Speed Graph
	public void ToggleHerbivoreSpeed(bool value)
	{
		if (value)
			ShowHerbivoreSpeed();
		else
			ClearHerbivoreSpeedData();
	}
	private void ShowHerbivoreSpeed()
	{
		ClearHerbivoreHeightData();
		RectTransform holder = CreateDataHolder("Speed Average");
		ShowGraph(Counter.Instance.AverageSpeedHerbivore, Counter.Instance.PeakSpeedAverage, 5.0f, new Color(1, 0, 150f/255f, 1), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivoreSpeedData()
	{
		yMaxValue = 0;
		GameObject a = _currentShownGraphs.Find(o => o.name == "Speed Average");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}

	// Herbivore Height Graph
	public void ToggleHerbivoreHeight(bool value)
	{
		if (value)
			ShowHerbivoreHeight();
		else
			ClearHerbivoreHeightData();
	}
	private void ShowHerbivoreHeight()
	{
		ClearHerbivoreSpeedData();
		RectTransform holder = CreateDataHolder("Height Average");
		ShowGraph(Counter.Instance.AverageHeightHerbivore, 2.0f, 0.3f, new Color(1, 150f/255f, 0, 1), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivoreHeightData()
	{
		yMaxValue = 0;
		GameObject a = _currentShownGraphs.Find(o => o.name == "Height Average");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}

	// Herbivore Peer Utility Graph
	public void ToggleHebivorePeerUtility(bool value)
	{
		if (value)
		{
			ClearHerbivoreSocializingChanceData();
			ShowHerbivorePeerUtility();
		}
		else
			ClearHerbivorePeerUtilityData();
	}
	private void ShowHerbivorePeerUtility()
	{
		RectTransform holder = CreateDataHolder("Peer Utility Average");
		ShowGraph(Counter.Instance.AveragePeerUtilityHerbivore, 1.0f, -1.0f, new Color(50f / 255f, 50f / 255f, 255f / 255f), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivorePeerUtilityData()
	{
		yMaxValue = 0;
		GameObject a = _currentShownGraphs.Find(o => o.name == "Peer Utility Average");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}

	// Herbivore Opponent Utility Graph
	public void ToggleHebivoreOpponentUtility(bool value)
	{
		if (value)
		{
			ClearHerbivoreSocializingChanceData();
			ShowHerbivoreOpponentUtility();
		}
		else
			ClearHerbivoreOpponentUtilityData();
	}
	private void ShowHerbivoreOpponentUtility()
	{
		RectTransform holder = CreateDataHolder("Opponent Utility Average");
		ShowGraph(Counter.Instance.AverageOpponentUtilityHerbivore, 1.0f, -1.0f, new Color(255f / 255f, 50f / 255f, 50f / 255f), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivoreOpponentUtilityData()
	{
		yMaxValue = 0;
		GameObject a = _currentShownGraphs.Find(o => o.name == "Opponent Utility Average");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}

	// Herbivore Socializing Chance Graph
	public void ToggleHerbivoreSocializingChance(bool value)
	{
		if (value)
		{
			ClearHerbivorePeerUtilityData();
			ClearHerbivoreOpponentUtilityData();
			ShowHerbivoreSocializingChance();
		}
		else
			ClearHerbivoreSocializingChanceData();
	}
	private void ShowHerbivoreSocializingChance()
	{
		RectTransform holder = CreateDataHolder("Socializing Chance Average");
		ShowGraph(Counter.Instance.AverageSocializingChanceHerbivore, 110.0f, 0.0f, new Color(255f / 255f, 50f / 255f, 50f / 255f), holder);
		_currentShownGraphs.Add(holder.gameObject);
	}
	private void ClearHerbivoreSocializingChanceData()
	{
		yMaxValue = 0;
		GameObject a = _currentShownGraphs.Find(o => o.name == "Socializing Chance Average");
		if (a != null)
		{
			_currentShownGraphs.Remove(a);
			Destroy(a.gameObject);
		}
	}

	#region Drawing Functions

	// Show graph for int values
	private void ShowGraph(List<int> valueList, float peakValue, float lowestValue, Color color, RectTransform parent)
	{
		if (yMaxValue < peakValue)
		{
			yMaxValue = peakValue;
			for (int i = 1; i < 21; i++) // Set the text mark values (20 text marks) on the Y axis depending on the highest and lowest value of the gived data
				yAxisValues[i - 1].text = (i * ((yMaxValue - lowestValue) / 20.0f) + lowestValue).ToString("F0"); // "F0" for showing whole numbers since we deal with int data
		}

		GameObject lastPoint = null; // Keep track of last point, used for connecting two consecutive points

		// Place points based on data input 
		for (int i = 0; i < valueList.Count; i++)
		{
			float xPos = (i * xDivisionLength) + padding; // Cycle on X axis
			float yPos = (valueList[i] - lowestValue) / (yMaxValue - lowestValue) * yAxisLength + padding * 0.7f;
			GameObject point = DrawPoint(new Vector2(xPos, yPos), color, parent.GetChild(1).GetComponent<RectTransform>());

			if (lastPoint != null) // If there is a last point, then create a connection
				DrawConnection(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition, color, parent.GetChild(0).GetComponent<RectTransform>());

			lastPoint = point;
		}
	}

	// Show graph for float values
	private void ShowGraph(List<float> valueList, float peakValue, float lowestValue, Color color, RectTransform parent)
	{
		if (yMaxValue < peakValue)
		{
			yMaxValue = peakValue;
			for (int i = 1; i < 21; i++) // Set the text mark values (20 text marks) on the Y axis depending on the highest and lowest value of the gived data
				yAxisValues[i - 1].text = (i * ((yMaxValue - lowestValue) / 20.0f) + lowestValue).ToString("F2"); // "F2" for showing decimals since we deal with float data
		}

		GameObject lastPoint = null; // Keep track of last point, used for connecting two consecutive points

		// Place points based on data input 
		for (int i = 0; i < valueList.Count; i++)
		{
			float xPos = (i * xDivisionLength) + padding; // Cycle on X axis
			float yPos = (valueList[i] - lowestValue) / (yMaxValue - lowestValue) * yAxisLength + padding * 0.7f;
			GameObject point = DrawPoint(new Vector2(xPos, yPos), color, parent.GetChild(1).GetComponent<RectTransform>());

			if (lastPoint != null) // If there is a last point, then create a connection
				DrawConnection(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition, color, parent.GetChild(0).GetComponent<RectTransform>());

			lastPoint = point;
		}
	}

	private void DrawAxes(float padding, int numberOfCycles)
	{
		// X Axis
		RectTransform rectXAxis = Instantiate(axisPrefab, graphAxesAndMarkings, false).GetComponent<RectTransform>();
		rectXAxis.anchoredPosition = new Vector2(padding, padding * 0.7f);
		rectXAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.x - (padding * 1.5f), 3.0f);
		xAxisLength = rectXAxis.sizeDelta.x;

		// Y Axis
		RectTransform rectYAxis = Instantiate(axisPrefab, graphAxesAndMarkings, false).GetComponent<RectTransform>();
		rectYAxis.anchoredPosition = new Vector2(padding, padding * 0.7f);
		rectYAxis.sizeDelta = new Vector2(graphContainer.sizeDelta.y - (padding * 1.5f), 3.0f);
		rectYAxis.localEulerAngles = new Vector3(0, 0, 90);
		yAxisLength = rectYAxis.sizeDelta.x;

		DrawMarks(padding, numberOfCycles);
	}

	public void DrawMarks(float padding, int numberOfCycles)
	{
		int cycleMarks = 5 + (numberOfCycles / 5) * 5;
		xDivisionLength = xAxisLength / cycleMarks;
		float yDivisionLength = yAxisLength / 20;

		// Draw the marks and text marks on X axis
		for (int i = 1; i < cycleMarks + 1; i++)
		{
			// Mark
			GameObject mark = Instantiate(markPrefab, graphAxesAndMarkings, false);
			RectTransform rectTransform = mark.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(yAxisLength, 2f);
			rectTransform.anchoredPosition = new Vector2(xDivisionLength * i + padding, padding * 0.7f);
			rectTransform.localEulerAngles = new Vector3(0, 0, 90);

			// Text Mark
			if (i == cycleMarks)
				continue;
			else if (i % 5 == 0)
			{
				GameObject text = Instantiate(textPrefab, graphAxesAndMarkings, false);
				text.GetComponent<TextMeshProUGUI>().text = i.ToString();
				text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
				text.GetComponent<RectTransform>().anchoredPosition = new Vector2(xDivisionLength * i + padding, 35.0f * 0.7f);
			}

			
		}

		// Draw the marks and text marks on Y axis
		for (int i = 1; i < 21; i++)
		{
			// Mark
			GameObject mark = Instantiate(markPrefab, graphAxesAndMarkings, false);
			RectTransform rectTransform = mark.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(xAxisLength, 2f);
			rectTransform.anchoredPosition = new Vector2(padding, yDivisionLength * i + padding * 0.7f);

			// Text Mark
			GameObject text = Instantiate(textPrefab, graphAxesAndMarkings, false);
			yAxisValues.Add(text.GetComponent<TextMeshProUGUI>());
			text.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding - 45.0f, yDivisionLength * i + padding * 0.7f);
		}
	}

	private GameObject DrawPoint(Vector2 anchoredPosition, Color color, RectTransform parent)
	{
		GameObject point = Instantiate(pointPrefab, parent, false);
		point.GetComponent<Image>().color = Color.white;
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

	private RectTransform CreateDataHolder(string name)
	{
		GameObject dataHolder = new GameObject(name, typeof(RectTransform));
		GameObject dataHolderConnection = new GameObject("Connections", typeof(RectTransform));
		GameObject dataHolderPoints = new GameObject("Points", typeof(RectTransform));
		dataHolder.transform.SetParent(graphContainer, false);
		dataHolderConnection.transform.SetParent(dataHolder.transform, false);
		dataHolderPoints.transform.SetParent(dataHolder.transform, false);
		RectTransform d = dataHolder.GetComponent<RectTransform>();
		d.anchorMin = new Vector2(0, 0);
		d.anchorMax = new Vector2(0, 0);
		d.pivot = new Vector2(0, 0);
		d.sizeDelta.Set(graphContainer.sizeDelta.x, graphContainer.sizeDelta.y);
		return d;
	}

	#endregion
}