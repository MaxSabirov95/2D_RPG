﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public Inventory inventory;
    public int slotNumber;
    public bool isFull;

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isSlotTaken[slotNumber] = false;
        }
    }

    public void FlagNotFull()
    {
        isFull = false;
        inventory.inventoryFull = false;
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
