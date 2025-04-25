using System.Runtime.CompilerServices;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] private PlayerBaseInfo player;

    [SerializeField] private float horDistance;
    [SerializeField] private float vertDistance;

    [SerializeField] float speed;

    private Vector3 velocity = Vector3.one;

    [SerializeField] public bool lookOnVelocity;

    private bool isWork = false;

    [SerializeField] private float mouseLookSpeed;
    [SerializeField] private float inactivityThreshold;

    private Vector3 initPos;
    private float inactivityTimer;
    private float up;
    private float right;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isWork)
        {
            lookOnVelocity = !lookOnVelocity;
        }
    }

    private void Start()
    {
        initPos = target.transform.position - player.transform.position;
    }

    private void FixedUpdate()
    {
        if (!RewindManager.Instance.isRewind && isWork)
        {
            Vector3 toPos;
            if (lookOnVelocity)
            {
                Vector3 forward = player.movementVector.magnitude > 0.15f ? player.movementVector : player.transform.forward;
                forward.y = 0;
                forward.Normalize();
                toPos = player.transform.position - forward * horDistance + Vector3.up * vertDistance;
                transform.LookAt(player.transform.position);

            }
            else
            {
                toPos = player.transform.position + (player.transform.rotation * new Vector3(0, vertDistance, -horDistance));
                transform.LookAt(target.transform, target.transform.up);
            }
            RotateByMouse();

            RaycastHit hit;
            Vector3 direction = toPos - target.transform.position;
            float distance = direction.magnitude;

            if (Physics.Raycast(player.transform.position, direction.normalized, out hit, distance) && !lookOnVelocity)
            {
                toPos = hit.point - direction.normalized * 0.5f;
            }

            transform.position = Vector3.SmoothDamp(transform.position, toPos, ref velocity, speed);
        }
    }
    
    public void SetWorkState(bool work)
    {
        isWork = work; 
    }

    private void RotateByMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            inactivityTimer = 0f;
            right += mouseX * mouseLookSpeed * Time.deltaTime;
            up += mouseY * mouseLookSpeed * Time.deltaTime;

            var rightvect = player.transform.right * right;
            var upvect = player.transform.up * up;

            target.transform.position = player.transform.position + initPos + rightvect + upvect;
        }
        else
        {
            inactivityTimer += Time.deltaTime;

            if (inactivityTimer >= inactivityThreshold)
            {
                right = 0;
                up = 0;
                target.transform.position = player.transform.position + initPos;
            }
        }
    }
}
