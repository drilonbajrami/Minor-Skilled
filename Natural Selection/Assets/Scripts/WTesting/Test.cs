using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private float timeScale = 1;

	float a = 0;

	private void Update()
	{
		TimeControls();

		if (Input.GetKeyDown(KeyCode.E))
			a = 0;

		if (a < 1)
		{
			a += Time.deltaTime;
			Debug.Log(a);
		}
	}

	void SpeedUpTime()
	{
		timeScale += 1;
		timeScale = Mathf.Clamp(timeScale, 0, 20);
		Debug.Log($"Time scale: { timeScale }");
		Time.timeScale = timeScale;
	}

	void SlowDownTime()
	{
		timeScale -= 1;
		timeScale = Mathf.Clamp(timeScale, 0, 20);
		Debug.Log($"Time scale: { timeScale }");
		Time.timeScale = timeScale;
	}

	void TimeControls()
	{
		if (Input.GetKeyDown(KeyCode.Keypad1)) SpeedUpTime();
		else if (Input.GetKeyDown(KeyCode.Keypad2)) SlowDownTime();
	}
}