using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("RangeAnswerQuestionCollider"))
        {
            col.gameObject.transform.parent.GetComponent<PlayerController>().setClosest(this.gameObject);
            Debug.LogError(col.gameObject.transform.parent.name);
        }
    }

    //public bool CanPickUp()
    //{

    //}
}
