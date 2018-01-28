using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private List<GameObject> _students = new List<GameObject>();
    private List<GameObject> _professors= new List<GameObject>();
    public int MaxQuestionsInRoom = 3;
    public int NumberQuestionsInRoom = 0;
    public int MaxCopiesInRoom = 3;
    public int NumberCopiesInRoom = 0;

    public int NumberQuestionsSolved = 0;
    public int NumberQuestionsFailed = 0;
    public int NumberStudentsBusted = 0;
    // Use this for initialization
    void Start () {
	    _students = new List<GameObject>(GameObject.FindGameObjectsWithTag("Student"));
	    _professors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Professor"));
    }

    private float accumulated = 0;
    void Update ()
    {
        accumulated += Time.deltaTime;
        if (accumulated > 5)
        {
            accumulated -= 5;
            this.PrintScore();
        }
        //    foreach (var professor in _professors) {
        //        foreach (var student in _students)
        //        {
        //            //var distance = professor.transform.position - student.
        //        }
        //    }

    }

    private void PrintScore()
    {
        Debug.Log("Busted: " + NumberStudentsBusted + " Failed ???: " + NumberQuestionsFailed + " Successful ???: " + NumberQuestionsSolved);
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
        //Debug.Log(NumberQuestionsInRoom + " :???: " + MaxQuestionsInRoom);
        return NumberQuestionsInRoom < MaxQuestionsInRoom;
    }
    public void StopQuestions()
    {
        NumberQuestionsInRoom--;
    }

    public void ActivateQuestion()
    {
        NumberQuestionsInRoom++;
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
