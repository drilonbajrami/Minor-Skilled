using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0, 20)]
    [SerializeField] private float timeScale = 1;

	private void OnValidate()
	{
        timeScale = Mathf.Round(timeScale * 2) / 2;
        Time.timeScale = timeScale;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Keypad1)) SpeedUpTime();
		else if (Input.GetKeyDown(KeyCode.Keypad2)) SlowDownTime();
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