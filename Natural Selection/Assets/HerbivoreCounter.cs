using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerbivoreCounter : MonoBehaviour
{
    public static int counter = 0;

	private void Start()
	{
		counter = 70;
	}

	void Update()
    {
        gameObject.GetComponent<Text>().text = "Herbivore Count: " + counter.ToString();
    }
}
