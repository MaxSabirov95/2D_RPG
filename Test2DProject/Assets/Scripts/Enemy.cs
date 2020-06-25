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
    public class Enemy : MonoBehaviourPun, IPunObservable
    {
        public enum enemytypes { FireSlime, Enemy1, Enemy2 };
        public enemytypes TypesOfEnemies;

        public Rigidbody2D rb;
        public TMP_Text HPText;

        public int HP;

        public int GiveXP;
        public int DamageToPlayer;

        public PlayerUI goals;
        public PlayerUI damagingPlayer;

        public int MinGiveCoinsAfterDeath;
        public int MaxGiveCoinsAfterDeath;
        public static int MinCoins;
        public static int MaxCoins;

        public bool isDead;

        public GameObject[] itemstodrop;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(HP);
                stream.SendNext(isDead);
            }
            else
            {
                HP = (int)stream.ReceiveNext();
                HPText.GetComponent<TMP_Text>().text = "" + HP.ToString("f0");
                isDead = (bool)stream.ReceiveNext();
                //if (isDead)
                //{
                //    photonView.RPC("OnDeath", RpcTarget.AllBuffered);
                //}
                //isDead = (bool)stream.ReceiveNext();
            }
        }

        [PunRPC]
        public void StartProperties()
        {
            MinCoins = MinGiveCoinsAfterDeath;
            MaxCoins = MaxGiveCoinsAfterDeath;
            HPText.GetComponent<TMP_Text>().text = "" + HP.ToString("f0");

            Physics2D.IgnoreLayerCollision(11, 11);
            Physics2D.IgnoreLayerCollision(11, 10);

            rb = GetComponent<Rigidbody2D>();
            
        }

        private void Update()
        {
            if (isDead)
            {
                photonView.RPC("OnDeath", RpcTarget.MasterClient);
            }
        }

        public void TakeDamage(int EnemyDamge, PlayerUI damagingPlayer)
        {
            this.damagingPlayer = damagingPlayer;
            photonView.RPC("EnemyTakeDamage", RpcTarget.AllBufferedViaServer, new object[] { EnemyDamge});
        }

        [PunRPC]
        public void EnemyTakeDamage(int EnemyDamage)
        {
            HP -= EnemyDamage;
            HPText.GetComponent<TMP_Text>().text = "" + HP.ToString("f0");
            
            if (HP <= 0)
            {
                isDead = true;
                photonView.RPC("OnDeath", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        public void OnDeath()
        {
            switch (TypesOfEnemies)
            {
                case enemytypes.FireSlime:
                    if (PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC("DropItems", RpcTarget.MasterClient);
                    }
                    //goals.Killquest();
                    //damagingPlayer.XP += GiveXP;
                    break;
                default:
                    break;
            }
            photonView.RPC("Die", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void Die()
        {
            BlackBoard.spawnerInstance.EnemySpawn();
            int ID = gameObject.GetComponent<PhotonView>().ViewID;
            Destroy(PhotonView.Find(ID).gameObject);
        }

        [PunRPC]
        public void DropItems()
        {
            
            PhotonNetwork.Instantiate("Coin", transform.position, Quaternion.identity, 0);

            photonView.RPC("RandomHpOrMana", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void RandomHpOrMana()
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
                        PhotonNetwork.Instantiate("BigHPPotion", transform.position, Quaternion.identity, 0);

                    }
                    else
                    {
                        PhotonNetwork.Instantiate("HPPotion", transform.position, Quaternion.identity, 0);

                    }
                }
                else if (hpOrMana == 2)
                {
                    PhotonNetwork.Instantiate("ManaPotion", transform.position, Quaternion.identity, 0);

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
