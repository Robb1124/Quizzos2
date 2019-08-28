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
    [SerializeField] TextMeshProUGUI stagePreviewNameField;
    [SerializeField] TextMeshProUGUI stageDescriptionField;
    StageFile currentStage;
    [SerializeField] MonsterManager monsterManager;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterManager = FindObjectOfType<MonsterManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methode pour ouvrir un preview du level

    public void LoadLevel(StageFile stageFile)
    {
        monsterManager.ReceiveStageFile(stageFile);
        monsterManager.SpawnWaveOfMonsters(1);
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PlayerTurn>());
        worldMapElementsHolder.SetActive(false);
        instanceElementsHolder.SetActive(true);
        stageName.text = currentStage.stageName;
        
    }

    public void StagePreviewPopUp(int stageClicked)
    {
        currentStage = stages[stageClicked - 1];
        stagePreviewNameField.text = stageClicked + " - " + currentStage.stageName;
        stageDescriptionField.text = "Level Range : " + currentStage.levelRange + "\n" +
                                       "Number of fights : " + currentStage.rounds.Length + "\n" +
                                        "Reward : \n" +
                                        "xxx XP  \n" +
                                        "xxx Gold ";
    }
}
