using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintID : MonoBehaviour
{
    public GameObject go;

    public MemoryData a;

	private void Start()
	{
        a = new MemoryData(go);
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log($"Is active: {a.Object.activeSelf}");
    }
}
