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
    public bool PlayerHasAnswered { get; set; } = false;
    public bool AttackIsSuccessfull { get; set; } = false;
    public float CurrentAbilityDmgModifier { get; set; }
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

        }
        else if ((Abilities)ability == Abilities.SpecialAbility1)
        {
            ActivateTarget(false);
            currentAbility = Abilities.SpecialAbility1;

        }
        else if ((Abilities)ability == Abilities.SpecialAbility2)
        {
            currentAbility = Abilities.SpecialAbility2;

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
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].interactable = false;
        }

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

    public void CastAbilities(Monster target)
    {
        currentTarget = target;
        quizManager.DrawQuestions(player.CharacterClass.CastAbilities(currentAbility));
        StartCoroutine(WaitForPlayerToAnswer());
    }

    private IEnumerator WaitForPlayerToAnswer()
    {
        while (!PlayerHasAnswered)
        {
            yield return new WaitForSeconds(0.1f);
        }
        TriggerAbilities(currentAbility);
    }

    public void TriggerAbilities(Abilities currentAbility)
    {
        player.CharacterClass.TriggerAbilities(quizManager.NumberOfRightAnswers);
        StartCoroutine(DelayForPlayerAttack());
        
    }

    private IEnumerator DelayForPlayerAttack()
    {
        yield return new WaitForSeconds(0.7f);
        if (AttackIsSuccessfull)
        {
            currentTarget.TakeDamage(player.PlayerBaseDmg * CurrentAbilityDmgModifier);
        }
        else
        {
            //popup fail message
        }
        yield return new WaitForSeconds(0.7f);
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<MonsterTurn>());
    }
}
