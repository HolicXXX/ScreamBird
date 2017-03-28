using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour {

	public GameObject _mainCamera;

	public float _Width;

	private bool _callNext;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (1.0f, Screen.height / 1334.0f, 1.0f);
		_callNext = true;
		//_mainCamera.transform.position = new Vector2 (_mainCamera.transform.position.x + _Width, _mainCamera.transform.position.y);
	}

	public void setbgPosition(){
		//_mainCamera.transform.position = new Vector2 (_mainCamera.transform.position.x + _Width, _mainCamera.transform.position.y);
	}

	// Update is called once per frame
	void Update () {
		if (_callNext && _mainCamera.transform.position.x - transform.position.x >= -0.1f) {
			//Debug.Log (_mainCamera.transform.position.x + " - " + transform.position.x);
			_callNext = false;
			_mainCamera.GetComponent<CameraMove> ().CallNextBackGround (transform.position.x);
		} 
		if (_mainCamera.transform.position.x - transform.position.x >= _Width / 100.0f + 0.1f) {
			GameObject.Destroy (gameObject);
		}
	}
}
