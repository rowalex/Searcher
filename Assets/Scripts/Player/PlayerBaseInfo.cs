using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseInfo : MonoBehaviour
{
    public float movementSpeed;
    public Vector3 movementVector;
    private Vector3 prevPos;
    private Vector3 nextPos;
    public float hitValue;
    public float timer;
    public float immunityWindow = 1;
    public int hpAmount = 3;


    private void Start()
    {
        nextPos = transform.position;
    }

    private void FixedUpdate()
    {   prevPos = nextPos;  
        nextPos = transform.position;
        movementSpeed = Vector3.Distance(prevPos, nextPos) / Time.deltaTime;
        movementVector = nextPos - prevPos;

        if (timer == immunityWindow)
        {
            hpAmount--;
        }
        timer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse.magnitude);
        if (collision.impulse.magnitude > hitValue && timer < 0)
        {
            timer = immunityWindow;
        }
    }
}
