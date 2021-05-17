using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightArea
{
    public SightSection[] sightSections;

    public SightArea(int sectionCount)
    {
        sightSections = new SightSection[sectionCount];

        // Get section angle, 360 degrees divided by the number of sections
        float sectionAngle = 360.0f / sectionCount;
        float startAngle;

        for (int i = 0; i < sightSections.Length; i++)
        {
            // Increment starting angle for next section
            startAngle = i * sectionAngle;
            sightSections[i] = new SightSection(startAngle, sectionAngle);
        }
    }

    public void CalculateUtilityValues(Transform entityTransform, List<GameObject> objectsWithinSight)
    {
        float difference;
        float medianAngle;

        foreach (GameObject gameObject in objectsWithinSight)
        {
            Vector3 toGameObject = gameObject.transform.position - entityTransform.position;
            float objectAngle = TransformUtils.GetAngle(entityTransform.forward, toGameObject, entityTransform.up);

            for (int i = 0; i < sightSections.Length; i++)
            {
                medianAngle = sightSections[i].MedianAngle;

                if (objectAngle >= medianAngle)
                {
                    difference = objectAngle - medianAngle;

                    if (difference >= 180)
                        difference = (360 - objectAngle) + medianAngle;
                }
                else
                {
                    difference = medianAngle - objectAngle;

                    if (difference >= 180)
                        difference = (360 - medianAngle) + objectAngle;
                }

                //sightSections[i].EditUtilityValue(gameObject.UtilValue * GetPercentage(180-difference));
            }
        }
    }

    public int GetPercentage(float value)
    {
        return Mathf.CeilToInt(value / 1.8f);
    }
}