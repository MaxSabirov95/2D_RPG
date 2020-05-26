using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoals
{
    public GoalType goalType;

    public int requiredAmount;//How much you need 
    public int currentAmount;//How much you have

    public bool isReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if (goalType == GoalType.KillQuest)
        {
            currentAmount++;
            Debug.Log(currentAmount);
        }
    }
}

public enum GoalType
{
    KillQuest,
    PickUpQuest
}
