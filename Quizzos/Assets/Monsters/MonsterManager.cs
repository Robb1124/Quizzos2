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
    [SerializeField] MusicManager musicManager;
    [SerializeField] MessagePopup messagePopup;
    MonsterSheet[] currentRound;
    StageFile stageFile;
    int stageRoundsCount;
    bool stageComplete = false;
    int expCalculated = 0;
    public bool AllDead { get; set; } = false;
    public bool IsReadyForMonsterTurn { get; internal set; } = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnWaveOfMonsters(int roomNumber)
    {
        messagePopup.ReceiveStringAndShowPopup("Room " + roomNumber);
        currentRound = stageFile.rounds[roomNumber - 1].round;
        roomText.text = "Room : " + roomNumber;
        monstersToSpawn = currentRound.Length;
        monsterCount = monstersToSpawn;
        for (int i = 1; i <= monstersToSpawn; i++)
        {
            monsters[i - 1].gameObject.SetActive(true);
            monsters[i - 1].MonsterSheet = currentRound[i - 1];
            monsters[i - 1].MonsterNumber = i;
            monsters[i - 1].OnSpawn(stageFile.stageLevel);
            if(monsters[i - 1].MonsterSheet.IsABoss())
            {
                musicManager.ChangeMusicTrack(3); //boss track, button cant take enum as parameter so had to make it int.
                Debug.Log("hello");
            }
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
        IsReadyForMonsterTurn = false;
        stageManager.MonsterDeathAddExpAndGold(monsters[monsterNumber - 1].MonsterSheet.GetExpType(), monsters[monsterNumber - 1].MonsterSheet.GetGoldType());
        StartCoroutine(DeathAnimation(monsterNumber));
        monsterCount--;
        
    }

    private IEnumerator DeathAnimation(int monsterNumber)
    {
        print("dead");
        float fillAmount = 1f;
        while (fillAmount > 0f)
        {
            print("looping");
            fillAmount -= 0.02f;
            monsters[monsterNumber - 1].MonsterImage.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.015f);
        }
        monsters[monsterNumber - 1].gameObject.SetActive(false);
        monsters[monsterNumber - 1].MonsterImage.fillAmount = 1f;
        if (monsterCount == 0)
        {
            AllDead = true;
            StartCoroutine(DelayBetweenSpawn());
        }
        else
        {
            IsReadyForMonsterTurn = true;
        }
    }

    private IEnumerator DelayBetweenSpawn()
    {
        if(roomNumber > stageRoundsCount)
        {
            //stop turn state from changing
            turnManager.CombatIsOver = true;
        }
        yield return new WaitForSeconds(1);
        if(roomNumber <= stageRoundsCount)
        {
            SpawnWaveOfMonsters(roomNumber);
            IsReadyForMonsterTurn = true;
        }
        else
        {
            if(!stageManager.StageCompleted[stageFile.stageNumber - 1])
            {
                stageManager.StageCompleted[stageFile.stageNumber - 1] = true;
                stageManager.StageRewardReceived[stageFile.stageNumber - 1] = true;
                stageManager.FirstTimeStageCompletionReward();
            }
            roomNumber++;
            turnManager.StageCompleted();           
        }
    }

    public void ReceiveStageFile(StageFile stageFile)
    {
        expCalculated = 0;
        this.stageFile = stageFile;
        stageRoundsCount = stageFile.rounds.Length;
    }

    public void InitialSetup()
    {
        AllDead = false;
        roomNumber = 1;
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].gameObject.SetActive(false);
        }
    }
}
