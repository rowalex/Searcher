using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseInfo : MonoBehaviour
{
    public float movementSpeed;
    private Vector3 movementVector;
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
        if (collision.gameObject.GetComponent<BaseInfo>())
        {
            Vector3 comp;
            Vector3 hitVector = collision.gameObject.GetComponent<BaseInfo>().nextPos - collision.gameObject.GetComponent<BaseInfo>().prevPos;
            comp = Vector3.Project(movementVector, hitVector);
            Debug.Log((hitVector - comp).magnitude);
            if ((hitVector - comp).magnitude * 100 > hitValue && timer < 0)
            {
                timer = immunityWindow;
            }
        }
        else
        {
            //Vector3 hitpos = collision.GetContact(0).point;
            //Vector3 hitVector = hitpos - transform.position;
            //Vector3 finvector = Vector3.Project(movementVector, hitVector);

            Debug.Log((movementVector - Vector3.zero).magnitude);
            if ((movementVector - Vector3.zero).magnitude * 100 > hitValue && timer<0)
            {
                timer = immunityWindow;
            }
        }
    }
}
