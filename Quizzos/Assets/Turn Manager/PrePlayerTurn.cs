﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpecialEffectsType { None, Poison, Burn, Shock, ShieldUp, Concussion, DamageBoost};

[System.Serializable]
public class SpecialEffect
{
    [SerializeField]
    SpecialEffectsType effectType;
    [SerializeField]
    int turnDuration;
    [SerializeField]
    float percentageAffectedBy;

    public SpecialEffectsType EffectType { get => effectType; set => effectType = value; }
    public int TurnDuration { get => turnDuration; set => turnDuration = value; }
    public float PercentageAffectedBy { get => percentageAffectedBy; set => percentageAffectedBy = value; }

    public SpecialEffect(SpecialEffectsType effectType, int turnDuration = 1, float percentageAffectedBy = 0)
    {
        this.EffectType = effectType;
        this.TurnDuration = turnDuration;
        this.PercentageAffectedBy = percentageAffectedBy;
    }
}

public class PrePlayerTurn : TurnState
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] QuizManager quizManager;
    [SerializeField] Player player;
    [SerializeField] Image[] specialEffectsSlots;
    [SerializeField] Sprite shieldUpSprite;
    [SerializeField] Sprite poisonSprite;
    [SerializeField] Sprite shockSprite;
    [SerializeField] Sprite burnSprite;
    [SerializeField] Sprite concussionSprite;
    [SerializeField] Sprite damageBoostSprite;
    [SerializeField] Image shieldUpSlot;
    [SerializeField] Image poisonSlot;
    [SerializeField] Image shockSlot;
    [SerializeField] Image burnSlot;
    [SerializeField] Image concussionSlot;
    [SerializeField] Image damageBoostSlot;
    [SerializeField] float concussionEffectBaseDmgReduction;
    [SerializeField] float burnEffectDamagePercentage;
    [TextArea(2, 5)]
    [SerializeField] string shieldUpText;
    [TextArea(2, 5)]
    [SerializeField] string poisonText;
    [TextArea(2, 5)]
    [SerializeField] string shockText;
    [TextArea(2, 5)]
    [SerializeField] string burnText;
    [TextArea(2, 5)]
    [SerializeField] string concussionText;
    [TextArea(2, 5)]
    [SerializeField] string damageBoostText;
    float damageBoostPercentageMemory;
    public List<SpecialEffect> CurrentSpecialEffects { get; set; } = new List<SpecialEffect>();
    public bool CurrentEffectIsDone { get; set; } = false;
    public bool ShieldUpActive { get; set; } = false;
    public bool PoisonActive { get; set; } = false;
    public bool ShockActive { get; set; } = false;
    public bool BurnActive { get; set; } = false;
    public bool ConcussionActive { get; set; } = false;
    public bool DamageBoostActive { get; set; } = false;


    // Start is called before the first frame update
    void Start()
    {
        quizManager.onWrongAnswers += OnWrongAnswers;
        player.onPlayerDeath += OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStateChange()
    {
        if(CurrentSpecialEffects != null)
        {
            ReduceEffectsDurationAndRemoveExpired();
            StartCoroutine(WaitForSpecialEffectsCompletion());

        }
        else
        {
            GoToPlayerTurn();
        }
        //if there is some effects to resolve, do so and then minus 1 turn on their duration.
        //Any spell being channeled that needs a question query ?
        
    }

    void ReduceEffectsDurationAndRemoveExpired()
    {
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            if(CurrentSpecialEffects[i].EffectType != SpecialEffectsType.ShieldUp)
            {
                CurrentSpecialEffects[i].TurnDuration--;
                if(CurrentSpecialEffects[i].TurnDuration <= 0)
                {
                    RemoveSpecialEffects(CurrentSpecialEffects[i].EffectType);
                    i--;
                }
            }
        }
    }
    private IEnumerator WaitForSpecialEffectsCompletion()
    {       
        CurrentEffectIsDone = false;        
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            switch (CurrentSpecialEffects[i].EffectType)
            {
                case SpecialEffectsType.ShieldUp:

                    quizManager.AbilityText.text = player.CharacterClass.GetAbilityTextForQuizTitle(2);
                    quizManager.DrawQuestions(player.GetComponent<Warrior>().ShieldUpQuestionQuery);
                    shieldUpSlot = specialEffectsSlots[i];
                    shieldUpSlot.gameObject.SetActive(true);
                    shieldUpSlot.sprite = shieldUpSprite;
                    while (!CurrentEffectIsDone)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
            }
        }
        
        
        //RESOLVE ALL OTHER EFFECTS UP THERE.
        yield return new WaitForSeconds(0.1f);
        GoToPlayerTurn();
    }

    public void DoneWithTheQuiz(int numberOfRightAnswers) //refactor to specify which special effect the quiz is about
    {
        if (numberOfRightAnswers == 1)
        {
            CurrentEffectIsDone = true;
            player.CharacterClass.currentAbility = Abilities.SpecialAbility2;
            player.CharacterClass.PlaySFX();
        }
        else
        {
            CurrentEffectIsDone = true;
            RemoveSpecialEffects(SpecialEffectsType.ShieldUp);
        }
    }

    private void GoToPlayerTurn()
    {
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PlayerTurn>());
    }

    public void RemoveSpecialEffects(SpecialEffectsType specialEffectTypeToRemove)
    {
        bool isThereSuchAnEffect = false;
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            if(CurrentSpecialEffects[i].EffectType == specialEffectTypeToRemove)
            {
                isThereSuchAnEffect = true;
                CurrentSpecialEffects.RemoveAt(i);
                break;
            }
        }
        if (isThereSuchAnEffect)
        {
            switch (specialEffectTypeToRemove)
            {
                case SpecialEffectsType.ShieldUp:
                    player.CharacterClass.RemoveShieldUp();
                    ShieldUpActive = false;
                    break;
                case SpecialEffectsType.Shock:
                    ShockActive = false;
                    break;
                case SpecialEffectsType.Burn:
                    BurnActive = false;
                    break;
                case SpecialEffectsType.Poison:
                    PoisonActive = false;
                    break;
                case SpecialEffectsType.Concussion:
                    player.RemoveConcussion(concussionEffectBaseDmgReduction);
                    ConcussionActive = false;
                    break;
                case SpecialEffectsType.DamageBoost:
                    player.RemoveDamageBoost();
                    DamageBoostActive = false;
                    break;
            }
            RefreshSpecialEffectsSlots();
        }        
    }

    public void AddSpecialEffects(SpecialEffectsType specialEffectToAdd, float percentageAffectedBy = 0, int turnDuration = 0)
    {
        bool added = false;
        int bonusTurn = 0; //Bonus turn for some effects from monster that immediately lose a turn since the method is called right after their turn.
        switch (specialEffectToAdd)
        {
            case SpecialEffectsType.ShieldUp: //Channeling effect, no turn duration taken into account
                if (!ShieldUpActive)
                {
                    ShieldUpActive = true;
                    CurrentSpecialEffects.Add(new SpecialEffect(SpecialEffectsType.ShieldUp, turnDuration));
                }
                break;
            case SpecialEffectsType.Shock:                
                if (ShockActive)
                {
                    added = true;
                }
                else
                {
                    ShockActive = true;
                    bonusTurn = 1;
                }
                break;
            case SpecialEffectsType.Burn:
                if (BurnActive)
                {
                    added = true;
                }
                else
                {                    
                    BurnActive = true;
                    bonusTurn = 1;
                }
                break;
            case SpecialEffectsType.Poison:
                if (PoisonActive)
                {
                    added = true;
                }
                else
                {
                    PoisonActive = true;
                    bonusTurn = 1;
                }
                break;
            case SpecialEffectsType.Concussion:
                if (ConcussionActive)
                {
                    added = true;
                }
                else
                {
                    ConcussionActive = true;
                    bonusTurn = 1;
                }
                break;
            case SpecialEffectsType.DamageBoost:
                if (DamageBoostActive)
                {                    
                    player.RemoveDamageBoost();
                    player.AddDamageBoost(percentageAffectedBy);
                    added = true;
                }
                else
                {
                    player.AddDamageBoost(percentageAffectedBy);
                    DamageBoostActive = true;
                }
                damageBoostPercentageMemory = percentageAffectedBy;
                break;
        }
        if (!added)
        {
            CurrentSpecialEffects.Add(new SpecialEffect(specialEffectToAdd, turnDuration + bonusTurn));
        }
        else
        {
            FindSpecificEffect(specialEffectToAdd).TurnDuration = turnDuration;
        }
        RefreshSpecialEffectsSlots();
    }

    public void RefreshSpecialEffectsSlots()
    {
        if (CurrentSpecialEffects != null)
        {
            for (int i = 0; i < CurrentSpecialEffects.Count; i++)
            {
                specialEffectsSlots[i].gameObject.SetActive(true);
                switch (CurrentSpecialEffects[i].EffectType)
                {
                    case SpecialEffectsType.ShieldUp:
                        shieldUpSlot = specialEffectsSlots[i];
                        shieldUpSlot.sprite = shieldUpSprite;
                        ShieldUpActive = true;
                        break;
                    case SpecialEffectsType.Shock:
                        shockSlot = specialEffectsSlots[i];
                        shockSlot.sprite = shockSprite;
                        ShockActive = true;
                        break;
                    case SpecialEffectsType.Burn:
                        burnSlot = specialEffectsSlots[i];
                        burnSlot.sprite = burnSprite;
                        BurnActive = true;
                        break;
                    case SpecialEffectsType.Poison:
                        poisonSlot = specialEffectsSlots[i];
                        poisonSlot.sprite = poisonSprite;
                        PoisonActive = true;
                        break;
                    case SpecialEffectsType.Concussion:
                        concussionSlot = specialEffectsSlots[i];
                        concussionSlot.sprite = concussionSprite;
                        ConcussionActive = true;
                        break;
                    case SpecialEffectsType.DamageBoost:
                        damageBoostSlot = specialEffectsSlots[i];
                        damageBoostSlot.sprite = damageBoostSprite;
                        DamageBoostActive = true;
                        break;

                }
            }            
        }
        for (int i = 0; i < specialEffectsSlots.Length; i++)
        {
            if (i >= CurrentSpecialEffects.Count)
            {
                specialEffectsSlots[i].gameObject.SetActive(false);
            }
        }

    }

    private void OnWrongAnswers(int badAnswersInCombat)
    {
        if (BurnActive)
        {
            player.TakeBurnDamage(burnEffectDamagePercentage);
        }
    }

    public string GetSpecialEffectText(int index)
    {
        int turnRemaining = CurrentSpecialEffects[index].TurnDuration;
        switch (CurrentSpecialEffects[index].EffectType)
        {
            case SpecialEffectsType.ShieldUp:
                return shieldUpText;
            case SpecialEffectsType.Shock:
                return string.Format(shockText, turnRemaining);
            case SpecialEffectsType.Burn:
                return string.Format(burnText, turnRemaining);
            case SpecialEffectsType.Poison:
                return string.Format(poisonText, turnRemaining);
            case SpecialEffectsType.Concussion:
                return string.Format(concussionText, turnRemaining);
            case SpecialEffectsType.DamageBoost:
                return string.Format(damageBoostText, (damageBoostPercentageMemory * 100), turnRemaining);
        }
        return null;
    }

    private void OnPlayerDeath()
    {
        RemoveAllSpecialEffects();       
    }

    public void RemoveAllSpecialEffects()
    {
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            RemoveSpecialEffects(CurrentSpecialEffects[0].EffectType);
        }
    }

    SpecialEffect FindSpecificEffect(SpecialEffectsType effectType)
    {
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            if(CurrentSpecialEffects[i].EffectType == effectType)
            {
                return CurrentSpecialEffects[i];
            }
        }
        return null;
    }
}
