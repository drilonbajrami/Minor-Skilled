using UnityEngine;

public class PrimaryState : State
{
	private enum SubState { ROAMING, IDLE }
	//private SubState _currentSubState = SubState.ROAMING;

	public PrimaryState() : base()
	{
		_stateName = "Primary State";
	}

	public override void HandleState(Entity entity)
	{
		RoamingSubState(entity);
		
		// For now we will keep only roaming state
		//switch (_currentSubState)
		//{
		//	case SubState.ROAMING:
		//		RoamingSubState(entity);
		//		break;
		//	case SubState.IDLE:
		//		IdleSubState(entity);
		//		break;
		//	default:
		//		break;
		//}
	}

	private void RoamingSubState(Entity entity)
	{
		if (entity.NeedsToEat())
			if (entity.IsCarnivore())
				entity.ChangeState(new ChaseState());
			else
				entity.ChangeState(new HungryThirstyState());

		if (entity.IsStopped() || !entity.HasPath())
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
	}

	private void IdleSubState(Entity entity)
	{

	}
}