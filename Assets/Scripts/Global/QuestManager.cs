using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    List<Quest> quests;
    List<int> finID;
    public GameObject questWindow;
    public Text questName;
    public Text questComment;

    public bool isQuest;

    void Awake()
    {
        isQuest = false;
        quests = new List<Quest>();
        finID = new List<int>();
        questWindow.SetActive(false);
    }

    private void Update()
    {
        if (quests.Count > 0)
        {
            isQuest = true;
            ShowQuest();

        }
        else
        {
            isQuest = false;
            questWindow.SetActive(false);
        }
    }



    public void ShowQuest()
    {
        questName.text = quests[0].questName;
        questComment.text = quests[0].questComment;
        questWindow.SetActive(true);
    }


    public  bool FinishQuest(Quest quest)
    {
        foreach (Quest q in quests)
            if (q.questId == quest.questId)
            {
                finID.Add(q.questId);
                quests.Remove(q);
                return true;
            }
        return false;
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    public bool IsAddQuest(int id)
    {
        foreach (Quest q in quests)
            if (q.questId == id)
            {
                return true;
            }
        return false;
    }
    public bool IsFinQuest(int id)
    {
        foreach (int q in finID)
            if (q == id)
            {
                return true;
            }
        return false;
    }
}
