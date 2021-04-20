using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	private GameObject prey;
	private bool chasing;

	private float maxChaseTime;
	private float currentChaseTime;

	public ChaseState(Entity entity) : base(entity)
	{
		_stateName = "Chase State";
		prey = null;
		chasing = false;
		maxChaseTime = 600.0f;
		currentChaseTime = maxChaseTime;
		//_entity.SetSkinColor(EntityGenderColor.CHASING);
		_entity.DangerColor();	// CHANGE COLOR
	}

	public override void HandleState()
	{
		if (!chasing)
		{
			ChoosePrey();
			_entity.SetDestination(TransformUtils.RandomTarget(_entity.GetTransform(), _entity.sightRadius, _entity.FOV));
		}
		else
			ChasePrey();
	}

	private void ChoosePrey()
	{
		if (_entity.Smell.HasPreyAround())
		{
			prey = _entity.Smell.ChoosePrey();
			if (prey != null)
			{
				prey.GetComponent<Entity>().SetPredator(_entity.gameObject);
				_entity.IncreaseMaxSpeed();
				chasing = true;
			}
		}
	}

	private void ChasePrey()
	{
		if (prey != null)
		{
			_entity.SetDestination(prey.transform.position);

			if (TransformUtils.CheckIfClose(_entity.GetTransform(), prey.transform, 2.5f))
			{
				GameObject.Destroy(prey.gameObject);
				_entity.hungriness = 0;
				_entity.thirstiness = 0;
				_entity.ResetColor(); // CHANGE COLOR
				ChangeEntityState(new PrimaryState(_entity));
			}
		}
		else
		{
			prey = null;
			chasing = false;
			currentChaseTime = maxChaseTime;
		}

		currentChaseTime -= Time.deltaTime;
		if (currentChaseTime <= 0.0f)
		{
			prey.gameObject.GetComponent<Entity>().predator = null;
			prey = null;
			chasing = false;
			currentChaseTime = maxChaseTime;
		}
	}
}
