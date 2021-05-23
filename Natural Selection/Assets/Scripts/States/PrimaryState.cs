using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrimaryState : State
{
	private enum SubState { ROAMING, IDLE }
	private SubState _currentSubState = SubState.ROAMING;

	private GameObject prey;
	private float hungerTimer = 30.0f;
	private float timer;
	private bool chasing = false;
	private bool hungry = false;

	public PrimaryState() : base()
	{
		//_entity.isOnReproducingState = false;
		//_entity.fleeing = false;
		_stateName = "Primary State";
		//_entity.isMating = false;
		timer = hungerTimer;
	}

	public override void HandleState(Entity entity)
	{
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

	private void RoamingSubState(Entity entity)
	{
		//if (chasing == false && hungry == false && entity.order == Order.CARNIVORE)
		//{
		//	timer -= Time.deltaTime;
		//	if (timer < 0.0f)
		//	{
		//		timer = hungerTimer;
		//		hungry = true;
		//	}
		//}

		//if (entity.order == Order.CARNIVORE && hungry == true && chasing == false)
		//{
		//	prey = entity.Sight.ChoosePrey();
		//	if (prey != null)
		//	{
		//		chasing = true;
		//		entity.Stop();
		//	}
		//}

		//if (prey == null && hungry == true)
		//{
		//	chasing = false;
		//}

		//if (prey != null && !entity.HasPath())
		//{
		//	entity.SetDestination(prey.transform.position);
		//}

		//if (prey != null)
		//{
		//	if (TransformUtils.CheckIfClose(entity.GetTransform(), prey.transform, 3.0f))
		//	{
		//		prey.GetComponent<Entity>().Die();
		//		prey = null;
		//		chasing = false;
		//		hungry = false;
		//	}
		//}

		//if (IsHungryOrThirsty(50.0f))
		//{
		//	ChangeEntityState(new HungryThirstyState(_entity));
		//}
		//float a = Random.Range(0, 10);

		//if (a < 5 && entity.order == Order.HERBIVORE)
		//{
		//	float bestUtilAngle = entity.Sight.EvaluateUtilities();
		//	Vector3 i = TransformUtils.UtilityRandomTarget(entity.GetTransform(), 10.0f, bestUtilAngle, entity.Sight.sightArea.halfAngle);
		//	entity.SetDestination(i);
		//}
		if ((entity.IsStuck() || !entity.HasPath())/* && chasing == false*/)
		{
			float bestUtilAngle = entity.Sight.EvaluateUtilities();
			Vector3 i = TransformUtils.UtilityRandomTarget(entity.Transform, 20.0f, bestUtilAngle, entity.Sight.sightArea.halfAngle);
			entity.SetDestination(i);
		}

		if (entity.Velocity() > 0.01f)
		{
			//entity.thirstiness += Time.deltaTime * entity.Velocity() / 5;
			//entity.hungriness += Time.deltaTime * entity.Velocity() / 5;
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