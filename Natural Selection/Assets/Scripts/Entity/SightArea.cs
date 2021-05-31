using System.Collections.Generic;
using UnityEngine;

public class SightArea
{
	public SightSection[] sections;
	private float halfAngle;

	public float IdealDirectionAngle   { get; private set; }
	public float HighestUtilityValue   { get; private set; }
	public float LowestUtilityValue    { get; private set; }

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

	public void AssessUtilityValues(Entity entity, Dictionary<int, MemoryData> objectsOnSight)
	{
		ResetUtilityValues();

		if (objectsOnSight.Count == 0 || objectsOnSight == null) return;

		foreach (KeyValuePair<int, MemoryData> o in objectsOnSight)
		{
			if (o.Value.Object == null)
				continue;

			Vector3 dirToObject = o.Value.Object.transform.localPosition - entity.Transform.localPosition;
			float objectAngle = entity.GetAngleTo(dirToObject);
			float objectUtilityValue = 0.0f;

			Entity other = o.Value.Object.GetComponent<Entity>();

			if (other != null)
			{
				if (other.order == Order.HERBIVORE)
					objectUtilityValue = entity.preyU;
				else
					objectUtilityValue = entity.predatorU;
			}

			AssessUtilityValuePerSection(objectAngle, objectUtilityValue);
		}

		AssessUtilityOptions();
	}

	private void AssessUtilityValuePerSection(float objectAngle, float utilityValue)
	{
		for (int i = 0; i < sections.Length; i++)
		{
			float sectionAngle = sections[i].Angle;
			float difference;

			if (objectAngle >= sectionAngle)
			{
				difference = objectAngle - sectionAngle;
				if (difference >= 180) difference = (360 - objectAngle) + sectionAngle;
			}
			else
			{
				difference = sectionAngle - objectAngle;
				if (difference >= 180) difference = (360 - sectionAngle) + objectAngle;
			}

			float effectPercentage = GetPercentage(180 - difference);
			sections[i].EditUtilityValue(utilityValue * effectPercentage);
		}
	}

	private void ResetUtilityValues()
	{
		for (int i = 0; i < sections.Length; i++) sections[i].ResetUtilityValue();
	}

	private void AssessUtilityOptions()
	{
		IdealDirectionAngle = 0.0f;
		HighestUtilityValue = 0;
		LowestUtilityValue = 0;

		for (int i = 0; i < sections.Length; i++)
		{
			if (HighestUtilityValue < sections[i].UtilityValue)
			{
				HighestUtilityValue = sections[i].UtilityValue;
				IdealDirectionAngle = sections[i].Angle;
			}

			if (LowestUtilityValue > sections[i].UtilityValue)
			{
				LowestUtilityValue = sections[i].UtilityValue;
			}
		}

		if (IdealDirectionAngle == 0)
		{
			if (Random.Range(0, 2) == 0)
				IdealDirectionAngle = Random.Range(0, 45);
			else
				IdealDirectionAngle = Random.Range(315, 360);
		}
	}

	public float GetIdealDirectionAngle()
	{
		return Random.Range(IdealDirectionAngle - halfAngle, IdealDirectionAngle + halfAngle);
	}

	private float GetPercentage(float value)
	{
		return (value / 1.8f) / 100;
	}
}