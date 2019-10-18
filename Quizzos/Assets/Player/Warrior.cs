using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Warrior : CharacterClass
{
    Player player;
    PlayerTurn playerTurnState;
    PrePlayerTurn prePlayerTurnState;
    [SerializeField] int maxHP = 120;
    [SerializeField] int baseDmg = 25;
    [SerializeField] float poisonResist;
    [SerializeField] float burnResist;
    [SerializeField] float shockResist;
    [SerializeField] float frostResist;
    [SerializeField] float concussionResist;
    [SerializeField] float basicAttackDmgModifier = 1;
    [SerializeField] int chargeAttackCooldown = 3;
    [SerializeField] float chargeAttackDmgModifier = 4;
    [SerializeField] int shieldUpCooldown = 4;
    [SerializeField] float shieldUpDmgModifier = 0;
    [SerializeField] bool shieldUpActive;
    [SerializeField] float shieldUpDmgReduction = 0.95f;
    [SerializeField] float passiveAttackDmgModifier = 1;
    [SerializeField] int numberOfBadAnswersForPassiveProc = 3;
    [SerializeField] AudioClip basicAttackSFX;
    [SerializeField] AudioClip chargeAttackSFX;
    [SerializeField] AudioClip shieldUpSFX;
    [TextArea(3,6)]
    [SerializeField] string[] abilityTextsForTooltip;
    [SerializeField] TextMeshProUGUI passiveText;
    [SerializeField] string[] abilityTextForQuestionPopUp = { "Basic Attack", "Charge", "Shield Up" };
    [SerializeField] QuestionQuery basicAttackQuestionQuery = new QuestionQuery(0.75f, 1, QuestionCategory.Any); //Do not forget to change skill description when you modify skills.
    [SerializeField] QuestionQuery chargeAttackQuestionQuery = new QuestionQuery(0.5f, 1, QuestionCategory.Sports);
    [SerializeField] QuestionCategory mainQuestionCategory;
    AudioSource audioSource;
    Monster currentTarget;
    [SerializeField] QuizManager quizManager;

    public int ChargeAttackCooldown { get => chargeAttackCooldown; set => chargeAttackCooldown = value; }
    public int ShieldUpCooldown { get => shieldUpCooldown; set => shieldUpCooldown = value; }
    public QuestionQuery ShieldUpQuestionQuery { get; } = new QuestionQuery(1f, 1, QuestionCategory.Any);
    public float ShieldUpDmgReduction { get => shieldUpDmgReduction; set => shieldUpDmgReduction = value; }
    public QuestionCategory MainQuestionCategory { get => mainQuestionCategory; set => mainQuestionCategory = value; }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerTurnState = FindObjectOfType<PlayerTurn>();
        quizManager.onWrongAnswers += OnWrongAnswers;
        player = GetComponent<Player>();
        SpecialAbility2SelfCast = true;
        player.SetPlayerMaxHpAndBaseDmgAndResists(maxHP, baseDmg, poisonResist, burnResist, shockResist, frostResist, concussionResist);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClassEquip()
    {
        
        //Draw starting cards corresponding to class
    }

    private void OnWrongAnswers(int badAnswersInCombat)
    {        
        if(badAnswersInCombat % 3 == 0 && badAnswersInCombat > 0)
        {
            playerTurnState.PassiveProc(4, 1);
        }
    }

    public override QuestionQuery CastAbilities(Abilities currentAbility)
    {
        this.currentAbility = currentAbility;
        switch (currentAbility)
        {
            case Abilities.BasicAttack:
                return basicAttackQuestionQuery;
            case Abilities.SpecialAbility1:
                return chargeAttackQuestionQuery;
            case Abilities.SpecialAbility2:
                return ShieldUpQuestionQuery;
            case Abilities.ItemPouch:
                break;
        }
        return null;
    }

    public override void TriggerAbilities(int numberOfCorrectAnswers)
    {
        abilitySlot = FindObjectOfType<AbilitySlot>();
        prePlayerTurnState = FindObjectOfType<PrePlayerTurn>();
        switch (currentAbility)
        {
            case Abilities.BasicAttack:
                playerTurnState.CurrentAbilityDmgModifier = basicAttackDmgModifier;
                if(numberOfCorrectAnswers == 1)
                {
                    playerTurnState.AttackIsSuccessfull = true;
                    playerTurnState.isAnAttack = true;
                }
                else
                {
                    playerTurnState.AttackIsSuccessfull = false;
                }
                break;
            case Abilities.SpecialAbility1:
                playerTurnState.CurrentAbilityDmgModifier = chargeAttackDmgModifier;
                abilitySlot.ActivateSpecialAbilityCooldown(1, chargeAttackCooldown);
                if (numberOfCorrectAnswers == 1)
                {
                    playerTurnState.AttackIsSuccessfull = true;
                    playerTurnState.isAnAttack = true;
                    playerTurnState.CurrentTarget.StunEnemy(); //trigger stuns effect on the target
                }
                else
                {
                    playerTurnState.AttackIsSuccessfull = false;
                }
                break;
            case Abilities.SpecialAbility2:
                playerTurnState.CurrentAbilityDmgModifier = shieldUpDmgModifier;
                abilitySlot.ActivateSpecialAbilityCooldown(2, shieldUpCooldown);
                if (numberOfCorrectAnswers == 1)
                {
                    playerTurnState.AttackIsSuccessfull = true;
                    playerTurnState.isAnAttack = false;
                    prePlayerTurnState.AddSpecialEffects(SpecialEffects.ShieldUp);
                    PlaySFX();
                    player.DmgReduction += shieldUpDmgReduction;
                }
                else
                {
                    playerTurnState.AttackIsSuccessfull = false;
                }
                break;
            case Abilities.ItemPouch:
                break;
        }
    }

    public override void RemoveShieldUp()
    {
        player.DmgReduction -= shieldUpDmgReduction;
    }

    public override string GetAbilityTextForQuizTitle(int abilityIndex)
    {
        return abilityTextForQuestionPopUp[abilityIndex];
    }

    public override string GetAbilityToolTip(int abilityIndex)
    {
        return abilityTextsForTooltip[abilityIndex];
    }

    public override QuestionCategory GetMainQuestionCategory()
    {
        return mainQuestionCategory;
    }

    public override void PlaySFX()
    {
        
        switch (currentAbility)
        {
            case Abilities.BasicAttack:
                audioSource.clip = basicAttackSFX;
                audioSource.Play();
                break;
            case Abilities.SpecialAbility1:
                audioSource.clip = chargeAttackSFX;
                audioSource.Play();
                break;
            case Abilities.SpecialAbility2:
                audioSource.clip = shieldUpSFX;
                audioSource.Play();
                break;
        }
              
    }

    public void ActivateCooldownOnCharge()
    {
        abilitySlot.ActivateSpecialAbilityCooldown(1, chargeAttackCooldown);
    }

    
}
