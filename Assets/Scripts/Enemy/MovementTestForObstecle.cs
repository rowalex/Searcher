using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTestForObstecle : MonoBehaviour
{
    public Vector3 movementVector;
    public float speed;
    private float timer = 0.5f;
    private float index = 1;
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            index *= -1;
        }

        transform.position = transform.position + movementVector * index * speed * Time.deltaTime;
    }
}
