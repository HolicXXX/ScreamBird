using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyTips : MonoBehaviour {

//	public float _delay;
//
//	private float _count;

	// Use this for initialization
	void Start () {
//		_count = 0.0f;
		StartCoroutine(removeTip());
	}
	
	// Update is called once per frame
	void Update () {
//		_count += Time.deltaTime;
//		if (_count < 0.5f) {
//			return;
//		}
//		var color = GetComponent<RawImage> ().color;
//		float alpha = (1.0f - _count) * 2.0f * color.a;
//		if (alpha < 0.0f)
//			alpha = 0.0f;
//		color = new Color (0.0f, 0.0f, 0.0f, alpha);
//		if (color.a == 0)
//			GameObject.Destroy (gameObject);
	}

	IEnumerator removeTip(){
		yield return new WaitForSeconds (1.0f);
		GameObject.Destroy (gameObject);
	}
}
