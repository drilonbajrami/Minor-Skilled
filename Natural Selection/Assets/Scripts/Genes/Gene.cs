using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    private GeneType _geneType;
    private float _value;

    public float Value
    {
        get { return _value; }
        set { _value = Mathf.Clamp(_value, 0.01f, 1.0f); }
    }

    public GeneType GeneType { get { return _geneType; } }

    public Gene(GeneType pType,  float pValue)
    {
        _geneType = pType;
        _value = pValue;
    }
}
