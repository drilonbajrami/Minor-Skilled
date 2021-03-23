using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attributes
{
    private float thirsty;
    private float hungry;
    private float energy;

    public float Thirsty { get { return thirsty; } set { thirsty = value; } }
    public float Hungry { get { return hungry; } set { hungry = value; } }
    public float Eenergy { get { return energy; } set { energy = value; } }

    public Attributes(float Thirst, float Hunger, float Energy)
    {
        thirsty = Thirst;
        hungry  = Hunger;
        energy  = Energy;
    }
}
