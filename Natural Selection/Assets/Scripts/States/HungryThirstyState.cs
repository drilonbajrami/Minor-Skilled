using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryThirstyState : State
{
	private GameObject _resource = null;

	public HungryThirstyState() : base()
	{
		_stateName = "Hungry/Thirsty State";
	}

	public override void HandleState(Entity entity)
	{
		if (entity.Velocity() > 0.01f)
		{
			entity.thirstiness += Time.deltaTime * entity.Velocity() / 10;
			entity.hungriness += Time.deltaTime * entity.Velocity() / 10;
		}

		ChooseEssentialResource(entity);

		if (_resource != null)
		{
			float distance = GetDistanceToResource(entity);

			if (distance < entity.sightRadius)
			{
				if (!entity.Sight.CanSee(_resource))
				{
					entity.Memory.ForgetResource(_resource);
					_resource = null;
				}
				else if (distance < 0.5f)
					ConsumeResource(entity, _resource);
			}
		}

		if (entity.predator != null)
			entity.ChangeState(new FleeState());
	}

	private void ChooseEssentialResource(Entity entity)
	{
		if (_resource == null) {
			if (entity.IsThirsty() && entity.IsHungry() && Mathf.Abs(entity.thirstiness - entity.hungriness) > 20) 
			{
				if (entity.thirstiness > entity.hungriness && entity.Memory.KnowsAboutResource(ResourceType.WATER))
					_resource = entity.Memory.FindClosestResource(ResourceType.WATER); // Go for water
				else
					_resource = entity.Memory.FindClosestResource(ResourceType.FOOD); // Go for food

				GoForResource(entity, _resource);
			}
			else if (entity.IsThirsty()) {
				_resource = entity.Memory.FindClosestResource(ResourceType.WATER);
				GoForResource(entity, _resource);
			}
			else if (entity.IsHungry()) {
				_resource = entity.Memory.FindClosestResource(ResourceType.FOOD);
				GoForResource(entity, _resource);
			}
			else
				entity.ChangeState(new PrimaryState());
		}
	}

	private void GoForResource(Entity entity, GameObject resource)
	{
		if (resource != null)
			entity.SetDestination(resource.transform.position);
		else if (resource == null)
			entity.SetDestination(TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV));
	}

	private float GetDistanceToResource(Entity entity)
	{
		Vector3 deltaPos = entity.GetPosition() - entity.GetDestination();
		deltaPos.y = 0;
		return deltaPos.magnitude;
	}

	private void ConsumeResource(Entity entity, GameObject resource)
	{
		if (resource == null)
			return;
		else {
			entity.Memory.ForgetResource(resource);
			if (resource.GetComponent<Resource>().GetResourceType() == ResourceType.WATER)
				entity.thirstiness = 0;
			else
				entity.hungriness = 0;

			resource.GetComponent<Resource>().Consume();
			_resource = null;
		}
	}
}
