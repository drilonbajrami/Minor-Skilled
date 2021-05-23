using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    float deltaX;
    float deltaZ;

    public float delta = 0.5f;

    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //        deltaX -= delta;
    //    else if (Input.GetKeyDown(KeyCode.D))
    //        deltaX += delta;
    //    else if (Input.GetKeyDown(KeyCode.W))
    //        deltaZ += delta;
    //    else if (Input.GetKeyDown(KeyCode.S))
    //        deltaZ -= delta;

    //    gameObject.transform.position = new Vector3(deltaX, 0.5f, deltaZ);
    //}
}
