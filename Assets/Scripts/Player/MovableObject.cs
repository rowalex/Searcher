using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class MovableObject : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] public bool isPickedUp = false;

    [SerializeField] public PlayerBaseInfo player;

    [SerializeField] private float force;

    [SerializeField] public Rigidbody rb;

    [SerializeField] public SphereCollider sphereCollider;

    public Action OnUpdate;

    [SerializeField] private Vector3 resetPos; 


    private void Start()
    {
        ResetObject();
    }

    public void ResetObject()
    {
        rb.isKinematic = true;
        transform.position = resetPos;
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (!RewindManager.Instance.isRewind)
        {
            OnUpdate?.Invoke();
        }
    }

    public void InteractObject(GameObject player)
    {
        rb.isKinematic = false;
        this.player = player.GetComponent<PlayerBaseInfo>();
        if (isPickedUp)
        {
            sphereCollider.enabled = true;
            rb.drag = 0.1f;
            rb.useGravity = true;
            isPickedUp = false;
            OnUpdate -= GravitateTowardPosition;
        }
        else
        {
            sphereCollider.enabled = false;
            rb.drag = 3;
            rb.useGravity = false;
            isPickedUp = true;
            OnUpdate += GravitateTowardPosition;
        }
    }
    private void GravitateTowardPosition()
    {
        rb.AddForce((player.GetPickedUpObjectPosition() - transform.position) * force);
    }

    

}
