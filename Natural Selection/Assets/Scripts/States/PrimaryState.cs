using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryState : State
{
	private SubState _currentSubState;
	private string _subStateName;

	public PrimaryState()
	{
		_stateName = "Primary State";
	}

	public override void HandleState()
	{
		switch (_currentSubState)
		{
			case SubState.ROAMING:
				RoamingSubState();
				break;
			case SubState.IDLE:
				IdleSubState();
				break;
			default:
				break;
		}
	}

	private void RoamingSubState()
	{
		_subStateName = "Roaming SubState";

		if (IsHungryOrThirsty(50.0f))
		{
			_entity.TurnOnSense();
			ChangeEntityState(new SearchResourcesState());
		}

		if (NotMovingOrNoPath())
		{
			Vector3 i = Utility.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV);
			_entity.SetDestination(i);
			//GameObject.Instantiate(_entity.beacon, i, Quaternion.identity);
		}

		if (_entity.Velocity() > 0.01f)
		{
			_entity.thirstiness += Time.deltaTime * _entity.Velocity() / 5;
			_entity.hungriness += Time.deltaTime * _entity.Velocity() / 5;
		}

		if (ResourcesAreSufficient(20.0f))
			_currentSubState = SubState.IDLE;
	}

	private void IdleSubState()
	{
		_subStateName = "Idle SubState";

		_entity.thirstiness += Time.deltaTime * 5 / 20;
		_entity.hungriness += Time.deltaTime * 5 / 20;

		if (_entity.thirstiness > 30.0f || _entity.hungriness > 30.0f)
			_currentSubState = SubState.ROAMING;
	}

	private bool ResourcesAreSufficient(float threshold)
	{
		return _entity.thirstiness < threshold && _entity.hungriness < threshold;
	}

	private bool IsHungryOrThirsty(float threshold)
	{
		return _entity.thirstiness > threshold || _entity.hungriness > threshold;
	}

	private bool NotMovingOrNoPath()
	{
		return _entity.Velocity() < _entity.minVelocity || !_entity.HasPath();
	}

	private string GetSubStateName()
	{
		return _subStateName;
	}	
}

public enum SubState
{
	ROAMING, IDLE
}
