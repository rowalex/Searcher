using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTimeBody : ITimeBody
{
    private void Start()
    {
        rewindManager = RewindManager.Instance;
        rewindManager.OnRewind += OnRewind;
        rewindManager.OnSaveInfo += OnSaveInfo;
        rewindManager.OnClear += OnClear;
    }

    [SerializeField] private PlayerBaseInfo baseInfo;
    [SerializeField] private Movement movement;
    [SerializeField] private Capturing capturing;
    [SerializeField] private Invisibility invisibility;

    private List<PlayerFields> fields = new List<PlayerFields>();


    protected override void OnRewind()
    {
        fields[0].GetFields(baseInfo, movement, capturing, invisibility);
        fields.RemoveAt(0);
    }

    protected override void OnSaveInfo()
    {
        fields.Insert(0, new PlayerFields(baseInfo, movement, capturing, invisibility));
    }

    protected override void OnClear()
    {
        fields.Clear();
        fields.TrimExcess();
    }

}

class PlayerFields
{
    private int currHP;
    private float regenTimer;
    private float currFuel;
    private Vector3 position;
    private Vector3 velocity;
    private Quaternion rotation;
    private bool useGravity;
    private float drag;
    private bool isAbleToMove;
    private float alpha;
    private bool isPickUp;

    public PlayerFields(PlayerBaseInfo info, Movement movement, Capturing capturing, Invisibility invisibility)
    {
        this.currHP = info.hpAmount;
        this.regenTimer = info.regenTimer;
        this.currFuel = movement.currFuel;
        this.position = movement.transform.position;
        this.velocity = movement.rb.velocity;
        this.rotation = movement.transform.rotation;
        this.useGravity = movement.rb.useGravity;
        this.drag = movement.rb.drag;
        this.isAbleToMove = movement.isAbleToMove;
        this.alpha = invisibility.alpha;
        this.isPickUp = capturing.isPickUp;
    }

    public void GetFields(PlayerBaseInfo info, Movement movement, Capturing capturing, Invisibility invisibility)
    {
        info.hpAmount = this.currHP;
        info.regenTimer = this.regenTimer;
        movement.currFuel = this.currFuel;
        movement.transform.position = this.position;
        movement.rb.velocity = this.velocity;
        movement.transform.rotation = this.rotation;
        movement.rb.useGravity = this.useGravity;
        movement.rb.drag = this.drag;
        movement.isAbleToMove= this.isAbleToMove;
        invisibility.alpha = this.alpha;
        capturing.isPickUp= this.isPickUp;
    }
}
