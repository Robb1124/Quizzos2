using System;
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
    MonsterSheet[] currentRound;
    StageFile stageFile;
    int stageRoundsCount;

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
            //stage is over ! VICTORY
        }
    }

    public void ReceiveStageFile(StageFile stageFile)
    {
        this.stageFile = stageFile;
        stageRoundsCount = stageFile.rounds.Length;
    }

    
}
