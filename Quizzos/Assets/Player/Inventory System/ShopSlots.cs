using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopSlots : MonoBehaviour
{
    [SerializeField] Items itemToSell;
    [SerializeField] bool unlimited;
    [SerializeField] int amountOfItems;
    [SerializeField] GemsAndGoldSystem goldSystem;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] TextMeshProUGUI priceText;  
    

    int price;

    public Items ItemToSell { get => itemToSell; set => itemToSell = value; }

    // Start is called before the first frame update
    void Start()
    {
        price = ItemToSell.ShopPrice;
        priceText.text = price.ToString();
        GetComponent<Image>().sprite = ItemToSell.ItemImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {        
        if(goldSystem.GetGold() >= price)
        {
            goldSystem.RemoveGold(price);
            inventorySystem.AddItemToMemory(itemToSell);
        }
        else
        {
            inventorySystem.SetMessagePopup("Not enough gold");
        }
    }
}
