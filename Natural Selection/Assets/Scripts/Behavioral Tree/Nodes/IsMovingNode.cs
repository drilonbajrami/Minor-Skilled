using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMovingNode : Node
{
	private Entity _entity;
	private float _minimalSpeed;
	private float _currentSpeed;

	public IsMovingNode(Entity entity, float minimalSpeed)
	{
		this._entity = entity;
		this._minimalSpeed = minimalSpeed;
	}

	public override NodeState Evaluate()
	{
		_currentSpeed = _entity.agent.velocity.magnitude;
		_entity.thirstiness += Time.deltaTime * _currentSpeed / 5;
		_entity.hungriness += Time.deltaTime * _currentSpeed / 5;
		return _entity.agent.velocity.magnitude <= _minimalSpeed ? NodeState.SUCCESS : NodeState.FAILURE;
	}
}
