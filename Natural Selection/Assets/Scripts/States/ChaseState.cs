using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	private GameObject prey;
	private bool chasing;

	private float maxChaseTime;
	private float currentChaseTime;

	public ChaseState() : base()
	{
		_stateName = "Chase State";
		prey = null;
		chasing = false;
		maxChaseTime = 600.0f;
		currentChaseTime = maxChaseTime;
		//_entity.SetSkinColor(EntityGenderColor.CHASING);
		//_entity.DangerColor();	// CHANGE COLOR
	}

	public override void HandleState(Entity entity)
	{
		if (!chasing)
		{
			ChoosePrey(entity);
			entity.SetDestination(TransformUtils.RandomTarget(entity.GetTransform(), entity.sightRadius, entity.FOV));
		}
		else
			ChasePrey(entity);
	}

	private void ChoosePrey(Entity entity)
	{
		if (entity.Smell.HasPreyAround())
		{
			prey = entity.Smell.ChoosePrey();
			if (prey != null)
			{
				prey.GetComponent<Entity>().SetPredator(entity.gameObject);
				entity.IncreaseMaxSpeed();
				chasing = true;
			}
		}
	}

	private void ChasePrey(Entity entity)
	{
		if (prey != null)
		{
			entity.SetDestination(prey.transform.position);

			if (TransformUtils.CheckIfClose(entity.GetTransform(), prey.transform, 2.5f))
			{
				prey.gameObject.GetComponent<Entity>().Die();
				GameObject.Destroy(prey.gameObject);
				entity.hungriness = 0;
				entity.thirstiness = 0;
				//entity.ResetColor(); // CHANGE COLOR
				entity.ChangeState(new PrimaryState());
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
