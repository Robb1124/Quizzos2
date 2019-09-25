﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpecialEffects { None, Poison, Burn, Shock, ShieldUp, Concussion};

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
    [SerializeField] Image shieldUpSlot;
    [SerializeField] Image poisonSlot;
    [SerializeField] Image shockSlot;
    [SerializeField] Image burnSlot;
    [SerializeField] Image concussionSlot;
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
    public List<SpecialEffects> CurrentSpecialEffects { get; set; } = new List<SpecialEffects>();
    public bool CurrentEffectIsDone { get; set; } = false;
    public bool ShieldUpActive { get; set; } = false;
    public bool PoisonActive { get; set; } = false;
    public bool ShockActive { get; set; } = false;
    public bool BurnActive { get; set; } = false;
    public bool ConcussionActive { get; set; } = false;


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
            StartCoroutine(WaitForSpecialEffectsCompletion());
        }
        else
        {
            GoToPlayerTurn();
        }
        //if there is some effects to resolve, do so and then minus 1 turn on their duration.
        //Any spell being channeled that needs a question query ?
        
    }

    private IEnumerator WaitForSpecialEffectsCompletion()
    {       
        CurrentEffectIsDone = false;        
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            switch (CurrentSpecialEffects[i])
            {
                case SpecialEffects.ShieldUp:

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
            RemoveSpecialEffects(SpecialEffects.ShieldUp);
        }
    }

    private void GoToPlayerTurn()
    {
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PlayerTurn>());
    }

    public void RemoveSpecialEffects(SpecialEffects specialEffectToRemove)
    {
        CurrentSpecialEffects.Remove(specialEffectToRemove);
        switch (specialEffectToRemove)
        {
            case SpecialEffects.ShieldUp:
                player.CharacterClass.RemoveShieldUp();
                ShieldUpActive = false;
                break;
            case SpecialEffects.Shock:
                ShockActive = false;
                break;
            case SpecialEffects.Burn:
                BurnActive = false;
                break;
            case SpecialEffects.Poison:
                PoisonActive = false;
                break;
            case SpecialEffects.Concussion:
                player.RemoveConcussion(concussionEffectBaseDmgReduction);
                ConcussionActive = false;
                break;
        }            
        RefreshSpecialEffectsSlots();
    }

    public void AddSpecialEffects(SpecialEffects specialEffectToAdd)
    {
        switch (specialEffectToAdd)
        {
            case SpecialEffects.ShieldUp:
                if (!ShieldUpActive)
                {
                    ShieldUpActive = true;
                    CurrentSpecialEffects.Add(SpecialEffects.ShieldUp);
                }
                break;
            case SpecialEffects.Shock:
                if (!ShockActive)
                {
                    ShockActive = true;
                    CurrentSpecialEffects.Add(SpecialEffects.Shock);
                }
                break;
            case SpecialEffects.Burn:
                if (!BurnActive)
                {
                    BurnActive = true;
                    CurrentSpecialEffects.Add(SpecialEffects.Burn);
                }
                break;
            case SpecialEffects.Poison:
                if (!PoisonActive)
                {
                    PoisonActive = true;
                    CurrentSpecialEffects.Add(SpecialEffects.Poison);
                }
                break;
            case SpecialEffects.Concussion:
                if (!ConcussionActive)
                {
                    ConcussionActive = true;
                    player.AddConcussion(concussionEffectBaseDmgReduction);
                    CurrentSpecialEffects.Add(SpecialEffects.Concussion);
                }
                break;
        }
        RefreshSpecialEffectsSlots();
    }

    public void RefreshSpecialEffectsSlots()
    {
        if (CurrentSpecialEffects != null)
        {
            for (int i = 0; i < CurrentSpecialEffects.Count; i++)
            {                
                switch (CurrentSpecialEffects[i])
                {
                    case SpecialEffects.ShieldUp:
                        shieldUpSlot = specialEffectsSlots[i];
                        specialEffectsSlots[i].gameObject.SetActive(true);
                        shieldUpSlot.sprite = shieldUpSprite;                       
                        break;
                    case SpecialEffects.Shock:
                        shockSlot = specialEffectsSlots[i];
                        specialEffectsSlots[i].gameObject.SetActive(true);
                        shockSlot.sprite = shockSprite;
                        ShockActive = true;
                        break;
                    case SpecialEffects.Burn:
                        burnSlot = specialEffectsSlots[i];
                        specialEffectsSlots[i].gameObject.SetActive(true);
                        burnSlot.sprite = burnSprite;
                        BurnActive = true;
                        break;
                    case SpecialEffects.Poison:
                        poisonSlot = specialEffectsSlots[i];
                        specialEffectsSlots[i].gameObject.SetActive(true);
                        poisonSlot.sprite = poisonSprite;
                        PoisonActive = true;
                        break;
                    case SpecialEffects.Concussion:
                        concussionSlot = specialEffectsSlots[i];
                        specialEffectsSlots[i].gameObject.SetActive(true);
                        concussionSlot.sprite = concussionSprite;
                        ConcussionActive = true;
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
        switch (CurrentSpecialEffects[index])
        {
            case SpecialEffects.ShieldUp:
                return shieldUpText;
            case SpecialEffects.Shock:
                return shockText;
            case SpecialEffects.Burn:
                return burnText;
            case SpecialEffects.Poison:
                return poisonText;
            case SpecialEffects.Concussion:
                return concussionText;
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
            RemoveSpecialEffects(CurrentSpecialEffects[0]);
        }
    }
}
