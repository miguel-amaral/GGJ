using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject GameOverMenu;
    public GameObject MainMenu;

    private string currentScene;
    // Use this for initialization
    void Start () {
        if (ScoreKeeper.KeepingScore)
        {
            this.ActivateEndScente();
        }
	}

    public void PlayGame() {
        SceneManager.LoadScene("_menu");
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
                case "GameOver":
                    child.GetComponent<TextMeshProUGUI>().text = ScoreKeeper.Victory ? "Victory!!!" : "Game Over";
                    break;
            }
            //child is your child transform
        }

        currentScene = ScoreKeeper.CurrentScene;

    }
}
