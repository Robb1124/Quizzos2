using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum MonsterAttacks {
    [Description("Basic Attack")]
    BasicAttack,
    [Description("Headbutt")]
    Headbutt,
    [Description("Tail Whip")]
    TailWhip
}

[System.Serializable]
public class MonsterAttacksHolder
{
    public MonsterAttacks monsterAttack;
    [Range(0f, 1f)]
    public float useProbability;
}
