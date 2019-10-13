using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsSaveStruct
{
    int itemId;
    int quantity;

    public int ItemId { get => itemId; set => itemId = value; }
    public int Quantity { get => quantity; set => quantity = value; }

    public ItemsSaveStruct(int itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }
}
