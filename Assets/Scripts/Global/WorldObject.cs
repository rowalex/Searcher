using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private Vector3 pointA;
    [SerializeField] private Vector3 finPositioon;
    private Vector3 rotationA;
    [SerializeField] private Vector3 finRotation;
    [SerializeField] private float durationPos;
    [SerializeField] private float durationRot;
    [SerializeField] private AnimationCurve positionCurvePos;
    [SerializeField] private AnimationCurve positionCurveRot;

    private float elapsedTimePos;
    private float elapsedTimeRot;

    private Action OnUpdatePos;
    private Action OnUpdateRot;
    
    public void LaunchPositionAnim()
    {
        elapsedTimePos = 0;
        pointA = transform.position;
        OnUpdatePos += PosUpdate;
    }
    public void LaunchRotationAnim()
    {
        elapsedTimeRot = 0;
        rotationA = transform.rotation.eulerAngles;
        OnUpdateRot += RotUpdate;
    }


    private void FixedUpdate()
    {
        OnUpdatePos?.Invoke();
        OnUpdateRot?.Invoke();
    }
       

    private void PosUpdate()
    {
        elapsedTimePos += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTimePos / durationPos);

        float curveValue = positionCurvePos.Evaluate(t);

        transform.position = Vector3.Lerp(pointA, finPositioon, curveValue);

        if (t >= 1f)
        {
            OnUpdatePos -= PosUpdate;
        }
    }
    private void RotUpdate()
    {

        float t = Mathf.Clamp01(elapsedTimeRot / durationRot);
        
        float curveValue = positionCurveRot.Evaluate(t);

        Vector3 currRot = Vector3.Lerp(rotationA, finRotation, curveValue);

        Debug.Log(currRot);

        transform.rotation = Quaternion.Euler(currRot);

        if (t >= 1f)
        {
            OnUpdateRot -= RotUpdate;
        }

        elapsedTimeRot += Time.deltaTime;
    }
}
