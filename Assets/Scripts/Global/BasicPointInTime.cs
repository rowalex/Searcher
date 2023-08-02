using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public BasicPointInTime(Vector3 _position , Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }

}
