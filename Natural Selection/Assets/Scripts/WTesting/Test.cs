using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Bounds bound;
    // Start is called before the first frame update
    void Start()
    {
        bound = gameObject.GetComponent<MeshCollider>().bounds;

        Debug.Log($"Min X: {bound.min.x}");
        Debug.Log($"Min Z: {bound.min.z}");
        Debug.Log($"Max X: {bound.max.x}");
        Debug.Log($"Max Z: {bound.max.z}");

        Debug.Log($"Scale: {gameObject.transform.localScale.x}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
