using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTimeBody : ITimeBody
{

    private void Start()
    {
        rewindManager = RewindManager.Instance;
        rewindManager.OnRewind += OnRewind;
        rewindManager.OnSaveInfo += OnSaveInfo;
        rewindManager.OnClear += OnClear;
    }

    [SerializeField] private CameraMovement cameraScript;

    private List<CameraFields> fields = new List<CameraFields>();

     
    protected override void OnRewind()
    {
        fields[0].GetField(cameraScript);
        fields.RemoveAt(0);
    }

    protected override void OnSaveInfo()
    {
        fields.Insert(0, new CameraFields(cameraScript));
    }

    protected override void OnClear()
    {
        fields.Clear();
    }

}


class CameraFields
{
    private Vector3 position;
    private Quaternion rotation;
    private bool cameraType;
    
    public CameraFields(CameraMovement obj)
    {
        this.position = obj.transform.position;
        this.rotation = obj.transform.rotation;
        this.cameraType = obj.lookOnVelocity;
    }

    public void GetField(CameraMovement obj)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.lookOnVelocity = cameraType;
    }
}
