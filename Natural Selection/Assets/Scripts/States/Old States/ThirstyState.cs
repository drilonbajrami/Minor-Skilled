//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ThirstyState : State
//{
//	public ThirstyState()
//	{
//		_stateName = "Thirst State";
//	}

//	public override void HandleState()
//	{
//		if (_entity.waterToDrink != null && _entity.foodToEat != null)
//		{
//			if (_entity.thirstiness > 50 && _entity.thirstiness < 80 && _entity.hungriness > 50)
//			{
//				// HERE
//				if (TransformUtils.ClosestTo(_entity.gameObject, _entity.waterToDrink, _entity.foodToEat) == _entity.foodToEat)
//					ChangeEntityState(new HungryState());
//			}
//		}

//		if (_entity.thirstiness >= 100.0f || _entity.hungriness >= 100.0f)
//			GameObject.Destroy(_entity.gameObject);
//		else
//		{
//			if (_entity.waterToDrink != null)
//			{
//				_entity.agent.SetDestination(_entity.waterToDrink.transform.position);

//				if (_entity.agent.velocity.magnitude < 0.1f)
//					_entity.waterToDrink = null;
//				else if (TransformUtils.CheckIfClose(_entity.transform, _entity.waterToDrink.transform, 0.5f)) // HERE
//				{
//					GameObject.Destroy(_entity.waterToDrink.gameObject);
//					_entity.waterToDrink = null;
//					_entity.thirstiness = 0;
//					_entity.gameObject.GetComponent<SphereCollider>().enabled = false;
//					ChangeEntityState(new RoamingState());
//				}
//			}
//			else
//			{
//				if (_entity.agent.velocity.magnitude < 0.1f)
//					_entity.agent.SetDestination(TransformUtils.RandomTarget(_entity.GetPosition(), 20.0f));// HERE
//			}

//			if (_entity.agent.velocity.magnitude > 0.1f)
//			{
//				_entity.thirstiness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
//				_entity.hungriness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
//			}
//		}
//	}
//}
