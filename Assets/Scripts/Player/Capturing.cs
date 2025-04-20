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
    [SerializeField] private LayerMask captureMask;
    [SerializeField] private LayerMask interactMask;


    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        gameManager.SetCapturingUI(isCapturing && !gameManager.IsRewind());
        gameManager.SetDistanceToCaptureBool(isDistanceToCapture && !gameManager.IsRewind());



        if (Physics.CheckSphere(transform.position, captureDistance, interactMask))
        {
            isDistanceToCapture = true;
            if (Input.GetKeyDown(captureKey))
            {
                Interact();
            }

        }else if (Physics.CheckSphere(transform.position, captureDistance, captureMask))
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
            Collider[] targets = Physics.OverlapSphere(transform.position, captureDistance, captureMask);

            foreach (Collider target in targets)
            {
                foreach (QuestTrigger s in target.GetComponents<QuestTrigger>())
                    if (s.triggerType == QuestTrigger.TriggerType.onCapture) s.QuestInvoko();
                foreach (SentenceTrigger s in target.GetComponents<SentenceTrigger>())
                    if (s.triggerType == SentenceTrigger.TriggerType.onCapture) s.SentSentence();
                foreach (ActionTrigger s in target.GetComponents<ActionTrigger>())
                    if (s.triggerType == ActionTrigger.TriggerType.onCapture) s.InvokeAction();
            }
        }
        isCapturing = false;
    }

    public void Interact()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, captureDistance, interactMask);

        foreach (Collider target in targets)
        {
            foreach (QuestTrigger s in target.GetComponents<QuestTrigger>())
                if (s.triggerType == QuestTrigger.TriggerType.onInteract) s.QuestInvoko();
            foreach (SentenceTrigger s in target.GetComponents<SentenceTrigger>())
                if (s.triggerType == SentenceTrigger.TriggerType.onInteract) s.SentSentence();
            foreach (ActionTrigger s in target.GetComponents<ActionTrigger>())
                if (s.triggerType == ActionTrigger.TriggerType.onInteract) s.InvokeAction();
        }
    }
}
