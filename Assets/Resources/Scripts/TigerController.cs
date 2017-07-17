﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerController : MonoBehaviour {

	private Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		
		target = GameObject.FindGameObjectWithTag ("Player")!=null?GameObject.FindGameObjectWithTag("Player").transform:null;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (target.position);
		float step = speed * Time.deltaTime;

		if (Vector3.Distance (transform.position, target.position) > 1.0f)
			transform.position = Vector3.MoveTowards(transform.position, new Vector3 (target.position.x-1.0f,target.position.y,target.position.z-1.0f), step);
		transform.rotation = Quaternion.identity;
		transform.LookAt (new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
	}
}
