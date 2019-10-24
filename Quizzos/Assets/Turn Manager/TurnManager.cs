using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{    
    [SerializeField] TurnState turnState;
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPopup;
    [SerializeField] GameObject stageCompletedPopup;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] LevelSystem levelSystem;
    [SerializeField] StageManager stageManager;
    [SerializeField] GemsAndGoldSystem goldSystem;
    [SerializeField] GameObject gemsRewardText;
    bool stageComplete = false;
    public TurnState TurnState { get => turnState; set => turnState = value; }
    public int ExpCalculated { get; set; } = 0;
    public int GoldCalculated { get; set; } = 0;
    public bool CombatIsOver { get; set; } = false;

    public delegate void OnTurnChangeForPlayer(); // declare new delegate type
    public event OnTurnChangeForPlayer onTurnChangeForPlayer; // instantiate an observer set


    // Start is called before the first frame update
    void Start()
    {
        player.onPlayerDeath += OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTurnState(TurnState turnState)
    {
        if (turnState is PrePlayerTurn)
        {
            onTurnChangeForPlayer?.Invoke(); //Observer pattern for cooldown and special ability availability management. If it isnt null, it will run (invoke)
        }

        if (!CombatIsOver)
        {
            this.TurnState = turnState;
            turnState.OnStateChange();
        }
    }

    private void OnPlayerDeath()
    {
        gameOverPopup.SetActive(true);
        //Game over Sound
        CalculateRewardsSetUIAndRemoveEffects();
        //button click closes menu and calls the claim reward method
    }

    private void CalculateRewardsSetUIAndRemoveEffects()
    {
        PrePlayerTurn prePlayerTurn = GetComponentInChildren<PrePlayerTurn>();
        prePlayerTurn.RemoveAllSpecialEffects();
        ExpCalculated = stageManager.CalculateExp(stageComplete);
        GoldCalculated = stageManager.CalculateGold(stageComplete);
        rewardText.text = "You have gained : \n \n" +
            ExpCalculated + " <sprite=2> & \n" +
            GoldCalculated + " <sprite=0> from your adventure!";
        if (stageManager.FirstTimeCompletion)
        {
            gemsRewardText.SetActive(true);
        }
        stageComplete = false;
    }

    public void StageCompleted()
    {
        stageCompletedPopup.SetActive(true);
        stageComplete = true;
        CalculateRewardsSetUIAndRemoveEffects();
    }
    public void ClaimRewardsButton()
    {
        gemsRewardText.SetActive(false);
        levelSystem.GainExp(ExpCalculated);
        goldSystem.AddGold(GoldCalculated);
        player.SavePlayer();
        stageManager.ActivateUnlockedStageButtons();
        CombatIsOver = false;
        //place for other rewards methods
    }


}
