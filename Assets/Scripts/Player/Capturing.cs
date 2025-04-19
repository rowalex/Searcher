using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Capturing : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private bool isCapturing;
    [SerializeField] private bool isDistanceToCapture;
    [SerializeField] private float timeToCapture;
    [SerializeField] private float captureDistance;
    [SerializeField] private KeyCode captureKey = KeyCode.F;
    [SerializeField] private LayerMask mask;


    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        gameManager.SetCapturingUI(isCapturing && !gameManager.IsRewind());
        gameManager.SetDistanceToCaptureBool(isDistanceToCapture && !gameManager.IsRewind());

        if (Physics.CheckSphere(transform.position, captureDistance, mask))
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
        if (isCapturing)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, captureDistance, mask);

            foreach (Collider target in targets)
            {
                foreach (QuestTrigger s in target.GetComponents<QuestTrigger>())
                    if (s.triggerType == QuestTrigger.TriggerType.onCapture) s.Capturing();
                foreach (SentenceTrigger s in target.GetComponents<SentenceTrigger>())
                    if (s.triggerType == SentenceTrigger.TriggerType.onCapture) s.Capturing();
            }
        }
        isCapturing = false;
    }
}
