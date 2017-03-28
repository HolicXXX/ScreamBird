using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager {

	public static bool _isFirstEnterStartScene = true;

	public static int _stageJsonObj_1 = 11;
	public static int _stageJsonObj_2 = 28;
	public static int _stageJsonObj_3 = 27;
	public static int[] _stageObjNum = { 11, 28, 27 };

	public static string NickName {
		get;
		set;
	}

	public static int _passedNum{ 
		get; 
		set;
	}

	public static int _currentStage{ get; set; }

	public static bool _needUpdateRank{ get; set;}

	public static float _volumNum{ get; set;}

	public static bool _isOver{ get; set;}
	public static float _timeCount{ get; set; }
	public static bool _isFailed{ get; set; }
	public static bool _isPause{ get; set;}

	public static bool _didRec{ get; set; }
	public static bool _isRec{ get; set; }
	public static bool _waitForRebirth{ get; set; }
	public static bool _isUsedRebirth{ get; set; }

	public static int _money{ get; set; }
	public static int _moneyGetToday{ get; set; }

	public static int _deathCount{ get; set; }

	public static bool _waitOperate{ get; set; }
}
