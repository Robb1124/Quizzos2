using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject[] abilitySlots;
    [SerializeField] Sprite[] warriorAbilitySprites;
    MonsterManager monsterManager;



    // Start is called before the first frame update
    void Start()
    {
        monsterManager = FindObjectOfType<MonsterManager>();
        monsterManager.onTurnChangeForPlayer += OnTurnChangeForPlayer;
        player = FindObjectOfType<Player>();
        if (player.gameObject.GetComponent<Warrior>())
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                abilitySlots[i].GetComponent<Image>().sprite = warriorAbilitySprites[i];
            }
            
        }
    }

    private void OnTurnChangeForPlayer()
    {
        //Minus the cooldown by 1 for all skills that has 1 or more of cooldowns active.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
