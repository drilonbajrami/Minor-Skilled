using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
	//private bool lookingAway = false;

	public FleeState() : base()
	{
		_stateName = "Flee State";
		//lookingAway = false;
		//_entity.fleeing = true;
	}

	public override void HandleState(Entity entity)
	{
		Flee(entity);
		CheckIfSafe(entity);
	}

	public void Flee(Entity entity)
	{
		//if (!lookingAway)
		//{
		//	entity.transform.LookAt(entity.Transform.localPosition - (entity.predator.transform.position - entity.Transform.localPosition));
		//	lookingAway = true;
		//}
		//else
		//{
		//	entity.IncreaseMaxSpeed();
		//	entity.SetDestination(TransformUtils.RandomTarget(entity.Transform, 20.0f, entity.FOV));
		//}
	}

	private void CheckIfSafe(Entity entity)
	{
		//if (entity.predator == null)
		//{
		//	entity.DecreaseMaxSpeed();
		//	entity.ChangeState(new PrimaryState());
		//}
	}
}