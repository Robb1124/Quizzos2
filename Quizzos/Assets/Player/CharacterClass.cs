using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    protected AbilitySlot abilitySlot;
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
}