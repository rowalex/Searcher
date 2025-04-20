using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTrigger : MonoBehaviour
{
    public enum TriggerType { onTrigger, onStart, onQuestFin, onQuestAdd, onCapture, onInteract};
    [SerializeField] public TriggerType triggerType;

    [SerializeField] private GameObject triggerObject;

    public int questID;

    private QuestManager questManager;

    public UnityEvent action;

    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

        if (triggerType == TriggerType.onStart)
        {
            InvokeAction();
        }
    }

    private void Update()
    {
        if (triggerType == TriggerType.onQuestAdd)
        {
            if (questManager.IsAddQuest(questID))
            {
                InvokeAction();
            }
        }
        if (triggerType == TriggerType.onQuestFin)
        {
            if (questManager.IsFinQuest(questID))
            {
                InvokeAction();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + (other.gameObject == triggerObject));
        if (triggerType == TriggerType.onTrigger && other.gameObject == triggerObject)
        {
            InvokeAction();
        }
    }


    public void InvokeAction()
    {
        Debug.Log("InvokeAction");
        action?.Invoke();
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (gameObject.GetComponent<BoxCollider>()) gameObject.GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }
}
