using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject[] abilitySlots;
    [SerializeField] Sprite[] warriorAbilitySprites;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player.gameObject.GetComponent<Warrior>())
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                abilitySlots[i].GetComponent<Image>().sprite = warriorAbilitySprites[i];
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
