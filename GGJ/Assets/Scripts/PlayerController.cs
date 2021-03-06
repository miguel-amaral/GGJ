﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int PlayerSpeed = 20;
    private Rigidbody rb;
    public int BustedStudents { get; set; }
    private Animator teacherAnimator;

    public string player_string;
    private string horizontal_axis_name;
    private string vertical_axis_name;
    private string action_name_button;

    private GameObject Closest;
    private GameObject Holding;

    private float lastPickUp;
    public float DelayBetweenActions = 1;

    // Use this for initialization
    void Start ()
	{
	    this.rb = this.gameObject.GetComponent<Rigidbody>();
	    BustedStudents = 0;
        teacherAnimator = GetComponent<Animator>();

	    var keyboard = player_string.Equals("P1")
	        ? PlayerPrefs.GetInt("P1_Keyboard") == 1
	        : PlayerPrefs.GetInt("P2_Keyboard") == 1;


        horizontal_axis_name = player_string + "_Horizontal_" + (keyboard?"Keyboard":"Gamepad") ;
	    vertical_axis_name = player_string + "_Vertical_" + (keyboard ? "Keyboard" : "Gamepad");
	    action_name_button = player_string + "_Action_" + (keyboard ? "Keyboard" : "Gamepad"); 

	}
	
	// TeacherIsAway is called once per frame
	void Update ()
	{
        //float hor_axis = Input.GetAxisRaw("Horizontal");
        //float ver_axis = Input.GetAxisRaw("Vertical");

        //   var move_vector = (Vector3.right * hor_axis + Vector3.forward * ver_axis)* PlayerSpeed;
        //   this.rb.AddForce(move_vector);
	    var axis_hor = Input.GetAxisRaw(horizontal_axis_name);
	    var axis_ver = Input.GetAxisRaw(vertical_axis_name);



        Vector3 direction = new Vector3(axis_hor, 0.0f, axis_ver);
	    if (!direction.Equals(Vector3.zero))
	    {
	        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15F);
            teacherAnimator.SetBool("walk", true);
        } else
        {
            teacherAnimator.SetBool("walk", false);
        }

	    var action = Input.GetButton(action_name_button);
	    var currentTime = Time.realtimeSinceStartup;
	    if (action && currentTime-lastPickUp > DelayBetweenActions)
	    {
	        lastPickUp = currentTime;
	        if (Holding != null)
	        {
	            this.DropBoard();
	        }
	        else
	        {
	            this.TryToPickBoard();
            }
        }
	    

        //transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        //this.gameObject.transform.position += move_vector;
        //transform.Translate(movement * PlayerSpeed * Time.deltaTime, Space.World);

        this.rb.velocity = new Vector3(axis_hor*PlayerSpeed, rb.velocity.y,axis_ver*PlayerSpeed);
        
	    
	    //this.transform.rotation.to

	}

    private void DropBoard()
    {
        Debug.LogWarning("Drop");
        var obsManager = Holding.GetComponent<ObstacleManager>();
        obsManager.BeDropedDown();
        this.Holding = null;
 
        //throw new System.NotImplementedException();
    }

    private void TryToPickBoard()
    {
        if (Closest != null && Holding == null)
        {   
            var obsManager = Closest.GetComponent<ObstacleManager>();
            Debug.LogWarning(obsManager.CanPickUp());
            if (obsManager.CanPickUp())
            {
                Holding = Closest;
                //Closest = null;

                obsManager.BePickedUp(this.gameObject);
            } else
            {
                Debug.LogWarning("Board dont let me pick up");
            }
        }
        else
        {
            //1e
        }
    }

    void OnTriggerEnter(Collider col)
    {
        
    }

    public void Bust(StudentManager studentManager)
    {
        BustedStudents++;
    }

    public void SetClosest(GameObject obstacleManager)
    {
        Debug.Log("Setting Closest: " + obstacleManager);
        this.Closest = obstacleManager;
    }
}
