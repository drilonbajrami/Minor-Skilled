using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeResourceNode : Node
{
	private Entity _entity;

	public ConsumeResourceNode(Entity entity)
	{
		_entity = entity;
	}

	public override NodeState Evaluate()
	{
		GameObject resource;
		if (_entity.waterToDrink != null && _entity.foodToEat != null)
			resource = Utility.ClosestTo(_entity.gameObject, _entity.waterToDrink, _entity.foodToEat);
		else if (_entity.waterToDrink != null)
			resource = _entity.waterToDrink;
		else
			resource = _entity.foodToEat;

		_entity.agent.SetDestination(resource.transform.position);
		if (Utility.CheckIfClose(_entity.transform, resource.transform, 0.5f))
		{
			if (resource.tag == "Water")
			{
				GameObject.Destroy(_entity.waterToDrink.gameObject);
				_entity.waterToDrink = null;
				_entity.thirstiness = 0;
				_entity.gameObject.GetComponent<SphereCollider>().enabled = false;
				return NodeState.SUCCESS;
			}
			else
			{
				GameObject.Destroy(_entity.foodToEat.gameObject);
				_entity.foodToEat = null;
				_entity.hungriness = 0;
				_entity.gameObject.GetComponent<SphereCollider>().enabled = false;
				return NodeState.SUCCESS;
			}
		}
		else
		{
			return NodeState.RUNNING;
		}
	}
}
