using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonHarvester : MonoBehaviour
{
    [SerializeField] int initialDeckQuestions;
    [SerializeField] float mainCategoryPercentage;
    public Question[] questions;
    string path;
    string jsonString;
    QuizManager quizManager;
    TextAsset file;
    string fileContent;
    int SECURITY_THRESHOLD = 2000;
    // Start is called before the first frame update
    void Start()
    {
        file = Resources.Load("triviaDemo") as TextAsset;
        fileContent = file.ToString();
        //path = "C:\\Users\\Client\\Desktop\\Version control\\Quizzos\\Quizzos\\Assets\\Quiz\\Question Database" + "\\triviaDemo.json";
        quizManager = FindObjectOfType<QuizManager>();
        HarvestQuestionsFromFileAndSetupPool();
    }

    private void HarvestQuestionsFromFileAndSetupPool()
    {
        //jsonString = File.ReadAllText(path);
        questions = JsonHelper.FromJson<Question>(fileContent);
        Debug.Log(questions.Length);

        for (int i = 0; i < questions.Length; i++)
        {
            quizManager.PoolOfKnowledge.Add(questions[i]);
        }
        //What I have used to modify to json file to my needs (add id in this case)
        //for (int i = 0; i < questions.Length; i++)
        //{
        //    questions[i].id = i;
        //}
        //fileContent = JsonHelper.ToJson<Question>(questions);

        //File.WriteAllText("C:\\Users\\Client\\Desktop\\Version control\\Quizzos\\Quizzos\\Assets\\Resources\\triviaDemo.json", fileContent);
    }    

    public void OnClickChooseANewClass()
    {
        AddQuestionsToTheDeck(initialDeckQuestions);
    }

    public void OnLoadCharacter()
    {

    }

    public void AddQuestionsToTheDeck(int questionsAmount)
    {
        int questionsRemaining = questionsAmount;
        float mainCategoryQuestions = questionsAmount * mainCategoryPercentage;
        int mainCategoryQuestionsRounded = (int)mainCategoryQuestions;
        int questionIndex;
        int securityIndex = 0; //in case the player already have every questions in the pool or in a specific category;
        switch (FindObjectOfType<Player>().CharacterClass.GetMainQuestionCategory())
        {
            case QuestionCategory.Sports:
                for (int i = 0; i < mainCategoryQuestionsRounded; i++)
                {                    
                    do
                    {
                        questionIndex = Random.Range(0, questions.Length);
                        securityIndex++;
                        if(securityIndex > SECURITY_THRESHOLD)
                        {
                            Debug.Log("No more SPORTS questions to draw from");
                            goto AnyQuestions;
                        }
                    } while (questions[questionIndex].category != QuestionCategory.Sports.ToString() || IsThisQuestionADuplicate(questionIndex));
                    quizManager.PlayerDeckOfQuestions.Add(questions[questionIndex]);
                    questionsRemaining--;
                }
                AnyQuestions:
                
                break;
        }
        securityIndex = 0;
        for (int i = 0; i < questionsRemaining; i++)
        {
            do
            {
                questionIndex = Random.Range(0, questions.Length);
                securityIndex++;
                if (securityIndex > SECURITY_THRESHOLD)
                {
                    Debug.Log("No more questions to draw from");
                    return;
                }
            } while (questions[questionIndex].category == QuestionCategory.Sports.ToString() || IsThisQuestionADuplicate(questionIndex));
            quizManager.PlayerDeckOfQuestions.Add(questions[questionIndex]);
        }
        quizManager.RefreshQuestionsIdsForSave();
    }

    private bool IsThisQuestionADuplicate(int questionIndex)
    {
        for (int i = 0; i < quizManager.PlayerDeckOfQuestions.Count; i++)
        {
            if(questions[questionIndex].id == quizManager.PlayerDeckOfQuestions[i].id)
            {
                return true; //question is already in the player deck
            }
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public Question QuestionByIndex(int questionIndex)
    {
        return questions[questionIndex];
    }


}
[System.Serializable]
public class Question
{
    public int id;
    public string category;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}