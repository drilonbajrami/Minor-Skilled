using System;
using System.Collections;
using UnityEngine;

public class Cycle : MonoBehaviour
{
	public static event EventHandler CycleStart;
	public static event EventHandler CycleEnd;

	[Tooltip("Time scale")]
	[Range(0, 20)] [SerializeField] private float timeScale = 1;

	[Tooltip("Cycle duration in minutes")]
	[Range(1, 10)] [SerializeField] private int cycleDuration = 1;

	public static int cycleCount = 0;
	private bool coroutineStarted = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad1)) SpeedUpTime();
		else if (Input.GetKeyDown(KeyCode.Keypad2)) SlowDownTime();
	}

	// Cycle coroutine
	private IEnumerator StartCycle()
	{
		OnCycleStart();
		yield return new WaitForSeconds(cycleDuration * 60);
		OnCycleEnd();
		
	}

	// Notifiy subscribers that a new cycle has started
	protected virtual void OnCycleStart()
	{
		coroutineStarted = true;
		CycleStart?.Invoke(this, EventArgs.Empty);
	}

	// Notifiy subscribers that the ongoing cycle has ended
	protected virtual void OnCycleEnd()
	{
		CycleEnd?.Invoke(this, EventArgs.Empty);
		coroutineStarted = false;
		cycleCount++;
	}

	// Start Cycle Button
	private void OnGUI()
	{
		if (!coroutineStarted && EntitySpawner.ReadyToStartNewCycle && GUI.Button(new Rect(10, 10, 400, 100), $"Start Cycle ({cycleCount + 1})"))
		{
			StartCoroutine(StartCycle());
		}
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
}
