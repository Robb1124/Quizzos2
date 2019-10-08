using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] Items itemHeld;
    [SerializeField] int amountOfItems = 0;
    [SerializeField] Text amountOfItemsText;
    public Items ItemHeld { get => itemHeld; set => itemHeld = value; }
    public int AmountOfItems { get => amountOfItems; set => amountOfItems = value; }

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveItem()
    {

    }

    public void RefreshInventorySlots()
    {
        if (ItemHeld)
        {
            gameObject.SetActive(true);
            slotImage = (slotImage) ? slotImage : GetComponent<Image>();
            amountOfItemsText = (amountOfItemsText) ? amountOfItemsText : GetComponentInChildren<Text>();
            amountOfItemsText.text = (amountOfItems > 0) ? amountOfItems.ToString() : "";

            slotImage.sprite = ItemHeld.ItemImage;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
}
