using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StageManager : MonoBehaviour
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] Image stageBackground;
    [SerializeField] Text stageName;
    [SerializeField] GameObject worldMapElementsHolder;
    [SerializeField] GameObject instanceElementsHolder;
    [SerializeField] StageFile[] stages;
    [SerializeField] Button[] stageButtons;
    [SerializeField] List<bool> stageCompleted;
    [SerializeField] TextMeshProUGUI stagePreviewNameField;
    [SerializeField] TextMeshProUGUI stageDescriptionField;
    [SerializeField] Text xpText;
    [SerializeField] int[] baseExpPerType;
    [SerializeField] float baseExpMultiplier = 1.1f;
    float expMultiplier;
    [SerializeField] float bonusExpPercentageForCompletion;
    int expToGain = 0;
    [SerializeField] QuizManager quizManager;
    StageFile currentStage;
    [SerializeField] MonsterManager monsterManager;
    int totalStageExpPreview;
    List<ExpType> expList = new List<ExpType>();
    public List<bool> StageCompleted { get => stageCompleted; set => stageCompleted = value; }

    public delegate void OnStageLoad(); // declare new delegate type
    public event OnStageLoad onStageLoad; // instantiate an observer set

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            StageCompleted.Add(false);
        }
        ActivateUnlockedStageButtons();
        monsterManager = FindObjectOfType<MonsterManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //methode pour ouvrir un preview du level

    public void LoadLevel()
    {
        quizManager.BadAnswersInCombat = 0;
        expToGain = 0;
        xpText.text = "0";
        monsterManager.InitialSetup();
        monsterManager.ReceiveStageFile(currentStage);
        monsterManager.SpawnWaveOfMonsters(1);
        onStageLoad(); //for player to regen his life + remove all status effects
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PrePlayerTurn>());
        worldMapElementsHolder.SetActive(false);
        instanceElementsHolder.SetActive(true);
        stageName.text = currentStage.stageName;
        
    }

    public void StagePreviewPopUp(int stageClicked)
    {
        totalStageExpPreview = 0;
        expList.Clear();
        currentStage = stages[stageClicked - 1];
        for (int i = 0; i < currentStage.rounds.Length; i++)
        {
            for (int j = 0; j < currentStage.rounds[i].round.Length; j++)
            {
                expList.Add(currentStage.rounds[i].round[j].GetExpType());
            }
        }
        expMultiplier = Mathf.Pow(baseExpMultiplier, currentStage.stageLevel - 1);
        foreach(ExpType expType in expList)
        {
            switch (expType)
            {
                case ExpType.Low:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[0] * expMultiplier);
                    break;
                case ExpType.Medium:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[1] * expMultiplier);
                    break;
                case ExpType.High:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[2] * expMultiplier);
                    break;
                case ExpType.Boss:
                    totalStageExpPreview += Mathf.RoundToInt(baseExpPerType[3] * expMultiplier);
                    break;
            }
        }
        totalStageExpPreview = (int)(totalStageExpPreview * bonusExpPercentageForCompletion);
        stagePreviewNameField.text = currentStage.stageName;
        stageDescriptionField.text = "Level Range : " + currentStage.stageLevel + "\n" +
                                       "Number of fights : " + currentStage.rounds.Length + "\n" +
                                        "Reward : \n" +
                                        "Up to " + totalStageExpPreview + " XP";
    }

    public void ActivateUnlockedStageButtons()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stageButtons[i].interactable = false;
        }
        stageButtons[0].interactable = true;
        for (int i = 0; i < stages.Length; i++)
        {
            if (StageCompleted[i])
            {
                stageButtons[i + 1].interactable = true;
            }           
        }
    }

    public void MonsterDeathAddExp(ExpType monsterExpType)
    {
        switch (monsterExpType)
        {
            case ExpType.Low:
                expToGain += Mathf.RoundToInt(baseExpPerType[0] * expMultiplier);
                break;
            case ExpType.Medium:
                expToGain += Mathf.RoundToInt(baseExpPerType[1] * expMultiplier);
                break;
            case ExpType.High:
                expToGain += Mathf.RoundToInt(baseExpPerType[2] * expMultiplier);
                break;
            case ExpType.Boss:
                expToGain += Mathf.RoundToInt(baseExpPerType[3] * expMultiplier);
                break;
        }
        xpText.text = expToGain.ToString();
    }

    public float CalculateExp(bool stageCompleted)
    {
        if (stageCompleted)
        {
            expToGain = Mathf.RoundToInt(expToGain * bonusExpPercentageForCompletion);
            xpText.text = expToGain.ToString();
        }
        return expToGain;
    }
}
