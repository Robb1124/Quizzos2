﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StageManager : MonoBehaviour
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] Image stageBackground;
    [SerializeField] Text stageName;
    [SerializeField] GameObject worldMapElementsHolder;
    [SerializeField] GameObject instanceElementsHolder;
    [SerializeField] StageFile[] stages;
    [SerializeField] Button[] stageButtons;
    [SerializeField] List<bool> stageCompleted;
    [SerializeField] List<bool> stageRewardReceived;
    [SerializeField] TextMeshProUGUI stagePreviewNameField;
    [SerializeField] TextMeshProUGUI stageDescriptionField;
    [SerializeField] Text xpText;
    [SerializeField] Text goldText;
    [SerializeField] int[] baseExpPerType;
    [SerializeField] int[] baseGoldPerType;
    [SerializeField] float baseExpMultiplier = 1.1f;
    [SerializeField] float baseGoldMultiplier = 1.1f;
    float expMultiplier;
    float goldMultiplier;
    [SerializeField] float bonusExpPercentageForCompletion;
    [SerializeField] float bonusGoldPercentageForCompletion;
    [SerializeField] int gemsRewardForCompletingStage;
    bool firstTimeCompletion = false;
    int expToGain = 0;
    int goldToGain = 0;
    [SerializeField] QuizManager quizManager;
    StageFile currentStage;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] GemsAndGoldSystem gemsAndGoldSystem;
    int totalStageExpPreview;
    int totalStageGoldPreview;
    List<ExpType> expList = new List<ExpType>();
    List<GoldType> goldList = new List<GoldType>();
    public List<bool> StageCompleted { get => stageCompleted; set => stageCompleted = value; }
    public List<bool> StageRewardReceived { get => stageRewardReceived; set => stageRewardReceived = value; }
    public bool FirstTimeCompletion { get => firstTimeCompletion; set => firstTimeCompletion = value; }

    public delegate void OnStageLoad(); // declare new delegate type
    public event OnStageLoad onStageLoad; // instantiate an observer set

    // Start is called before the first frame update
    void Start()
    {
        for (int i = -1; i < stages.Length; i++)
        {
            StageCompleted.Add(false);
            StageRewardReceived.Add(false);
        }
        ActivateUnlockedStageButtons();
        monsterManager = FindObjectOfType<MonsterManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methode pour ouvrir un preview du level

    public void LoadLevel()
    {
        FirstTimeCompletion = false;
        quizManager.BadAnswersInCombat = 0;
        expToGain = 0;
        goldToGain = 0;
        xpText.text = "0";
        goldText.text = "0";
        monsterManager.InitialSetup();
        monsterManager.ReceiveStageFile(currentStage);
        monsterManager.SpawnWaveOfMonsters(1);
        onStageLoad(); //for player to regen his life + remove all status effects
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PrePlayerTurn>());
        worldMapElementsHolder.SetActive(false);
        instanceElementsHolder.SetActive(true);
        stageName.text = currentStage.stageName;
        
    }

    public void StagePreviewPopUp(int stageClicked)
    {
        totalStageExpPreview = 0;
        totalStageGoldPreview = 0;
        expList.Clear();
        currentStage = stages[stageClicked - 1];
        for (int i = 0; i < currentStage.rounds.Length; i++)
        {
            for (int j = 0; j < currentStage.rounds[i].round.Length; j++)
            {
                expList.Add(currentStage.rounds[i].round[j].GetExpType());
            }
        }
        goldList.Clear();
        for (int i = 0; i < currentStage.rounds.Length; i++)
        {
            for (int j = 0; j < currentStage.rounds[i].round.Length; j++)
            {
                goldList.Add(currentStage.rounds[i].round[j].GetGoldType());
            }
        }
        expMultiplier = Mathf.Pow(baseExpMultiplier, currentStage.stageLevel - 1);
        goldMultiplier = Mathf.Pow(baseGoldMultiplier, currentStage.stageLevel - 1);
        foreach (ExpType expType in expList)
        {
            switch (expType)
            {
                case ExpType.Low:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[0] * expMultiplier);
                    break;
                case ExpType.Medium:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[1] * expMultiplier);
                    break;
                case ExpType.High:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[2] * expMultiplier);
                    break;
                case ExpType.Boss:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[3] * expMultiplier);
                    break;
            }
        }
        foreach (GoldType goldType in goldList)
        {
            switch (goldType)
            {
                case GoldType.Low:
                    totalStageGoldPreview += Mathf.RoundToInt(baseGoldPerType[0] * goldMultiplier);
                    break;
                case GoldType.Medium:
                    totalStageGoldPreview += Mathf.RoundToInt(baseGoldPerType[1] * goldMultiplier);
                    break;
                case GoldType.High:
                    totalStageGoldPreview += Mathf.RoundToInt(baseGoldPerType[2] * goldMultiplier);
                    break;
                case GoldType.Boss:
                    totalStageGoldPreview += Mathf.RoundToInt(baseGoldPerType[3] * goldMultiplier);
                    break;
            }
        }

        totalStageExpPreview = (int)(totalStageExpPreview * bonusExpPercentageForCompletion);
        totalStageGoldPreview = (int)(totalStageGoldPreview * bonusGoldPercentageForCompletion);
        stagePreviewNameField.text = currentStage.stageName;
        stageDescriptionField.text = "Level Range : " + currentStage.stageLevel + "\n" +
                                       "Number of fights : " + currentStage.rounds.Length + "\n" +
                                        "Reward : \n" +
                                        "Up to " + totalStageExpPreview + " <sprite=2>\n" +
                                        "Up to " + totalStageGoldPreview + " <sprite=0>";
    }

    public void ActivateUnlockedStageButtons()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stageButtons[i].interactable = false;
        }
        stageButtons[0].interactable = true;
        for (int i = 0; i < stages.Length; i++)
        {
            if (StageCompleted[i])
            {
                stageButtons[i + 1].interactable = true;
            }           
        }
    }

    public void MonsterDeathAddExpAndGold(ExpType monsterExpType, GoldType monsterGoldType)
    {
        switch (monsterExpType)
        {
            case ExpType.Low:
                expToGain += Mathf.RoundToInt(baseExpPerType[0] * expMultiplier);
                break;
            case ExpType.Medium:
                expToGain += Mathf.RoundToInt(baseExpPerType[1] * expMultiplier);
                break;
            case ExpType.High:
                expToGain += Mathf.RoundToInt(baseExpPerType[2] * expMultiplier);
                break;
            case ExpType.Boss:
                expToGain += Mathf.RoundToInt(baseExpPerType[3] * expMultiplier);
                break;
        }
        switch (monsterGoldType)
        {
            case GoldType.Low:
                goldToGain += Mathf.RoundToInt(baseGoldPerType[0] * expMultiplier);
                break;
            case GoldType.Medium:
                goldToGain += Mathf.RoundToInt(baseGoldPerType[1] * expMultiplier);
                break;
            case GoldType.High:
                goldToGain += Mathf.RoundToInt(baseGoldPerType[2] * expMultiplier);
                break;
            case GoldType.Boss:
                goldToGain += Mathf.RoundToInt(baseGoldPerType[3] * expMultiplier);
                break;
        }
        xpText.text = expToGain.ToString();
        goldText.text = goldToGain.ToString();
    }

    public int CalculateExp(bool stageCompleted)
    {
        if (stageCompleted)
        {
            expToGain = Mathf.RoundToInt(expToGain * bonusExpPercentageForCompletion);
            xpText.text = expToGain.ToString();            
        }
        return expToGain;
    }

    public int CalculateGold(bool stageCompleted)
    {
        if (stageCompleted)
        {
            goldToGain = Mathf.RoundToInt(goldToGain * bonusGoldPercentageForCompletion);
            goldText.text = goldToGain.ToString();
        }
        return goldToGain;
    }

    public void FirstTimeStageCompletionReward()
    {
        FirstTimeCompletion = true;
        gemsAndGoldSystem.AddGems(gemsRewardForCompletingStage);
    }

    public void RewardPlayerForPastCompletedStage()
    {
        for (int i = 0; i < StageCompleted.Count; i++)
        {
            if(StageRewardReceived[i] == false && stageCompleted[i] == true)
            {
                StageRewardReceived[i] = true;
                gemsAndGoldSystem.AddGems(gemsRewardForCompletingStage);
            }
            else
            {
                return;
            }
        }
        
    }
}
