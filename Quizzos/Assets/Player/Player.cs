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

    [Range(0.00f, 1.00f)]
    [SerializeField] float critChance = 0.1f;
    [SerializeField] float critDamage = 1.5f;

    [SerializeField] float dmgReduction = 0;
    [SerializeField] float dmgMultiplierEffects = 1f;
    [SerializeField] Text playerHpText;
    [SerializeField] Image playerHpBar;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] PrePlayerTurn prePlayerTurn;
    [SerializeField] LevelSystem levelSystem;
    [SerializeField] StageManager stageManager;
    [SerializeField] QuizManager quizManager;
    [SerializeField] GemsAndGoldSystem gemsAndGoldSystem;
    [SerializeField] TimeManager timeManager;
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] AudioClip[] takeDamageSFXs;
    [SerializeField] AudioClip regenerationSFX;
    [SerializeField] AudioClip burnDamageSFX;
    float dmgBoostPercentage;
    float critHitChanceBoostPercentage;
    bool playerDead = false;
    bool isCrit = false;
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
    public bool IsCrit { get => isCrit; set => isCrit = value; }

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

    public void TakeDamage(float amountOfDamage, SpecialEffect onHitSpecialEffect = null)
    {      
        if(onHitSpecialEffect.EffectType != SpecialEffectsType.None)
        {
            float rand;
            switch (onHitSpecialEffect.EffectType)
            {
                case SpecialEffectsType.Poison:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if(rand > PoisonResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect.EffectType, 0, onHitSpecialEffect.TurnDuration);
                    }
                    break;
                case SpecialEffectsType.Burn:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > BurnResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect.EffectType, 0, onHitSpecialEffect.TurnDuration);
                    }
                    break;
                case SpecialEffectsType.Shock:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > ShockResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect.EffectType, 0, onHitSpecialEffect.TurnDuration);
                    }
                    break;
                case SpecialEffectsType.Concussion:
                    rand = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (rand > ConcussionResist)
                    {
                        prePlayerTurn.AddSpecialEffects(onHitSpecialEffect.EffectType, 0, onHitSpecialEffect.TurnDuration); //TODO : enable customisation of concussion dmg reduc?
                    }
                    break;
                    //frost here
            }
        }
        audioSource.clip = (prePlayerTurn.ShieldUpActive) ? takeDamageSFXs[1] : takeDamageSFXs[0];
        audioSource.Play();

        //Randomness in the dmg +- 10%
        float randomnDmgMultiplier = UnityEngine.Random.Range(0.9f, 1.1f);
        playerCurrentHp -= (int)((amountOfDamage*randomnDmgMultiplier) * (1 - DmgReduction));
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
   
    public void HealDamage(float healAmount)
    {
        playerCurrentHp = Mathf.Clamp(playerCurrentHp + healAmount, 0, playerMaxHp);
    }

    public void RegeneratePercentageOfLife(float percentage)
    {
        audioSource.clip = regenerationSFX;
        audioSource.Play();
        playerCurrentHp = Mathf.Clamp(Mathf.RoundToInt(playerCurrentHp + (percentage * playerMaxHp)), 0, playerMaxHp);
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
        SaveSystem.SavePlayer(this, levelSystem, stageManager, quizManager, gemsAndGoldSystem, inventorySystem, timeManager, rewardedAdsButton);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        levelSystem.PlayerLevel = data.playerLevel;
        if(!data.lvlUpRewardsCorrected) //so players can retroactively claim new level up rewards
        {
            gemsAndGoldSystem.AddGems(25 * (levelSystem.PlayerLevel - 1));
            levelSystem.LvlupRewardsCorrected = true;
        }
        else
        {
            levelSystem.LvlupRewardsCorrected = data.lvlUpRewardsCorrected;
        }
        levelSystem.ExperiencePoints = data.expPoints;
        levelSystem.UpdateExpBar();
        gemsAndGoldSystem.AddGold(data.gold);
        gemsAndGoldSystem.AddGems(data.gems);
        timeManager.DailyAdsCompletionDate = data.adsCompletionDate;
        rewardedAdsButton.AdsRemainingForToday = (data.adsCompletionDate is null) ? rewardedAdsButton.AdsPerDay : data.adsRemainingToday;
        inventorySystem.FromSaveStructToMemory(data.itemsSaveStructs);        
        playerMaxHp = data.playerMaxHp;
        playerCurrentHp = playerMaxHp;
        PlayerBaseDmg = data.playerBaseDmg;
        PoisonResist = data.poisonResist;
        BurnResist = data.burnResist;
        ShockResist = data.shockResist;
        FrostResist = data.frostResist;
        classIndex = data.classIndex;

        for (int i = 0; i < data.stageCompleted.Count; i++)
        {
            stageManager.StageCompleted[i] = data.stageCompleted[i];
        }

        if(!(data.stageRewardReceived is null))
        {
            for (int i = 0; i < data.stageRewardReceived.Count; i++)
            {
                stageManager.StageRewardReceived[i] = data.stageRewardReceived[i];
            }
        }
        
        stageManager.RewardPlayerForPastCompletedStage();
        stageManager.ActivateUnlockedStageButtons();
        if(!(data.playerDeckQuestionIds is null))
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
        isCrit = false;
        float rand = UnityEngine.Random.Range(0.00f, 1.00f);
        IsCrit = (rand <= critChance) ? true : false;
        return (IsCrit) ? (playerBaseDmg * dmgMultiplierEffects) * critDamage: playerBaseDmg * dmgMultiplierEffects; //can change this to change how effects change the player damage. Ex: concussion gets calculated on top of everything, or removes 25% from 225% player dmg?
    }

    public void AddConcussion(float baseDmgReductionPercentage)
    {
        dmgMultiplierEffects -= baseDmgReductionPercentage;
    }
    public void RemoveConcussion(float baseDmgReductionPercentage)
    {
        dmgMultiplierEffects += baseDmgReductionPercentage;
    }

    public void AddDamageBoost(float baseDmgIncreasePercentage)
    {
        dmgBoostPercentage = baseDmgIncreasePercentage;
        dmgMultiplierEffects += baseDmgIncreasePercentage;
    }

    public void RemoveDamageBoost()
    {        
        dmgMultiplierEffects -= dmgBoostPercentage;
    }

    public void AddCriticalHitChanceBoost(float critHitChancePercentage)
    {
        critHitChanceBoostPercentage = critHitChancePercentage;
        critChance += critHitChancePercentage;
    }

    public void RemoveCriticalHitChanceBoost()
    {
        critChance -= critHitChanceBoostPercentage;
    }

    public bool IsPlayerFullHealth()
    {
        if(playerCurrentHp >= playerMaxHp)
        {
            return true;
        }
        return false;
    }


}
