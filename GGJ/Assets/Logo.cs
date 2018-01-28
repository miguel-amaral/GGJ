using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject logo;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
          
            mainMenu.SetActive(true);
            logo.SetActive(false);
        }
    }
}
