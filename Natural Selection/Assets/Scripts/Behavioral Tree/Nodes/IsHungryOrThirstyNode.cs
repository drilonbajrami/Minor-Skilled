using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHungryOrThirstyNode : Node
{
	private Entity _entity;

	public IsHungryOrThirstyNode(Entity entity)
	{
		_entity = entity;
	}

	public override NodeState Evaluate()
	{
		if (_entity.thirstiness >= 50.0f || _entity.hungriness >= 50.0f)
			_entity.gameObject.GetComponent<SphereCollider>().enabled = true;
			
		return _entity.waterToDrink != null || _entity.foodToEat != null ?  NodeState.FAILURE : NodeState.SUCCESS;
	}
}
