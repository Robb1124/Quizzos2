using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInventorySlot : MonoBehaviour
{
    [SerializeField] int StackSize;
    [SerializeField] ConsumableType consumableTypeToHold;
    [SerializeField] Player player;
    int numberOfItems;
    int itemHeldId = 0;
    Consumables itemHeld;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveItem(Consumables item)
    {
        if(numberOfItems < StackSize && (itemHeldId == 0 || item.ItemId == itemHeldId))
        {
            numberOfItems++;
            itemHeld = item;
            itemHeldId = itemHeld.ItemId;
        }
    }

    public void UseItem() //faire en sorte que le system permette pas plus d'un usage par tour en disablant les buttons et affichant un avertissement
    {
        switch (consumableTypeToHold)
        {
            case ConsumableType.HealingPotion:
                //create player.heal (amount)..
                break;
        }
    }

}
