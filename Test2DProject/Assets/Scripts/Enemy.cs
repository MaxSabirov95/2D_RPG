using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

//  Items:
//  0-Coins
//  1-Big HP Potion
//  2-HP Potion
//  3-Mana Potion

namespace Max_Almog.MyCompany.MyGame
{
    public class Enemy : MonoBehaviourPun
    {
        public enum enemytypes { FireSlime, Enemy1, Enemy2 };
        public enemytypes TypesOfEnemies;

        public Rigidbody2D rb;
        public TMP_Text HPText;
        public int HP;
        public int GiveXP;
        public int DamageToPlayer;

        public PlayerUI goals;

        public int MinGiveCoinsAfterDeath;
        public int MaxGiveCoinsAfterDeath;
        public static int MinCoins;
        public static int MaxCoins;

        public GameObject[] itemstodrop;

        public void StartProperties()
        {
            MinCoins = MinGiveCoinsAfterDeath;
            MaxCoins = MaxGiveCoinsAfterDeath;
            HPText.GetComponent<TMP_Text>().text = "" + HP.ToString("f0");

            Physics2D.IgnoreLayerCollision(11, 11);
            Physics2D.IgnoreLayerCollision(11, 10);

            rb = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(int EnemyDamage, PlayerUI damagingPlayer)
        {
            photonView.RPC("EnemyTakeDamage", RpcTarget.All, new object[] { EnemyDamage, damagingPlayer });
        }

        [PunRPC]
        void EnemyTakeDamage(int EnemyDamage, PlayerUI damagingPlayer)
        {
            HP -= EnemyDamage;
            HPText.GetComponent<TMP_Text>().text = "" + HP.ToString("f0");
            if (HP <= 0)
            {
                OnDeath(damagingPlayer);
            }
        }
        
        public void OnDeath(PlayerUI killingPlayer)
        {
            switch (TypesOfEnemies)
            {
                case enemytypes.FireSlime:
                    DropItems();
                    //goals.Killquest();
                    killingPlayer.XP += GiveXP;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

        void DropItems()
        {
            Instantiate(itemstodrop[0], transform.position, Quaternion.identity);
            RandomHpOrMana();
        }

        void RandomHpOrMana()
        {
            int ifGetHpOrMana = Random.Range(1, 101);
            if (ifGetHpOrMana > 25)
            {
                int hpOrMana = Random.Range(1, 3);
                if (hpOrMana == 1)
                {
                    int whichHp = Random.Range(1, 101);
                    if (whichHp > 75)
                    {
                        Instantiate(itemstodrop[1], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(itemstodrop[2], transform.position, Quaternion.identity);
                    }
                }
                else if (hpOrMana == 2)
                {
                    Instantiate(itemstodrop[3], transform.position, Quaternion.identity);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D _player)
        {
            PlayerUI player = _player.gameObject.GetComponent<PlayerUI>();
            if (player)
            {
                player.TakeDamage(DamageToPlayer);
            }
        }
    }
}
