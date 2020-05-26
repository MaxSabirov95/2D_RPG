using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerUI Player;
    public GameObject talkWithNPC;

    public GameObject questTable;
    public Text titleText;
    public Text descriptionText;
    public Text expText;
    public Text coinText;

    bool questOpen;


    void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 9);
        talkWithNPC.SetActive(false);
    }

    void Update()
    {
        if (Vector2.Distance(Player.transform.position, transform.position) < 3f)
        {
            if (!questOpen)
            {
                talkWithNPC.SetActive(true);
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenQuestScreen();
                questOpen = true;
            }
        }
        else
        {
            talkWithNPC.SetActive(false);
            questTable.SetActive(false);
            questOpen = false;
        }
    }


    public void OpenQuestScreen()
    {
        talkWithNPC.SetActive(false);
        questTable.SetActive(true);
        //titleText.text = quest.questTitle;
        descriptionText.text = quest.questDescription;
        expText.text = quest.expReward.ToString();
        coinText.text = quest.coinReward.ToString();
    }

    public void AcceptQuest()
    {
        questTable.SetActive(false);
        quest.questIsActive = true;
        Player.quest = quest;
    }
}
