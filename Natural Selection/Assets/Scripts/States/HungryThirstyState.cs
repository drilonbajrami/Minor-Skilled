using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryThirstyState : State
{
	private GameObject _resource = null;

	public HungryThirstyState(Entity entity) : base(entity)
	{
		_stateName = "Hungry/Thirsty State";
	}

	public override void HandleState()
	{
		if (_entity.Velocity() > 0.01f)
		{
			_entity.thirstiness += Time.deltaTime * _entity.Velocity() / 10;
			_entity.hungriness += Time.deltaTime * _entity.Velocity() / 10;
		}

		ChooseEssentialResource();

		if (_resource != null)
		{
			float distance = GetDistanceToResource();

			if (distance < _entity.sightRadius)
			{
				if (!_entity.Sight.CanSee(_resource))
				{
					_entity.Memory.ForgetResource(_resource);
					_resource = null;
				}
				else if (distance < 0.5f)
					ConsumeResource(_resource);
			}
		}

		if (_entity.predator != null)
			ChangeEntityState(new FleeState(_entity));
	}

	private void ChooseEssentialResource()
	{
		if (_resource == null) {
			if (_entity.IsThirsty() && _entity.IsHungry() && Mathf.Abs(_entity.thirstiness - _entity.hungriness) > 20) 
			{
				if (_entity.thirstiness > _entity.hungriness && _entity.Memory.KnowsAboutResource(ResourceType.WATER))
					_resource = _entity.Memory.FindClosestResource(ResourceType.WATER); // Go for water
				else
					_resource = _entity.Memory.FindClosestResource(ResourceType.FOOD); // Go for food

				GoForResource(_resource);
			}
			else if (_entity.IsThirsty()) {
				_resource = _entity.Memory.FindClosestResource(ResourceType.WATER);
				GoForResource(_resource);
			}
			else if (_entity.IsHungry()) {
				_resource = _entity.Memory.FindClosestResource(ResourceType.FOOD);
				GoForResource(_resource);
			}
			else
				ChangeEntityState(new PrimaryState(_entity));
		}
	}

	private void GoForResource(GameObject resource)
	{
		if (resource != null)
			_entity.SetDestination(resource.transform.position);
		else if (resource == null)
			_entity.SetDestination(TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
	}

	private float GetDistanceToResource()
	{
		Vector3 deltaPos = _entity.GetPosition() - _entity.GetDestination();
		deltaPos.y = 0;
		return deltaPos.magnitude;
	}

	private void ConsumeResource(GameObject resource)
	{
		if (resource == null)
			return;
		else {
			_entity.Memory.ForgetResource(resource);
			if (resource.GetComponent<Resource>().GetResourceType() == ResourceType.WATER)
				_entity.thirstiness = 0;
			else
				_entity.hungriness = 0;

			resource.GetComponent<Resource>().Consume();
			_resource = null;
		}
	}
}
