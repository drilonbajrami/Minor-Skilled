using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatDrinkState : State
{
	private Transform _resource;

	public EatDrinkState()
	{
		_stateName = "Eat & Drink State";
	}

	public override void HandleState()
	{
		if (_entity.waterToDrink != null && _entity.foodToEat != null)
		{
			if (_entity.thirstiness > _entity.hungriness)
				GoForResource(_entity.waterToDrink);
			else
				GoForResource(_entity.foodToEat);
		}
		else if (_entity.waterToDrink != null)
			GoForResource(_entity.waterToDrink);
		else if (_entity.foodToEat != null)
			GoForResource(_entity.foodToEat);

		if ((IsWaterAvailable() || IsFoodAvailable()) && Utility.CheckIfClose(_entity.GetTransform(), _resource, 0.5f))
		{
			if (_resource.tag == "Water")
			{
				GameObject.Destroy(_entity.waterToDrink.gameObject);
				_entity.waterToDrink = null;
				_entity.thirstiness = 0;
				_resource = null;
			}
			else
			{
				GameObject.Destroy(_entity.foodToEat.gameObject);
				_entity.foodToEat = null;
				_entity.hungriness = 0;
				_resource = null;
			}

			if (_entity.hungriness < 50.0f && _entity.thirstiness < 50.0f)
			{
				_entity.TurnOffSense();
				ChangeEntityState(new PrimaryState());
			}
		}
		else
			ChangeEntityState(new SearchResourcesState());
	}

	private void GoForFood()
	{
		_resource = _entity.foodToEat.transform;
		if (!_entity.agent.hasPath && _entity.foodToEat != null)
			_entity.SetDestination(_resource.position);
		else if (_entity.agent.hasPath && _entity.foodToEat == null)
			_entity.SetDestination(Utility.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
	}

	private void GoForWater()
	{
		_resource = _entity.waterToDrink.transform;
		if (!_entity.agent.hasPath && _entity.waterToDrink != null)
			_entity.SetDestination(_resource.position);
		else if (_entity.agent.hasPath && _entity.waterToDrink == null)
			_entity.SetDestination(Utility.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
	}

	private void GoForResource(GameObject resource)
	{
		_resource = resource.transform;
		if (!_entity.agent.hasPath && resource != null)
			_entity.SetDestination(_resource.position);
		else if (_entity.HasPath() && resource == null)
			_entity.SetDestination(Utility.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
	}

	private bool IsFoodAvailable()
	{
		return _entity.foodToEat != null ? true : false;
	}

	private bool IsWaterAvailable()
	{
		return _entity.waterToDrink != null ? true : false;
	}
}
