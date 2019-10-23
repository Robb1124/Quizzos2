using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumableType { HealingPotion, SpecialEffectPotion, TriviaPotion}
public enum StatsAffected { BaseDamage, HealthPoints}
[CreateAssetMenu(menuName = ("Items/Consumables"))]
public class Consumables : Items
{
    [SerializeField] ConsumableType consumableType;


    //if healing potion
    [SerializeField] int healingAmount;

    //if special effects potion
    [SerializeField] SpecialEffectsType specialEffect;
    [SerializeField] float percentageAffectedBy;
    [SerializeField] int turnsDuration;
    public ConsumableType ConsumableType { get => consumableType; set => consumableType = value; }
    public int HealingAmount { get => healingAmount; set => healingAmount = value; }
    public float PercentageAffectedBy { get => percentageAffectedBy; set => percentageAffectedBy = value; }
    public SpecialEffectsType SpecialEffect { get => specialEffect; set => specialEffect = value; }
    public int TurnsDuration { get => turnsDuration; set => turnsDuration = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetDescription()
    {
        if(consumableType == ConsumableType.HealingPotion)
        {
            return string.Format(itemDescription, healingAmount);
        }
        else if (consumableType == ConsumableType.SpecialEffectPotion)
        {
            return string.Format(itemDescription, (percentageAffectedBy * 100), TurnsDuration);
        }
        else
        {
            return string.Empty;
        }
    }
}
