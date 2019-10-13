using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryMemory
{
    public Items item; //cannot serialize this so we're using ItemsSaveStruct struct to serialize ID + quantity. Retrieves from a list of items when loading save files.
    public int quantity;

    public InventoryMemory(Items item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
