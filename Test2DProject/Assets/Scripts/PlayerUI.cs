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
        public GameObject playerCanvas;
        public Text HPText;
        public Text ManaText;
        public Text XPText;
        public Text LevelText;
        public Text superAttackTimerText;
        public Text enemyDieCounter;
        public float HP=100;
        public float Mana = 50;
        public float XP = 0;
        public float Level = 1;
        public float WholeHP=100;
        public float WholeMana=50;
        public float superAttackTimer;
        public float WholeXP;

        public Quest quest;

        public static int killCount;

        PlayerMovement playerMove;

        public ParticleSystem LevelUp;
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                playerCanvas.SetActive(false);
            }
        }

        private void Start()
        {
            playerMove = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            HPText.text = "HP " + HP.ToString("f0")+ "/" + WholeHP.ToString("f0");
            ManaText.text = "MN " + Mana.ToString("f0")+ "/" + WholeMana.ToString("f0");
            XPText.text = "XP " + XP.ToString("f0")+ "/" + WholeXP.ToString("f0");
            LevelText.text = "" + Level.ToString("f0")+"  Level";
            superAttackTimerText.text = "" + superAttackTimer.ToString("f0");
            enemyDieCounter.text = "" + killCount.ToString();

            if (HP <= 0)
            {
                StartCoroutine(PlayerDeath());
            }
            else if (HP < WholeHP)
            {
                HP += 1*0.75f*Time.deltaTime;
                HP = Mathf.Clamp(HP, 0, WholeHP);
            }
            else
            {
                HP = Mathf.Clamp(HP, 0, WholeHP);
            }

            if (Mana < WholeMana)
            {
                Mana += 1 * 0.75f * Time.deltaTime;
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

        private void ResetStats()
        {
            HP = WholeHP = 100;
            Mana = WholeMana = 50;
            XP = 0;
            WholeXP = 100;
            Level = 1;
            GameUI.skillpoints = 0;
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
            if (HP > 0)
                HP -= damage;
        }

        IEnumerator PlayerDeath()
        {
            playerMove.Die();
            yield return new WaitForSeconds(1f);
            transform.position = new Vector3(-7f, 0f, 0f);
            ResetStats();
            playerMove.ResetDeadFlag();
        }
    }
}
