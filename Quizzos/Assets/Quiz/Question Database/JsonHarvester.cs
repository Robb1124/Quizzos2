using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonHarvester : MonoBehaviour
{
    [SerializeField] int initialDeckQuestions;
    public Question[] questions;
    string path;
    string jsonString;
    QuizManager quizManager;
    // Start is called before the first frame update
    void Start()
    {
        path = "C:\\Users\\Client\\Quizzos\\Assets\\Quiz\\Question Database" + "\\triviaDemo.json";
        quizManager = FindObjectOfType<QuizManager>();
        HarvestQuestionsFromFileAndSetupQuiz();

    }

    private void HarvestQuestionsFromFileAndSetupQuiz()
    {
        jsonString = File.ReadAllText(path);
        questions = JsonHelper.FromJson<Question>(jsonString);
        Debug.Log(questions.Length);

        for (int i = 0; i < questions.Length; i++)
        {
            quizManager.PoolOfKnowledge.Add(questions[i]);
        }
        for (int i = 0; i < initialDeckQuestions; i++)
        {
            //TODO make it respect deck composition
            quizManager.PlayerDeckOfQuestions.Add(questions[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
[System.Serializable]
public class Question
{

    public string category;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}