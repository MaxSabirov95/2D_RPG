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
        private void Awake()
        {
            BlackBoard.playerUI = this;
        }
        void Update()
        {
            HPText.GetComponent<Text>().text = "HP " + HP.ToString("f0")+ "/" + WholeHP.ToString("f0");
            ManaText.GetComponent<Text>().text = "MN " + Mana.ToString("f0")+ "/" + WholeMana.ToString("f0");
            XPText.GetComponent<Text>().text = "XP " + XP.ToString("f0")+ "/" + WholeXP.ToString("f0");
            LevelText.GetComponent<Text>().text = "" + Level.ToString("f0")+"  Level";
            superAttackTimerText.GetComponent<Text>().text = "" + superAttackTimer.ToString("f0");

            if (HP <= 0)
            {
                GameManager.instance.LeaveRoom();
            }
            else if (HP < WholeHP)
            {
                HP += 1*0.5f*Time.deltaTime;
                HP = Mathf.Clamp(HP, 0, WholeHP);
            }
            else
            {
                HP = Mathf.Clamp(HP, 0, WholeHP);
            }

            if (Mana < WholeMana)
            {
                Mana += 1 * 0.25f * Time.deltaTime;
                Mana = Mathf.Clamp(Mana, 0, WholeMana);
            }
            else
            {
                HP = Mathf.Clamp(HP, 0, WholeHP);
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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                XP += 100;
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
