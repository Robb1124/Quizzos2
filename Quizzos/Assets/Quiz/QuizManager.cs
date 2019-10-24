using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuizManager : MonoBehaviour
{
    Question currentQuestion;
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameObject questionPopUp;
    [SerializeField] GameObject poisonMask;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI[] choicesText;
    [SerializeField] Player player;
    [SerializeField] Image[] choicesBackgrounds;
    [SerializeField] PlayerTurn playerTurnState;
    [SerializeField] PrePlayerTurn prePlayerTurn;
    [SerializeField] TextMeshProUGUI abilityText;
    [SerializeField] TextMeshProUGUI poolOrDeckText;
    [SerializeField] string poolText;
    [SerializeField] string deckText;
    [SerializeField] TextMeshProUGUI categoryText;
    [SerializeField] Image timerFillImage;
    [SerializeField] float timeToAnswer;
    [SerializeField] Color[] timerBarColors;
    [SerializeField] AudioClip poisonClip;
    AudioSource audioSource;
    float timer;
    bool stopTimer = false;
    bool ranOutOfTime = false;
    int previousQuestionId = -1;
    bool playerHasAnswered = false;
    int numberOfQuestionsRemaining;
    bool initialSetupIsDone = false;
    List<String> choices = new List<String>();

    public int NumberOfRightAnswers { get; set; }
    public List<Question> PoolOfKnowledge { get; set; } = new List<Question>();
    public List<Question> PlayerDeckOfQuestions { get; set; } = new List<Question>();
    public int BadAnswersInCombat { get; set; } = 0;
    public TextMeshProUGUI AbilityText { get => abilityText; set => abilityText = value; }
    public int[] PlayerDeckQuestionIds { get; set; } 


    public delegate void OnWrongAnswers(int badAnswersInCombat); // declare new delegate type
    public event OnWrongAnswers onWrongAnswers; // instantiate an observer set

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawQuestions(QuestionQuery questionQuery)
    {       
        float rand = UnityEngine.Random.Range(0.00f, 1.00f);
        bool inThePool = (rand <= questionQuery.percentageToDrawFromDeck) ? false : true;
        
        if (!initialSetupIsDone)
        {
            InitialSetup(questionQuery.amountOfQuestions);
            initialSetupIsDone = true;
        }
        
        if(inThePool && questionQuery.questionCategory == QuestionCategory.Any)
        {
            do
            {
                currentQuestion = PoolOfKnowledge[UnityEngine.Random.Range(0, PoolOfKnowledge.Count)];
            } while (previousQuestionId == currentQuestion.id);

        }
        else if(inThePool && questionQuery.questionCategory != QuestionCategory.Any)
        {
            string requestedCategory = questionQuery.questionCategory.ToString();           
            do
            {
                currentQuestion = PoolOfKnowledge[UnityEngine.Random.Range(0, PoolOfKnowledge.Count)];
            } while ( currentQuestion.category != requestedCategory || previousQuestionId == currentQuestion.id);
        }
        else if(!inThePool && questionQuery.questionCategory == QuestionCategory.Any)
        {
            do
            {
                currentQuestion = PlayerDeckOfQuestions[UnityEngine.Random.Range(0, PlayerDeckOfQuestions.Count)];
            } while (previousQuestionId == currentQuestion.id);
        }
        else if(!inThePool && questionQuery.questionCategory != QuestionCategory.Any)
        {
            string requestedCategory = questionQuery.questionCategory.ToString();
            do
            {
                currentQuestion = PlayerDeckOfQuestions[UnityEngine.Random.Range(0, PlayerDeckOfQuestions.Count)];
            } while (currentQuestion.category != requestedCategory || previousQuestionId == currentQuestion.id);
        }

        questionPopUp.SetActive(true);
        if (prePlayerTurn.PoisonActive)
        {
            poisonMask.SetActive(true);
            audioSource.clip = poisonClip;
            audioSource.Play();
        }
        categoryText.text = currentQuestion.category;
        poolOrDeckText.text = (inThePool) ? poolText : deckText;
        questionText.text = currentQuestion.question;
        previousQuestionId = currentQuestion.id;
        ShuffleAnswers();
        for(int i = 0; i < choices.Count; i++)
        {
            choicesText[i].text = choices[i];
        }
        numberOfQuestionsRemaining--;
        StartCoroutine(WaitForPlayerToAnswer());
        StartCoroutine(WaitForTriviaResolution());

    }

    private IEnumerator WaitForPlayerToAnswer()
    {
        ranOutOfTime = false;
        stopTimer = false;
        timer = timeToAnswer;
        timerFillImage.fillAmount = timer / timeToAnswer;
        while(timer > 0 && !stopTimer)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            timerFillImage.fillAmount = timer / timeToAnswer;
            timerFillImage.color = Color.Lerp(timerBarColors[1], timerBarColors[0], (timer / timeToAnswer));
        }
        if (timer <= 0)
        {
            ranOutOfTime = true;
            IsThisTheRightAnswer(0);
        }
    }
    private IEnumerator WaitForTriviaResolution()
    {
        while (!playerHasAnswered)
        {
            yield return new WaitForEndOfFrame();
        }
        if (turnManager.TurnState is PlayerTurn)
        {
            playerTurnState.TriggerAbilities();
        }
        else if(turnManager.TurnState is PrePlayerTurn)
        {
            prePlayerTurn.DoneWithTheQuiz(NumberOfRightAnswers);
        }        
    }

    private void InitialSetup(int numberOfQuestions)
    {
        numberOfQuestionsRemaining = numberOfQuestions;
        NumberOfRightAnswers = 0;
        playerHasAnswered = false;
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
        stopTimer = true;
        poisonMask.SetActive(false);
        if (choices[answerChosen] == currentQuestion.correct_answer && !ranOutOfTime)
        {
            choicesBackgrounds[answerChosen].color = Color.green;
            //Popup une felicitation
            NumberOfRightAnswers++;             
        }
        else
        {
            BadAnswersInCombat++;
            onWrongAnswers(BadAnswersInCombat);
            if (!ranOutOfTime)
            {
                choicesBackgrounds[answerChosen].color = Color.red;
            }
            else
            {
                for (int i = 0; i < choices.Count; i++)
                {
                    choicesBackgrounds[i].color = Color.red;
                }
            }
            for (int i = 0; i < choices.Count; i++)
            {
                if(choices[i] == currentQuestion.correct_answer)
                {
                    choicesBackgrounds[i].color = Color.green;
                }
            }
        }        
        StartCoroutine(ShowGoodAnswerDelay());
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

    private void CheckQuestionsRemaining()
    {
        if (numberOfQuestionsRemaining == 0)
        {
            playerHasAnswered = true;
            questionPopUp.SetActive(false);
            initialSetupIsDone = false;
        }
        else
        {
            //demander une autre question
        }
    }

    public void RefreshQuestionsIdsForSave()
    {
        PlayerDeckQuestionIds = new int[PlayerDeckOfQuestions.Count];
        for (int i = 0; i < PlayerDeckOfQuestions.Count; i++)
        {
            PlayerDeckQuestionIds[i] = PlayerDeckOfQuestions[i].id;
        }
    }

    public void RecreatePlayerDeckOnLoad()
    {
        JsonHarvester jsonHarvester = GetComponent<JsonHarvester>();
        for (int i = 0; i < PlayerDeckQuestionIds.Length; i++)
        {
            PlayerDeckOfQuestions.Add(jsonHarvester.QuestionByIndex(PlayerDeckQuestionIds[i]));
        }
    }
}
