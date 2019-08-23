using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Round
{
    public MonsterSheet[] round;
}

[CreateAssetMenu(menuName = "Stage File")]
public class StageFile : ScriptableObject
{
    public int stageNumber;
    public string stageName;
    public string levelRange;
    public Image stageBackground;
    public Round[] rounds;
}
