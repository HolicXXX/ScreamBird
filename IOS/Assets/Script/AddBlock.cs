using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class AddBlock : MonoBehaviour {

	public GameObject goalPrefab;
	public GameObject blockPrefab;
	public GameObject blockTopPrefab;
	public GameObject enemyPrefab;
	public GameObject stairPrefab;
	public GameObject upSpikePrefab;
	public GameObject downSpikePrefab;

	public Camera mainCamera;

	public GameObject _playerTips1;
	public GameObject _playerTips2;

	// Use this for initialization
	void Start () {
		List<BlockBase> list = LoadJson.LoadJsonByName (DataManager._stageInfo);
		//Debug.Log (list.Count);
		foreach (BlockBase b in list) {
			addBlockWithData (b);
			//Debug.Log (b._id.ToString() + " " + b._type.ToString() + " " + b._startX.ToString() + " " + b._endX.ToString() + " " + b._Y.ToString());
		}
		if (DataManager._currentStage != 1) {
			_playerTips1.SetActive (false);
			_playerTips2.SetActive (false);
		} else {
			_playerTips1.GetComponent<TextMesh>().text = LoadJson.GetLanguageText (_playerTips1.name);
			_playerTips2.GetComponent<TextMesh>().text = LoadJson.GetLanguageText (_playerTips2.name);
		}

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void addBlockWithData(BlockBase data){
		switch (data._type) {
		case BlockType.BT_GOAL:
			{
				//Debug.Log ("add goal");
				GameObject goal = Instantiate (goalPrefab, new Vector2 (data._startX / 100.0f + 0.385f, data._Y / 100.0f + 0.555f), Quaternion.identity);
			}
			break;
		case BlockType.BT_DOWNBLOCK:
			{
				//Debug.Log ("add downblock");
				GameObject dblock = Instantiate (blockPrefab, Vector2.zero, Quaternion.identity);
				Vector3 scale = new Vector3 ((data._endX - data._startX) / 100.0f, 30.0f, 1.0f);
				Vector2 pos = new Vector2 (data._startX / 100.0f + scale.x / 2, data._Y / 100.0f - 15.0f);
				dblock.transform.localScale = scale;
				dblock.transform.position = pos;

				//add top colorblock
				GameObject btop = Instantiate(blockTopPrefab,Vector2.zero,Quaternion.identity);

				//btop.transform.parent = dblock.transform;
				btop.transform.localScale = new Vector3 (scale.x, 0.15f, 1.0f);

				btop.transform.position = new Vector2 (pos.x, pos.y + 15.0f - 0.075f);
				dblock.transform.SetParent (transform);
			}
			break;
		case BlockType.BT_UPBLOCK:
			{
				//Debug.Log ("add upblock");
				GameObject dblock = Instantiate (blockPrefab, Vector2.zero, Quaternion.identity);
				Vector3 scale = new Vector3 ((data._endX - data._startX) / 100.0f, 30.0f, 1.0f);
				Vector2 pos = new Vector2 (data._startX / 100.0f + scale.x / 2, data._Y / 100.0f + 15.0f);
				dblock.transform.localScale = scale;
				dblock.transform.position = pos;
				dblock.transform.SetParent (transform);
			}
			break;
		case BlockType.BT_DOWNSPIKE:
			{
				//Debug.Log ("add downSpike");
				int num = Mathf.FloorToInt((data._endX - data._startX) / 33.0f);
				for (int i = 0; i < num; ++i) {
					GameObject ds = Instantiate (downSpikePrefab, Vector2.zero, Quaternion.identity);
					Vector2 pos = new Vector2((data._startX + i * 33.0f) / 100.0f + 0.165f,data._Y / 100.0f + 0.23f);
					ds.transform.position = pos;
					ds.transform.SetParent (transform);
				}
			}
			break;
		case BlockType.BT_UPSPIKE:
			{
				//Debug.Log ("add upSpike");
				int num = Mathf.FloorToInt((data._endX - data._startX) / 33.0f);
				for (int i = 0; i < num; ++i) {
					GameObject ds = Instantiate (upSpikePrefab, Vector2.zero, Quaternion.identity);
					Vector2 pos = new Vector2((data._startX + i * 33.0f) / 100.0f + 0.165f,data._Y / 100.0f - 0.23f);
					ds.transform.position = pos;
					ds.transform.SetParent (transform);
				}
			}
			break;
		case BlockType.BT_STAIR:
			{
				//Debug.Log ("add stair");
				GameObject stair = Instantiate (stairPrefab, Vector2.zero, Quaternion.identity);
				float scaleX = (data._endX - data._startX) / 100.0f;
				float posX = data._startX / 100.0f + scaleX / 2;
				float posY = data._Y / 100.0f - 0.075f;
				stair.transform.position = new Vector2 (posX, posY);
				stair.transform.localScale = new Vector3 (scaleX, 0.15f, 1.0f);
				stair.transform.SetParent (transform);
				var script = stair.GetComponent<StairMovement> ();
				script._originPos = new Vector2 (posX, posY);
			}
			break;
		case BlockType.BT_LRSTAIR:
			{
				//Debug.Log ("ADD LRStair");
				GameObject stair = Instantiate (stairPrefab, Vector2.zero, Quaternion.identity);
				float scaleX = (data._endX - data._startX) / 100.0f;
				float posX = data._startX / 100.0f + scaleX / 2;
				float posY = data._Y / 100.0f - 0.075f;
				stair.transform.position = new Vector2 (posX, posY);
				stair.transform.localScale = new Vector3 (scaleX, 0.15f, 1.0f);
				var script = stair.GetComponent<StairMovement> ();
				script.moveMin = data._stairXStart / 100.0f;
				script.moveMax = data._stairXEnd / 100.0f;
				script.moveTime = data._stairMoveTime;
				script._stairType = BlockType.BT_LRSTAIR;
				script._originPos = new Vector2 (posX, posY);
				stair.transform.SetParent (transform);
			}
			break;
		case BlockType.BT_UDSTAIR:
			{
				//Debug.Log ("ADD UDStair");
				GameObject stair = Instantiate (stairPrefab, Vector2.zero, Quaternion.identity);
				float scaleX = (data._endX - data._startX) / 100.0f;
				float posX = data._startX / 100.0f + scaleX / 2;
				float posY = data._Y / 100.0f - 0.075f;
				stair.transform.position = new Vector2 (posX, posY);
				stair.transform.localScale = new Vector3 (scaleX, 0.15f, 1.0f);
				var script = stair.GetComponent<StairMovement> ();
				script.moveMin = data._stairYStart / 100.0f;
				script.moveMax = data._stairYEnd/ 100.0f;
				script.moveTime = data._stairMoveTime;
				script._stairType = BlockType.BT_UDSTAIR;
				script._originPos = new Vector2 (posX, posY);
				stair.transform.SetParent (transform);
			}
			break;
		case BlockType.BT_FALLSTAIR:
			{
				//Debug.Log ("ADD FallStair");
				GameObject stair = Instantiate (stairPrefab, Vector2.zero, Quaternion.identity);
				float scaleX = (data._endX - data._startX) / 100.0f;
				float posX = data._startX / 100.0f + scaleX / 2;
				float posY = data._Y / 100.0f - 0.075f;
				stair.transform.position = new Vector2 (posX, posY);
				stair.transform.localScale = new Vector3 (scaleX, 0.15f, 1.0f);
				var script = stair.GetComponent<StairMovement> ();
				script.fallDelay = data._stairDelay;
				script._stairType = BlockType.BT_FALLSTAIR;
				script._originPos = new Vector2 (posX, posY);
				stair.transform.SetParent (transform);
			}
			break;
		case BlockType.BT_ENEMY:
			{
				//Debug.Log ("Add Enemy");
				GameObject enemy = Instantiate (enemyPrefab, new Vector2 (data._startX / 100.0f + 0.27f, data._Y / 100.0f + 0.51f), Quaternion.identity);
				enemy.GetComponent<EnemyMove> ()._mainCamera = mainCamera;
				enemy.transform.SetParent (transform);
			}
			break;
		};
	}
}
