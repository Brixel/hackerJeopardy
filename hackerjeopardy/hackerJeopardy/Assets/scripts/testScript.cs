using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;



public class testScript : MonoBehaviour
{
    void Start()
    {

        List<tCategory> meowMix = new List<tCategory>();
        tCategory thisCat = new tCategory();
        thisCat.questions = new List<tQuestion>();
        thisCat.categoryName = "hello";
        thisCat.categoryColorR = 255;
        thisCat.categoryColorG = 255;
        thisCat.categoryColorB = 255;
        tQuestion q1 = new tQuestion();
        q1.questionText = "question?";
        q1.answer = "answer!";
        thisCat.questions.Add(q1);
        tQuestion q2 = new tQuestion();
        q2.questionText = "question2?";
        q2.answer = "answer2!";
        thisCat.questions.Add(q2);
        

        meowMix.Add(thisCat);
        tCategory anotherCat = new tCategory();
        anotherCat.questions = new List<tQuestion>();
        anotherCat.categoryName = "world";
        anotherCat.categoryColorR = 255;
        anotherCat.categoryColorG = 255;
        anotherCat.categoryColorB = 255;
        tQuestion q3 = new tQuestion();
        q3.questionText = "question3?";
        q3.answer = "answer3!";
        anotherCat.questions.Add(q3);
        tQuestion q4 = new tQuestion();
        q4.questionText = "question4?";
        q4.answer = "answer4!";
        anotherCat.questions.Add(q4);
        meowMix.Add(anotherCat);

        string jsonCats = JsonConvert.SerializeObject(meowMix);
        Debug.Log(jsonCats);

    }
}

public class tCategory 
{
    public string categoryName;
    public int categoryColorR;
    public int categoryColorG;
    public int categoryColorB;
    public List<tQuestion> questions;
}
[Serializable]
public class tQuestion
{
    public int value;
    public int questionColorR;
    public int questionColorG;
    public int questionColorB;
    public int PresentationType;
    public string questionText;
    public string questionImage;
    public string questionVideo;

    public string answerImage;
    public string answerVideo;

    public string answer;
    public bool isAvailable;
    public string questionNote;
}
