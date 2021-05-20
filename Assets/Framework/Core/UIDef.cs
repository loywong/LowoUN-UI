using UnityEngine;
using System;

using UIPanelClass = LowoUN.Business.UI.UIPanelClass;
using UIPanelType = LowoUN.Business.UI.UIPanelType;

namespace LowoUN.Module.UI
{
	//???
	public enum UIEnum_Module_Bubble_Say {
		Op_Atk,
		Op_Heal,
		Op_Def,
		Op_Adv,
		Be_Atk,
		Be_Heal,
		Be_Def,
		Be_Adv,
		Speechless,
	}

	//1, enum
	public enum UIEnum_LoadingTyp {
		None,
		FromServer,
		FromClient,
	}

	public enum UIEnum_CheckDlgBtnType {
		Confirm,
		Cancel,
	}

	public enum UIEnum_SysNotifyType {
		Top,
		Center,
		Bottom,
	}

	public enum UIEnum_Hud_UDriectionType {
		Up,
		Down,
		Left,
		Right,
		LeftUp,
		RightUp,
		LeftDown,
		RightDown
	}

	public enum UIEnum_HudAct_Typ {
		Hong,
		Pa,
		Boom,
	}

	public enum UIEnum_HudTxt_PropType {
		None,
		Hp,
		Def,
		Addv,
	}

	public enum UIEnum_HudTxt_AlignU {
		None,
		Left,
		Middle,
		Right,
	}

	public enum UIEnum_DlgBG {
		None   = -1,  //无背景,即非Dialog类型
		A_100  = 0,   //NoAlpha, // alpha = 100%
		Normal = 1,   //Dark,    // alpha = 75%
		A_50,    	  //Normal,  // alpha = 50%
		A_25,    	  //Light,   //alpha = 25%
		A_0,          //全透明
	}

	//2, class
	[System.Serializable]
	public class UIObject
	{
		public int id;
		public GameObject obj;
	}

	[System.Serializable]
	public class UIEvent
	{
		public int _eventID;
		public GameObject obj;
	}

	//3, attribute
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class UIPrefabDesc : Attribute 
	{
		public string prefabName        { get; private set;}
		public UIPanelClass prefabClass { get; private set;}
		public bool isDialog            { get{ return (int)_dlgBgTyp > -1;} }
		private UIEnum_DlgBG _dlgBgTyp;
		public UIEnum_DlgBG dlgBgTyp    { get{ return _dlgBgTyp;}}

		public UIPrefabDesc(string prefabName, UIPanelClass panelClass = UIPanelClass.Comn, UIEnum_DlgBG atyp = UIEnum_DlgBG.None)
		{
			this.prefabName = prefabName;
			this.prefabClass = panelClass;
			this._dlgBgTyp = atyp;
		}
	}

	public class UIBinderAtt:Attribute
	{
		public UIPanelType panelType;

		public UIBinderAtt(UIPanelType panelType)
		{
			this.panelType = panelType;
		}
	}

	public class UIActionAtt:Attribute
	{
		public int eventID;
		public UIPanelType uiPanel;

		public UIActionAtt(int actionID, UIPanelType uiPanel)
		{
			this.eventID = actionID;
			this.uiPanel = uiPanel;
		}
	}

	public class EvtsAtt4UIInspector:Attribute
	{
		public UIPanelType uiPanelType;

		public EvtsAtt4UIInspector(UIPanelType uipanelType)
		{
			this.uiPanelType = uipanelType;
		}
	}

	public class ObjsAtt4UIInspector:Attribute
	{
		public UIPanelType uiPanelType;

		public ObjsAtt4UIInspector(UIPanelType uipanelType)
		{
			this.uiPanelType = uipanelType;
		}
	}

	public class EventsEnumAtt:Attribute
	{
		public UIPanelType uiPanel;
		public Enum eventsEnum;

		public EventsEnumAtt(UIPanelType uiPanel, Enum eventsEnum)
		{
			this.uiPanel = uiPanel;
			this.eventsEnum = eventsEnum;
		}
	}

	public class Events4NotifyAtt:Attribute
	{
		public bool isRegister;

		public Events4NotifyAtt(bool isRegister)
		{
			this.isRegister = isRegister;
		}
	}
}