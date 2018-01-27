﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class StudentManager : MonoBehaviour
{

    private int nearTeacher = 0;
    public int CheatingRadius = 50;
    public int TryToCopyEveryAmountOfSeconds = 0;
    public int CopyProbability = 13;
    public int QuestionProbability = 13;
    public int TimeToSendAnswer = 3;
    private List<GameObject> _neighbours = new List<GameObject>();

    private float CopyRemainingTime = 0;

    private bool Receiving = false;
    private bool Sending = false;
    private bool Questioning = false;

    private GameObject InteractingWith = null;

    private float TimeAccumulated = 0;

    private Animator myAnimator;

    public GameObject paper;


    // Use this for initialization
    void Start () {
        var temp_neighbours = new List<GameObject>(GameObject.FindGameObjectsWithTag("Student"));
        
        foreach (var neighbour in temp_neighbours)
        {
            if (neighbour != this.gameObject && DistanceToMe(neighbour) <= CheatingRadius)
            {
                _neighbours.Add(neighbour);
            }
        }

        if (CopyProbability + QuestionProbability > 100)
        {
            Debug.LogError("Probabilities summed > 100%");
        }

        myAnimator = GetComponentInChildren<Animator>();

    }

    public bool CanCopy()

    {
        return nearTeacher == 0 && !Receiving && !Sending && !Questioning;
    }

    private float DistanceToMe(GameObject other)
    {
        return Vector3.Distance(other.transform.position, this.transform.position);
    }

    // Update is called once per frame
    void Update ()
    {
        TimeAccumulated += Time.deltaTime;
        if (CopyRemainingTime > 0)
        {
            CopyRemainingTime -= Time.deltaTime;
            if (CopyRemainingTime <= 0)
            {
                StopCopying();
            }
        }
        if (TimeAccumulated > TryToCopyEveryAmountOfSeconds)
        {
            TimeAccumulated -= TryToCopyEveryAmountOfSeconds;
            TryToCopy();
        }
    }

    private void StopCopying()
    {
        Receiving = false;
        Sending = false;
        var temp = InteractingWith;
        InteractingWith = null;
        if(temp!=null) temp.GetComponent<StudentManager>().StopCopying();

        myAnimator.SetBool("toFront", false);
        myAnimator.SetBool("toBack", false);
        myAnimator.SetBool("toLeft", false);
        myAnimator.SetBool("toRight", false);
    }

    private void TryToCopy()
    {
        var probability = Random.Range(0, 99);
        var probability_sum = 0;
        var tempNeighbours = new List<StudentManager>();
        if (CanCopy())
        {
            if (probability < CopyProbability)
            {
                foreach (var neighbour in _neighbours)
                {
                    var neightbour_script = neighbour.GetComponent<StudentManager>();
                    if (neightbour_script.CanCopy()) {
                        tempNeighbours.Add(neightbour_script);
                    }
                }
                //Copy Will happen
                var numberCheaters = tempNeighbours.Count;
                if (numberCheaters > 0) {
                    var index = Random.Range(0, numberCheaters);

                    var otherCheater = tempNeighbours.ElementAt(index);
                    SendCopy(otherCheater);
                    //Debug.Log(this.gameObject.name + " " + InteractingWith.gameObject.name);
                } else
                {
                    TimeAccumulated += TryToCopyEveryAmountOfSeconds / 2.0f;
                }

                
            }
        }

        probability_sum += CopyProbability;
        if (CanQuestion())
        {
            if (probability_sum < probability && probability <= probability_sum+QuestionProbability)
            {
                Questioning = true;
            }
        }
    }

    private bool CanQuestion()
    {
        return !Sending && !Receiving && !Questioning;
    }

    private void SendCopy(StudentManager otherCheater)
    {
        otherCheater.GiveMeCheat(this.gameObject, TimeToSendAnswer);
        Sending = true;
        InteractingWith = otherCheater.gameObject;
        this.CopyRemainingTime = TimeToSendAnswer;

        SendAnimation();

    }

    private void SendAnimation()
    {
        var direction = InteractingWith.gameObject.transform.position - this.gameObject.transform.position;
        var back = direction.z > 0;
        var forward = direction.z < 0;
        var right = direction.x < 0;
        var left = direction.x > 0;

        Debug.Log(this.gameObject.name + back + right);
        if (back)
        {
            myAnimator.SetBool("toBack", true);
        }
        else if (forward)
        {
            myAnimator.SetBool("toFront", true);
        }
        else if (right)
        {
            myAnimator.SetBool("toRight", true);
        }
        else if (left)
        {
            myAnimator.SetBool("toLeft", true);

        }
    }

    private void ReceiveAnimation()
    {
        var direction = InteractingWith.gameObject.transform.position - this.gameObject.transform.position;
        var back = direction.z > 0;
        var forward = direction.z < 0;
        var right = direction.x < 0;
        var left = direction.x > 0;

        Debug.Log(this.gameObject.name + back + right);
        if (back)
        {
            myAnimator.SetBool("toBack", true);
        }
        else if (forward)
        {
            myAnimator.SetBool("toFront", true);

        }
        else if (right)
        {
            myAnimator.SetBool("toRight", true);
        }
        else if (left)
        {
            myAnimator.SetBool("toLeft", true);

        }
    }

    private void GiveMeCheat(GameObject sender, int timeToWaitCopy)
    {
        Receiving = true;
        InteractingWith = sender;
        CopyRemainingTime = timeToWaitCopy;
        ReceiveAnimation();
    }


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Professor"))
        {
            nearTeacher++;
            if (Sending || Receiving)
            {
                Busted(col.gameObject);
            }
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag.Equals("Professor"))
        {
            Questioning = false;

        }
    }

    private void Busted(GameObject teacher)
    {
        var teacher_script = teacher.GetComponent<PlayerController>();
        teacher_script.Bust(this);
        StopCopying();
    }

    public void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.Equals("Professor")) {
            nearTeacher--;
        }
    }

    void OnDrawGizmos() {
        if (Questioning)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(this.transform.position + new Vector3(0, 10, 0), 3);
        }

        if (nearTeacher > 0)
        {
            Handles.color = Color.red;
        }
        else if (Receiving)
        {
            Handles.color = Color.yellow;


        } else if (Sending) {
            Handles.color = Color.blue;
          
        }
         else
        {
            Handles.color = Color.green;
        }
        Handles.DrawWireDisc(this.transform.position, Vector3.up, 10);
        
        if (InteractingWith != null && Sending)
        {
            Gizmos.color=Color.white;
            float percentage = (TimeToSendAnswer - CopyRemainingTime )/ TimeToSendAnswer;
            var direction = InteractingWith.gameObject.transform.position - this.transform.position;
            var vector_percentage = direction * percentage; 
            Gizmos.DrawLine(this.transform.position, this.transform.position+vector_percentage);

            paper.transform.Translate(this.transform.position.x, this.transform.position.x + vector_percentage.x, Time.deltaTime);



        }
        //Gizmos.color = Color.black;
        //foreach (var neighbour in _neighbours)
        //{
        //    Gizmos.DrawLine(this.transform.position,neighbour.transform.position);
        //}
    }

    private void sendPaper()
    {

    }
}
