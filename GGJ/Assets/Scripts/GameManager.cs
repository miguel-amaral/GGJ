using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private List<GameObject> _students = new List<GameObject>();
    private List<GameObject> _professors= new List<GameObject>();


    // Use this for initialization
	void Start () {
	    _students = new List<GameObject>(GameObject.FindGameObjectsWithTag("Student"));
	    _professors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Professor"));
    }

    // TeacherIsAway is called once per frame
    void Update () {
        foreach (var professor in _professors) {
            foreach (var student in _students)
            {
                //var distance = professor.transform.position - student.
            }
        }
    }

    void OnDrawGizmos() {


        Handles.color = Color.red;
        foreach (var professor in _professors)
        {
            var collider = professor.GetComponent<CapsuleCollider>();
            Handles.DrawWireDisc(professor.transform.position, Vector3.up, collider.radius);
        }
    }
}
