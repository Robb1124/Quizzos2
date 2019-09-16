using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monster Sheet")]
public class MonsterSheet : ScriptableObject
{
    [SerializeField] int monsterHp = 100;
    [SerializeField] string monsterName = "default monster name";
    [SerializeField] Sprite monsterImage;
    [SerializeField] float monsterBaseDamage = 1;
    [SerializeField] SpecialEffects specialEffectOnHit;
    [SerializeField] bool isABoss = false;

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

    public SpecialEffects GetSpecialEffectOnHit()
    {
        return specialEffectOnHit;
    }

    public bool IsABoss()
    {
        return isABoss;
    }
}
