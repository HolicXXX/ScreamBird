using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Slider m_volumControl;

	public bool m_isTouching;
	public bool m_isLanding;
	public bool m_isUp;

	private static int _maxTime = 1800;
	private static int _recRate = 44100;

	private AudioClip m_clip;

	private bool m_isOnStair;
	private bool m_isOnTwoStair;

	private GameObject _lastStandBlock;

	public Sprite[] _skinSprites;

	float getSound(){
		//get sound
		if(Microphone.IsRecording(null)){
			int sampleSize = 256;
			float[] _inData = new float[sampleSize];
			int pos = Microphone.GetPosition (null);
			int start = (pos - (sampleSize + 1) < 0) ? 0 : (pos - (sampleSize + 1));

			m_clip.GetData (_inData, start);
			float sum = 0;
			foreach (float d in _inData) {
				sum += Mathf.Abs (d);
			}

			return sum / sampleSize * m_volumControl.value;
//			float _peak = 0;
//			for (int i = 0; i < _inData.Length; ++i) {
//				if (_inData [i] > _peak) {
//					_peak = _inData [i];
//				}
//			}
//			if (m_volumControl != null)
//				return _peak * 100 * m_volumControl.value;
//			else
//				return _peak * 100;
		}
		return 0;
	}

	void SetSpeed(float soundLevel){
		//Debug.Log (soundLevel);
		//set speed base on delta sound
		var rb = GetComponent<Rigidbody2D> ();
		if (soundLevel >= 0.025f) {
			rb.velocity = new Vector2 (2.0f, rb.velocity.y);
			if (m_isLanding) {
				m_isLanding = false;
				//m_isTouching = false;
				//Debug.Log ("add force");
				if (soundLevel < 0.06f) {
					rb.AddForce (Vector2.up * 160f);
				} else {
					rb.AddForce (Vector2.up * 225f);
				}
//				float jump = (soundLevel - 2) * 50.0f / 100.0f + 2.0f ;
//				Debug.Log (jump);
//				rb.velocity = new Vector2 (rb.velocity.x, jump);//depend on volum
				//if((int)(LoadJson.LoadSavedUserInfo()["Sound"]) == 1)
				if(PlayerPrefs.GetInt("Sound") == 1)
					GetComponent<AudioSource>().Play();
//				if (!GetComponent<Animator> ().GetBool ("IsJumping")) {
//					GetComponent<Animator> ().SetBool ("IsJumping", true);
//				}
			} else {
				if (!m_isUp && m_isTouching) {
					rb.velocity = new Vector2 (rb.velocity.x, -2.0f);
				}
			}
		} else {
			rb.velocity = new Vector2 (2.0f, rb.velocity.y);
//			if (!GetComponent<Animator> ().GetBool ("IsJumping")) {
//				GetComponent<Animator> ().SetBool ("IsWalking", true);
//			}
		}
	}

	public void RebirthPlayer(){
		m_isTouching = false;
		m_isLanding = false;
		m_isUp = false;
		m_isOnStair = false;
		m_isOnTwoStair = false;
		m_clip = Microphone.Start (null, false, _maxTime, _recRate);

		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Collider2D> ().enabled = true;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Animator> ().SetBool ("IsDead", false);

		Vector2 pos = new Vector2(
			_lastStandBlock.transform.position.x + _lastStandBlock.transform.localScale.x / 2 - 1.0f,
			_lastStandBlock.transform.position.y + _lastStandBlock.transform.localScale.y / 2 + 1.0f
		);

		Debug.Log ("Player rebirth pos:" + pos.x + " , " + pos.y);

		transform.position = pos;
	}

	// Use this for initialization
	void Start () {
		m_isTouching = false;
		m_isLanding = false;
		m_isUp = false;
		m_isOnStair = false;
		m_isOnTwoStair = false;
		m_clip = Microphone.Start (null, false, _maxTime, _recRate);

		_skinSprites = Resources.LoadAll<Sprite> (string.Format ("Skin_{0}/", PlayerPrefs.GetInt("CurrentSkin")));
	}

	void Update(){
		if (DataManager._isPause == true || DataManager._waitForRebirth == true) {
			return;
		}
		if (m_isLanding) {
			if (GetComponent<Rigidbody2D> ().velocity.x != 0) {
				GetComponent<Animator> ().SetBool ("IsJumping", false);
				GetComponent<Animator> ().SetBool ("IsWalking", true);
			} else {
				GetComponent<Animator> ().SetBool ("IsJumping", false);
				GetComponent<Animator> ().SetBool ("IsWalking", false);
			}
		} else {
			GetComponent<Animator> ().SetBool ("IsJumping", true);
			GetComponent<Animator> ().SetBool ("IsWalking", false);
		}
	}

	void LateUpdate(){
		//
		if (PlayerPrefs.GetInt ("CurrentSkin") == 0)
			return;
		var renderer = GetComponent<SpriteRenderer>();
		string currentName = renderer.sprite.name;
		var newSprite = Array.Find (_skinSprites, item => item.name == currentName);
		if (newSprite) {
			renderer.sprite = newSprite;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		//get sound
		//if have sound then move x and detact m_isUp else set x 0 and 
		//if false 
		if (DataManager._isPause == true || DataManager._waitForRebirth == true) {
			return;
		}
		if (DataManager._isOver || DataManager._isFailed)
			return;
		var rb = GetComponent<Rigidbody2D> ();
		if (rb.velocity.y > 0) {
			m_isUp = true;
		} else {
			m_isUp = false;
		}
		float soundMax = getSound ();
		//Debug.Log ("get sound: " + soundMax);
		if (soundMax >= 0.008f) {
			SetSpeed (soundMax);
		}
		else{
			rb.velocity = new Vector2 (0.0f, rb.velocity.y);
//			if (GetComponent<Animator> ().GetBool ("IsWalking")) {
//				GetComponent<Animator> ().SetBool ("IsWalking", false);
//			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		//m_isTouching = true;
		if (other.gameObject.CompareTag ("Floor")) {
			//m_isTouching = true;
			m_isLanding = true;
			_lastStandBlock = other.transform.parent.gameObject;
		}
		if (other.gameObject.CompareTag ("Block")) {
			m_isTouching = true;
		}
		if (other.gameObject.CompareTag ("Stair")) {
			m_isTouching = true;
			m_isLanding = true;
//			Debug.Log ("enter next stair");
//			if (m_isOnStair) {
//				m_isOnTwoStair = true;
//				Debug.Log ("enter two stair");
//				Debug.Break ();
//			} else {
//				m_isOnTwoStair = false;
//			}
//			m_isOnStair = true;
			if (other.gameObject.GetComponent<StairMovement> ()._stairType != BlockType.BT_FALLSTAIR) {
				gameObject.transform.parent = other.transform;
			}
		}
		if(other.gameObject.CompareTag("Goal")){
			GetComponent<Rigidbody2D> ().isKinematic = true;
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent<Collider2D> ().enabled = false;
			GetComponent<Animator> ().SetBool ("IsGoal", true);
			DataManager._isOver = true;
		}
		if (other.gameObject.CompareTag ("Enemy")) {
			SetDead ();
		}
	}

	public void SetDead(){
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<Collider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Animator> ().SetBool ("IsDead", true);
		if (DataManager._isUsedRebirth) {
			DataManager._isFailed = true;
		} else {
			DataManager._waitForRebirth = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if(other.gameObject.CompareTag("Floor")){
			m_isLanding = false;
		}
		if(other.gameObject.CompareTag("Block")){
			m_isTouching = false;
		}
		if (other.gameObject.CompareTag ("Stair")) {
			//Debug.Log ("leave last stair");
			if (other.gameObject.GetComponent<StairMovement> ()._stairType != BlockType.BT_FALLSTAIR) {
				gameObject.transform.parent = null;
			}
//			else {
//				if (m_isOnTwoStair) {
//					Debug.Log ("exit two stair");
//					m_isOnTwoStair = false;
//					m_isOnStair = true;
//					m_isTouching = true;
//					m_isLanding = true;
//				} else {
//					m_isOnStair = false;
//					m_isTouching = false;
//					m_isLanding = false;
//				}
//			}

		}
	}

	void OnBecameInvisible() {
//		GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
//		GetComponent<Rigidbody2D> ().isKinematic = true;
//		GetComponent<Collider2D> ().enabled = false;
//		DataManager._isFailed = true;
	}

//	void OnTriggerEnter2D(Collider2D other) {
//		
//	}
}
