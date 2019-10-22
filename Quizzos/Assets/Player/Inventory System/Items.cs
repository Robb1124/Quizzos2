using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory { Consumable }
public enum Currency { Gold, Gems }
public class Items : ScriptableObject
{
    [SerializeField] int itemId;
    [SerializeField] string nameOfItem;
    [SerializeField] Sprite itemImage;
    [SerializeField] int stackSize = 1;
    [SerializeField] Currency currency;
    [SerializeField] int shopPrice;
    [SerializeField] ItemCategory itemCategory;
    [TextArea(3, 4)]
    [SerializeField] protected string itemDescription;
    
    public int ItemId { get => itemId; set => itemId = value; }
    public Sprite ItemImage { get => itemImage; set => itemImage = value; }
    public int ShopPrice { get => shopPrice; set => shopPrice = value; }
    public ItemCategory ItemCategory { get => itemCategory; set => itemCategory = value; }
    public int StackSize { get => stackSize; set => stackSize = value; }
    public string NameOfItem { get => nameOfItem; set => nameOfItem = value; }
    public Currency Currency { get => currency; set => currency = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
