﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{


    public int NumberTotalQuestionsInLevel = 10;
    public int QuestionsIncrementalValue = 1;


    private float[] ScoresLetters = new float[] {0.5f, 0.75f, 1};
    private int CurrentMaxQuestions = 0;
    private int TotalQuestionsMade = 0;

    private float QuestionsIncrementalPeriod = 5;
    private float AccumulatorTimeQuestionsIncremental = 0;


    private List<GameObject> _students = new List<GameObject>();
    private List<StudentManager> _studentsScripts = new List<StudentManager>();

    private List<GameObject> _professors= new List<GameObject>();

    public float LevelTime = 20f;
    private float LevelRemainingTime; 
    public int MaxSimoultaneousQuestionsInRoom = 3;
    private int NumberQuestionsInRoom = 0;
    public int MaxCopiesInRoom = 3;
    private int NumberCopiesInRoom = 0;

    private int NumberQuestionsSolved = 0;
    private int NumberQuestionsFailed = 0;
    private int NumberStudentsBusted = 0;
    // Use this for initialization
    void Start () {
	    _students = new List<GameObject>(GameObject.FindGameObjectsWithTag("Student"));
	    _professors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Professor"));
        foreach (var student in _students)
        {
            _studentsScripts.Add(student.GetComponent<StudentManager>());
        }
        LevelRemainingTime = LevelTime;

        QuestionsIncrementalPeriod = ((LevelTime * 0.66f)/NumberTotalQuestionsInLevel  ) * QuestionsIncrementalValue;
        CurrentMaxQuestions = 0;
    }

    private float accumulated = 0;
    void Update ()
    {
        accumulated += Time.deltaTime;
        AccumulatorTimeQuestionsIncremental += Time.deltaTime;
        if (accumulated > 20)
        {
            accumulated -= 20;
            this.PrintScore();
        }

        if (AccumulatorTimeQuestionsIncremental > QuestionsIncrementalPeriod && CurrentMaxQuestions < NumberTotalQuestionsInLevel)
        {
            AccumulatorTimeQuestionsIncremental -= QuestionsIncrementalPeriod;
            CurrentMaxQuestions += QuestionsIncrementalValue;
            CurrentMaxQuestions = Math.Min(CurrentMaxQuestions,NumberTotalQuestionsInLevel);
        }



        LevelRemainingTime -= Time.deltaTime;
        if (LevelRemainingTime < 0)
        {
            FinishLevelTime();
            LevelRemainingTime = LevelTime;
        }

        
        //    foreach (var professor in _professors) {
        //        foreach (var student in _students)
        //        {
        //            //var distance = professor.transform.position - student.
        //        }
        //    }

    }

    private void FinishLevelTime()
    {
        Debug.LogError("GAME OVER BOY : " + CalculateFinalScore() );
    }

    private int CalculateFinalScore()
    {
        return NumberStudentsBusted - NumberQuestionsFailed * 5 + NumberQuestionsSolved;
    }

    private void PrintScore()
    {
        var quality = (NumberQuestionsSolved * 1.0f) / TotalQuestionsMade;

        int stars = 0;
        foreach (var scoresLetter in ScoresLetters)
        {
            if (scoresLetter <= quality)
            {
                stars++;
            }
        }

        Debug.Log("Quality %??: " + NumberQuestionsSolved + " / " +  TotalQuestionsMade  + " : " + quality);
        Debug.Log("Busted: " + NumberStudentsBusted + " Failed ???: " + NumberQuestionsFailed + " Successful ???: " + NumberQuestionsSolved);
        Debug.Log("Media: " + CalculateClassAverage() + " :stars: " + stars);
    }

    private float CalculateClassAverage()
    {
        float total = 0;
        int number_students = _students.Count;
        foreach (var student in _studentsScripts)
        {
            total += student.GetTestScore();
        }

        return total / number_students;
    }

    void OnDrawGizmos() {
        Handles.color = Color.red;
        foreach (var professor in _professors)
        {
            var collider = professor.GetComponent<CapsuleCollider>();
            Handles.DrawWireDisc(professor.transform.position, Vector3.up, collider.radius);
        }
    }



    public bool CanCopy() {
        //Debug.Log(NumberCopiesInRoom + " :<->: " + MaxCopiesInRoom );
        return NumberCopiesInRoom < MaxCopiesInRoom;
    }
    public void StopCopy() {
        NumberCopiesInRoom--;
    }
    public void ActivateCopy() {
        NumberCopiesInRoom++;
    }


    //----------------------------------
    public bool CanQuestion() {
        return NumberQuestionsInRoom < MaxSimoultaneousQuestionsInRoom && TotalQuestionsMade < CurrentMaxQuestions;
    }
    public void StopQuestions()
    {
        NumberQuestionsInRoom--;
    }
    public void ActivateQuestion()
    {
        NumberQuestionsInRoom++;
        TotalQuestionsMade++;
    }

    //------------------------------------

    public void BustStudent()
    {
        NumberStudentsBusted++;
    }

    public void QuestionSolved()
    {
        NumberQuestionsSolved++;
    }

    public void QuestionTimeUp()
    {
        NumberQuestionsFailed++;
    }

    public Vector3 NearestTeacherDirection(Vector3 position)
    {
        Vector3 bestDirection = Random.Range(0,2) == 0 ?  Vector3.left : Vector3.right;
        float bestDistance = float.MaxValue;
        foreach (var professor in _professors)
        {
            var direction = professor.transform.position - position;
            var distance = direction.magnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestDirection = direction;
            }
        }
        return bestDirection;
    }
}
