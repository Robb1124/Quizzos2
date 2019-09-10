﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{    
    [SerializeField] TurnState turnState;
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPopup;
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
        ExpCalculated = monsterManager.CalculateExp();
        //button click closes menu and calls the claim reward method
    }

    public void ClaimRewardsButton()
    {
        levelSystem.GainExp(ExpCalculated);
        SaveSystem.SavePlayer(player, levelSystem, stageManager, quizManager);
        stageManager.ActivateUnlockedStageButtons();

        //place for other rewards methods
    }


}
