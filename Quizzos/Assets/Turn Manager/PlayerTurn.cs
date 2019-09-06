using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : TurnState
{
    [SerializeField] Button[] abilityButtons;
    [SerializeField] AbilitySlot abilitySlot;
    [SerializeField] Button[] targetButtons;
    [SerializeField] GameObject selectTargetPopUp;
    [SerializeField] Monster[] monstersSlot;
    [SerializeField] Player player;
    [SerializeField] QuizManager quizManager;
    [SerializeField] MonsterManager monsterManager;
    [SerializeField] TurnManager turnManager;

    Abilities currentAbility;
    Monster currentTarget;
    public bool AttackIsSuccessfull { get; set; } = false;
    public bool isAnAttack { get; set; } = false;
    public float CurrentAbilityDmgModifier { get; set; }
    public Monster CurrentTarget { get => currentTarget; set => currentTarget = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStateChange()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].interactable = true;
        }
        if (!abilitySlot.SpecialAbility1IsReady)
        {
            abilityButtons[1].interactable = false;
        }
        if (!abilitySlot.SpecialAbility2IsReady)
        {
            abilityButtons[2].interactable = false;
        }
    }

    public void ClickAbilities(int ability)
    {
        if ((Abilities)ability == Abilities.BasicAttack)
        {
            ActivateTarget(false);
            currentAbility = Abilities.BasicAttack;
            quizManager.AbilityText.text = player.CharacterClass.GetAbilityTextForQuizTitle(0);

        }
        else if ((Abilities)ability == Abilities.SpecialAbility1)
        {
            ActivateTarget(false);
            currentAbility = Abilities.SpecialAbility1;
            quizManager.AbilityText.text = player.CharacterClass.GetAbilityTextForQuizTitle(1);
        }
        else if ((Abilities)ability == Abilities.SpecialAbility2)
        {
            currentAbility = Abilities.SpecialAbility2;
            quizManager.AbilityText.text = player.CharacterClass.GetAbilityTextForQuizTitle(2);
            if (player.CharacterClass.SpecialAbility2SelfCast)
            {
                DesactivateAbilityButtons();
                quizManager.DrawQuestions(player.CharacterClass.CastAbilities(currentAbility)); //self cast doesnt need to go throught targeting.
            }
        }
        else if ((Abilities)ability == Abilities.ItemPouch)
        {
            currentAbility = Abilities.ItemPouch; //surement une autre structure pour les items TODO

        }
    }

    public void ActivateTarget(bool AreaOfEffect)
    {
        if (!AreaOfEffect)
        {
            for (int i = 0; i < targetButtons.Length; i++)
            {
                targetButtons[i].interactable = true;
            }
            //Border qui apparait autour des cibles potentiels
            selectTargetPopUp.SetActive(true);
        }
        else
        {
            //Borders qui clignote pour montrer que tout le monde est toucher
            //Area of effect popup (change le texte)
        }
    }

    public void ConfirmTarget(int monsterNumber)
    {
        //Desactivate Enemy Targets button
        for (int i = 0; i < targetButtons.Length; i++)
        {
            targetButtons[i].interactable = false;
            selectTargetPopUp.SetActive(false);
        }
        //Deactivate Ability buttons until next turn
        DesactivateAbilityButtons();

        switch (monsterNumber)
        {
            case 0:
                CastAbilities(monstersSlot[0]);
                break;
            case 1:
                CastAbilities(monstersSlot[1]);
                break;
            case 2:
                CastAbilities(monstersSlot[2]);
                break;
        }
    }

    private void DesactivateAbilityButtons()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].interactable = false;
        }
    }

    public void CastAbilities(Monster target)
    {
        CurrentTarget = target;
        quizManager.DrawQuestions(player.CharacterClass.CastAbilities(currentAbility));
    }

    

    public void TriggerAbilities()
    {
        player.CharacterClass.TriggerAbilities(quizManager.NumberOfRightAnswers);
        StartCoroutine(DelayForPlayerAttack());        
    }

    private IEnumerator DelayForPlayerAttack()
    {
        yield return new WaitForSeconds(0.7f);
        if (AttackIsSuccessfull && isAnAttack)
        {
            CurrentTarget.TakeDamage(player.PlayerBaseDmg * CurrentAbilityDmgModifier);
        }
        else if (AttackIsSuccessfull && !isAnAttack)
        {
            //special sound/animation for the successull ability.
        }
        else
        {
            //popup fail message
        }
        yield return new WaitForSeconds(0.7f);
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<MonsterTurn>());
    }

    public void PassiveProc(int target, float damageModifier)
    {
        if(target == 4)
        {
            int randomTarget;
            do
            {
                randomTarget = Random.Range(0, monstersSlot.Length);
            } while (!monstersSlot[randomTarget].isActiveAndEnabled);
            StartCoroutine(DelayForPassiveProc(randomTarget, damageModifier));
        }
    }

    public IEnumerator DelayForPassiveProc(int target, float damageModifier)
    {
        yield return new WaitForSeconds(1.5f);
        monstersSlot[target].TakeDamage(player.PlayerBaseDmg * damageModifier);
    }
}
