using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrimaryState : State
{
	private enum SubState { ROAMING, IDLE }
	private SubState _currentSubState = SubState.ROAMING;

	public PrimaryState() : base()
	{
		//_entity.isOnReproducingState = false;
		//_entity.fleeing = false;
		_stateName = "Primary State";
		//_entity.isMating = false;
	}

	public override void HandleState(Entity entity)
	{
		if (SceneManager.GetActiveScene().buildIndex == 3)
		{
			if (entity.predator != null)
				entity.ChangeState(new FleeState());

			if (entity.order == Order.CARNIVORE && IsHungryOrThirsty(entity, 50.0f))
				entity.ChangeState(new ChaseState());

			switch (_currentSubState)
			{
				case SubState.ROAMING:
					RoamingSubState(entity);
					break;
				case SubState.IDLE:
					IdleSubState(entity);
					break;
				default:
					break;
			}
		}
		else if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			if (entity.order == Order.HERBIVORE && IsHungryOrThirsty(entity, 50.0f))
				entity.ChangeState(new HungryThirstyState());

			switch (_currentSubState)
			{
				case SubState.ROAMING:
					RoamingSubState(entity);
					break;
				case SubState.IDLE:
					IdleSubState(entity);
					break;
				default:
					break;
			}
		}
		else if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			if (Input.GetKeyDown(KeyCode.M))
			{
					if (entity.gender == Gender.FEMALE && entity.gestationDuration <= 0.0f)
						entity.ChangeState(new ReproduceState());
					else if (entity.gender == Gender.MALE && entity.maleReproductionDuration <= 0.0)
						entity.ChangeState(new ReproduceState());
			}

			//if (ResourcesAreSufficient(30.0f))
			//{
			//	if (_entity.gender == Gender.FEMALE && _entity.gestationDuration <= 0.0f)
			//		ChangeEntityState(new ReproduceState(_entity));
			//	else if (_entity.gender == Gender.MALE && _entity.maleReproductionDuration <= 0.0)
			//		ChangeEntityState(new ReproduceState(_entity));
			//}

			switch (_currentSubState)
			{
				case SubState.ROAMING:
					RoamingSubState(entity);
					break;
				case SubState.IDLE:
					IdleSubState(entity);
					break;
				default:
					break;
			}
		}
		else
		{
			if (ResourcesAreSufficient(entity, 30.0f))
			{
				if (entity.gender == Gender.FEMALE && entity.gestationDuration <= 0.0f)
					entity.ChangeState(new ReproduceState());
				else if (entity.gender == Gender.MALE && entity.maleReproductionDuration <= 0.0)
					entity.ChangeState(new ReproduceState());
			}

			if (entity.order == Order.HERBIVORE && IsHungryOrThirsty(entity, 50.0f))
				entity.ChangeState(new HungryThirstyState());

			switch (_currentSubState)
			{
				case SubState.ROAMING:
					RoamingSubState(entity);
					break;
				case SubState.IDLE:
					IdleSubState(entity);
					break;
				default:
					break;
			}

			if (entity.predator != null)
				entity.ChangeState(new FleeState());

			if (entity.order == Order.CARNIVORE && IsHungryOrThirsty(entity, 50.0f))
				entity.ChangeState(new ChaseState());
		}
	}

	private void RoamingSubState(Entity entity)
	{
		//if (IsHungryOrThirsty(50.0f))
		//{
		//	ChangeEntityState(new HungryThirstyState(_entity));
		//}

		if (entity.IsStuck() || !entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV);
			entity.SetDestination(i);
		}

		if (entity.Velocity() > 0.01f)
		{
			entity.thirstiness += Time.deltaTime * entity.Velocity() / 5;
			entity.hungriness += Time.deltaTime * entity.Velocity() / 5;
		}

		//if (ResourcesAreSufficient(20.0f))
		//	_currentSubState = SubState.IDLE;
	}

	private void IdleSubState(Entity entity)
	{
		entity.thirstiness += Time.deltaTime * 5 / 20;
		entity.hungriness += Time.deltaTime * 5 / 20;

		if (entity.thirstiness > 30.0f || entity.hungriness > 30.0f)
			_currentSubState = SubState.ROAMING;
	}

	private bool ResourcesAreSufficient(Entity entity, float threshold)
	{
		return entity.thirstiness < threshold && entity.hungriness < threshold;
	}

	private bool IsHungryOrThirsty(Entity entity, float threshold)
	{
		return entity.thirstiness > threshold || entity.hungriness > threshold;
	}
}