using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    

    [Header("MainReferences")]
    private GameManager gameManager;
    public Rigidbody rb;

    [Header("BasicMovement")]
    public bool isAbleToMove;
    public bool isMoving;
    [SerializeField] private float forwardForce;
    [SerializeField] private float sideForce;
    [SerializeField] private KeyCode rightRollPowerKey = KeyCode.E;
    [SerializeField] private KeyCode leftRollPowerKey = KeyCode.Q;
    [SerializeField] private float sideRollsPower;

    [Header("Thrust")]    
    public bool isAbleToThrust;
    public float currFuel;
    [SerializeField] private float maxFuel;
    [SerializeField] private float thrustCost;
    [SerializeField] private float refuelSpeed;
    [SerializeField] private KeyCode thrustKey = KeyCode.Mouse1;
    [SerializeField] private float maxThrustPower = 2;

    [Header("Stabilization")]
    [SerializeField] private float stablizationSpeedX;
    [SerializeField] private float stablizationSpeedZ;

    [Header("Landing")]    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private float landingLoweringPercent = 0.6f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float landingSpeed = 10f;
    [SerializeField] private float rayDistanse;
    [SerializeField] private Transform landPointFront;
    [SerializeField] private Transform landPointRight;
    [SerializeField] private Transform landPointLeft;
    [SerializeField] private Transform landPointBack;

    RaycastHit hitInfoFront;
    RaycastHit hitInfoRight;
    RaycastHit hitInfoLeft;
    private RaycastHit hitInfoBack;

    private void Start()
    {
        gameManager = GameManager.Instance;
        rb.useGravity = true;
        currFuel = maxFuel;

        gameManager.SetThrustingUI(isAbleToThrust);

    }

    private void Update()
    {
        gameManager.SetThrustSliderValue(currFuel / maxFuel);

        if (!gameManager.IsRewind() && isAbleToMove)
        {
            NewInput();
            ThrustManager();
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.IsRewind() && isAbleToMove)
        {
            NewMovement();
        }
    }

    private void NewInput()
    {
        if (Input.GetKeyUp(thrustKey) && isAbleToThrust)
        {
            Vector3 jump = Vector3.zero;
            
            jump += transform.up;

            Debug.Log(jump);
            Jump(jump.normalized);
            
        }
    }

    private void NewMovement()
    {
        Time.timeScale = 0.3f;
        RaycastHit hit;
        float forInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");

        if (Physics.Raycast(groundCheck.position, -transform.up, out hit, groundDistance, groundMask))
        {
            rb.useGravity = false;
            rb.drag = 4;


            Vector3 normal = hit.normal;
            float height = groundDistance;
            Vector3 target = hit.point + normal * height;

            if ((transform.position - hit.point).magnitude >= groundDistance * landingLoweringPercent)
            {
                Debug.Log("lowering");
                rb.AddForce(-transform.up * landingSpeed * Time.deltaTime);
            }

            if (Physics.Raycast(landPointFront.position, -transform.up, out hitInfoFront, rayDistanse, groundMask) && Physics.Raycast(landPointRight.position, -transform.up, out hitInfoRight, rayDistanse, groundMask)
            && Physics.Raycast(landPointLeft.position, -transform.up, out hitInfoLeft, rayDistanse, groundMask) && Physics.Raycast(landPointBack.position, -transform.up, out hitInfoBack, rayDistanse, groundMask))
            {
                Vector3 forwardVector = Vector3.Normalize(hitInfoFront.point - hitInfoBack.point);
                Vector3 sideVector = Vector3.Normalize(hitInfoRight.point - hitInfoLeft.point);
                float angleX = Vector3.SignedAngle(transform.forward, forwardVector, transform.right);
                float angleZ = Vector3.SignedAngle(transform.right, sideVector, transform.forward);
                Stabilization(TranslateDegreceXIntoEulerXForMainBody(transform.eulerAngles.x + angleX), TranslateDegreceZIntoEulerZForMainBody(transform.eulerAngles.z + angleZ));
            }

            float speedFor = forwardForce;
            float speedRot = 100f;

            Vector3 dir = Vector3.Dot(transform.forward, normal) * normal;
            dir = transform.forward - dir;

            rb.AddForce( dir * forInput * speedFor);
            transform.Rotate(Vector3.up * sideInput * Time.deltaTime * speedRot);

        }
        else
        {
            rb.useGravity = true;
            rb.drag = 0f;

            Vector3 rotateVector = Vector3.zero;

            if (Input.GetKey(leftRollPowerKey))
            {
                rotateVector += Vector3.forward * sideRollsPower * Time.deltaTime;
            }
            if (Input.GetKey(rightRollPowerKey))
            {
                rotateVector -= Vector3.forward * sideRollsPower * Time.deltaTime;
            }

            rotateVector += Vector3.right * forInput * sideRollsPower * Time.deltaTime;

            rotateVector += Vector3.up * sideInput * sideRollsPower * Time.deltaTime;

            transform.Rotate(rotateVector);
        }
    }

    public void Jump(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            if (FuelReduce())
            {
                Debug.Log("jump");
                rb.AddForce(dir * maxThrustPower, ForceMode.Impulse);
            }
        }

    }
    public bool FuelReduce()
    {
        if (currFuel >= thrustCost)
        {
            currFuel -= thrustCost;
            return true;
        }
        else return false;
    }

    private void ThrustManager()
    {
        if (currFuel < maxFuel)
            currFuel += refuelSpeed * Time.deltaTime;
        else
            currFuel = maxFuel;
    }

    #region stabilization_Hell
    public bool Stabilization(float x_value, float z_value)
    {
        float a = transform.eulerAngles.x - x_value;

        float b = transform.eulerAngles.z - z_value;

        float changedXEularRotation = TranslateDegreceXIntoEulerXForMainBody(a);

        float changedZEularRotation = TranslateDegreceZIntoEulerZForMainBody(b);



        bool x_rotation = true;
        bool z_rotation = true;
        if (changedXEularRotation > -Time.fixedDeltaTime * stablizationSpeedX * 2 &&
            changedXEularRotation < Time.fixedDeltaTime * stablizationSpeedX * 2) x_rotation = false;
        if (changedZEularRotation > -Time.fixedDeltaTime * stablizationSpeedZ &&
            changedZEularRotation < Time.fixedDeltaTime * stablizationSpeedZ * 2) z_rotation = false;
        if ((!x_rotation) && (!z_rotation)) return false;
        if (x_rotation) StabilizationX(changedXEularRotation, changedZEularRotation);
        if (z_rotation) StabilizationZ(changedZEularRotation);
        return true;
    }
    public void StabilizationX(float changedXEularRotation, float changedZEularRotation)
    {

        if ((changedXEularRotation >= 0f && changedXEularRotation <= 90f && !(changedZEularRotation > 90 && changedZEularRotation < 270)))
            transform.Rotate(-Vector3.right * stablizationSpeedX * Time.deltaTime);
        else if (changedXEularRotation >= 270f && changedXEularRotation <= 360f && !(changedZEularRotation > 90 && changedZEularRotation < 270))
            transform.Rotate(Vector3.right * stablizationSpeedX * Time.deltaTime);
        else if (changedXEularRotation >= 0f && changedXEularRotation <= 90f)
            transform.Rotate(Vector3.right * stablizationSpeedX * Time.deltaTime);
        else if (changedXEularRotation >= 270f && changedXEularRotation <= 360f)
            transform.Rotate(-Vector3.right * stablizationSpeedX * Time.deltaTime);
    }
    public void StabilizationZ(float changedZEularRotation)
    {


        if (changedZEularRotation > 180 && changedZEularRotation < 360)
            transform.Rotate(Vector3.forward * stablizationSpeedZ * Time.deltaTime);
        else
            transform.Rotate(-Vector3.forward * stablizationSpeedZ * Time.deltaTime);
    }



    public float TranslateDegreceXIntoEulerXForMainBody(float a)
    {
        return a > 0f && a <= 90f ? a :
               a > 90f && a <= 180f ? a - 90f :
               a > 180f && a <= 270f ? 360f - (a - 180) :
               a > 270f && a <= 360f ? a :
               a > 360f ? TranslateDegreceXIntoEulerXForMainBody(a - 360f) : TranslateDegreceXIntoEulerXForMainBody(a + 360f);
    }
    public float TranslateDegreceZIntoEulerZForMainBody(float a)
    {
        return a >= 0 && a <= 360 ? a :
               a > 360 ? TranslateDegreceZIntoEulerZForMainBody(a - 360) : TranslateDegreceZIntoEulerZForMainBody(a + 360);
    }
    #endregion

    public bool GettingContact()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }
}
