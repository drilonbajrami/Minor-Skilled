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
}