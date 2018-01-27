using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitRoom : MonoBehaviour {

	public GameObject target;
	public int speed;

	private void Start()
	{
		if(target != null)
		{
			transform.LookAt(target.transform);
		}
	}

	// Update is called once per frame
	void Update () {
		if(target != null) {
			transform.LookAt(target.transform);
			transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
		}
	}
}
