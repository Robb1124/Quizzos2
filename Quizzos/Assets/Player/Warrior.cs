using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : CharacterClass
{
    Player player;
    [SerializeField] int maxHP = 120;
    [SerializeField] int baseDmg = 25;
    [SerializeField] float basicAttackDmgModifier = 1;
    [SerializeField] float chargeAttackDmgModifier = 4;
    Monster currentTarget;
    Abilities currentAbility;
    QuizManager quizManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClassEquip()
    {
        print("cookie");
        player = GetComponent<Player>();
        player.SetPlayerMaxHpAndBaseDmg(maxHP, baseDmg);
        //Draw starting cards corresponding to class
    }

    public override QuestionQuery CastAbilities(Abilities currentAbility)
    {
        this.currentAbility = currentAbility;
        switch (currentAbility)
        {
            case Abilities.BasicAttack:
                return new QuestionQuery(false, 1, QuestionCategory.Any);
            case Abilities.SpecialAbility1:
                return new QuestionQuery(true, 1, QuestionCategory.Sports);
            case Abilities.SpecialAbility2:

                break;
            case Abilities.ItemPouch:
                break;
        }
        return null;
    }

    public override void TriggerAbilities(int numberOfCorrectAnswers)
    {
        switch (currentAbility)
        {
            case Abilities.BasicAttack:
                player.currentAbilityDmgModifier = basicAttackDmgModifier;
                if(numberOfCorrectAnswers == 1)
                {
                    player.AttackIsSuccessfull = true;
                }
                else
                {
                    player.AttackIsSuccessfull = false;
                }
                break;
            case Abilities.SpecialAbility1:
                player.currentAbilityDmgModifier = chargeAttackDmgModifier;
                if (numberOfCorrectAnswers == 1)
                {
                    player.AttackIsSuccessfull = true;
                }
                else
                {
                    player.AttackIsSuccessfull = false;
                }
                break;
            case Abilities.SpecialAbility2:

                break;
            case Abilities.ItemPouch:
                break;
        }
    }



}
