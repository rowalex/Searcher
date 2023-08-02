using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;


public class EnemiesFields
{
    private Vector3 lastTargetPosition;
    private bool isFollowPoint;
    private float hitTimer;
    private int pointIndex;
    private List<Vector3> trackPositions;
    private Vector3 pos;
    private Quaternion rot;

    public EnemiesFields(EnemiesAI obj)
    {
        lastTargetPosition = obj.lastTargetPosition;
        isFollowPoint = obj.isFollowPoint;
        hitTimer = obj.hitTimer;
        pointIndex = obj.pointIndex;
        trackPositions = new List<Vector3>(obj.trackPositions);
        pos = obj.transform.position;
        rot = obj.transform.rotation;
    }

    public void GetField(EnemiesAI obj)
    {
        obj.lastTargetPosition = lastTargetPosition;
        obj.isFollowPoint = isFollowPoint;
        obj.hitTimer = hitTimer;
        obj.pointIndex = pointIndex;
        obj.trackPositions = new List<Vector3>(trackPositions);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }
}



public class EnemiesAI : MonoBehaviour
{
    private GameManager gameManager;
    private List<EnemiesFields> fields;

    private EnemiesFOV fov;
    private Rigidbody rb;

    private bool isWork = true;
    public Vector3 lastTargetPosition;
    public bool isFollowPoint;
    
    [Range(0,10)]
    public float turningSpeed;

    public float chargeDistance;
    public float movementSpeed; 
    public float chargeSpeed;

    public float checkDistance;

    public float hitTimer;

    public Transform[] patrolPoints;
    public int pointIndex = 0;
    
    public List<Vector3> trackPositions;
    private float trackDistance = 5;

    void Start()
    {
        fields = new List<EnemiesFields>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        fov = GetComponent<EnemiesFOV>();
        rb = GetComponent<Rigidbody>();
        isFollowPoint = false;
        trackPositions = new List<Vector3>();
    }

    void FixedUpdate()
    {
        isWork = GameObject.Find("GameManager").GetComponent<GameManager>().isEnemiesWork;
        if (isWork)
        {
            if (!gameManager.isRewind)
            {
                if (fov.canSeeTarget)
                {
                    lastTargetPosition = fov.target.transform.position;
                    isFollowPoint = true;
                }

                hitTimer -= Time.deltaTime;

                 if (hitTimer > 0 && fov.canSeeTarget)
                {
                    rb.AddForce((transform.position - lastTargetPosition).normalized * movementSpeed  / 2 * Time.deltaTime, ForceMode.VelocityChange);
                }
                if (isFollowPoint && hitTimer < 0)
                {
                    GoToPoint(lastTargetPosition);
                    isFollowPoint = !CheckPoint(lastTargetPosition);
                    TrackingPath();
                }

                if (isFollowPoint)
                {
                    LookAtPoint(lastTargetPosition);
                }

                if (!isFollowPoint)
                {
                    if (trackPositions.Count == 0)
                    {
                        LookAtPoint(patrolPoints[pointIndex].position);
                        GoToPoint(patrolPoints[pointIndex].position);
                        if (CheckPoint(patrolPoints[pointIndex].position)) pointIndex = (pointIndex + 1) % patrolPoints.Length;
                    }
                    else
                    {
                        LookAtPoint(trackPositions[trackPositions.Count - 1]);
                        GoToPoint(trackPositions[trackPositions.Count - 1]);
                        if (CheckPoint(trackPositions[trackPositions.Count - 1])) trackPositions.RemoveAt(trackPositions.Count - 1);
                    }
                }
                if (fields.Count > (gameManager.timeForRewind / Time.fixedDeltaTime) - 1) fields.RemoveAt(fields.Count - 1);
                fields.Insert(0, new EnemiesFields(this));
            }
            else
            {
                if (fields.Count > 0)
                {
                    fields[0].GetField(this);
                    fields.RemoveAt(0);
                }
            }
        }
    }

    private void TrackingPath()
    {
        if (trackPositions.Count < 1) trackPositions.Add(transform.position);
        Vector3 pos = transform.position;
        if (Vector3.Distance(pos, trackPositions[trackPositions.Count - 1]) > trackDistance) trackPositions.Add(pos);
    }
    public void LookAtPoint(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotGoal, Time.deltaTime * turningSpeed);
    }

    public void GoToPoint(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < chargeDistance && fov.target.position == target)
           rb.AddForce((target - transform.position).normalized * chargeSpeed * Time.deltaTime, ForceMode.VelocityChange);
        else
           rb.AddForce((target - transform.position).normalized * movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }


    public bool CheckPoint(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < checkDistance)
        {
            return true;
        }
        else
            return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fov.target == collision.gameObject.transform) hitTimer = 1.5f;
    }

}
