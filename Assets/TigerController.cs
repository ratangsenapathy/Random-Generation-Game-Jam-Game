using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerController : MonoBehaviour {

	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (target.position);
		float step = speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (target.position.x-0.5f,target.position.y,target.position.z-0.5f), step);
		transform.rotation = Quaternion.identity;
		transform.LookAt (new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
	}
}
