using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int slotNumber;

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[slotNumber] = false;
        }
    }

    public void ClickLeft()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void SetInventory(Inventory i)
    {
        inventory = i;
    }
}
