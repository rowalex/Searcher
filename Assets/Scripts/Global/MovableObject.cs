using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

class MovableObjectFields
{
    private Vector3 pos;
    private Quaternion rot;
    private Action onUpdate;
    private bool isPickeup;
    private PlayerBaseInfo player;
    private Vector3 velocity;
    private float drag;
    private bool isGravity;
    private bool isSCWork;

    public MovableObjectFields(MovableObject obj)
    {
        this.pos = obj.transform.position;
        this.rot = obj.transform.rotation;
        this.onUpdate = obj.OnUpdate;
        this.isPickeup = obj.isPickedUp;
        this.velocity = obj.rb.velocity;
        this.drag = obj.rb.drag;
        this.isGravity = obj.rb.useGravity;
        this.isSCWork = obj.sphereCollider.enabled;
    }

    public void GetFields(MovableObject obj)
    {
        obj.transform.position = this.pos;
        obj.transform.rotation = this.rot;
        obj.OnUpdate = this.onUpdate;
        obj.isPickedUp = this.isPickeup;
        obj.rb.velocity = this.velocity;
        obj.rb.drag = this.drag;
        obj.rb.useGravity = this.isGravity;
        obj.sphereCollider.enabled = this.isSCWork;
    }
}


public class MovableObject : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] public bool isPickedUp = false;

    [SerializeField] public PlayerBaseInfo player;

    [SerializeField] private float force;

    [SerializeField] public Rigidbody rb;

    [SerializeField] public SphereCollider sphereCollider;

    private List<MovableObjectFields> fields = new List<MovableObjectFields>();

    public Action OnUpdate;

    private void FixedUpdate()
    {
        if (gameManager.isRewind)
        {
            if (fields.Count > 0)
            {
                fields[0].GetFields(this);
                fields.RemoveAt(0);
            }
        }
        else
        {
            if (fields.Count > (gameManager.timeForRewind / Time.fixedDeltaTime) - 1) fields.RemoveAt(fields.Count - 1);
            fields.Insert(0, new MovableObjectFields(this));
            OnUpdate?.Invoke();
        }
    }

    public void InteractObject(GameObject player)
    {
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
