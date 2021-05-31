using System;
using System.Collections;
using UnityEngine;

public class Cycle : MonoBehaviour
{
	public static event EventHandler CycleStart;
	public static event EventHandler CycleEnd;

	public static int cycleCount = 0;

	[Tooltip("Cycle duration in minutes")]
	[Range(1, 10)] public int cycleDuration = 1;

	bool coroutineStarted = false;

	private IEnumerator StartCycle()
	{
		coroutineStarted = true;
		OnCycleStart();
		yield return new WaitForSeconds(cycleDuration * 60);
		OnCycleEnd();
		cycleCount++;
	}

	protected virtual void OnCycleStart()
	{
		//Debug.Log("Cycle started");
		CycleStart?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnCycleEnd()
	{
		//Debug.Log("Cycle ended");
		CycleEnd?.Invoke(this, EventArgs.Empty);
		coroutineStarted = false;
	}

	private void OnGUI()
	{
		if (!coroutineStarted && EntitySpawner.ReadyToStartNewCycle && GUI.Button(new Rect(10, 10, 130, 30), $"Start Cycle ({cycleCount + 1})"))
		{
			StartCoroutine(StartCycle());
		}
	}
}
