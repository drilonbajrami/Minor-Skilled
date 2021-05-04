using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
	private string _currentSceneName;

	private void Start()
	{
		_currentSceneName = SceneManager.GetActiveScene().name;
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(_currentSceneName);
		}
    }
}
