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
    [SerializeField] PrePlayerTurn prePlayerTurn;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] Image slotImage;
    [SerializeField] InGameInventorySlot actualInGameSlot;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip drinkPotionClip;

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
            RefreshTextAndButton();
            return true;
        }
        return false;
    }

    public void UseItem() //faire en sorte que le system permette pas plus d'un usage par tour en disablant les buttons et affichant un avertissement
    {
        if (!inventorySystem.ConsumableUsedThisTurn)
        {
            switch (consumableTypeToHold)
            {                
                case ConsumableType.HealingPotion:
                    if (!player.IsPlayerFullHealth())
                    {
                        player.HealDamage(itemHeld.HealingAmount);
                        numberOfItems--;
                    }
                    else
                    {
                        inventorySystem.SetMessagePopup("You are already full health");
                        return;
                    }                   
                    break;
                case ConsumableType.SpecialEffectPotion:
                    prePlayerTurn.AddSpecialEffects(itemHeld.SpecialEffect, itemHeld.PercentageAffectedBy, itemHeld.TurnsDuration);
                    numberOfItems--;
                    break;
            }
            audioSource.PlayOneShot(drinkPotionClip);
            inventorySystem.ConsumableUsedThisTurn = true;
            RefreshTextAndButton();
        }
        else
        {
            inventorySystem.SetMessagePopup("You already used an item this turn");
        }
    }
        

    public void RefreshTextAndButton()
    {
        useButton.interactable = (numberOfItems > 0) ? true : false;
        stackAmountText.text = (numberOfItems <= 0) ? "" : numberOfItems.ToString();
    }

    public void RemoveItemFromPreInstanceInventory()
    {
        if (itemHeld)
        {
            inventorySystem.AddItemToMemory(itemHeld);
            if (numberOfItems > 1)
            {
                numberOfItems--;
            }
            else if (numberOfItems == 1)
            {
                numberOfItems--;
                itemHeld = null;
                itemHeldId = 0; //resets the slot so it can welcome a new item
                slotImage.sprite = emptySlotSprite;
            }
            RefreshTextAndButton();
            inventorySystem.FromMemoryToBag(true);
        }
    }

    public void RemoveFromActualInGameInventory() //TODO Huge copy of code to mirror changes from pre instance inventory to ingame inventory. Refactoring needed.
    {
        if (itemHeld)
        {           
            if (numberOfItems > 1)
            {
                numberOfItems--;
            }
            else if (numberOfItems == 1)
            {
                numberOfItems--;
                itemHeld = null;
                itemHeldId = 0; //resets the slot so it can welcome a new item
                slotImage.sprite = emptySlotSprite;
            }
            RefreshTextAndButton();
        }
    }

    public void ReturnItemsToInventory()
    {
        int max = numberOfItems;
        for (int i = 0; i < max; i++)
        {
            RemoveItemFromPreInstanceInventory();
            actualInGameSlot.RemoveFromActualInGameInventory();
        }
    }
    public void EmptyPreInstanceAndInGameInventory()
    {
        numberOfItems = 0;
        itemHeld = null;
        itemHeldId = 0; //resets the slot so it can welcome a new item
        slotImage.sprite = emptySlotSprite;
        RefreshTextAndButton();
    }
}
