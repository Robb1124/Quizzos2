using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public int playerLevel;
    public int expPoints;
    public int playerMaxHp;
    public int playerBaseDmg;
    public int classIndex;
    public List<bool> stageCompleted;

    public PlayerData(Player player, LevelSystem levelSystem, StageManager stageManager)
    {
        playerLevel = levelSystem.PlayerLevel;
        expPoints = (int)levelSystem.ExperiencePoints;
        playerMaxHp = player.PlayerMaxHp;
        playerBaseDmg = player.PlayerBaseDmg;
        classIndex = player.ClassIndex;
        stageCompleted = stageManager.StageCompleted;
    }
}
