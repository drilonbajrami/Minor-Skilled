using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryThirstyState : State
{
	private GameObject _resource = null;
	private bool goingForFood = false;

	public HungryThirstyState() : base()
	{
		_stateName = "Hungry/Thirsty State";
		_resource = null;
		goingForFood = false;
	}

	public override void HandleState(Entity entity)
	{
		if (entity.Sight.SightArea.LowestUtilityValue < 0.0f)
			entity.ChangeState(new PrimaryState());

		if (!goingForFood)
		{
			_resource = entity.Memory.FindClosestResource(ResourceType.FOOD);
			if (_resource != null)
				goingForFood = true;
			else
				entity.SetDestination(entity.GetIdealRandomDestination(true));
		}
		else
		{
			if (_resource != null)
			{
				if (entity.CheckIfClose(_resource, entity.SightRadius))
				{
					if (!entity.Sight.CanSee(_resource))
					{
						entity.Memory.ForgetResource(_resource);
						_resource = null;
						goingForFood = false;
					}
					else if (entity.CheckIfClose(_resource, 0.5f))
						ConsumeFood(entity, _resource);
				}
			}
			else
			{
				goingForFood = false;
				entity.SetDestination(entity.GetIdealRandomDestination(true));
			}
		}

		//if (entity.Velocity() > 0.01f)
		//{
		//	entity.thirstiness += Time.deltaTime * entity.Velocity() / 10;
		//	entity.hungriness += Time.deltaTime * entity.Velocity() / 10;
		//}

		if (!entity.NeedsResources(50.0f))
			entity.ChangeState(new PrimaryState());

		//if (entity.predator != null)
		//	entity.ChangeState(new FleeState());
	}

	//private void ChooseEssentialResource(Entity entity)
	//{
	//	if (_resource == null)
	//	{
	//		if (entity.IsThirsty() && entity.IsHungry() && Mathf.Abs(entity.thirstiness - entity.hungriness) > 20)
	//		{
	//			if (entity.thirstiness > entity.hungriness && entity.Memory.KnowsAboutResource(ResourceType.WATER))
	//				_resource = entity.Memory.FindClosestResource(ResourceType.WATER); // Go for water
	//			else
	//				_resource = entity.Memory.FindClosestResource(ResourceType.FOOD); // Go for food

	//			GoForResource(entity, _resource);
	//		}
	//		else if (entity.IsThirsty())
	//		{
	//			_resource = entity.Memory.FindClosestResource(ResourceType.WATER);
	//			GoForResource(entity, _resource);
	//		}
	//		else if (entity.IsHungry())
	//		{
	//			_resource = entity.Memory.FindClosestResource(ResourceType.FOOD);
	//			GoForResource(entity, _resource);
	//		}
	//		else
	//			entity.ChangeState(new PrimaryState());
	//	}
	//}

	//private void GoForResource(Entity entity, GameObject resource)
	//{
	//	if (resource != null)
	//		entity.SetDestination(resource.transform.position);
	//	else if (resource == null)
	//		entity.SetDestination(TransformUtils.RandomTarget(entity.Transform, 20.0f, entity.FOV));
	//}

	//private float GetDistanceToResource(Entity entity)
	//{
	//	Vector3 deltaPos = entity.Transform.localPosition - entity.GetDestination();
	//	deltaPos.y = 0;
	//	return deltaPos.magnitude;
	//}

	//private void ConsumeResource(Entity entity, GameObject resource)
	//{
	//	if (resource == null)
	//		return;
	//	else {
	//		entity.Memory.ForgetResource(resource);
	//		//if (resource.GetComponent<Resource>().GetResourceType() == ResourceType.WATER)
	//		//	entity.thirstiness = 0;
	//		//else
	//		//	entity.hungriness = 0;

	//		resource.GetComponent<Resource>().Consume();
	//		_resource = null;
	//	}
	//}

	private void ConsumeFood(Entity entity, GameObject resource)
	{
		if (resource == null)
			return;
		else
		{
			entity.Memory.ForgetResource(resource);
			entity.Vitals.ReplenishResources(40.0f);
			resource.GetComponent<Resource>().Consume();
			_resource = null;
		}
	}
}
