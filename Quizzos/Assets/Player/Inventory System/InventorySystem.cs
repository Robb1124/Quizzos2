using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] InGameInventorySlot[] inGameInventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddConsumableToInGameInventory(Consumables item)
    {
        switch (item.ConsumableType)
        {
            case ConsumableType.HealingPotion:
                inGameInventorySlots[0].ReceiveItem(item);
                break;
        }
    }
}
