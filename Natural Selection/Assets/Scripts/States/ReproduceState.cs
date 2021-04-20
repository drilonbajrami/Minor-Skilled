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

	public ReproduceState(Entity entity) : base(entity)
	{
		_entity.isOnReproducingState = true;
		_stateName = "Reproduce State";
		goingToMate = false;
		_partner = null;
		_positionToMate = new Vector3();
	}

	public override void HandleState()
	{
		switch (_currentSubState)
		{
			case SubState.SEARCHING:
				SearchingSubState();
				break;
			case SubState.MATING:
				MatingSubState();
				break;
			default:
				break;
		}

		_entity.thirstiness += Time.deltaTime * 5 / 10;
		_entity.hungriness += Time.deltaTime * 5 / 10;

		if (_entity.predator != null)
		{
			_entity.partner = null;
			_entity.isOnReproducingState = false;
			ChangeEntityState(new FleeState(_entity));
		}

		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			
		}
		else
		{
			if (_entity.hungriness > 50.0f && _entity.thirstiness > 50.0f)
				ChangeEntityState(new PrimaryState(_entity));
		}
	}

	private void SearchingSubState()
	{
		switch (_entity.gender)
		{
			case Gender.MALE:
				SearchingSubStateMale();
				break;
			case Gender.FEMALE:
				SearchingSubStateFemale();
				break;
			default:
				break;
		}
	}

	private void MatingSubState()
	{
		switch (_entity.gender)
		{
			case Gender.MALE:
				MatingSubStateMale();
				break;
			case Gender.FEMALE:
				MatingSubStateFemale();
				break;
			default:
				break;
		}
	}

	// FEMALE SEARCHING
	private void SearchingSubStateFemale()
	{
		if (_entity.Smell.HasPartnersAround() && _partner == null)
		{
			_partner = _entity.Smell.ChoosePartner();
			if (_partner != null)
			{
				if (!_partner.gameObject.GetComponent<Entity>().isMating)
				{
					_partner.GetComponent<Entity>().SetMatingPartner(_entity.gameObject);
					_entity.transform.LookAt(_partner.transform);
					_positionToMate = TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV);
					_entity.Stop();
					_currentSubState = SubState.MATING;
				}
			}
			else
			{
				_partner = null;
			}
		}

		if (_entity.IsStuck() || !_entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV);
			_entity.SetDestination(i);
		}
	}

	// MALE SEARCHING
	private void SearchingSubStateMale()
	{
		if (_entity.partner != null)
		{
			_partner = _entity.partner;
			_entity.Stop();
			_currentSubState = SubState.MATING;
		}

		if (_entity.IsStuck() || !_entity.HasPath())
		{
			Vector3 i = TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV);
			_entity.SetDestination(i);
		}
	}

	
	// MALE MATING
	private void MatingSubStateMale()
	{
		if (!goingToMate)
		{
			_entity.ReproduceColor(); // CHANGE COLOR
			goingToMate = true;
		}
		else
		{
			if (_entity.HasPath() && TransformUtils.CheckIfClose(_entity.GetTransform(), _partner.gameObject.transform, 5.0f))
			{
				_entity.ClearPath();
				_partner.gameObject.GetComponent<Entity>().ClearPath();
			}
		}

		if (goingToMate && _entity.isMating == false)
		{
			_entity.ResetColor();	// CHANGE COLOR
			//_entity.maleReproductionDuration = 30.0f;

			if (SceneManager.GetActiveScene().buildIndex == 4)
				_entity.maleReproductionDuration = 0.0f;
			else
				_entity.maleReproductionDuration = 30.0f;
			ChangeEntityState(new PrimaryState(_entity));
		}
	}

	// FEMALE MATING
	private void MatingSubStateFemale()
	{
		if (!goingToMate)
		{
			_entity.ResetColor();	// CHANGE COLOR
			_entity.SetDestination(_positionToMate);
			_partner.gameObject.GetComponent<Entity>().SetDestination(_positionToMate);
			_entity.isMating = true;
			goingToMate = true;
		}

		if (_partner != null)
		{
			if (TransformUtils.CheckIfClose(_entity.GetTransform(), _partner.gameObject.transform, 5.0f) && goingToMate)
			{
				_partner.gameObject.GetComponent<Entity>().DiscardMatingPartner();
				GameObject offspring = _entity.gameObject.transform.parent.gameObject.GetComponent<TestPosition>().CreateNewEntity(_entity.order);
				Vector3 pos = _entity.GetPosition();
				pos.x -= 2;
				offspring.transform.position = pos;
				offspring.transform.localScale.Set(0.5f, 0.5f, 0.5f);
				offspring.transform.parent = _entity.gameObject.transform.parent;
		
				if (SceneManager.GetActiveScene().buildIndex == 4)
					_entity.gestationDuration = 0.0f;
				else
					_entity.gestationDuration = 60.0f;

				_entity.ResetColor();
				_entity.isMating = false;

				ChangeEntityState(new PrimaryState(_entity));
			}
		}
		else
		{

		}
	}
}
