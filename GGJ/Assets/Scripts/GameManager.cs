using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Text ScoreText;
    public Text TimeLeftText;


    public int NumberTotalQuestionsInLevel = 10;
    public int QuestionsIncrementalValue = 1;
    public float AvgMax = 9.5f;

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

        ScoreText.text = 0.ToString();
        TimeLeftText.text = LevelRemainingTime.ToString();
    }

    private float accumulated = 0;
    void Update ()
    {
        accumulated += Time.deltaTime;
        AccumulatorTimeQuestionsIncremental += Time.deltaTime;
        if (accumulated > 0.5f)
        {
            accumulated -= 0.5f;
            UpdateScoreUI();

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
            //LevelRemainingTime = LevelTime;
        }


        //    foreach (var professor in _professors) {
        //        foreach (var student in _students)
        //        {
        //            //var distance = professor.transform.position - student.
        //        }
        //    }

    }

    private void UpdateScoreUI()
    {
        int timeLeft = (int) LevelRemainingTime;
        var totalTime = LevelTime;
        var percentageTimeLevel = LevelRemainingTime / LevelTime;

        var avgClass = CalculateClassAverage();

        //this.ScoreText.text = timeLeft.ToString();
        this.ScoreText.text = avgClass.ToString("0.0");
        this.TimeLeftText.text = timeLeft.ToString();   
        //Update Score in UI
    }

    private void FinishLevelTime()
    {
        float quality = 1;
        if (TotalQuestionsMade != 0)
        {
            quality = (NumberQuestionsSolved * 1.0f) / (TotalQuestionsMade );
        }

        int stars = 0;
        foreach (var scoresLetter in ScoresLetters) {
            if (scoresLetter <= quality) {
                stars++;
            }
        }

        string letter;
        switch (stars)
        {
            case 3:
                letter = "A";
                break;
            case 2:
                letter = "B";
                break;
            case 1:
                letter = "C";
                break;
            default:
                letter = "F";
                break;
        }

        //Debug.LogError("GAME OVER BOY : " + CalculateFinalScore() );
        ScoreKeeper.KeepingScore = true;

        var avg = CalculateClassAverage();
        bool victory = avg < AvgMax && LevelRemainingTime < 0;
        ScoreKeeper.Victory = victory;
        ScoreKeeper.Letter = (victory ? letter : "F");
        ScoreKeeper.Score = avg;
        ScoreKeeper.CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("_menu");
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

        //Debug.Log("Quality %??: " + NumberQuestionsSolved + " / " +  TotalQuestionsMade  + " : " + quality);
        //Debug.Log("Busted: " + NumberStudentsBusted + " Failed ???: " + NumberQuestionsFailed + " Successful ???: " + NumberQuestionsSolved);
        //Debug.Log("Media: " + CalculateClassAverage() + " :stars: " + stars);
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
