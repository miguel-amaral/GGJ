using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    private bool isUp = false;
    private GameObject parent = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (isUp)
	    {
	        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,parent.GetComponent<Rigidbody>().velocity.z);
	    }
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("RangeAnswerQuestionCollider"))
        {
            Debug.LogWarning("Collision Enter");
            col.gameObject.transform.parent.GetComponent<PlayerController>().SetClosest(this.gameObject);
            //Debug.LogError(col.gameObject.transform.parent.name);
        }
    }

    public void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.Equals("RangeAnswerQuestionCollider")) {
            Debug.LogWarning("Collision Exit");
            col.gameObject.transform.parent.GetComponent<PlayerController>().SetClosest(null);
            //Debug.LogError(col.gameObject.transform.parent.name);
        }
    }

    public bool CanPickUp()
    {
        return !isUp;
    }

    public void BePickedUp(GameObject parent)
    {
        this.parent = parent;

        Debug.LogWarning("Was Picked");
        isUp = true;
    }

    public void BeDropedDown()
    {
        this.parent = null;
        Debug.LogWarning("Was Droped");
        isUp = false;
    }
}
