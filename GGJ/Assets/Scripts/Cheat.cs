using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

    public GameObject myCheat;

    private ParabolaController pController;
    private bool isAnimating;

    // Use this for initialization
    void Start () {
        myCheat.SetActive(false);
        isAnimating = false;

        pController = GetComponent<ParabolaController>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            startAnimation();
        }
    
        if (isAnimating)
        {
            stopAnimation();
        }
	}

    public void startAnimation ()
    {
        myCheat.SetActive(true);
        GetComponent<ParabolaController>().FollowParabola();
        isAnimating = true;
    }

    private void stopAnimation ()
    {
        Debug.Log("My cheat: " + myCheat.transform.position);
        Debug.Log("Final: " + pController.GetComponent<ParabolaController>().ParabolaRoot.transform.GetChild(2).transform.position);

        if (myCheat.transform.position == pController.GetComponent<ParabolaController>().ParabolaRoot.transform.GetChild(2).transform.position)
        {
            Debug.Log("STOP ANIMATION!");
            myCheat.SetActive(false);
            isAnimating = false;
        }
    }
}
