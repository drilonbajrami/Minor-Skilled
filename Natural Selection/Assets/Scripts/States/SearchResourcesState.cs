using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResourcesState : State
{
	public SearchResourcesState()
	{
		_stateName = "Search Resources State";
	}

	public override void HandleState()
	{
		if (IsWaterWithinSight() || IsFoodWithinSight())
			ChangeEntityState(new EatDrinkState());
		else if (!_entity.HasPath() || _entity.Velocity() < _entity.minVelocity)
			_entity.SetDestination(Utility.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));

		IncreaseHungerThirstWhenMoving();
	}

	private void IncreaseHungerThirstWhenMoving()
	{
		if (_entity.agent.velocity.magnitude > 0.01f)
		{
			_entity.thirstiness += Time.deltaTime * _entity.Velocity() / 8;
			_entity.hungriness += Time.deltaTime * _entity.Velocity() / 8;
		}
	}

	private bool IsFoodWithinSight()
	{
		return _entity.foodToEat != null ? true : false;
	}

	private bool IsWaterWithinSight()
	{
		return _entity.waterToDrink != null ? true : false;
	}
}
