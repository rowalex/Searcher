using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfo : MonoBehaviour
{

    public Vector3 prevPos;
    public Vector3 nextPos;
    public float movementSpeed;
    private void Start()
    {
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {       
        prevPos = nextPos;
        nextPos = transform.position;
        movementSpeed = Vector3.Distance(prevPos, nextPos) / Time.deltaTime;
    }
}
