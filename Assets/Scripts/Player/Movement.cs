using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [Header("MainReferences")]
    private GameManager gameManager;
    public Rigidbody rb;


    [Header("BasicMovement")]
    public bool isMoving;
    private bool isAbleToMove;
    public KeyCode frontForceKey = KeyCode.W;
    public float frontForce;
    public KeyCode rightForceKey = KeyCode.D;
    public KeyCode leftForceKey = KeyCode.A;
    public float sideForce;
    public KeyCode backForceKey = KeyCode.S;
    public float backForce;
    public KeyCode upForceKey = KeyCode.Space;
    public float upForce;
    public KeyCode downForceKey = KeyCode.LeftShift;
    public float downForce;
    public KeyCode rightRollPowerKey = KeyCode.E;
    public KeyCode leftRollPowerKey = KeyCode.Q;
    public float sideRollsPower;
    public float globalMovementMultiplyer = 5f;
    private bool isFrontForce = false;
    private bool isRightForce = false;
    private bool isLeftForce = false;
    private bool isBackForce = false;
    private bool isUpForce = false;
    private bool isDownForce = false;

    [Header("MouseControl")]
    public bool isMouseMoving;
    private bool isMouseWork = true;
    public float mouseSensitivityX = 1f;
    public float mouseSensitivityY = 1f;
    private Vector2 mouseTurn;

    [Header("Thrust")]
    public bool isThrusting = false;
    private bool isAbleToThrust;
    public KeyCode thrustKey = KeyCode.Mouse1;
    public float maxThrustPower = 2;
    public float minThrustPower = 1.5f;
    public float thrusterDrag = 1;
    private float currentThrustPower = 1;

    [Header("Stabilization")]    
    public bool isStabializing = false;
    private bool isAbleToStabizate;
    public KeyCode stabilizationKey = KeyCode.T;
    public float stablizationSpeedX;
    public float stablizationSpeedZ;
    public float x_value;
    public float z_value;


    [Header("Landing")]    
    public bool isLandable = false;
    public bool isLanding = false;
    private bool isAbleToLand;
    public KeyCode landingKey = KeyCode.R;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public float landingSpeed = 10f;
    public float rayDistanse;
    public Transform landPointFront;
    public Transform landPointRight;
    public Transform landPointLeft;
    public Transform landPointBack;


    RaycastHit hitInfoFront;
    RaycastHit hitInfoRight;
    RaycastHit hitInfoLeft;
    RaycastHit hitInfoBack;



    public void SetPossibilities()
    {
        isAbleToLand = gameManager.isAbleToLand;
        isAbleToMove = gameManager.isAbleToMove;
        isAbleToStabizate = gameManager.isAbleToStabizate;
        isAbleToThrust = gameManager.isAbleToThrust;
        isMouseWork = gameManager.isMouseWork;
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetPossibilities();
    }

    private void Update()
    {
        SetPossibilities();

        mouseTurn.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivityX;
        mouseTurn.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivityY;    

        if (isMouseWork)
        {
            transform.Rotate(-Vector3.right * mouseTurn.y);
            transform.Rotate(Vector3.up * mouseTurn.x);
        }


        if (isAbleToStabizate)
        {
            if (Input.GetKeyDown(stabilizationKey)) isStabializing = true;
            if (Input.GetKeyUp(stabilizationKey)) isStabializing = false;
        }

        if (isAbleToLand)
        {
            if (Physics.Raycast(landPointFront.position, -transform.up, out hitInfoFront, rayDistanse) && Physics.Raycast(landPointRight.position, -transform.up, out hitInfoRight, rayDistanse)
                && Physics.Raycast(landPointLeft.position, -transform.up, out hitInfoLeft, rayDistanse) && Physics.Raycast(landPointBack.position, -transform.up, out hitInfoBack, rayDistanse))
                isLandable = true;
            else
                isLandable = false;

            if (Input.GetKeyDown(landingKey))
                isLanding = true;
        }


        if (isAbleToThrust)
        {
            if (Input.GetKeyDown(thrustKey))
            {
                if (gameManager.FuelReduce())
                {
                    isThrusting = true;
                    currentThrustPower = maxThrustPower;
                }
            }
            if (Input.GetKeyUp(thrustKey))
            {
                isThrusting = false;
                currentThrustPower = 1;
            }
        }


    }

    void FixedUpdate()
    {


        isFrontForce = false;
        isBackForce = false;
        isLeftForce = false;
        isRightForce = false;
        isUpForce = false;
        isDownForce = false;
        if (isAbleToMove)
        {
            Vector3 movementVector = Vector3.zero;
            int n = 0;

            #region Basic_Movement_Input
            if (Input.GetKey(frontForceKey))
            {
                movementVector += transform.forward;
                isFrontForce = true;
                n++;
            }

            if (Input.GetKey(backForceKey))
            {
                movementVector += -transform.forward;
                isBackForce = true;
                n++;
            }

            if (Input.GetKey(leftForceKey))
            {
                movementVector += -transform.right;
                isLeftForce = true;
                n++;
            }

            if (Input.GetKey(rightForceKey))
            {
                movementVector += transform.right;
                isRightForce = true;
                n++;
            }

            if (Input.GetKey(upForceKey))
            {
                movementVector += transform.up;
                isUpForce = true;
                n++;
            }

            if (Input.GetKey(downForceKey))
            {
                movementVector += -transform.up;
                isDownForce = true;
                n++;
            }

            #endregion

            if (isFrontForce && isBackForce)
            {
                n -= 2;
                isFrontForce = false;
                isBackForce = false;
            }
            if (isLeftForce && isRightForce)
            {
                n -= 2;
                isLeftForce = false;
                isRightForce = false;
            }
            if (isUpForce && isDownForce)
            {
                n -= 2;
                isUpForce = false;
                isDownForce = false;
            }

            if (movementVector != Vector3.zero)
            {
                movementVector = movementVector.normalized * ((isFrontForce ? frontForce : 0) + (isBackForce ? backForce : 0) + (isLeftForce ? sideForce : 0) + (isRightForce ? sideForce : 0) + (isUpForce ? upForce : 0) + (isDownForce ? downForce : 0)) / n;
            }

            Vector3 rotateVector = Vector3.zero;


            if (Input.GetKey(leftRollPowerKey))
            {
                rotateVector += Vector3.forward * sideRollsPower * Time.deltaTime;
            }
            if (Input.GetKey(rightRollPowerKey))
            {
                rotateVector -= Vector3.forward * sideRollsPower * Time.deltaTime;
            }

            if (rotateVector != Vector3.zero || movementVector != Vector3.zero)
                isMoving = true;
            else 
                isMoving = false;

            transform.Rotate(rotateVector);
            rb.AddForce(movementVector * globalMovementMultiplyer * currentThrustPower * Time.deltaTime, ForceMode.VelocityChange);
        }

        if (isThrusting) Thrust();
        if (isStabializing) isStabializing = Stabilization(x_value, z_value);
        if (isLanding && isLandable) isLanding = Landing();
        else isLanding = false;

    }

    public void Thrust()
    {
        int percent = ((int)((currentThrustPower - minThrustPower) * 10)) / (int)((maxThrustPower - minThrustPower));
        if (currentThrustPower > minThrustPower) currentThrustPower -= percent * thrusterDrag * Time.deltaTime;
        else currentThrustPower = minThrustPower;
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

    public bool Landing()
    {
        bool stabilizationPhase = true;
        bool landingPhase = true;

        Vector3 forwardVector = Vector3.Normalize(hitInfoFront.point - hitInfoBack.point);
        Vector3 sideVector = Vector3.Normalize(hitInfoRight.point - hitInfoLeft.point);
        float angleX = Vector3.SignedAngle(transform.forward, forwardVector, transform.right);
        float angleZ = Vector3.SignedAngle(transform.right, sideVector, transform.forward);
        if (!Stabilization(TranslateDegreceXIntoEulerXForMainBody(transform.eulerAngles.x + angleX), TranslateDegreceZIntoEulerZForMainBody(transform.eulerAngles.z + angleZ)))
            stabilizationPhase = false;

        if (!stabilizationPhase && !GettingContact())
            rb.AddForce(-transform.up * landingSpeed * Time.deltaTime, ForceMode.VelocityChange);
        else
            landingPhase = false;

        if (!landingPhase && !stabilizationPhase)
            return false;
        else
            return true;

    }
    public bool GettingContact()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }
}
