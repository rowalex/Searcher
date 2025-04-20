using System;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectTimeBody : ITimeBody
{
    private void Start()
    {
        rewindManager = RewindManager.Instance;
        rewindManager.OnRewind += OnRewind;
        rewindManager.OnSaveInfo += OnSaveInfo;
        rewindManager.OnClear += OnClear;
    }

    [SerializeField] private MovableObject movableObject;

    private List<MovableObjectFields> fields = new List<MovableObjectFields>();


    protected override void OnRewind()
    {
        fields[0].GetFields(movableObject);
        fields.RemoveAt(0);
    }

    protected override void OnSaveInfo()
    {
        fields.Insert(0, new MovableObjectFields(movableObject));
    }

    protected override void OnClear()
    {
        fields.Clear();
        fields.TrimExcess();
    }

}

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
    private bool isKinematic;

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
        this.isKinematic = obj.rb.isKinematic;
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
        obj.rb.isKinematic = this.isKinematic;
    }
}
