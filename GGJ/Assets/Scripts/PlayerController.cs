using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int PlayerSpeed = 20;
    private Rigidbody rb;

	// Use this for initialization
	void Start ()
	{
	    this.rb = this.gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        //float hor_axis = Input.GetAxisRaw("Horizontal");
        //float ver_axis = Input.GetAxisRaw("Vertical");

        //   var move_vector = (Vector3.right * hor_axis + Vector3.forward * ver_axis)* PlayerSpeed;
        //   this.rb.AddForce(move_vector);
	    var axis_hor = Input.GetAxisRaw("Horizontal");
	    var axis_ver = Input.GetAxisRaw("Vertical");



        Vector3 direction = new Vector3(axis_hor, 0.0f, axis_ver);
	    if (!direction.Equals(Vector3.zero))
	    {
	        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15F);
        }


        //transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        //this.gameObject.transform.position += move_vector;
        //transform.Translate(movement * PlayerSpeed * Time.deltaTime, Space.World);

        this.rb.velocity = new Vector3(axis_hor*PlayerSpeed, rb.velocity.y,axis_ver*PlayerSpeed);
        
	    
	    //this.transform.rotation.to

	}
}
