using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryMemory
{
    public Items item;
    public int quantity;

    public InventoryMemory(Items item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
