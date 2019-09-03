using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    protected AbilitySlot abilitySlot;

    public bool SpecialAbility2SelfCast { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void OnClassEquip()
    {
        print("caca");

    }

    public virtual QuestionQuery CastAbilities(Abilities currentAbility)
    {
        return null;
    }

    public virtual void TriggerAbilities(int numberOfCorrectAnswers)
    {

    }

    public virtual void RemoveShieldUp()
    {

    }

    public virtual string GetAbilityTextForQuizTitle(int abilityIndex)
    {
        return "nothing";
    }

    public virtual string GetAbilityToolTip(int abilityIndex)
    {
        return null;
    }
}