using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryState : State
{
	private enum SubState { ROAMING, IDLE }
	private SubState _currentSubState = SubState.ROAMING;

	public PrimaryState(Entity entity) : base(entity)
	{
		_entity.isOnReproducingState = false;
		_entity.fleeing = false;
		_stateName = "Primary State";
		_entity.isMating = false;
	}

	public override void HandleState()
	{
		if (ResourcesAreSufficient(30.0f))
		{
			if(_entity.gender == Gender.FEMALE && _entity.gestationDuration <= 0.0f)
				ChangeEntityState(new ReproduceState(_entity));
			else if (_entity.gender == Gender.MALE && _entity.maleReproductionDuration <= 0.0)
				ChangeEntityState(new ReproduceState(_entity));
		}

		if (_entity.order == Order.HERBIVORE && IsHungryOrThirsty(50.0f))
			ChangeEntityState(new HungryThirstyState(_entity));

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

		if (_entity.predator != null)
			ChangeEntityState(new FleeState(_entity));

		if (_entity.order == Order.CARNIVORE && IsHungryOrThirsty(50.0f))
			ChangeEntityState(new ChaseState(_entity));
	}

	private void RoamingSubState()
	{
		if (IsHungryOrThirsty(50.0f))
		{
			ChangeEntityState(new HungryThirstyState(_entity));
		}

		if (_entity.IsStuck() || !_entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV);
			_entity.SetDestination(i);
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
}