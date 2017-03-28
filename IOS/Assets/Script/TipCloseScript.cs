using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipCloseScript : MonoBehaviour {

	public RawImage _mask;
	public bool _closeMask;
	public Text _tipText;

	// Use this for initialization
	void Start () {
		_closeMask = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnConfirmButtonCallBack(){
		gameObject.SetActive (false);
		if (_closeMask) {
			if (_mask != null) {
				gameObject.GetComponent<AudioSource> ().Play ();
				_mask.gameObject.SetActive (false);
			}
		}
	}
}
