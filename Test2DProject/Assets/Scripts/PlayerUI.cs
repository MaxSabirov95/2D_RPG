using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Max_Almog.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviourPunCallbacks
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
        public float HP=100;
        public float Mana = 100;
        public float XP = 0;
        public float Level = 1;
        public float WholeHP=100;
        public float WholeMana=100;
        public float superAttackTimer;
        public float WholeXP;

        public Quest quest;

        public ParticleSystem LevelUp;

        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            //HPText.GetComponent<Text>().text = "" + HP.ToString("f0");
            //ManaText.GetComponent<Text>().text = "" + Mana.ToString("f0");
            //XPText.GetComponent <Text>().text = "" + XP.ToString("f0");
            //LevelText.GetComponent<Text>().text = "" + Level.ToString("f0");
            //WholeHPText.GetComponent<Text>().text = "" + WholeHP.ToString("f0");
            //WholeManaText.GetComponent<Text>().text = "" + WholeMana.ToString("f0");
            //WholeXPText.GetComponent<Text>().text = "" + WholeXP.ToString("f0");
            //LevelText.GetComponent<Text>().text = "" + Level.ToString("f0");
            //superAttackTimerText.GetComponent<Text>().text = "" + superAttackTimer.ToString("f0");

            if (HP <= 0)
            {
                GameManager.instance.LeaveRoom();
            }
            else if (HP < WholeHP)
            {
                HP += 1*0.5f*Time.deltaTime;
                HP = Mathf.Clamp(HP, 0, WholeHP);
            }

            if (Mana < WholeMana)
            {
                Mana += 1 * 0.25f * Time.deltaTime;
                Mana = Mathf.Clamp(Mana, 0, WholeMana);
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

        public void TakeDamage(float damage)
        {
            HP -= damage;
        }
    }
}
