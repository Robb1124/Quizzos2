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
    StageFile currentStage;
    [SerializeField] MonsterManager monsterManager;

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
        monsterManager.InitialSetup();
        monsterManager.ReceiveStageFile(currentStage);
        monsterManager.SpawnWaveOfMonsters(1);
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PrePlayerTurn>());
        worldMapElementsHolder.SetActive(false);
        instanceElementsHolder.SetActive(true);
        stageName.text = currentStage.stageName;
        onStageLoad(); //for player to regen his life + remove all status effects
        
    }

    public void StagePreviewPopUp(int stageClicked)
    {
        currentStage = stages[stageClicked - 1];
        stagePreviewNameField.text = stageClicked + " - " + currentStage.stageName;
        stageDescriptionField.text = "Level Range : " + currentStage.levelRange + "\n" +
                                       "Number of fights : " + currentStage.rounds.Length + "\n" +
                                        "Reward : \n" +
                                        "xxx XP  \n" +
                                        "xxx Gold ";
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
}
