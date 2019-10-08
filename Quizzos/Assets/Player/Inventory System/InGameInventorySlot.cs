using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameInventorySlot : MonoBehaviour
{
    [SerializeField] int StackSize;
    [SerializeField] ConsumableType consumableTypeToHold;
    [SerializeField] Player player;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] Text stackAmountText;
    [SerializeField] Button useButton;
    [SerializeField] TurnManager turnManager;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] Image slotImage;

    int numberOfItems = 0;
    int itemHeldId = 0;
    Consumables itemHeld;

    // Start is called before the first frame update
    void Start()
    {
        turnManager.onTurnChangeForPlayer += OnTurnChangeForPlayer;
        if (itemHeldId == 0)
        {
            slotImage.sprite = emptySlotSprite;
        }
        else
        {
            slotImage.sprite = itemHeld.ItemImage;
        }
        RefreshTextAndButton();
    }

    private void OnTurnChangeForPlayer()
    {
        inventorySystem.ConsumableUsedThisTurn = false;
        RefreshTextAndButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ReceiveItem(Consumables item)
    {
        if(numberOfItems < StackSize && (itemHeldId == 0 || item.ItemId == itemHeldId))
        {
            if(itemHeldId == 0)
            {                
                slotImage.sprite = item.ItemImage;
            }
            numberOfItems++;

            itemHeld = item;
            itemHeldId = itemHeld.ItemId;
            return true;
        }
        return false;
    }

    public void UseItem() //faire en sorte que le system permette pas plus d'un usage par tour en disablant les buttons et affichant un avertissement
    {
        switch (consumableTypeToHold)
        {
            case ConsumableType.HealingPotion:
                player.HealDamage(itemHeld.HealingAmount);
                numberOfItems--;
                break;
        }
        inventorySystem.ConsumableUsedThisTurn = true;
        RefreshTextAndButton();
    }

    public void RefreshTextAndButton()
    {
        if (numberOfItems <= 0 || inventorySystem.ConsumableUsedThisTurn)
        {
            useButton.interactable = false;
        }
        else if (numberOfItems > 0)
        {            
            useButton.interactable = true;
        }
        stackAmountText.text = numberOfItems.ToString();

    }
}
