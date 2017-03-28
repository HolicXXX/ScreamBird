using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PauseControl : MonoBehaviour {

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		DataManager._isPause = false;
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPauseClicked(){
		rb.velocity = Vector2.zero;
		rb.isKinematic = true;
		gameObject.GetComponent<Collider2D> ().enabled = false;
	}
	public void OnResumeClicked(){
		rb.isKinematic = false;
		gameObject.GetComponent<Collider2D> ().enabled = true;
	}
}
