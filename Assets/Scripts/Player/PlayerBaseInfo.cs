using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public Vector2 pickUpVector;
    public float pickUpDistance;
    public LayerMask pickUpMask;
    public GameObject currPickUp;

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
        if (collision.gameObject.tag == "Obstacle" && timer < 0)
        {
            timer = immunityWindow;
            gameObject.GetComponent<Rigidbody>().AddForce(collision.impulse.normalized * 10, ForceMode.Impulse);
            return;
        }
        Debug.Log(collision.impulse.magnitude);
        if (collision.impulse.magnitude > hitValue && timer < 0)
        {
            timer = immunityWindow;
        }
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, pickUpDistance, pickUpMask) && Input.GetKeyDown(KeyCode.F))
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, pickUpDistance, pickUpMask);
            if (currPickUp != null)
            {
                currPickUp.GetComponent<MovableObject>().InteractObject(gameObject);
                currPickUp = null;
                return;
            }
            currPickUp = targets[0].gameObject;
            currPickUp.GetComponent<MovableObject>().InteractObject(gameObject);

            
        }
    }

    public Vector3 GetPickedUpObjectPosition()
    {
        Vector3 pos = transform.position + transform.forward * pickUpVector.x + transform.up * pickUpVector.y;
        return pos;
    }

}
