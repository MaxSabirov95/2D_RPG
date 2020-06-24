using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;
//--Distance(Float)=sqrt((Y2-Y1)^2 + (X2-X1)^2)
//--X2Y2 origin.position      X1Y1 destenetion.position

namespace Max_Almog.MyCompany.MyGame
{
    public class ItemsPickUp : MonoBehaviourPun
    {
        public float searchRadius = 2.5f;
        private Rigidbody2D rb;
        private Collider2D col;
        public float itemSpeed;
        private PlayerMovement Player;
        bool canBePicked=false;

        private Inventory inventory;

        public GameObject Object;
        public LayerMask playerLayer;

        private int coinRandomNumber;
        private static bool playerInventoryFull;

        void Start()
        {
            Player = null;
            Physics2D.IgnoreLayerCollision(10, 9);
            rb = GetComponent<Rigidbody2D>();
            col = gameObject.GetComponent<Collider2D>();
            
        }

        void Update()
        {
            if (Player == null)
            {
                Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, searchRadius, playerLayer);

                if (players.Length > 0)
                {
                    //assign player variable
                    if (players.Length == 2)
                    {
                        Player = players[0].GetComponent<PlayerMovement>();
                        inventory = Player.GetComponent<Inventory>();
                        Debug.Log(inventory.name);
                    }
                }
            }
            else
            {
                if (Vector2.Distance(Player.transform.position, transform.position) < searchRadius && !playerInventoryFull)
                {
                    col.isTrigger = true;
                    Vector2 deriction = Player.transform.position - transform.position;
                    rb.MovePosition((Vector2)transform.position + (deriction * itemSpeed * Time.deltaTime));
                    photonView.RPC("Coin", RpcTarget.AllBuffered);                    photonView.RPC("HpPotion", RpcTarget.AllBuffered);                    photonView.RPC("BigHpPotion", RpcTarget.AllBuffered);                    photonView.RPC("ManaPotion", RpcTarget.AllBuffered);
                }
                else if (Vector2.Distance(Player.transform.position, transform.position) > searchRadius)
                {
                    Player = null;
                }
            }
        }

        [PunRPC]
        void Coin()
        {
            if (gameObject.CompareTag("Coin"))
            {
                
               // if (Vector2.Distance(Player.transform.position, transform.position) < 0.8f)
               // {
                    coinRandomNumber = Random.Range(Enemy.MinCoins, Enemy.MaxCoins);
                    GameItems.money += coinRandomNumber;
                    photonView.RPC("DestroyItem", RpcTarget.AllBuffered);
                //}
            }
        }

        [PunRPC]
        public void DestroyItem()
        {
            int ID = gameObject.GetComponent<PhotonView>().ViewID;
            Destroy(PhotonView.Find(ID).gameObject);
        }

        [PunRPC]
        void HpPotion()
        {
            if (gameObject.CompareTag("HpPotion"))
            {
                HpAndMana();
            }
        }

        [PunRPC]
        void BigHpPotion()
        {
            if (gameObject.CompareTag("BigHpPotion"))
            {
                HpAndMana();
            }
        }

        [PunRPC]
        void ManaPotion()
        {
            if (gameObject.CompareTag("ManaPotion"))
            {
                HpAndMana();
            }
        }

        [PunRPC]
        void HpAndMana()
        {
            if (Player)
            {
                if (Vector2.Distance(Player.transform.position, transform.position) < 0.8f)
                {
                    for (int i = 0; i < inventory.slots.Length; i++)
                    {
                        if (!inventory.isFull[i])
                        {
                            inventory.isFull[i] = true;
                            Instantiate(Object, inventory.slots[i].transform, false);

                            PhotonView itemView = gameObject.GetComponent<PhotonView>();
                            photonView.RPC("DestroyItem", RpcTarget.AllBuffered);
                            return;
                        }
                    }
                    playerInventoryFull = true;
                }
                else
                {
                    col.isTrigger = false;
                }
            }
        }
    }
}
