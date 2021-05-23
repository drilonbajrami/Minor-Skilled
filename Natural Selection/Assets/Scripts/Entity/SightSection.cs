using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SightSection covers an area (pie slice) of the full sight area.
/// Contains an utility value (-1.00 to 1.00) that determines the likelihood of being choosen as an option.
/// </summary>
public class SightSection
{
	public readonly float Angle;
	public float UtilityValue { get; private set; }

	public SightSection(float startingAngle, float sectionAngle)
	{
		Angle = (startingAngle * 2 + sectionAngle) / 2;
		UtilityValue = 0.0f;
	}

	public void EditUtilityValue(float value)
	{
		UtilityValue += value;
		UtilityValue = Mathf.Clamp(UtilityValue, -1.0f, 1.0f);
	}

	public void ResetUtilityValue()
	{
		UtilityValue = 0.0f;
	}
}