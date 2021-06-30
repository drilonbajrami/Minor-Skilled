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
		{
			entity.Run();
			if(!entity.HasPath())
				entity.SetDestination(entity.GetIdealRandomDestination(false));
		}
		else if (!goingForFood)
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
				if (!entity.HasPath())
					entity.SetDestination(_resource.transform.position);

				if (entity.CheckIfClose(_resource, entity.SightRadius))
				{
					if (!_resource.activeSelf)
					{
						entity.Memory.ForgetResource(_resource);
						_resource = null;
						goingForFood = false;
					}
					else if (entity.CheckIfClose(_resource, 0.5f))
						ConsumeFood(entity);
				}
			}
			else
			{
				goingForFood = false;
				_resource = null;
				entity.SetDestination(entity.GetIdealRandomDestination(true));
			}
		}

		if (!entity.NeedsToEat())
			entity.ChangeState(new PrimaryState());
	}

	private void ConsumeFood(Entity entity)
	{
		if (_resource == null)
			return;
		else
		{
			entity.Memory.ForgetResource(_resource);
			entity.Vitals.RecoverEnergy(40.0f);
			_resource.GetComponent<Resource>().Consume();
			_resource = null;
		}
	}
}
