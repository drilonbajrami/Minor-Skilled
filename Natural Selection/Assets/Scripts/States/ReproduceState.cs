using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReproduceState : State
{
	private enum SubState { SEARCHING, MATING }
	private SubState _currentSubState = SubState.SEARCHING;

	private GameObject _partner = null;

	private Vector3 _positionToMate = new Vector3();
	private bool goingToMate = false;

	public ReproduceState() : base()
	{
		//_entity.isOnReproducingState = true;
		_stateName = "Reproduce State";
		goingToMate = false;
		_partner = null;
		_positionToMate = new Vector3();
	}

	public override void HandleState(Entity entity)
	{
		switch (_currentSubState)
		{
			case SubState.SEARCHING:
				SearchingSubState(entity);
				break;
			case SubState.MATING:
				MatingSubState(entity);
				break;
			default:
				break;
		}

		entity.thirstiness += Time.deltaTime * 5 / 10;
		entity.hungriness += Time.deltaTime * 5 / 10;

		if (entity.predator != null)
		{
			entity.partner = null;
			entity.isOnReproducingState = false;
			entity.ChangeState(new FleeState());
		}

		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			
		}
		else
		{
			if (entity.hungriness > 50.0f && entity.thirstiness > 50.0f)
				entity.ChangeState(new PrimaryState());
		}
	}

	private void SearchingSubState(Entity entity)
	{
		switch (entity.gender)
		{
			case Gender.MALE:
				SearchingSubStateMale(entity);
				break;
			case Gender.FEMALE:
				SearchingSubStateFemale(entity);
				break;
			default:
				break;
		}
	}

	private void MatingSubState(Entity entity)
	{
		switch (entity.gender)
		{
			case Gender.MALE:
				MatingSubStateMale(entity);
				break;
			case Gender.FEMALE:
				MatingSubStateFemale(entity);
				break;
			default:
				break;
		}
	}

	// FEMALE SEARCHING
	private void SearchingSubStateFemale(Entity entity)
	{
		if (entity.Smell.HasPartnersAround() && _partner == null)
		{
			_partner = entity.Smell.ChoosePartner();
			if (_partner != null)
			{
				if (!_partner.gameObject.GetComponent<Entity>().isMating)
				{
					_partner.GetComponent<Entity>().SetMatingPartner(entity.gameObject);
					entity.transform.LookAt(_partner.transform);
					_positionToMate = TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV);
					entity.Stop();
					_currentSubState = SubState.MATING;
				}
			}
			else
			{
				_partner = null;
			}
		}

		if (entity.IsStuck() || !entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV);
			entity.SetDestination(i);
		}
	}

	// MALE SEARCHING
	private void SearchingSubStateMale(Entity entity)
	{
		if (entity.partner != null)
		{
			_partner = entity.partner;
			entity.Stop();
			_currentSubState = SubState.MATING;
		}

		if (entity.IsStuck() || !entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(entity.GetTransform(), 20.0f, entity.FOV);
			entity.SetDestination(i);
		}
	}

	
	// MALE MATING
	private void MatingSubStateMale(Entity entity)
	{
		if (!goingToMate)
		{
			//entity.ReproduceColor(); // CHANGE COLOR
			goingToMate = true;
		}
		else
		{
			if (entity.HasPath() && TransformUtils.CheckIfClose(entity.GetTransform(), _partner.gameObject.transform, 5.0f))
			{
				entity.ClearPath();
				_partner.gameObject.GetComponent<Entity>().ClearPath();
			}
		}

		if (goingToMate && entity.isMating == false)
		{
			//entity.ResetColor();	// CHANGE COLOR
			//_entity.maleReproductionDuration = 30.0f;

			if (SceneManager.GetActiveScene().buildIndex == 4)
				entity.maleReproductionDuration = 0.0f;
			else
				entity.maleReproductionDuration = 30.0f;
			entity.ChangeState(new PrimaryState());
		}
	}

	// FEMALE MATING
	private void MatingSubStateFemale(Entity entity)
	{
		if (!goingToMate)
		{
			//entity.ResetColor();	// CHANGE COLOR
			entity.SetDestination(_positionToMate);
			_partner.gameObject.GetComponent<Entity>().SetDestination(_positionToMate);
			entity.isMating = true;
			goingToMate = true;
		}

		if (_partner != null)
		{
			if (TransformUtils.CheckIfClose(entity.GetTransform(), _partner.gameObject.transform, 5.0f) && goingToMate)
			{
				_partner.gameObject.GetComponent<Entity>().DiscardMatingPartner();
				GameObject offspring = entity.gameObject.transform.parent.gameObject.GetComponent<TestPosition>().CreateNewEntity(entity.order);
				Vector3 pos = entity.GetPosition();
				pos.x -= 2;
				offspring.transform.position = pos;
				offspring.transform.localScale.Set(0.5f, 0.5f, 0.5f);
				offspring.transform.parent = entity.gameObject.transform.parent;
		
				if (SceneManager.GetActiveScene().buildIndex == 4)
					entity.gestationDuration = 0.0f;
				else
					entity.gestationDuration = 60.0f;

				//entity.ResetColor();
				entity.isMating = false;

				entity.ChangeState(new PrimaryState());
			}
		}
	}
}
