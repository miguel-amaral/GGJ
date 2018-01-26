using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int PlayerSpeed = 30;
    public Rigidbody rb;

	// Use this for initialization
	void Start ()
	{
	    this.rb = this.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //float hor_axis = Input.GetAxisRaw("Horizontal");
	    //float ver_axis = Input.GetAxisRaw("Vertical");

     //   var move_vector = (Vector3.right * hor_axis + Vector3.forward * ver_axis)* PlayerSpeed;
     //   this.rb.AddForce(move_vector);


	    //this.gameObject.transform.position += move_vector;
        this.rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal")*PlayerSpeed, rb.velocity.y, Input.GetAxisRaw("Vertical")*PlayerSpeed);

	}
}
