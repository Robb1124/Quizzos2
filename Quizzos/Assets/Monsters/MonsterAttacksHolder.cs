using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum MonsterAttacks {
    [Description("Basic Attack")]
    BasicAttack,
    [Description("Headbutt")]
    Headbutt,
    [Description("Venomous Sting")]
    VenomousSting,
    [Description("Heavy Hit")]
    HeavyHit,
    [Description("Punch")]
    Punch,
    [Description("Cure")]
    Cure,
    [Description("Shield Bash")]
    ShieldBash,
    [Description("Flame Slash")]
    FlameSlash
}

[System.Serializable]
public class MonsterAttacksHolder
{
    public MonsterAttacks monsterAttack;
    [Range(0f, 1f)]
    public float useProbability;
}
