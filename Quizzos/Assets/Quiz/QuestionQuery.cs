using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionQuery
{
    public bool inThePool;
    public int amountOfQuestions;
    public QuestionCategory questionCategory;

    public QuestionQuery(bool inThePool, int amountOfCards, QuestionCategory questionCategory)
    {
        this.inThePool = inThePool;
        this.amountOfQuestions = amountOfCards;
        this.questionCategory = questionCategory;
    }
}
