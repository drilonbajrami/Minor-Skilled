using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetDestinationNode : Node
{
	private Entity _entity;

	public SetDestinationNode(Entity entity)
	{
		_entity = entity;
	}

	public override NodeState Evaluate()
	{
		_entity.agent.SetDestination(RandomNavmeshLocation(50.0f));
		return NodeState.RUNNING;
	}

	public Vector3 RandomNavmeshLocation(float radius)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += _entity.transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}
}
