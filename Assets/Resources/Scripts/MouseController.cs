using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour {

	public Texture2D crosshairImage;
	private AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}


	void OnGUI(){
		float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (crosshairImage.width / 4);
		float yMin = (Screen.height - Input.mousePosition.y) - (crosshairImage.height / 4);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width/2, crosshairImage.height/2), crosshairImage);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			//Debug.Log ("Mouse Click");
			audio.Play();
			RaycastHit[] hits;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction*2000);
			//Debug.DrawLine(ray.origin, Camera.main.ScreenPointToRay(Input.mousePosition), Color.red);
			hits = Physics.RaycastAll(ray,60.0f);
			if(hits.Length!=0) {

				foreach (RaycastHit hit in hits) {
					if (hit.collider != null) {

						if (hit.collider.tag == "Creature") {
							Destroy (hit.collider.gameObject);
							ScoreManager.score += 10;
							TerrainGenerator.tigerCount--;
//							TurtleGenerator.turtleCount--;
//							ScoreManager.score += 10;
							break;
						}
//						else if (hit.collider.tag == "Creature2") {
//							Destroy (hit.collider.gameObject);
////							IguanaGenerator.iguanaCount--;
////							ScoreManager.score += 30;
//							break;
//						}
					}
				}

			}
		}

		if (Input.GetKey(KeyCode.Escape)){
			SceneManager.LoadScene ("GameOverScene", LoadSceneMode.Single);
		}
	}
}
