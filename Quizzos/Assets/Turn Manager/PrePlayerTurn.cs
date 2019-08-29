using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpecialEffects { ShieldUp };

public class PrePlayerTurn : TurnState
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] QuizManager quizManager;
    [SerializeField] Player player;
    [SerializeField] Image[] specialEffectsSlots;
    [SerializeField] Sprite shieldUpSprite;
    [SerializeField] Image shieldUpSlot;
    public List<SpecialEffects> CurrentSpecialEffects { get; set; } = new List<SpecialEffects>();
    public bool CurrentEffectIsDone { get; set; } = false;
    public bool ShieldUpActive { get; set; } = false;

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
        CurrentEffectIsDone = false;
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
        if (ShieldUpActive)
        {
            
        }
        for (int i = 0; i < CurrentSpecialEffects.Count; i++)
        {
            switch (CurrentSpecialEffects[i])
            {
                case SpecialEffects.ShieldUp:
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

    public void DoneWithTheQuiz(int numberOfRightAnswers)
    {
        if (numberOfRightAnswers == 1)
        {
            CurrentEffectIsDone = true;
        }
        else
        {
            CurrentEffectIsDone = true;
            ShieldUpActive = false;
            shieldUpSlot.gameObject.SetActive(false);
            player.CharacterClass.RemoveShieldUp();
        }
    }

    private void GoToPlayerTurn()
    {
        turnManager.ChangeTurnState(turnManager.GetComponentInChildren<PlayerTurn>());
    }

    public void RefreshSpecialEffectsSlots()
    {
        if (CurrentSpecialEffects != null)
        {
            for (int i = 0; i < CurrentSpecialEffects.Count; i++)
            {
                specialEffectsSlots[i].gameObject.SetActive(false);
                switch (CurrentSpecialEffects[i])
                {
                    case SpecialEffects.ShieldUp:
                        shieldUpSlot = specialEffectsSlots[i];
                        shieldUpSlot.gameObject.SetActive(true);
                        shieldUpSlot.sprite = shieldUpSprite;                       
                        break;
                }
            }
        }
    }

}
