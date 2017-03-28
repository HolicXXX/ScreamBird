using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIfit : MonoBehaviour {

	public float _thisOriginWidht;
	public float _thisOriginHeight;

	void Start () {
		float _wRate = Screen.width / 750.0f;
		float _hRate = Screen.height / 1334.0f;
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (_thisOriginWidht * _wRate, _thisOriginHeight * _hRate);
		//gameObject.GetComponent<RectTransform> ().position = new Vector3 (transform.position.x / 7.5f * Screen.width / 100.0f, transform.position.y / 13.34f * Screen.height / 100.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
