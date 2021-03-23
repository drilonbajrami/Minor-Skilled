using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesNearNode : Node
{
	private Entity _entity;

	public ResourcesNearNode(Entity entity)
	{
		_entity = entity;
	}

	public override NodeState Evaluate()
	{
		return _entity.waterToDrink != null || _entity.foodToEat != null ? NodeState.SUCCESS : NodeState.FAILURE;
	}
}
