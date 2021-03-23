using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    private float timeSpan = 2;

	private void Update()
	{
		timeSpan -= Time.deltaTime;
		if (timeSpan < 0.0f)
			Destroy(this.gameObject);
	}
}
