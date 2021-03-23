using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryState : State
{
	public HungryState()
	{
		_stateName = "Hunger State";
	}

	public override void HandleState()
	{
		if (_entity.waterToDrink != null && _entity.foodToEat != null)
		{
			if (_entity.hungriness > 50 && _entity.hungriness < 80 && _entity.thirstiness > 50)
			{
				// HERE
				if (Utility.ClosestTo(_entity.gameObject, _entity.foodToEat, _entity.waterToDrink) == _entity.waterToDrink)
					ChangeEntityState(new ThirstyState());
			}
		}

		if (_entity.thirstiness >= 100.0f || _entity.hungriness >= 100.0f)
			GameObject.Destroy(_entity.gameObject);
		else
		{
			if (_entity.foodToEat != null)
			{
				_entity.agent.SetDestination(_entity.foodToEat.transform.position);

				if (_entity.agent.velocity.magnitude < 0.1f)
					_entity.foodToEat = null;
				else if (Utility.CheckIfClose(_entity.transform, _entity.foodToEat.transform, 0.5f)) // HERE
				{
					GameObject.Destroy(_entity.foodToEat.gameObject);
					_entity.foodToEat = null;
					_entity.hungriness = 0;
					_entity.gameObject.GetComponent<SphereCollider>().enabled = false;
					ChangeEntityState(new RoamingState());
				}
			}
			else
			{
				if (_entity.agent.velocity.magnitude < 0.1f)
					_entity.agent.SetDestination(Utility.RandomTarget(_entity.GetPosition(), 20.0f)); // HERE
			}

			if (_entity.agent.velocity.magnitude > 0.1f)
			{
				_entity.thirstiness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
				_entity.hungriness += Time.deltaTime * _entity.agent.velocity.magnitude / 5;
			}
		}
	}
}
