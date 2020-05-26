using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // HP-Mana-XP-Level
    public Text HPText;
    public Text ManaText;
    public Text XPText;
    public Text LevelText;
    public Text WholeHPText;
    public Text WholeManaText;
    public Text WholeXPText;
    public Text superAttackTimerText;
    public static float HP=100;
    public static float Mana = 100;
    public static float XP = 0;
    public static float Level = 1;
    public static float WholeHP=100;
    public static float WholeMana=100;
    public static float superAttackTimer;
    public float WholeXP;

    public Quest quest;

    public ParticleSystem LevelUp;

    void Update()
    {
        HPText.GetComponent<Text>().text = "" + HP.ToString("f0");
        ManaText.GetComponent<Text>().text = "" + Mana.ToString("f0");
        XPText.GetComponent <Text>().text = "" + XP.ToString("f0");
        LevelText.GetComponent<Text>().text = "" + Level.ToString("f0");
        WholeHPText.GetComponent<Text>().text = "" + WholeHP.ToString("f0");
        WholeManaText.GetComponent<Text>().text = "" + WholeMana.ToString("f0");
        WholeXPText.GetComponent<Text>().text = "" + WholeXP.ToString("f0");
        LevelText.GetComponent<Text>().text = "" + Level.ToString("f0");
        superAttackTimerText.GetComponent<Text>().text = "" + superAttackTimer.ToString("f0");

        if (HP < WholeHP)
        {
            HP += 1*0.5f*Time.deltaTime;
        }

        if (HP > WholeHP)
        {
            HP = WholeHP;
        }

        if (Mana > WholeMana)
        {
            Mana = WholeMana;
        }

        if (Mana < WholeMana)
        {
            Mana += 1 * 0.25f * Time.deltaTime;
        }

        if (XP >= WholeXP)
        {
            LevelUp.Play();
            XP -= WholeXP;
            Level++;
            GameUI.skillpoints++;
            WholeXP *= 1.2f;
            WholeHP += 10f;
            WholeMana += 10f;
        }

        if (superAttackTimer > 0)
        {
            superAttackTimer -= Time.deltaTime;
        }
    }

    public void Killquest()
    {
        if (quest.questIsActive)
        {
            quest.goal.EnemyKilled();
            if (quest.goal.isReached())
            {
                XP += quest.expReward;
                GameItems.money += quest.coinReward;
                quest.Complete();
            }
        }
    }
}
