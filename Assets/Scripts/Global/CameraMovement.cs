using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 offset;

    [SerializeField] float speed;

    private Vector3 velocity = Vector3.one;

    private void FixedUpdate()
    {
        Vector3 toPos = target.position + (target.rotation * offset);
        transform.position = Vector3.SmoothDamp(transform.position, toPos,ref velocity, speed);

        transform.LookAt(target, target.up);
    }
}
