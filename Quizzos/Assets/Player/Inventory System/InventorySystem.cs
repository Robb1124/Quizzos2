using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BagFilterType { None}

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
    [SerializeField] List<InventoryMemory> inventoryMemoryList = new List<InventoryMemory>();
    bool consumableUsedThisTurn = false;

    public bool ConsumableUsedThisTurn { get => consumableUsedThisTurn; set => consumableUsedThisTurn = value; }
    public List<InventoryMemory> InventoryMemoryList { get => inventoryMemoryList; set => inventoryMemoryList = value; }

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

    public void AddItemToMemory(Items itemToAdd)
    {
        for (int i = 0; i < InventoryMemoryList.Count; i++) //check if theres a memory slot that holds this item so we can just stack them (actual inventory will manage stack size)
        {
            if(InventoryMemoryList[i].item == itemToAdd)
            {
                InventoryMemoryList[i].quantity++;
                return;
            }
        }
        InventoryMemoryList.Add(new InventoryMemory(itemToAdd, 1));        
    }

    public void RemoveItemFromMemory(Items itemToRemove)
    {
        for (int i = 0; i < InventoryMemoryList.Count; i++) //check which memory slot is holding the item to remove
        {
            if (InventoryMemoryList[i].item == itemToRemove)
            {
                if(InventoryMemoryList[i].quantity > 1) //reduces the stack or remove completely from memory if there no more item of this type.
                {
                    InventoryMemoryList[i].quantity--;
                }
                else
                {
                    InventoryMemoryList.Remove(InventoryMemoryList[i]);
                }
                return;
            }
        }        
    }

    public void FromMemoryToBag() //todo add bag filters
    {
        int slotCounter = 0;
        for (int i = 0; i < inventorySlotsList.Count; i++)
        {
            inventorySlotsList[i].ItemHeld = null;
            inventorySlotsList[i].AmountOfItems = 0;           
        }

        for (int i = 0; i < InventoryMemoryList.Count; i++)
        {
            InventorySlot currentSlot = inventorySlotsList[slotCounter];
            for (int j = 0; j < InventoryMemoryList[i].quantity; j++)
            {
                currentSlot.ItemHeld = InventoryMemoryList[i].item;
                if (currentSlot.AmountOfItems < InventoryMemoryList[i].item.StackSize)
                {
                    currentSlot.AmountOfItems++;                                             
                }
                else
                {
                    slotCounter++;
                    currentSlot = inventorySlotsList[slotCounter];
                    currentSlot.ItemHeld = InventoryMemoryList[i].item;
                    currentSlot.AmountOfItems++;
                }
            }
            slotCounter++;
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
