using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class SentenceTrigger : MonoBehaviour
{
    [SerializeField] Sentence[] sent;

    public enum TriggerType { onTrigger, onStart, onQuestFin, onQuestAdd , onCapture, onInteract};
    [SerializeField] public TriggerType triggerType;
    [SerializeField] private GameObject triggerObject;

    public int questID;

    private QuestManager questManager;

    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

        if (triggerType == TriggerType.onStart)
        {
            SentSentence();
        }
    }

    private void Update()
    {
        if (triggerType == TriggerType.onQuestAdd)
        {
            if (questManager.IsAddQuest(questID))
            {
                SentSentence();
            }
        }
        if (triggerType == TriggerType.onQuestFin)
        {
            if (questManager.IsFinQuest(questID))
            {
                SentSentence();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.onTrigger && other.gameObject == triggerObject)
        {
            SentSentence();
        }
    }


    public void SentSentence()
    {
        DialogManager dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        foreach (Sentence c in sent)
            dialogManager.AddSentence(c);
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (gameObject.GetComponent<BoxCollider>()) gameObject.GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }
}

