using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Abilities { BasicAttack, SpecialAbility1, SpecialAbility2, ItemPouch};

public class Player : MonoBehaviour
{
    [SerializeField] int playerMaxHp = 100;
    [SerializeField] float playerCurrentHp;
    [SerializeField] int playerBaseDmg = 25;
    [SerializeField] float dmgReduction = 0;
    [SerializeField] float dmgMultiplierEffects = 1f;
    [SerializeField] Text playerHpText;
    [SerializeField] Image playerHpBar;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] PrePlayerTurn prePlayerTurn;
    [SerializeField] LevelSystem levelSystem;
    [SerializeField] StageManager stageManager;
    [SerializeField] QuizManager quizManager;
    [SerializeField] ItemAndGoldSystem goldSystem;
    [SerializeField] AudioClip[] takeDamageSFXs;
    [SerializeField] AudioClip burnDamageSFX;
    bool playerDead = false;
    AudioSource audioSource { get; set; }
    int classIndex;
    
    public CharacterClass CharacterClass { get => characterClass; set => characterClass = value; }
    public int PlayerBaseDmg { get => playerBaseDmg; set => playerBaseDmg = value; }
    public float DmgReduction { get => dmgReduction; set => dmgReduction = value; }
    public int PlayerMaxHp { get => playerMaxHp; set => playerMaxHp = value; }
    public int ClassIndex { get => classIndex; set => classIndex = value; }
    public float PoisonResist { get; set; }
    public float BurnResist { get; set; }
    public float ShockResist { get; set; }
    public float FrostResist { get; set; }
    public float ConcussionResist { get; set; }


    public delegate void OnPlayerDeath(); // declare new delegate type
    public event OnPlayerDeath onPlayerDeath; // instantiate an observer set

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCurrentHp = PlayerMaxHp;
        stageManager.onStageLoad += OnStageLoad;
    }
   

    // Update is called once per frame
    void Update()
    {
        playerHpText.text = playerCurrentHp.ToString();
        playerHpBar.fillAmount = playerCurrentHp / PlayerMaxHp;
    }

    public void TakeDamage(float amountOfDamage, SpecialEffects onHitSpecialEffect = SpecialEffects.None)
    {      
        if(onHitSpecialEffect != SpecialEffects.None)
        {
            float rand;
            switch (onHitSpecialEffect)
            {
                case SpecialEffects.Poison:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if(rand > PoisonResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect);
                    }
                    break;
                case SpecialEffects.Burn:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > BurnResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect);
                    }
                    break;
                case SpecialEffects.Shock:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > ShockResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect);
                    }
                    break;
                case SpecialEffects.Concussion:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > ConcussionResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect);
                    }
                    break;
                    //frost here
            }
        }
        audioSource.clip = (prePlayerTurn.ShieldUpActive) ? takeDamageSFXs[1] : takeDamageSFXs[0];
        audioSource.Play();
        

        playerCurrentHp -= (int)(amountOfDamage * (1 - DmgReduction));
        if (playerCurrentHp <= 0 && !playerDead)
        {
            GameOver();
            playerDead = true;
        }
    }

    public void TakeBurnDamage(float percentageOfDmg)
    {
        float dmgToTake = playerMaxHp * percentageOfDmg;
        playerCurrentHp -= (int)dmgToTake;
        audioSource.PlayOneShot(burnDamageSFX);
        if (playerCurrentHp <= 0)
        {
            GameOver();
        }
    }
   

    public void ReceiveClass(int choosenClassIndex)
    {
        switch (choosenClassIndex)
        {
            case 0:
                //Remove every other class script
                CharacterClass = GetComponent<Warrior>();
                ClassIndex = 0;
                break;
        }
        CharacterClass.OnClassEquip();
    }

    public void SetPlayerMaxHpAndBaseDmgAndResists(int maxHp, int baseDmg, float poisonResist, float burnResist, float shockResist, float frostResist, float concussionResist)
    {
        PlayerMaxHp = maxHp;
        playerCurrentHp = PlayerMaxHp;
        PlayerBaseDmg = baseDmg;
        this.PoisonResist = poisonResist;
        this.BurnResist = burnResist;
        this.ShockResist = shockResist;
        this.FrostResist = frostResist;
        this.ConcussionResist = concussionResist;
    }

    private void GameOver()
    {        
        onPlayerDeath();        
    }

    public void AddMaxHpAndBaseDamage(int maxHpGain, int maxDmgGain)
    {
        PlayerMaxHp += maxHpGain;
        playerCurrentHp = PlayerMaxHp;
        playerBaseDmg += maxDmgGain;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, levelSystem, stageManager, quizManager, goldSystem);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        levelSystem.PlayerLevel = data.playerLevel;
        levelSystem.ExperiencePoints = data.expPoints;
        levelSystem.UpdateExpBar();
        goldSystem.AddGold(data.gold);
        playerMaxHp = data.playerMaxHp;
        playerCurrentHp = playerMaxHp;
        PlayerBaseDmg = data.playerBaseDmg;
        PoisonResist = data.poisonResist;
        BurnResist = data.burnResist;
        ShockResist = data.shockResist;
        FrostResist = data.frostResist;
        classIndex = data.classIndex;
        stageManager.StageCompleted = data.stageCompleted;
        stageManager.ActivateUnlockedStageButtons();
        if(data.playerDeckQuestionIds != null)
        {
            quizManager.PlayerDeckQuestionIds = data.playerDeckQuestionIds;
            quizManager.RecreatePlayerDeckOnLoad();
        }
        else
        {
            //example of how to catch past saves errors, would do something here, ex : new questions based on level.
        }
        
        switch (classIndex)
        {
            case 0:
                //remove other classes
                characterClass = GetComponent<Warrior>();
                break;
        }

    }

    private void OnStageLoad()
    {
        playerDead = false;
        playerCurrentHp = playerMaxHp;
        prePlayerTurn.RemoveAllSpecialEffects();
    }

    public float GetCalculatedPlayerDmg()
    {
        return playerBaseDmg * dmgMultiplierEffects; //can change this to change how effects change the player damage. Ex: concussion gets calculated on top of everything, or removes 25% from 225% player dmg?
    }

    public void AddConcussion(float baseDmgReductionPercentage)
    {
        dmgMultiplierEffects -= baseDmgReductionPercentage;
    }
    public void RemoveConcussion(float baseDmgReductionPercentage)
    {
        dmgMultiplierEffects += baseDmgReductionPercentage;
    }
}
