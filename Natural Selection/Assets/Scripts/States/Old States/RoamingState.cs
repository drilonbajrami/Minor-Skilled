//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class RoamingState : State
//{
//	public RoamingState()
//	{
//		_stateName = "Roam State";
//	}

//	public override void HandleState()
//	{
//		CheckIfThirsty();
//		CheckIfHungry();
//		Roam();	
//	}

//	private void Roam()
//	{
//		if (_entity.agent.velocity.magnitude < 0.1f)
//			_entity.agent.SetDestination(TransformUtils.RandomTarget(_entity.GetPosition(), 20.0f));

//		if (_entity.agent.velocity.magnitude > 0.1f)
//		{
//			_entity.thirstiness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
//			_entity.hungriness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
//		}
//	}

//	private void CheckIfThirsty()
//	{
//		if (_entity.thirstiness > 50.0f) 
//		{
//			_entity.gameObject.GetComponent<SphereCollider>().enabled = true;
//			ChangeEntityState(new ThirstyState());
//		}
//	}

//	private void CheckIfHungry()
//	{
//		if (_entity.hungriness > 50.0f)
//		{
//			_entity.gameObject.GetComponent<SphereCollider>().enabled = true;
//			ChangeEntityState(new HungryState());
//		}
//	}

//	// Old code

//	//Vector3 destination = new Vector3();
//	//bool moving = false;
//	//public override void HandleState()
//	//{
//		//if (_entity.thirsty)
//		//	_entity.ChangeState(new ThirstyState());
//		//else if (_entity.hungry)
//		//	_entity.ChangeState(new HungryState());

//		//if (!moving)
//		//{
//		//	destination = GetRandomDestination();
//		//	moving = true;
//		//}
//		//else
//		//{
//		//	Move(destination);
//		//}
//	//}

//	//private Vector3 GetRandomDestination()
//	//{
//	//	Vector3 position = Random.insideUnitSphere * 100;
//	//	position.y = _entity.transform.localScale.y / 2;

//	//	if (position.x <= _entity.area.bounds.min.x || position.x >= _entity.area.bounds.max.x ||
//	//		position.z <= _entity.area.bounds.min.z || position.z >= _entity.area.bounds.max.z)
//	//		return position = GetRandomDestination();
//	//	else
//	//		return position;
//	//}

//	//private void Move(Vector3 destination)
//	//{
//	//	_entity.transform.position = Vector3.Lerp(_entity.transform.position, destination, 3 * Time.deltaTime);
//	//	//if (Vector3.Distance(_entity.transform.position, destination) < 0.01f)
//	//	//moving = false;

//	//	if (Mathf.Abs(_entity.transform.position.x - destination.x) < 0.05f && Mathf.Abs(_entity.transform.position.z - destination.z) < 0.015f)
//	//		moving = false;
//	//}
//}
