using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Abilities { BasicAttack, SpecialAbility1, SpecialAbility2, ItemPouch};

public class Player : MonoBehaviour
{
    [SerializeField] int playerMaxHp = 100;
    [SerializeField] float playerCurrentHp;
    [SerializeField] int playerBaseDmg = 25;
    [SerializeField] Text playerHpText;
    [SerializeField] Image playerHpBar;
    [SerializeField] Monster[] monstersSlot;
    [SerializeField] Button[] targetButtons;
    [SerializeField] Button[] abilityButtons;
    [SerializeField] GameObject selectTargetPopUp;
    [SerializeField] CharacterClass characterClass;
    public bool AttackIsSuccessfull = false;
    MonsterManager monsterManager;
    Abilities currentAbility;
    public float currentAbilityDmgModifier;
    Monster currentTarget;
    QuizManager quizManager;
    bool playerHasAnswered = false;

    public bool PlayerHasAnswered { get => playerHasAnswered; set => playerHasAnswered = value; }

    // Start is called before the first frame update
    void Start()
    {
        quizManager = FindObjectOfType<QuizManager>();
        monsterManager = FindObjectOfType<MonsterManager>();
        playerCurrentHp = playerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        playerHpText.text = playerCurrentHp.ToString();
        playerHpBar.fillAmount = playerCurrentHp / playerMaxHp;
    }

    public void ClickAbilities(int ability)
    {
        if((Abilities)ability == Abilities.BasicAttack)
        {
            ActivateTarget(false);
            currentAbility = Abilities.BasicAttack;

        }
        else if((Abilities)ability == Abilities.SpecialAbility1)
        {
            ActivateTarget(false);
            currentAbility = Abilities.SpecialAbility1;

        }
        else if((Abilities)ability == Abilities.SpecialAbility2)
        {
            currentAbility = Abilities.SpecialAbility2;

        }
        else if((Abilities)ability == Abilities.ItemPouch)
        {
            currentAbility = Abilities.ItemPouch; //surement une autre structure pour les items TODO

        }
    }

    public void CastAbilities(Monster target)
    {
        currentTarget = target;      
        quizManager.DrawQuestions(characterClass.CastAbilities(currentAbility));
        StartCoroutine(WaitForPlayerToAnswer());                      
    }

    public void TriggerAbilities(Abilities currentAbility)
    {
        characterClass.TriggerAbilities(quizManager.NumberOfRightAnswers);
        StartCoroutine(DelayForPlayerAttack());
        monsterManager.MonsterTurn();
    }

    private IEnumerator DelayForPlayerAttack()
    {
        yield return new WaitForSeconds(0.7f);
        if (AttackIsSuccessfull)
        {
            currentTarget.TakeDamage(playerBaseDmg * currentAbilityDmgModifier);
        }
        else
        {
            //popup fail message
        }
    }
    private IEnumerator WaitForPlayerToAnswer()
    {
        while (!playerHasAnswered)
        {
            yield return new WaitForSeconds(0.1f);
        }
        TriggerAbilities(currentAbility);
    }

    public void ActivateTarget(bool AreaOfEffect)
    {
        if (!AreaOfEffect)
        {
            for(int i = 0; i < targetButtons.Length; i++)
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

    public void TakeDamage(float amountOfDamage)
    {
        playerCurrentHp -= amountOfDamage;
    }

    public void PlayerTurn()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].interactable = true;
        }
    }   

    public void ReceiveClass(int choosenClassIndex)
    {
        switch (choosenClassIndex)
        {
            case 0:
                gameObject.AddComponent<Warrior>();
                characterClass = GetComponent<Warrior>();
                break;
        }
        characterClass.OnClassEquip();
    }

    public void SetPlayerMaxHpAndBaseDmg(int maxHp, int baseDmg)
    {
        playerMaxHp = maxHp;
        playerCurrentHp = playerMaxHp;
        playerBaseDmg = baseDmg;
    }



}
