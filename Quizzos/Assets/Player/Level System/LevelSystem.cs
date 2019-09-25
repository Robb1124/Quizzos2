﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] int playerLevel = 1;
    [SerializeField] int[] xpRequirementsForLevelUp;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image levelUpBar;
    [SerializeField] TextMeshProUGUI worldLevelText;
    [SerializeField] Image worldLevelUpBar;
    [SerializeField] GameObject levelUpPopUp;
    [SerializeField] TextMeshProUGUI levelUpPopUpText;
    [SerializeField] TurnManager turnManager;
    [SerializeField] float maxHpGainOnLvlUp;
    [SerializeField] float baseDmgGainOnLvlUp;
    [SerializeField] int questionsAddedOnLvlUp;
    [SerializeField] LevelUpGains warriorsLvlUpGains;
    [SerializeField] JsonHarvester jsonHarvester;
    Player player;

    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
    public float ExperiencePoints { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        UpdateExpBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainExp(int expPointsGained)
    {
        ExperiencePoints += expPointsGained;
        if(ExperiencePoints >= xpRequirementsForLevelUp[PlayerLevel - 1])
        {
            ExperiencePoints -= xpRequirementsForLevelUp[PlayerLevel - 1];
            OnLevelUp();
        }
        UpdateExpBar();
    }

    public void UpdateExpBar()
    {
        levelUpBar.fillAmount = ExperiencePoints / xpRequirementsForLevelUp[PlayerLevel - 1];
        worldLevelUpBar.fillAmount = ExperiencePoints / xpRequirementsForLevelUp[PlayerLevel - 1];
        levelText.text = "Level " + PlayerLevel;
        worldLevelText.text = "Level " + PlayerLevel;
    }

    private void OnLevelUp()
    {
        Debug.Log("LEVEL UP!");
        PlayerLevel++;
        if (player.GetComponent<Warrior>())
        {
            maxHpGainOnLvlUp = warriorsLvlUpGains.HpGains[PlayerLevel - 2];
            baseDmgGainOnLvlUp = warriorsLvlUpGains.baseDamageGains[PlayerLevel - 2];
            questionsAddedOnLvlUp = warriorsLvlUpGains.questionsGained[PlayerLevel - 2];
        }
        levelUpPopUpText.text = "You are now level " + PlayerLevel + " !\n" +
                                                            "+" + maxHpGainOnLvlUp + " Maximum Health points.\n" +
                                                            "+" + baseDmgGainOnLvlUp + " Base Damage\n" +
                                                            "+" + questionsAddedOnLvlUp + " Questions added to your Player Deck.";
        levelUpPopUp.gameObject.SetActive(true);
        player.AddMaxHpAndBaseDamage((int)maxHpGainOnLvlUp, (int)baseDmgGainOnLvlUp);
        jsonHarvester.AddQuestionsToTheDeck(questionsAddedOnLvlUp);
    }
}
