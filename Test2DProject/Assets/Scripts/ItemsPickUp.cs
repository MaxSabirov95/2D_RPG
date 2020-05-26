using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//--Distance(Float)=sqrt((Y2-Y1)^2 + (X2-X1)^2)
//--X2Y2 origin.position      X1Y1 destenetion.position
public class ItemsPickUp : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    public float itemSpeed;
    private GameObject Player;

    private Inventory inventory;

    public GameObject Object;

    private int coinRandomNumber;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(10, 9);
        Player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();

        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
    }

    private void Update()
    {
        Coin();
        HpPotion();
        BigHpPotion();
        ManaPotion();
    }

    void Coin()
    {
        if (gameObject.CompareTag("Coin"))
        {
            col.isTrigger = true;
            Vector2 deriction = Player.transform.position - transform.position;
            rb.MovePosition((Vector2)transform.position + (deriction * itemSpeed * Time.deltaTime));
            if (Vector2.Distance(Player.transform.position, transform.position) < 0.8f)
            {
                coinRandomNumber=Random.Range(Enemy.MinCoins, Enemy.MaxCoins);
                GameItems.money += coinRandomNumber;
                Destroy(gameObject);
            }
        }
    }

    void HpPotion()
    {
        if (gameObject.CompareTag("HpPotion"))
        {
            HpAndMana();
        }
    }

    void BigHpPotion()
    {
        if (gameObject.CompareTag("BigHpPotion"))
        {
            HpAndMana();
        }
    }

    void ManaPotion()
    {
        if (gameObject.CompareTag("ManaPotion"))
        {
            HpAndMana();
        }
    }

    ///////////////////
    void HpAndMana()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (!inventory.isFull[i])
            {
                if (Vector2.Distance(Player.transform.position, transform.position) < 2.5f)
                {
                    col.isTrigger = true;
                    Vector2 deriction = Player.transform.position - transform.position;
                    rb.MovePosition((Vector2)transform.position + (deriction * itemSpeed * Time.deltaTime));
                    if (Vector2.Distance(Player.transform.position, transform.position) < 0.8f)
                    {
                        inventory.isFull[i] = true;
                        Instantiate(Object, inventory.slots[i].transform, false);

                        Destroy(gameObject);
                        break;
                    }
                    else
                    {
                        col.isTrigger = false;
                    }
                }
            }
        }
    }
    ///////////////////
}
