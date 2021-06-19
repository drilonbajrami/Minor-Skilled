using UnityEngine;

public class PrimaryState : State
{
	private enum SubState { ROAMING, IDLE }
	private SubState _currentSubState = SubState.ROAMING;

	public PrimaryState() : base()
	{
		_stateName = "Primary State";
	}

	public override void HandleState(Entity entity)
	{
		switch (_currentSubState)
		{
			case SubState.ROAMING:
				RoamingSubState(entity);
				break;
			case SubState.IDLE:
				IdleSubState(entity);
				break;
			default:
				break;
		}
	}

	private void RoamingSubState(Entity entity)
	{
		if (entity.NeedsResources(50.0f))
		{
			if (entity.IsCarnivore())
			{
				entity.ChangeState(new ChaseState());
			}
			else
			{
				//entity.ChangeState(new HungryThirstyState());
			}
		}

		if ((entity.IsStopped() || !entity.HasPath()))
		{
			entity.Sight.AssessUtilities();
			if (entity.Sight.SightArea.LowestUtilityValue < 0)
				entity.Run();
			else
				entity.Walk();
			
			bool doNotSocialize = false;
			if(entity.Sight.SightArea.LowestUtilityValue >= 0)
				doNotSocialize = Random.Range(0.0f, 100.0f) >= entity.Genome.Behavior.IdealAllele.SocializingChance;
			entity.SetDestination(entity.GetIdealRandomDestination(doNotSocialize));
		}

		//if (entity.Velocity() > 0.01f)
		//{
		//	//entity.thirstiness += Time.deltaTime * entity.Velocity() / 5;
		//	entity.hungriness += Time.deltaTime * entity.Velocity() / 10;
		//}
	}

	private void IdleSubState(Entity entity)
	{
		//entity._vitals.ConvertResourcesToEnergy(0.25f);

		if (entity.NeedsResources(30.0f))
			_currentSubState = SubState.ROAMING;
	}
}