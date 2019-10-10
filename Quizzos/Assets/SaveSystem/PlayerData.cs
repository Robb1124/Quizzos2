using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public int playerLevel;
    public int expPoints;
    public int gold;
    public int playerMaxHp;
    public int playerBaseDmg;
    public float poisonResist;
    public float burnResist;
    public float shockResist;
    public float frostResist;
    public int classIndex;
    public List<bool> stageCompleted;
    public int[] playerDeckQuestionIds;
    public List<InventoryMemory> inventoryMemoryList;

    public PlayerData(Player player, LevelSystem levelSystem, StageManager stageManager, QuizManager quizManager, ItemAndGoldSystem goldSystem, InventorySystem inventorySystem)
    {
        playerLevel = levelSystem.PlayerLevel;
        expPoints = (int)levelSystem.ExperiencePoints;
        gold = goldSystem.GetGold();
        inventoryMemoryList = inventorySystem.InventoryMemoryList;
        playerMaxHp = player.PlayerMaxHp;
        playerBaseDmg = player.PlayerBaseDmg;
        classIndex = player.ClassIndex;
        stageCompleted = stageManager.StageCompleted;
        playerDeckQuestionIds = quizManager.PlayerDeckQuestionIds;
        poisonResist = player.PoisonResist;
        burnResist = player.BurnResist;
        shockResist = player.ShockResist;
        frostResist = player.FrostResist;        
    }
}
