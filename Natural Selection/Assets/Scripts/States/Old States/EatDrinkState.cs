using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatDrinkState : State
{
	//private Resource _resource = null;

	public EatDrinkState(Entity entity) : base(entity)
	{
		_stateName = "Eat & Drink State";
	}

	public override void HandleState()
	{
	//	if (!_entity.IsThirsty() && !_entity.IsHungry())
	//		ChangeEntityState(new PrimaryState());

		//if (_entity.waterToDrink != null && _entity.foodToEat != null)	// If both available
		//{
		//	if (_entity.thirstiness > _entity.hungriness)
		//		GoForResource(_entity.waterToDrink);
		//	else
		//		GoForResource(_entity.foodToEat);
		//}
		//else if (_entity.waterToDrink != null)							// If only water available
		//	GoForResource(_entity.waterToDrink);
		//else if (_entity.foodToEat != null)								// If only food available
		//	GoForResource(_entity.foodToEat);

		//if ((IsWaterAvailable() || IsFoodAvailable()) && TransformUtils.CheckIfClose(_entity.GetTransform(), _resource, 0.5f))
		//{
		//	if (_resource.tag == "Water")
		//		ConsumeWater();
		//	else
		//		ConsumeFood();

		//	if (_entity.hungriness < 50.0f && _entity.thirstiness < 50.0f)
		//	{
		//		ChangeEntityState(new PrimaryState());
		//	}
		//}
		//else
		//	ChangeEntityState(new SearchResourcesState());
	}

	//private void GoForResource(GameObject resource)
	//{
	//	_resource = resource.transform;
	//	if (!_entity.agent.hasPath && resource != null)
	//		_entity.SetDestination(_resource.position);
	//	else if (_entity.HasPath() && resource == null)
	//		_entity.SetDestination(TransformUtils.RandomTarget(_entity.GetTransform(), 20.0f, _entity.FOV));
	//}

	//private bool IsFoodAvailable()
	//{
	//	return _entity.foodToEat != null && !_entity.foodToEat.GetComponent<Consumed>().isConsumed() ? true : false;
	//}

	//private bool IsWaterAvailable()
	//{
	//	return _entity.waterToDrink != null && !_entity.waterToDrink.GetComponent<Consumed>().isConsumed() ? true : false;
	//}

	//private void ConsumeFood()
	//{
	//	GameObject.Destroy(_entity.foodToEat.gameObject);
	//	//_entity.foodToEat.gameObject.GetComponent<Consumed>().Consume();
	//	_entity.foodToEat = null;
	//	_entity.hungriness = 0;
	//	_resource = null;
	//}

	//private void ConsumeWater()
	//{
	//	//GameObject.Destroy(_entity.waterToDrink.gameObject);
	//	_entity.foodToEat.gameObject.GetComponent<Consumed>().Consume();
	//	_entity.waterToDrink = null;
	//	_entity.thirstiness = 0;
	//	_resource = null;
	//}

	//private void ConsumeResource(GameObject resource)
	//{
	//	resource.gameObject.GetComponent<Resource>().Consume();

	//	if (resource.GetComponent<Resource>().GetResourceType() == ResourceType.WATER)
	//		_entity.thirstiness = 0;
	//	else
	//		_entity.hungriness = 0;
	//}
}
