using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour {

	public RawImage _mask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickCallBack(){
		GetComponent<AudioSource> ().Play ();
		_mask.gameObject.SetActive (false);
	}
}
