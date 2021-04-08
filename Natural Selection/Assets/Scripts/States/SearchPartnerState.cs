using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPartnerState : State
{
	public SearchPartnerState(Entity entity) : base(entity)
	{
		_stateName = "Searching Partner State";
	}

	public override void HandleState()
	{
		throw new System.NotImplementedException();
	}
}
