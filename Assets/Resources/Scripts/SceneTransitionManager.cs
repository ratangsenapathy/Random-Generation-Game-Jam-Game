using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void GoToGameScene(){
		SceneManager.LoadScene ("GameScene", LoadSceneMode.Single);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape)){
			Application.Quit ();
		}
	}
}
