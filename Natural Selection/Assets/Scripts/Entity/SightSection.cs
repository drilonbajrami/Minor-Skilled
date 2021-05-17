using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SightSection covers an area (pie slice) of the full sight area.
/// Contains a starting angle and an ending angle which determine the area it covers within the sight area.
/// Contains an utility value that determines the likelihood of being choosen as an option.
/// </summary>
public class SightSection
{
	public float StartAngle { get; }
	public float EndAngle { get; }
	public float MedianAngle { get; }

	public float utilityValue;

	public SightSection(float pStartAngle, float sectionAngle)
	{
		StartAngle = pStartAngle;
		EndAngle = pStartAngle + sectionAngle;
		MedianAngle = (pStartAngle + EndAngle) / 2;
		utilityValue = Random.Range(-1.0f, 1.0f);
	}

	public void EditUtilityValue(float value)
	{
		utilityValue += value;
		Mathf.Clamp(utilityValue, -1.0f, 1.0f);
	}

	public void ResetUtilityValue()
	{
		utilityValue = 0.0f;
	}
}