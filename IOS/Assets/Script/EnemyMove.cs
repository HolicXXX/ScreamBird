using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

	public Camera _mainCamera;

	private bool _isMoving;

	// Use this for initialization
	void Start () {
		_isMoving = false;
	}

	// Update is called once per frame
	void Update () {
		if (DataManager._isPause) {
			return;
		}
		//if (!_isMoving) {
		if (transform.position.x - _mainCamera.transform.position.x <= 4 && GetComponent<Rigidbody2D> ().velocity.x > -0.75f) {
				GetComponent<Rigidbody2D> ().isKinematic = false;
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-0.75f, 0.0f);
				_isMoving = true;
			}
		//}
		if(_isMoving && transform.position.y - _mainCamera.transform.position.y <= -2){
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, -2.0f);
		}
	}
}
