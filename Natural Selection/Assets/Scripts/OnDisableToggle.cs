using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDisableToggle : MonoBehaviour
{
	private void OnDisable()
	{
		gameObject.GetComponent<Toggle>().isOn = false;
	}
}
