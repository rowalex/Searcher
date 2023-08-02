using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Capturing : MonoBehaviour
{
    public bool isCapturing;
    public bool isDistanceToCapture;
    private LayerMask mask;
    private KeyCode captureKey = KeyCode.F;
    private float timeToCapture;
    private float captureDistance;
    private bool isAbleToCapture;

    private void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isAbleToCapture = gameManager.isAbleToCapture;
        timeToCapture = gameManager.timeToCapture;
        captureDistance = gameManager.captureDistance;
        captureKey = gameManager.captureKey;
        mask = gameManager.captureMask;
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, captureDistance, mask) && isAbleToCapture)
        {
            isDistanceToCapture = true;
            if (Input.GetKeyDown(captureKey))
            {
                isCapturing = true;
                Invoke("Capture", timeToCapture);
            }

        }
        else
        {
            isCapturing = false;
            isDistanceToCapture = false;
        }
    }

    public void Capture()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, captureDistance, mask);

        foreach (Collider target in targets)
        {
            foreach (QuestTrigger s in target.GetComponents<QuestTrigger>())
                if (s.triggerType == QuestTrigger.TriggerType.onCapture) s.Capturing();
            foreach (SentenceTrigger s in target.GetComponents<SentenceTrigger>())
                if (s.triggerType == SentenceTrigger.TriggerType.onCapture) s.Capturing();
        }

        isCapturing = false;
    }
}
