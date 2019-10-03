using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumableType { HealingPotion, SpecialEffectPotion, TriviaPotion}
[CreateAssetMenu(menuName = ("Items/Consumables"))]
public class Consumables : Items
{
    [SerializeField] ConsumableType consumableType;
    [SerializeField] int healingAmount;

    public ConsumableType ConsumableType { get => consumableType; set => consumableType = value; }
    public int HealingAmount { get => healingAmount; set => healingAmount = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
