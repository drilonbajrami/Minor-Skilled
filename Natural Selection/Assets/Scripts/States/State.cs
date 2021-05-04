using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected string _stateName;

	protected State() { }

	public abstract void HandleState(Entity entity);

	public string GetStateName()
	{
		return _stateName;
	}
}
