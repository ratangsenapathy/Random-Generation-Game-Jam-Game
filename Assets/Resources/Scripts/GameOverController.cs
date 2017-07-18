using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		GetComponent<Text> ().text = "Score: " + ScoreManager.score;
	}
	
	public void GoToMainMenuScene()
	{
		SceneManager.LoadScene ("MainMenuScene", LoadSceneMode.Single);
	}
}
