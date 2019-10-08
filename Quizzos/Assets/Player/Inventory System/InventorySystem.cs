using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySystem : MonoBehaviour
{
    [SerializeField] InGameInventorySlot[] inGameInventorySlots;
    [SerializeField] MessagePopup messagePopup;
    [SerializeField] GameObject itemPreview;
    [SerializeField] Image itemImagePreview;
    [SerializeField] TextMeshProUGUI previewItemName;
    [SerializeField] TextMeshProUGUI previewItemCategory;
    [SerializeField] TextMeshProUGUI previewItemSubCategory;
    [SerializeField] TextMeshProUGUI previewItemDescription;
    bool previewItemIsOpen = false;
    [SerializeField] List<InventorySlot> inventorySlotsList = new List<InventorySlot>();
    bool consumableUsedThisTurn = false;

    public bool ConsumableUsedThisTurn { get => consumableUsedThisTurn; set => consumableUsedThisTurn = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (previewItemIsOpen && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CloseItemPreview();
            }
        }
#endif

#if UNITY_EDITOR
        if (previewItemIsOpen && Input.GetMouseButtonDown(0))
        {
            CloseItemPreview();
        }
#endif
    }

    public void AddItemToBag(Items item)
    {
        if(item.StackSize > 1)//check if there's a slot holding this type of item (if item is stackable)
        {
            for (int i = 0; i < inventorySlotsList.Count; i++) 
            {
                InventorySlot currentSlot = inventorySlotsList[i];
                if (currentSlot.ItemHeld && currentSlot.ItemHeld == item && currentSlot.AmountOfItems < item.StackSize)
                {                    
                    currentSlot.AmountOfItems++;
                    RefreshInventorySlots();
                    return;
                }
            }
        }       
        for (int i = 0; i < inventorySlotsList.Count; i++) //find a empty slot
        {
            InventorySlot currentSlot = inventorySlotsList[i];
            if (!currentSlot.ItemHeld)
            {
                currentSlot.ItemHeld = item;
                if(currentSlot.ItemHeld.StackSize > 1)
                {
                    currentSlot.AmountOfItems++;
                }
                break;
            }
        }
        RefreshInventorySlots();
    }

    public bool AddConsumableToInGameInventory(Consumables item)
    {
        switch (item.ConsumableType)
        {
            case ConsumableType.HealingPotion:
                if (inGameInventorySlots[0].ReceiveItem(item))
                {
                    return true;
                }               
                break;
        }
        return false;
    }

    public void SetMessagePopup(string messageString)
    {
        messagePopup.ReceiveStringAndShowPopup(messageString);
    }

    public void ItemPreview(GameObject itemHolderToPreview)
    {
        Debug.Log("a");
        Items itemToPreview = (itemHolderToPreview.GetComponent<InventorySlot>())? itemHolderToPreview.GetComponent<InventorySlot>().ItemHeld: itemHolderToPreview.GetComponent<ShopSlots>().ItemToSell;
        itemPreview.SetActive(true);
        previewItemIsOpen = true;
        itemImagePreview.sprite = itemToPreview.ItemImage;
        previewItemName.text = itemToPreview.NameOfItem;
        switch (itemToPreview.ItemCategory)
        {
            case ItemCategory.Consumable:
                previewItemCategory.text = "Consumable";
                previewItemSubCategory.text = ((Consumables)itemToPreview).ConsumableType.ToString();
                previewItemDescription.text = ((Consumables)itemToPreview).GetDescription();
                break;
        }
    }

    public void CloseItemPreview()
    {
        itemPreview.SetActive(false);
        previewItemIsOpen = false;
    }

    public void RefreshInventorySlots()
    {
        for (int i = 0; i < inventorySlotsList.Count; i++)
        {
            inventorySlotsList[i].RefreshInventorySlots();
        }
    }
   
}
