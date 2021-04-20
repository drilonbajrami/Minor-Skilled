using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
	private bool lookingAway = false;

	public FleeState(Entity entity) : base(entity)
	{
		_stateName = "Flee State";
		lookingAway = false;
		_entity.fleeing = true;
		//_entity.fleeing = true;
		//_entity.SetSkinColor(EntityGenderColor.FLEEING);
		_entity.DangerColor();	// CHANGE COLOR
	}

	public override void HandleState()
	{
		Flee();
		CheckIfSafe();
	}

	public void Flee()
	{
		if (!lookingAway)
		{
			_entity.transform.LookAt(_entity.GetPosition() - (_entity.predator.transform.position - _entity.GetPosition()));
			lookingAway = true;
		}
		else
		{
			_entity.IncreaseMaxSpeed();
			_entity.SetDestination(TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
		}
	}

	private void CheckIfSafe()
	{
		if (_entity.predator == null)
		{
			_entity.DecreaseMaxSpeed();
			_entity.ResetColor(); // CHANGE COLOR
			ChangeEntityState(new PrimaryState(_entity));
		}
	}
}