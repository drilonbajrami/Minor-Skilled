using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected string _stateName;
	protected Entity _entity;

	protected State(Entity entity)
	{
		_entity = entity;
	}

	public abstract void HandleState();

	public void SetEntity(Entity entity)
	{
		this._entity = entity;
	}

	public void ChangeEntityState(State state)
	{
		this._entity.ChangeState(state);
		//state.SetEntity(this._entity);
	}

	public string GetStateName()
	{
		return _stateName;
	}
}
