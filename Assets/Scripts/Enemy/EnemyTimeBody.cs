using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeBody : ITimeBody
{
    
    private void Start()
    {
        rewindManager = RewindManager.Instance;
        rewindManager.OnRewind += OnRewind;
        rewindManager.OnSaveInfo += OnSaveInfo;
        rewindManager.OnClear += OnClear;
    }

    [SerializeField] private EnemiesAI ai;
    [SerializeField] private EnemiesFOV fov;

    private List<EnemiesFields> fields = new List<EnemiesFields>();


    protected override void OnRewind()
    {
        fields[0].GetField(ai);
        fields.RemoveAt(0);
    }

    protected override void OnSaveInfo()
    {
        fields.Insert(0, new EnemiesFields(ai));
    }

    protected override void OnClear()
    {
        fields.Clear();
        fields.TrimExcess();
    }

}

public class EnemiesFields
{
    private Vector3 lastTargetPosition;
    private bool isFollowPoint;
    private float hitTimer;
    private int pointIndex;
    private List<Vector3> trackPositions;
    private Vector3 pos;
    private Quaternion rot;

    public EnemiesFields(EnemiesAI obj)
    {
        lastTargetPosition = obj.lastTargetPosition;
        isFollowPoint = obj.isFollowPoint;
        hitTimer = obj.hitTimer;
        pointIndex = obj.pointIndex;
        trackPositions = new List<Vector3>(obj.trackPositions);
        pos = obj.transform.position;
        rot = obj.transform.rotation;
    }

    public void GetField(EnemiesAI obj)
    {
        obj.lastTargetPosition = lastTargetPosition;
        obj.isFollowPoint = isFollowPoint;
        obj.hitTimer = hitTimer;
        obj.pointIndex = pointIndex;
        obj.trackPositions = new List<Vector3>(trackPositions);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }
}




