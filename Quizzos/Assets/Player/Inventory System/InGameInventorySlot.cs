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

    [SerializeField] Image slotImage;
    int numberOfItems = 0;
    int itemHeldId = 0;
    Consumables itemHeld;
    // Start is called before the first frame update
    void Start()
    {       
        if(itemHeldId == 0)
        {
            slotImage.sprite = emptySlotSprite;
        }
        else
        {
            slotImage.sprite = itemHeld.ItemImage;
        }
        RefreshTextAndButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveItem(Consumables item)
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
        }
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
        RefreshTextAndButton();
    }

    public void RefreshTextAndButton()
    {
        if(numberOfItems > 0)
        {
            stackAmountText.text = numberOfItems.ToString();
            useButton.interactable = true;
        }
        else
        {
            useButton.interactable = false;
            stackAmountText.text = "0";
        }
    }

}
