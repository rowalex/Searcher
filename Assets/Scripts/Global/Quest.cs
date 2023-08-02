using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string questComment;
    public int questId;

    Quest( string questName, string questComment,  int questId)
    {
        this.questName = questName;
        this.questComment = questComment;
        this.questId = questId;
    }
}
