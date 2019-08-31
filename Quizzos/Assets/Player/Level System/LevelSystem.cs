using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] int playerLevel = 1;
    [SerializeField] int[] xpRequirementsForLevelUp;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject levelUpPopUp;
    [SerializeField] TextMeshProUGUI levelUpPopUpText;
    [SerializeField] float experiencePoints;
    [SerializeField] TurnManager turnManager;
    [SerializeField] float maxHpGainOnLvlUp;
    [SerializeField] float baseDmgGainOnLvlUp;
    [SerializeField] int questionsAddedOnLvlUp;
    [SerializeField] LevelUpGains warriorsLvlUpGains;
    [SerializeField] JsonHarvester jsonHarvester;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        levelText.text = "Level " + playerLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainExp(float expPointsGained)
    {
        experiencePoints += expPointsGained;
        if(experiencePoints >= xpRequirementsForLevelUp[playerLevel - 1])
        {
            experiencePoints -= xpRequirementsForLevelUp[playerLevel - 1];
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        Debug.Log("LEVEL UP!");
        playerLevel++;
        levelText.text = "Level " + playerLevel;
        if (player.GetComponent<Warrior>())
        {
            maxHpGainOnLvlUp = warriorsLvlUpGains.HpGains[playerLevel - 2];
            baseDmgGainOnLvlUp = warriorsLvlUpGains.baseDamageGains[playerLevel - 2];
            questionsAddedOnLvlUp = warriorsLvlUpGains.questionsGained[playerLevel - 2];
        }
        levelUpPopUpText.text = "You are now level " + playerLevel + " !\n" +
                                                            "+" + maxHpGainOnLvlUp + "\n" +
                                                            "+" + baseDmgGainOnLvlUp + "\n" +
                                                            "+" + questionsAddedOnLvlUp;
        levelUpPopUp.gameObject.SetActive(true);
        player.AddMaxHpAndBaseDamage((int)maxHpGainOnLvlUp, (int)baseDmgGainOnLvlUp);
        jsonHarvester.AddQuestionsToTheDeck(questionsAddedOnLvlUp);
    }
}
