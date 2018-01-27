using System.Collections;
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
    public int TimeToSendAnswer = 3;
    private List<GameObject> _neighbours = new List<GameObject>();

    private float CopyRemainingTime = 0;

    private bool Receiving = false;
    private bool Sending = false;
    private GameObject InteractingWith = null;

    private float TimeAccumulated = 0;

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

    }

    public bool CanCopy()

    {
        return nearTeacher == 0 && !Receiving && !Sending;
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
    }

    private void TryToCopy()
    {
        var tempNeighbours = new List<StudentManager>();
        if (CanCopy())
        {
            var result = Random.Range(0, 99);
            if (result < CopyProbability)
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
                    Debug.Log(this.gameObject.name + " " + InteractingWith.gameObject.name);
                } else
                {
                    TimeAccumulated += TryToCopyEveryAmountOfSeconds / 2.0f;
                }

                
            }

            //tempNeighbours./*get*/
        }
    }

    private void SendCopy(StudentManager otherCheater) {
        otherCheater.GiveMeCheat(this.gameObject, TimeToSendAnswer);
        Sending = true;
        InteractingWith = otherCheater.gameObject;
        this.CopyRemainingTime = TimeToSendAnswer;
    }

    private void GiveMeCheat(GameObject sender, int timeToWaitCopy)
    {
        Receiving = true;
        InteractingWith = sender;
        CopyRemainingTime = timeToWaitCopy;
    }


    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Professor"))
        {
            Debug.Log(this.gameObject.name + "enter colision");
            nearTeacher++;
        }
    }

    public void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.Equals("Professor")) {
            nearTeacher--;
        }
    }

    void OnDrawGizmos()
    {
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
        
        if (InteractingWith != null)
        {
            Gizmos.DrawLine(this.transform.position, InteractingWith.transform.position);
        }
        //Gizmos.color = Color.black;
        //foreach (var neighbour in _neighbours)
        //{
        //    Gizmos.DrawLine(this.transform.position,neighbour.transform.position);
        //}
    }
}
