using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataMask : MonoBehaviour {

	public GameObject _mask;

	public Sprite _ok;

	private bool _done;

	private RectTransform _transform;

	private int _count;

	// Use this for initialization
	void Start () {
		_count = 0;
		_transform = GetComponent<RectTransform> ();
		_done = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (DataManager._stageInfo != null && !_done) {
			StartCoroutine (LoadDone ());
		}
		if (!_done) {
			++_count;
			if (_count >= 6) {
				_count = 0;
				var rotat = _transform.localEulerAngles;
				rotat.z -= 30;
				_transform.localEulerAngles = rotat;
			}
		}
	}

	IEnumerator LoadDone(){
		_done = true;
		var rotat = _transform.localEulerAngles;
		rotat.z = 0;
		_transform.localEulerAngles = rotat;
		GetComponent<Image> ().sprite = _ok;
		yield return new WaitForSeconds (0.75f);
		_mask.SetActive (false);
	}
}
