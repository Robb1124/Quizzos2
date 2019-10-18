using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionQuery
{
    public float percentageToDrawFromDeck;
    public int amountOfQuestions;
    public QuestionCategory questionCategory;

    public QuestionQuery(float percentageToDrawFromDeck, int amountOfCards, QuestionCategory questionCategory)
    {
        this.percentageToDrawFromDeck = percentageToDrawFromDeck;
        this.amountOfQuestions = amountOfCards;
        this.questionCategory = questionCategory;
    }
}
