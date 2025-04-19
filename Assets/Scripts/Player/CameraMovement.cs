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
            if (lookOnVelocity)
                lookOnVelocity = false;
            else lookOnVelocity = true;
        }
    }

    private void FixedUpdate()
    {
        if (!RewindManager.Instance.isRewind)
            if (lookOnVelocity)
            {
                Vector3 forward = player.movementVector.magnitude > 0.15f ? player.movementVector : target.transform.forward;
                forward.y = 0;
                forward.Normalize();
                Vector3 targetPosition = target.transform.position - forward * horDistance + Vector3.up * vertDistance;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, speed);
                transform.LookAt(player.transform.position);

            }
            else
            {
                Vector3 toPos = target.transform.position + (target.transform.rotation * new Vector3(0, vertDistance, -horDistance));
                transform.position = Vector3.SmoothDamp(transform.position, toPos, ref velocity, speed);
                transform.LookAt(target.transform, target.transform.up);
            }
    }
}
