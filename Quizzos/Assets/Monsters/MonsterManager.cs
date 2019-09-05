﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{

    [SerializeField] Monster[] monsters;
    [SerializeField] MonsterSheet[] monsterSheets;
    [SerializeField] int monsterCount;
    [SerializeField] int monstersToSpawn;
    [SerializeField] int roomNumber = 1;
    [SerializeField] Text roomText;
    [SerializeField] Player player;
    [SerializeField] TurnManager turnManager;
    [SerializeField] StageManager stageManager;
    MonsterSheet[] currentRound;
    StageFile stageFile;
    int stageRoundsCount;
    bool stageComplete = false;

    public bool AllDead { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnWaveOfMonsters(int roomNumber)
    {        
        currentRound = stageFile.rounds[roomNumber - 1].round;
        roomText.text = "Room : " + roomNumber;
        monstersToSpawn = currentRound.Length;
        monsterCount = monstersToSpawn;
        for (int i = 1; i <= monstersToSpawn; i++)
        {
            monsters[i - 1].gameObject.SetActive(true);
            monsters[i - 1].MonsterSheet = currentRound[i - 1];
            monsters[i - 1].MonsterNumber = i;
            monsters[i - 1].OnSpawn();
        }
        this.roomNumber++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MonsterDeath(int monsterNumber)
    {
        //Monster death (on peut inserer un delai pour animation/son)
        monsters[monsterNumber - 1].gameObject.SetActive(false);
        monsterCount--;
        if(monsterCount == 0)
        {
            AllDead = true;
            StartCoroutine(DelayBetweenSpawn());
            
        }
    }

    private IEnumerator DelayBetweenSpawn()
    {
        yield return new WaitForSeconds(1);
        if(roomNumber != stageRoundsCount)
        {
            SpawnWaveOfMonsters(roomNumber);
        }
        else
        {
            if(!stageManager.StageCompleted[stageFile.stageNumber - 1])
            {
                stageManager.StageCompleted[stageFile.stageNumber - 1] = true;
            }
        }
    }

    public void ReceiveStageFile(StageFile stageFile)
    {
        this.stageFile = stageFile;
        stageRoundsCount = stageFile.rounds.Length;
    }

    public float CalculateExp()
    {
        float expCalculated = 0;

        if (roomNumber > 1)
        {
            for (int i = 0; i < roomNumber - 2; i++) //gives exp only for room cleared
            {
                expCalculated += stageFile.rounds[i].xp;
            }
        }
        
        if (stageComplete)
        {
            
        }
        stageComplete = false;
        roomNumber = 1;
        return expCalculated;
       
    }

    public void InitialSetup()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].gameObject.SetActive(false);
        }
    }
}
