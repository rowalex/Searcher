using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemiesFOV : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeeTarget = false;

    public Transform target;


    [SerializeField] private bool showVision;
    [SerializeField] private Light visionArea;

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if (targetsInViewRadius.Length > 0)
        {
            target = targetsInViewRadius[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (target.gameObject.GetComponent<Invisibility>().isVisible)
                        canSeeTarget = true;
                    else
                        canSeeTarget = false;
                }
                else canSeeTarget = false;
            }
            else canSeeTarget = false;

        }
        else if (canSeeTarget)
            canSeeTarget = false;
    }

    public void SetVisionLight(bool isVisible)
    {
        visionArea.enabled = isVisible;
        visionArea.range = viewRadius;
        visionArea.spotAngle = viewAngle;
    }
}
