using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] private PlayerBaseInfo player;

    [SerializeField] private float horDistance;
    [SerializeField] private float vertDistance;

    [SerializeField] float speed;

    private Vector3 velocity = Vector3.one;

    [SerializeField] public bool lookOnVelocity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            lookOnVelocity = !lookOnVelocity;
        }
    }

    private void FixedUpdate()
    {
        if (!RewindManager.Instance.isRewind)
        {
            Vector3 toPos;
            if (lookOnVelocity)
            {
                Vector3 forward = player.movementVector.magnitude > 0.15f ? player.movementVector : target.transform.forward;
                forward.y = 0;
                forward.Normalize();
                toPos = target.transform.position - forward * horDistance + Vector3.up * vertDistance;
                transform.LookAt(player.transform.position);

            }
            else
            {
                toPos = target.transform.position + (target.transform.rotation * new Vector3(0, vertDistance, -horDistance));
                transform.LookAt(target.transform, target.transform.up);
            }

            // Raycasting from target to camera
            RaycastHit hit;
            Vector3 direction = toPos - target.transform.position;
            float distance = direction.magnitude;

            if (Physics.Raycast(target.transform.position, direction.normalized, out hit, distance) && !lookOnVelocity)
            {
                toPos = hit.point - direction.normalized * 0.5f;
            }

            transform.position = Vector3.SmoothDamp(transform.position, toPos, ref velocity, speed);
        }
    }
}
