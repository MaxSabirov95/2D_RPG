using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory:MonoBehaviour
{
    public bool inventoryFull;
    public bool[] isSlotTaken = new bool[16];
    public Slot[] slots;

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetInventory(this);
        }
    }
}
