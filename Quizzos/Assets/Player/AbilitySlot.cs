using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySlot : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject[] abilitySlots;
    [SerializeField] GameObject passiveSlot;
    [SerializeField] Sprite[] warriorAbilitySprites;
    [SerializeField] Sprite warriorPassiveSprite;
    [SerializeField] TextMeshProUGUI passiveText;
    [SerializeField] TurnManager turnManager;
    [SerializeField] TextMeshProUGUI[] specialAbilitiesCooldownTexts;
    [SerializeField] StageManager stageManager;

    [SerializeField] QuizManager quizManager;
    int specialAbility1CooldownTurns = 1;
    int specialAbility1RemainingCdTurns = 0;
    int specialAbility2CooldownTurns = 1;
    int specialAbility2RemainingCdTurns = 0;

    public bool SpecialAbility1IsReady { get; set; } = true;
    public bool SpecialAbility2IsReady { get; set; } = true;


    // Start is called before the first frame update
    void Start()
    {
        stageManager.onStageLoad += OnStageLoad;
        quizManager.onWrongAnswers += OnWrongAnswers;
        turnManager.onTurnChangeForPlayer += OnTurnChangeForPlayer;
        player = FindObjectOfType<Player>();
        if (player.gameObject.GetComponent<Warrior>())
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                abilitySlots[i].GetComponent<Image>().sprite = warriorAbilitySprites[i];
            }
            passiveSlot.GetComponent<Image>().sprite = warriorPassiveSprite;

            specialAbility1CooldownTurns = player.GetComponent<Warrior>().ChargeAttackCooldown;
            specialAbility2CooldownTurns = player.GetComponent<Warrior>().ShieldUpCooldown;
            
        }
    }

    private void OnStageLoad()
    {
        specialAbility1RemainingCdTurns = 0;
        specialAbility2RemainingCdTurns = 0;
    }

    private void OnTurnChangeForPlayer()
    {        
        if (specialAbility1RemainingCdTurns > 0)
        {
            specialAbility1RemainingCdTurns--;
            specialAbilitiesCooldownTexts[0].gameObject.SetActive(true);
        }
        if (specialAbility2RemainingCdTurns > 0)
        {
            specialAbility2RemainingCdTurns--;
            specialAbilitiesCooldownTexts[1].gameObject.SetActive(true);
        }
        if (specialAbility1RemainingCdTurns > 0)
        {
            SpecialAbility1IsReady = false;
        }
        else
        {
            SpecialAbility1IsReady = true;
            specialAbilitiesCooldownTexts[0].gameObject.SetActive(false);
        }
        if (specialAbility2RemainingCdTurns > 0)
        {
            SpecialAbility2IsReady = false;
        }
        else
        {
            SpecialAbility2IsReady = true;
            specialAbilitiesCooldownTexts[1].gameObject.SetActive(false);
        }
        specialAbilitiesCooldownTexts[0].text = specialAbility1RemainingCdTurns.ToString();
        specialAbilitiesCooldownTexts[1].text = specialAbility2RemainingCdTurns.ToString();
    }
      
    public void ActivateSpecialAbilityCooldown(int specialAbilityNum, int cooldownTurns)
    {
        switch (specialAbilityNum)
        {
            case 1:
                specialAbility1RemainingCdTurns = cooldownTurns + 1;
                break;
            case 2:
                specialAbility2RemainingCdTurns = cooldownTurns + 1;
                break;
        }
    }

    private void OnWrongAnswers(int badAnswersInCombat)
    {
        passiveText.gameObject.SetActive(true);
        passiveText.text = (badAnswersInCombat % 3).ToString();
        if (badAnswersInCombat % 3 == 0)
        {
            passiveText.gameObject.SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
