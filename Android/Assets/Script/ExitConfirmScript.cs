using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmScript : MonoBehaviour {

	public RawImage _mask;

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	}

	public void OnConfirmCallBack(){
		GetComponent<AudioSource> ().Play ();
		Application.Quit ();
	}

	public void OnCancelCallBack(){
		if (_mask) {
			GetComponent<AudioSource> ().Play ();
			_mask.gameObject.SetActive (false);
		}
	}
}
