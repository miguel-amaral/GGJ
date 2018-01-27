using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour {

	private Animator studentAnimation;

	// Use this for initialization
	void Start () {
		studentAnimation = this.GetComponent<Animator>();

		studentAnimation.Play(0, -1, Random.Range(0, 10));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
