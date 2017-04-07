using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class DataManager {

	public static bool _isFirstEnterStartScene = true;

	public static int _stageMaxNum = 3;

	public static JsonData _stageInfo = null;

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

	public static int _languageIndex{ 
		get { 
			if (Application.systemLanguage.ToString () != "Chinese" && Application.systemLanguage.ToString() != "ChineseSimplified") {
				return 2;
			}
			return 1;
		}
	}

	public static Dictionary<string,int> _nameIndex = new Dictionary<string, int> {
		{"Title",0},
		{"StartButton",1},
		{"MN_Title",2},
		{"MN_Info_1",3},
		{"MN_Info_2",4},
		{"MN_getCount",5},
		{"MN_CopyLink_Text",6},
		{"MN_ShareLink_Text",7},
		{"Music_Label",8},
		{"Sound_Label",9},
		{"1_4",10},
		{"2_4",11},
		{"3_4",12},
		{"R_Title_Label",13},
		{"RankNumber",14},
		{"C_Title",15},
		{"OnceTipText",16},
		{"Tips_1",17},
		{"Tips_2",18},
		{"PauseImage",19},
		{"FailedImage",20},
		{"F_EditRec_Text",21},
		{"RB_Title",22},
		{"RB_Rebirth_Text",23},
		{"RB_MoneyNum",24},
		{"WinImage_Text",25},
		{"W_NextStageButton_Text",26},
		{"W_EditRec_Text",21},
		{"CN_Confirm_Text",27},
		{"CN_Cancel_Text",28},
		{"E_Confirm_Text",27},
		{"E_Cancel_Text",28},
		{"S_Title",29},
		{"E_Text",30},
		{"TipComfirmButton_Text",27},
		{"S_CT_Comfirm_Text",27},
		{"S_CT_Cancle_Text",28},
		{"RB_LessMoneyTipText",31},
		{"RB_MoneyInfo_Text",32},
		{"CopyTips",33},
		{"CopyLinkText",34},
		{"Sensitvity",35},
		{"NI_Text",36},
		{"NI_Retry_Text",37},
		{"NI_Exit_Text",38},
		{"LD_Text",39}
	};
}
