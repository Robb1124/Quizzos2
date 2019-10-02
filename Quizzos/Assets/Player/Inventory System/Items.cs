using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : ScriptableObject
{
    [SerializeField] int itemId;
    [SerializeField] string nameOfItem;
    [SerializeField] Sprite itemImage;

    public int ItemId { get => itemId; set => itemId = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
