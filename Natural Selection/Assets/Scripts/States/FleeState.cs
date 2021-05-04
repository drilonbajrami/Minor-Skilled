using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
	private bool lookingAway = false;

	public FleeState() : base()
	{
		_stateName = "Flee State";
		lookingAway = false;
		//_entity.fleeing = true;
		//_entity.fleeing = true;
		//_entity.SetSkinColor(EntityGenderColor.FLEEING);
		//_entity.DangerColor();	// CHANGE COLOR
	}

	public override void HandleState(Entity entity)
	{
		Flee(entity);
		CheckIfSafe(entity);
	}

	public void Flee(Entity entity)
	{
		if (!lookingAway)
		{
			entity.transform.LookAt(entity.GetPosition() - (entity.predator.transform.position - entity.GetPosition()));
			lookingAway = true;
		}
		else
		{
			entity.IncreaseMaxSpeed();
			entity.SetDestination(TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV));
		}
	}

	private void CheckIfSafe(Entity entity)
	{
		if (entity.predator == null)
		{
			entity.DecreaseMaxSpeed();
			//entity.ResetColor(); // CHANGE COLOR
			entity.ChangeState(new PrimaryState());
		}
	}
}