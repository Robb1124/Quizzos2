using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionCategory { Any, Sports, History, Science, Geography, Politics, Animals, Art, GeneralKnowledge};

[CreateAssetMenu(menuName = "Question")]
public class Questions : ScriptableObject
{
    [SerializeField] string question;
    [SerializeField] QuestionCategory category;
    [SerializeField] string[] choices;
    [SerializeField] string answer;

    public string Question { get => question; set => question = value; }
    public QuestionCategory Category { get => category; set => category = value; }
    public string[] Choices { get => choices; set => choices = value; }
    public string Answer { get => answer; set => answer = value; }
}
