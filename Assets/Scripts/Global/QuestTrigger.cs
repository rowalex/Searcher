using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    public enum TriggerType {onTrigger, onStart, onCapture};
    [SerializeField] public TriggerType triggerType;
    enum QuestType { AddQuest, FinishQuest };
    [SerializeField] QuestType questType;
    [SerializeField] private GameObject triggerObject;

    enum Finishlvl { yes ,no}
    [SerializeField] Finishlvl isFinal; 
    private void Start()
    {
        if (triggerType == TriggerType.onStart)
        {
            switch (questType)
            {
                case QuestType.AddQuest: AddQuest(); break;
                case QuestType.FinishQuest: FinishQuest(); break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.onTrigger && other.gameObject == triggerObject)
        {
            switch (questType)
            {
                case QuestType.AddQuest: AddQuest(); break;
                case QuestType.FinishQuest: FinishQuest(); break;
            }
        }
    }
    public void AddQuest()
    {
        QuestManager questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        foreach (Quest q in quests)
            questManager.AddQuest(q);
        //if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
        //if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }
    public void FinishQuest()
    {
        QuestManager questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        foreach (Quest q in quests)
            if (!questManager.FinishQuest(q))
            {
                return;
            }
        //if(gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().enabled = false;
        //if(gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
        if (isFinal == Finishlvl.yes) GameManager.Instance.isFinish = true;
    }

    public void Capturing()
    {
        switch (questType)
        {
            case QuestType.AddQuest: AddQuest(); break;
            case QuestType.FinishQuest: FinishQuest(); break;
        }
    }
}
