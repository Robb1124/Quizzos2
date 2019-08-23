using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuizManager : MonoBehaviour
{
    Question currentQuestion;
    [SerializeField] GameObject questionPopUp;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI[] choicesText;
    [SerializeField] Player player;
    [SerializeField] Image[] choicesBackgrounds; 
    int numberOfQuestionsRemaining;
    int numberOfRightAnswers;
    bool initialSetupIsDone = false;
    List<String> choices = new List<String>();

    public int NumberOfRightAnswers { get => numberOfRightAnswers; set => numberOfRightAnswers = value; }
    public List<Question> PoolOfKnowledge { get; set; } = new List<Question>();
    public List<Question> PlayerDeckOfQuestions { get; set; } = new List<Question>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawQuestions(QuestionQuery questionQuery)
    {
        if (!initialSetupIsDone)
        {
            InitialSetup(questionQuery.amountOfQuestions);
            initialSetupIsDone = true;
        }
        
        if(questionQuery.inThePool && questionQuery.questionCategory == QuestionCategory.Any)
        {
            currentQuestion = PoolOfKnowledge[UnityEngine.Random.Range(0, PoolOfKnowledge.Count)];

        }
        else if(questionQuery.inThePool && questionQuery.questionCategory != QuestionCategory.Any)
        {
            string requestedCategory = questionQuery.questionCategory.ToString();           
            do
            {
                currentQuestion = PoolOfKnowledge[UnityEngine.Random.Range(0, PoolOfKnowledge.Count)];
            } while ( currentQuestion.category != requestedCategory);
        }
        else if(!questionQuery.inThePool && questionQuery.questionCategory == QuestionCategory.Any)
        {
            currentQuestion = PlayerDeckOfQuestions[UnityEngine.Random.Range(0, PlayerDeckOfQuestions.Count)];
        }
        questionPopUp.SetActive(true);
        questionText.text = currentQuestion.question;
        ShuffleAnswers();
        for(int i = 0; i < choices.Count; i++)
        {
            choicesText[i].text = choices[i];
        }
        numberOfQuestionsRemaining--;

    }

    private void InitialSetup(int numberOfQuestions)
    {
        numberOfQuestionsRemaining = numberOfQuestions;
        NumberOfRightAnswers = 0;
        player.PlayerHasAnswered = false;
    }

    public void ShuffleAnswers()
    {
        string tempString;
        foreach (var answer in currentQuestion.incorrect_answers)
        {
            choices.Add(answer);
        }
        choices.Add(currentQuestion.correct_answer);
       

        for (int i = 0; i < choices.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(0, choices.Count);
            tempString = choices[rnd];
            choices[rnd] = choices[i];
            choices[i] = tempString;
        }
    }

    public void IsThisTheRightAnswer(int answerChosen)
    {
        if(choices[answerChosen] == currentQuestion.correct_answer)
        {
            choicesBackgrounds[answerChosen].color = Color.green;
            StartCoroutine(ShowGoodAnswerDelay());
            //Popup une felicitation
            numberOfRightAnswers++;
              
        }
        else
        {
            choicesBackgrounds[answerChosen].color = Color.red;
            for(int i = 0; i < choices.Count; i++)
            {
                if(choices[i] == currentQuestion.correct_answer)
                {
                    choicesBackgrounds[i].color = Color.green;
                }
            }
            StartCoroutine(ShowGoodAnswerDelay());
            //Popup un mauvaise reponse
        }
    }

    private void CheckQuestionsRemaining()
    {
        if (numberOfQuestionsRemaining == 0)
        {
            player.PlayerHasAnswered = true;
            questionPopUp.SetActive(false);
            initialSetupIsDone = false;
        }
        else
        {
            //demander une autre question
        }
    }

    public IEnumerator ShowGoodAnswerDelay()
    {
        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < choicesBackgrounds.Length; i++)
        {
            choicesBackgrounds[i].color = Color.white;
        }
        int choicesCount = choices.Count;
        for (int i = 0; i < choicesCount; i++)
        {
            choices.Remove(choices[choicesCount - 1 - i]);
        }
        CheckQuestionsRemaining();
    }
}
