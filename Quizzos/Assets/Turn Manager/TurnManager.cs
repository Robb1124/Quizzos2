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
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] LevelSystem levelSystem;
    [SerializeField] StageManager stageManager;
    [SerializeField] QuizManager quizManager;
    public TurnState TurnState { get => turnState; set => turnState = value; }
    public float ExpCalculated { get; set; } = 0;

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
        if (turnState.GetComponent<PrePlayerTurn>())
        {
            onTurnChangeForPlayer?.Invoke(); //Observer pattern for cooldown and special ability availability management. If it isnt null, it will run (invoke)
        }
        this.TurnState = turnState;
        
        turnState.OnStateChange();
    }

    private void OnPlayerDeath()
    {
        gameOverPopup.SetActive(true);
        //Game over Sound
        CalculateRewardsAndSetUI();
        //button click closes menu and calls the claim reward method
    }

    private void CalculateRewardsAndSetUI()
    {
        ExpCalculated = monsterManager.CalculateExp();
        rewardText.text = "You have gained : \n" +
            ExpCalculated + " Experience Points.";
    }

    public void StageCompleted()
    {
        stageCompletedPopup.SetActive(true);
        CalculateRewardsAndSetUI();
    }
    public void ClaimRewardsButton()
    {
        levelSystem.GainExp(ExpCalculated);
        SaveSystem.SavePlayer(player, levelSystem, stageManager, quizManager);
        stageManager.ActivateUnlockedStageButtons();

        //place for other rewards methods
    }


}
