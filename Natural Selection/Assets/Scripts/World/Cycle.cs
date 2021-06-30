using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cycle : MonoBehaviour
{
	[SerializeField] private Button startCycleButton;

	public static event EventHandler CycleStart;
	public static event EventHandler CycleEnd;

	[Tooltip("Time scale")]
	[Range(0, 20)] [SerializeField] private float timeScale = 1;

	[Tooltip("Cycle duration in minutes")]
	[Range(1, 10)] [SerializeField] private int cycleDuration = 1;

	[SerializeField] private bool autoStartCycle = false;

	public static int cycleCount = 0;

	private void Start()
	{
		startCycleButton.onClick.AddListener(delegate { StartCoroutine(StartCycle()); });
		startCycleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Cycle: 1";
	}

	private void Update()
	{
		TimeControls();
	}

	// Cycle coroutine
	private IEnumerator StartCycle()
	{		
		OnCycleStart();
		yield return new WaitForSeconds(cycleDuration * 60);
		OnCycleEnd();
		startCycleButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Start Cycle: {cycleCount+1}";
		StartNewCycle();
	}

	// Notifiy subscribers that a new cycle has started
	protected virtual void OnCycleStart()
	{
		startCycleButton.interactable = false;
		CycleStart?.Invoke(this, EventArgs.Empty);
	}

	// Notifiy subscribers that the ongoing cycle has ended
	protected virtual void OnCycleEnd()
	{	
		CycleEnd?.Invoke(this, EventArgs.Empty);
		cycleCount++;
		startCycleButton.interactable = true;
	}

	private void StartNewCycle() // Maybe change condition when to stop?!?!?
	{
		if (cycleCount == 50 || Counter.Instance.AllSpeciesExtinct() || Counter.Instance.HerbivoresExtinct())
			autoStartCycle = false;
		if (autoStartCycle)
			StartCoroutine(StartCycle());
	}
	//====================================================================================================//
	//														Time Controls
	//====================================================================================================//
	private void OnValidate()
	{
		timeScale = Mathf.Round(timeScale * 2) / 2;
		Time.timeScale = timeScale;
	}

	void SpeedUpTime()
	{
		timeScale += 1;
		timeScale = Mathf.Clamp(timeScale, 0, 20);
		Time.timeScale = timeScale;
	}

	void SlowDownTime()
	{
		timeScale -= 1;
		timeScale = Mathf.Clamp(timeScale, 0, 20);
		Time.timeScale = timeScale;
	}

	void TimeControls()
	{
		if (Input.GetKeyDown(KeyCode.Keypad1)) SpeedUpTime();
		else if (Input.GetKeyDown(KeyCode.Keypad2)) SlowDownTime();
	}
}