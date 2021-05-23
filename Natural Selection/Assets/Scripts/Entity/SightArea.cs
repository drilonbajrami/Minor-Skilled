using System.Collections.Generic;
using UnityEngine;

public class SightArea
{
	public SightSection[] sections;
	public float halfAngle;

	public SightArea(int sectionCount)
	{
		sections = new SightSection[sectionCount];

		// Get section angle, 360 degrees divided by the number of sections
		float sectionAngle = 360.0f / sectionCount;
		halfAngle = sectionAngle / 2;
		float startAngle;

		for (int i = 0; i < sections.Length; i++)
		{
			// Increment starting angle for next section
			startAngle = i * sectionAngle;
			sections[i] = new SightSection(startAngle, sectionAngle);
		}
	}

	public float CalculateUtilityValues(Entity entity, List<GameObject> objectsWithinSight)
	{
		for (int i = 0; i < sections.Length; i++)
			sections[i].ResetUtilityValue();

		float difference;
		float sectionAngle;

		if (objectsWithinSight.Count == 0 || objectsWithinSight == null)
			return 0.0f;

		foreach (GameObject gameObject in objectsWithinSight)
		{
			Vector3 toGameObject = gameObject.transform.position - entity.gameObject.transform.position;
			float objectAngle = TransformUtils.GetAngle(entity.gameObject.transform.forward, toGameObject, entity.gameObject.transform.up);
			float utilityValue = 0.0f;

			if (gameObject == null)
				break;

			Entity other = gameObject.GetComponent<Entity>();

			if (other != null && other.order == Order.HERBIVORE)
				utilityValue = entity.preyU;
			else if (other != null && other.order == Order.CARNIVORE)
				utilityValue = entity.predatorU;
			else if (gameObject.CompareTag("Water"))
				utilityValue = entity.waterU;
			else if (gameObject.CompareTag("Plant"))
				utilityValue = entity.foodU;

			for (int i = 0; i < sections.Length; i++)
			{
				sectionAngle = sections[i].Angle;

				if (objectAngle >= sectionAngle)
				{
					difference = objectAngle - sectionAngle;
					if (difference >= 180)
						difference = (360 - objectAngle) + sectionAngle;
				}
				else
				{
					difference = sectionAngle - objectAngle;
					if (difference >= 180)
						difference = (360 - sectionAngle) + objectAngle;
				}

				float percentage = GetPercentage(180 - difference);
				sections[i].EditUtilityValue(utilityValue * percentage);
			}
		}

		return GetHighestUtilitySection();
	}

	private float GetHighestUtilitySection()
	{
		float bestUtility = float.MinValue;
		float bestAngle = 0.0f;

		for (int i = 0; i < sections.Length; i++)
		{
			if (bestUtility <= sections[i].UtilityValue)
			{
				bestUtility = sections[i].UtilityValue;
				bestAngle = sections[i].Angle;
			}
		}

		return bestAngle;
	}

	private float GetPercentage(float value)
	{
		return (value / 1.8f) / 100;
	}
}