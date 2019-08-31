using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Up Gains")]
public class LevelUpGains : ScriptableObject
{
    public int[] HpGains;
    public int[] baseDamageGains;
    public int[] questionsGained;
}
