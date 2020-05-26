using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool questIsActive;

    public string questTitle;
    [TextArea(1, 10)]
    public string questDescription;

    public int expReward;
    public int coinReward;

    public QuestGoals goal;

    public void Complete()
    {
        questIsActive = false;
        Debug.Log("Done");
    }
}
