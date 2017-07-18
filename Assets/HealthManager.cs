using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	// Use this for initialization
	Scrollbar healthBar;
	public static float health;
	void Start () {
		health = 1.0f;
		healthBar = GetComponent<Scrollbar> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.size = health>0.0f?health:0.0f;
	}
}
