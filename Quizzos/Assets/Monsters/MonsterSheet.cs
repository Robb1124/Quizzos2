using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExpType { Low, Medium, High, Boss };
public enum GoldType { Low, Medium, High, Boss };

[CreateAssetMenu(menuName = "Monster Sheet")]
public class MonsterSheet : ScriptableObject
{
    [SerializeField] int monsterHp = 100;
    [SerializeField] string monsterName = "default monster name";
    [SerializeField] Sprite monsterImage;
    [SerializeField] float monsterBaseDamage = 1;
    [SerializeField] MonsterAttacksHolder[] monsterAttacksHolders;
    [SerializeField] bool isABoss = false;
    [SerializeField] ExpType expType;
    [SerializeField] GoldType goldType;


    public int GetMonsterHp()
    {
        return monsterHp;
    }

    public string GetMonsterName()
    {
        return monsterName;
    }

    public float GetMonsterBaseDamage()
    {
        return monsterBaseDamage;
    }

    public Sprite GetMonsterImage()
    {
        return monsterImage;
    }

    public bool IsABoss()
    {
        return isABoss;
    }

    public ExpType GetExpType()
    {
        return expType;
    }

    public GoldType GetGoldType()
    {
        return goldType;
    }

    public MonsterAttacksHolder[] GetMonsterAttacksHolders()
    {
        return monsterAttacksHolders;
    }
}
