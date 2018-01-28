using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

	public void PlayKindergarden()
	{
		SceneManager.LoadScene("Kindergarden");
	}

	public void PlayBasicSchool()
	{
		SceneManager.LoadScene("BasicSchool");
	}

	public void PlayHighSchool()
	{
		SceneManager.LoadScene("HighSchool");
	}

	public void PlayUniversity()
	{
		SceneManager.LoadScene("University");
	}
}
