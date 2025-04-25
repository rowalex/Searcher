using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.PlayerLoop;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve positionCurve;
    public UnityEvent action;

    private Action OnUpdatePos;

    private float elapsedTimePos;

    private void Update()
    {
        OnUpdatePos?.Invoke();
    }

    public void StartMovement()
    {
        elapsedTimePos = 0;
        OnUpdatePos += PosUpdate;
    }

    private void PosUpdate()
    {
        elapsedTimePos += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTimePos / time);

        float curveValue = positionCurve.Evaluate(t);

        transform.position = Vector3.Lerp(point1.position, point2.position, curveValue);

        if (t >= 1f)
        {
            OnUpdatePos -= PosUpdate;
            action?.Invoke();
        }
    }
}
