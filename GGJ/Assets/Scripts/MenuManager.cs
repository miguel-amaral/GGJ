using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject GameOverMenu;
    public GameObject MainMenu;

    // Use this for initialization
    void Start () {
        if (ScoreKeeper.KeepingScore)
        {
            this.ActivateEndScente();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateEndScente()
    {
        ScoreKeeper.KeepingScore = false;
        MainMenu.gameObject.SetActive(false);
        GameOverMenu.SetActive(true);

        foreach (Transform child in GameOverMenu.transform) {
            switch (child.name)
            {
                case "Average":
                    child.GetComponent<TextMeshProUGUI>().text = "AVG: " + ScoreKeeper.Score.ToString();
                    break;
                case "Letter":
                    child.GetComponent<TextMeshProUGUI>().text = ScoreKeeper.Letter.ToString();
                    break;


            }
            //child is your child transform
        }

    }
}
