using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairMovement : MonoBehaviour {

	//public GameObject _player;

	public BlockType _stairType;

	public Vector2 _originPos;

	public float moveMin;
	public float moveMax;

	public float fallDelay;

	public float moveTime;
	private float moveCount;
	private bool moveTowards;

	private float delayCount;
	private bool startCountDown;

	// Use this for initialization
	void Start () {
		moveCount = 0;
		delayCount = 0;
		startCountDown = false;
		moveTowards = true;
	}

	public void resetStairState(){
		moveCount = 0;
		delayCount = 0;
		startCountDown = false;
		moveTowards = true;
		transform.position = _originPos;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	// Update is called once per frame
	void Update () {
		if (startCountDown) {
			delayCount += Time.deltaTime;
		}
		switch (_stairType) {
		case BlockType.BT_LRSTAIR:
			{
				if (moveTowards)
					moveCount += Time.deltaTime;
				else
					moveCount -= Time.deltaTime;
				if (moveTowards) {
					if (moveCount >= moveTime) {
						moveCount = moveTime;
						moveTowards = !moveTowards;
					}
				} else {
					if (moveCount <= 0) {
						moveCount = 0;
						moveTowards = !moveTowards;
					}
				}
				transform.position = new Vector2(moveMin + moveCount / moveTime * (moveMax - moveMin),transform.position.y);
			}
			break;
		case BlockType.BT_UDSTAIR:
			{
				if (moveTowards)
					moveCount += Time.deltaTime;
				else
					moveCount -= Time.deltaTime;
				if (moveTowards) {
					if (moveCount >= moveTime) {
						moveCount = moveTime;
						moveTowards = !moveTowards;
					}
				} else {
					if (moveCount <= 0) {
						moveCount = 0;
						moveTowards = !moveTowards;
					}
				}
				transform.position = new Vector2(transform.position.x, moveMin + moveCount / moveTime * (moveMax - moveMin));
			}
			break;
		case BlockType.BT_FALLSTAIR:
			{
				if (delayCount >= fallDelay) {
					GetComponent<Rigidbody2D> ().isKinematic = false;
					if (transform.FindChild ("Player") != null && transform.FindChild ("Player").parent == transform) {
						transform.FindChild ("Player").parent = null;
					}
				}
			}
			break;
		}
		if (transform.position.y < -10) {
			//GameObject.Destroy (gameObject);
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent<Rigidbody2D> ().isKinematic = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		startCountDown = true;
//		if (_stairType == BlockType.BT_FALLSTAIR && other.gameObject.CompareTag ("Player")) {
//			_player.transform.parent = transform;
//		}
	}

	void OnCollisionExit2D(Collision2D other){
//		if (_stairType == BlockType.BT_FALLSTAIR && other.gameObject.CompareTag ("Player")) {
//			if (_player.transform.parent == transform)
//				_player.transform.parent = null;
			//_player.transform.parent = transform;
//		}
	}
}
