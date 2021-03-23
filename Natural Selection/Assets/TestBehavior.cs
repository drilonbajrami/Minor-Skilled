using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBehavior : MonoBehaviour
{
	public NavMeshAgent agent;

	bool hungry = false;
	bool thirsty = false;
	bool roam = true;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			thirsty = true; roam = false; hungry = false;
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			hungry = true; roam = false; thirsty = false;
		}

		if (roam)
		{
			// agent.hasPath to check if currently moving or not
			if (!agent.hasPath)
			{
				Debug.Log("Setting Path");
				agent.SetDestination(Random.insideUnitSphere * 200);
				Debug.Log("Path set");
			}
		}

		if (thirsty)
			SearchForWater();

		if (hungry)
		{
			Chase();
		}
	}

	private void SearchForWater()
	{
		GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		foreach (GameObject go in waters)
		{
			float curDistance = Vector3.Distance(go.transform.position, transform.position);
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}

		agent.SetDestination(closest.transform.position);

		if (Vector3.Distance(gameObject.transform.position, closest.transform.position) < 10.0f)
			thirsty = false; roam = true;
	}

	public void Chase()
	{
		if (gameObject.tag == "Carnivore")
		{
			
			GameObject bunny = GameObject.FindGameObjectWithTag("Herbivore");
			if (bunny != null)
			{
				if (Vector3.Distance(gameObject.transform.position, bunny.transform.position) < 1000.0f)
				{
					agent.speed = 6;
					agent.SetDestination(bunny.transform.position);
				}
			}

			if (Vector3.Distance(gameObject.transform.position, bunny.transform.position) < 4.0f)
			{
				DestroyImmediate(bunny.gameObject);
				hungry = false; thirsty = true;
				agent.speed = 3.5f;
			}
		}
		else
			roam = true; hungry = false;
	}
}
