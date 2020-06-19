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
        private Rigidbody2D rb;
        private Collider2D col;
        public float itemSpeed;
        private PlayerMovement Player;
        bool canBePicked=false;

        private Inventory inventory;

        public GameObject Object;

        private int coinRandomNumber;
        private static bool playerInventoryFull;

        void Start()
        {
            Physics2D.IgnoreLayerCollision(10, 9);
            Player[] playerList = PhotonNetwork.PlayerList;
            Player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();
            col = gameObject.GetComponent<Collider2D>();

            inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
        }

        void Update()
        {
            if (Vector2.Distance(Player.transform.position, transform.position) < 2.5f && !playerInventoryFull)
            {
                col.isTrigger = true;
                Vector2 deriction = Player.transform.position - transform.position;
                rb.MovePosition((Vector2)transform.position + (deriction * itemSpeed * Time.deltaTime));
                photonView.RPC("Coin", RpcTarget.MasterClient);                photonView.RPC("HpPotion", RpcTarget.MasterClient);                photonView.RPC("BigHpPotion", RpcTarget.MasterClient);                photonView.RPC("ManaPotion", RpcTarget.MasterClient);
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
                    photonView.RPC("DestroyItem", RpcTarget.MasterClient);
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
            if (Vector2.Distance(Player.transform.position, transform.position) < 0.8f)
            {
                for (int i = 0; i < inventory.slots.Length; i++)
                {
                    if (!inventory.isFull[i])
                    {
                        inventory.isFull[i] = true;
                        Instantiate(Object, inventory.slots[i].transform, false);

                        PhotonView itemView = gameObject.GetComponent<PhotonView>();
                        photonView.RPC("DestroyItem", RpcTarget.MasterClient);
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
        ///////////////////

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    //PlayerMovement potentialPlayer = collision.GetComponent<PlayerMovement>();
        //    //if (Player == null && potentialPlayer)
        //    //{
        //    //    Player = potentialPlayer;
        //    //}

        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        //if (Vector2.Distance(Player.transform.position, transform.position) < 2.5f && !playerInventoryFull)
        //       // {
        //            col.isTrigger = true;
        //            Vector2 deriction = Player.transform.position - transform.position;
        //            rb.MovePosition((Vector2)transform.position + (deriction * itemSpeed * Time.deltaTime));
        //            photonView.RPC("Coin", RpcTarget.AllBuffered);        //            photonView.RPC("HpPotion", RpcTarget.AllBuffered);        //            photonView.RPC("BigHpPotion", RpcTarget.AllBuffered);        //            photonView.RPC("ManaPotion", RpcTarget.AllBuffered);
        //        //}
        //    }
        //}
    }
}
