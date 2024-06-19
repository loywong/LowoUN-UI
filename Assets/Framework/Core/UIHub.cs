using System;
using System.Collections.Generic;
using System.Linq;
using LowoUN.Business.UI;
using LowoUN.Module.UI.HUDText;
using LowoUN.Util;
using LowoUN.Util.Notify;
using UnityEngine;
using UIPanelClass = LowoUN.Business.UI.UIPanelClass;
using UIPanelType = LowoUN.Business.UI.UIPanelType;

namespace LowoUN.Module.UI {
	public sealed class UIHub {
		private static UIHub _instance;
		public static UIHub instance {
			get {
				if (_instance == null)
					_instance = new UIHub ();

				return _instance;
			}
		}

		public int ScreenWidthUsedByWeb = -1;
		public int ScreenHeightUsedByWeb = -1;

		//compose sub utils
		private UIBrowsing _UIBrowsing = new UIBrowsing ();

		private UIHub () {
			CachePanelTypeName ();
		}

		public void OnStart () {
			UILinker.instance.OnStart ();
		}

		public void OnUpdate () {
			UILinker.instance.OnUpdate ();
		}

		public void OnEnd () {
			UILinker.instance.OnEnd ();
			Clear ();
		}

		private Dictionary<string, UIPanelType> cachePanelTypeName = new Dictionary<string, UIPanelType> ();
		private void CachePanelTypeName () {
			foreach (int intValue in System.Enum.GetValues (typeof (UIPanelType))) {
				//Debug.Log ("intValue : " + intValue.ToString());
				//Debug.Log ("stringValue : " + System.Enum.GetName(typeof(UIPanelIns), intValue));
				cachePanelTypeName[Enum.GetName (typeof (UIPanelType), intValue)] = (UIPanelType) intValue;
			}
		}

		private List<int> GetHolderInsIDs (UIPanelType uiPanelType) {
			List<int> ids = UILinker.instance.uiHolderDict.Where (i => i.Value.typeID == uiPanelType).Select (i => i.Key).ToList ();
			return ids;
		}

		public List<UIHolder> GetHolders (UIPanelType uiHolderType) {
			List<UIHolder> holders = UILinker.instance.uiHolderDict.Where (i => i.Value.typeID == uiHolderType).Select (i => i.Value).ToList ();
			return holders;
		}

		public UIHolder GetHolder (int insID) {
			return UILinker.instance.uiHolderDict.Val (insID);
		}

		public UIPanelType GetHolderToppest () {
			var ps = UILinker.instance.uiHolderDict
				.Where (i => i.Value != null && !i.Value.isEmbed && i.Value.typeID != UIPanelType.DialogBG && UILinker.instance.GetPrefabClass (i.Value.typeID) != UIPanelClass.Notify)
				.Select (i => i.Value).ToList ()
				.OrderByDescending (i => i.insID).ToList ();

			if (ps.Any ())
				return ps[0].typeID;

			return UIPanelType.None;
		}

		public List<T> GetBinders<T> (UIPanelType uiHolderType) where T : UIBinder {
			List<T> ts = UILinker.instance.uiHolderDict.Where (i => i.Value.typeID == uiHolderType).Select (i => i.Value.GetBinder () as T).ToList ();
			return ts;
		}

		public T GetBinder<T> (int instID) where T : UIBinder {
			return UILinker.instance.uiHolderDict.Val (instID) == null?null : UILinker.instance.uiHolderDict.Val (instID).GetBinder<T> ();
		}

		private string GetDlgBGPref (int pid) {
			switch (UILinker.instance.GetPaneDialogBgTyp (pid)) {
				case UIEnum_DlgBG.Normal:
					return "UI_DialogBG_Nomal_75";
				case UIEnum_DlgBG.A_100:
					return "UI_DialogBG_A_100";
				case UIEnum_DlgBG.A_25:
					return "UI_DialogBG_A_25";
				case UIEnum_DlgBG.A_0:
					return "UI_DialogBG_A_0";
				default:
					return "UI_DialogBG_Nomal_75";
			}
		}

		//manage ui ---------------------------------------------------------------------------
		//public void PreloadUI (int panelTypeID) 
		public GameObject LoadUI (int panelTypeID) {
			return LoadUI ((UIPanelType) panelTypeID);
		}

		public GameObject LoadUI (UIPanelType uiPanelType, string prefabName = null) {
			//HACK for lobby main menu
			//if(UIHub.instance.IsTheFirstOneOfScene () && !CheckGlobalPanels(uiPanelType) && ! CheckSpecialPanel(uiPanelType) && !UIScene.instance.IsSceneDefaultUI((int)uiPanelType))
			//	UINotifyMgr.Broadcast<int> ("UI_LoadUIPanel4LobbyMainMenu", (int)uiPanelType);

			bool isDialog = UILinker.instance.CheckPanelIsDialog ((int) uiPanelType);

			UIHolder bg = null;
			if (isDialog) {
				bg = LoadUI (UIPanelType.DialogBG, GetDlgBGPref ((int) uiPanelType)).GetComponent<UIHolder> ();
				UIAsset.instance.SetDialogBgRoot (uiPanelType, bg.transform);
			}

			GameObject obj = UIAsset.instance.LoadPanelByType ((int) uiPanelType, UIAsset.instance.GetUIRootTyp ((int) uiPanelType), prefabName);
			if (obj != null) {
				_UIBrowsing.Backup4BrowseUI (obj.GetComponent<UIHolder> (), true);

				if (isDialog)
					obj.GetComponent<UIHolder> ().dialogBg = bg.insID;

				bool isCheckDlg = UIAsset.instance.GetUIRootTyp ((int) uiPanelType) == Enum_UIAsset.CheckDlg;
				//set close event to it's reletive dark bg
				if (isDialog && !isCheckDlg)
					BindCloseEvtToDarkBg (obj.GetComponent<UIHolder> (), bg);

				//!!!!!!
				NotifyMgr.Broadcast<int> ("UI_LoadUIPanel", (int) uiPanelType);
			} else {
#if UNITY_EDITOR
				Debug.LogWarning ("====== LowoUN-UI ===> Doesn't fine ui prefab with the uiPanelType: " + uiPanelType.ToString ());
#endif
			}

			return obj;
		}

		private void BindCloseEvtToDarkBg (UIHolder p, UIHolder bg) {
			if (bg == null || p == null)
				return;

			//bg.GetBinder<UIBinderDialogBG> ().SetHostInsid (p.insID);
			UIBinder b = p.GetBinder ();
			if (b != null)
				bg.GetBinder<UIBinderDialogBG> ().onTriggerHostPanel += b.OnBtnClose; //CloseUI(p.insID);
			else
				Debug.LogError (LowoUN.Util.Log.Format.UI () + "no binder has been constructed for the panel: " + p.typeID.ToString ());
		}

		public void LoadCheckDialogUI (string title, string des, IDictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			var binder = LoadUI (UIPanelType.CheckDialog).GetComponent<UIHolder> ().GetBinder<UIBinderCheckDialog> ();
			binder.SetInfo (title, des, btnsTextDict, onBtnResponce);
		}

		public void LoadCheckDialogUI_OneBtn (string title, object obj, Action onBtnRespConfirm = null, Action onBtnRespCancel = null, string btnConfirmName = "") {
			var binder = LoadUI (UIPanelType.CheckDialog_OneBtn).GetComponent<UIHolder> ().GetBinder<UIBinderCheckDialog_OneBtn> ();
			binder.SetInfo (title, obj, onBtnRespConfirm, onBtnRespCancel, btnConfirmName);
		}

		public void LoadCheckCostDialogUI (string costtitle, string costIcon, int cost, string des, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			UIBinderCheckCostDialog binder = LoadUI (UIPanelType.CheckCostDialog).GetComponent<UIHolder> ().GetBinder () as UIBinderCheckCostDialog;
			binder.SetInfo (costtitle, costIcon, cost, des, onBtnResponce, btnsTextDict);
		}

		public void LoadCheckDialog_Shop (string ReardTitle, string Cost, string FirstRewardDes, int mode = 0, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			UIBinderCheckDialog_Shop binder = LoadUI (UIPanelType.CheckDialog_Shop).GetComponent<UIHolder> ().GetBinder () as UIBinderCheckDialog_Shop;
			binder.SetInfo (ReardTitle, Cost, FirstRewardDes, mode, btnsTextDict, onBtnResponce);
		}

		public void LoadCheckDialog_Warning (string des, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			var binder = LoadUI (UIPanelType.CheckDialog_Warning).GetComponent<UIHolder> ().GetBinder<UIBinderCheckDialog_Warning> ();
			binder.SetInfo (des, btnsTextDict, onBtnResponce);
		}

		public void LoadCheckDialog_Notify (string des, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			var binder = LoadUI (UIPanelType.CheckDialog_Notify).GetComponent<UIHolder> ().GetBinder<UIBinderCheckDialog_Notify> ();
			binder.SetInfo (des, btnsTextDict, onBtnResponce);
		}

		private GameObject _notifyGo;
		public void LoadSysNotifyUI (string des, float time = 2f, int sysNotifyType = (int) LowoUN.Module.UI.UIEnum_SysNotifyType.Center) {
			CloseUI (UIPanelType.SystemNotify);
			_notifyGo = LoadUI (UIPanelType.SystemNotify);
			UIHudText_Notify_CacheOne.instance.Setinfo (_notifyGo, des, time, sysNotifyType);
		}

		public void LoadWebViewUI (string title, string url, Dictionary<Vector2, float> poses = null) { //, int charaID = -1
			UIHolder holder = LoadUI (UIPanelType.WebView).GetComponent<UIHolder> ();
			UIBinderWebView binder = holder.GetBinder () as UIBinderWebView;
			binder.SetInfo (title, url, poses); //, charaID
		}

		public void CloseUI (UIPanelType panelType) {
			GetHolderInsIDs (panelType).ForEach (i => CloseUI (i));
		}

		public void CloseUI (int instanceID) {
			//CheckEmbedPanelIsNotDialog (instanceID);
			RemoveUI (instanceID);
		}

		public void CloseUI4PopupAll (int instanceID) {
			//CheckEmbedPanelIsNotDialog(instanceID);
			RemoveUI4PopupAll (instanceID);
		}

		//TOREMOVE 由于嵌入式panel非显式的加载卸载，所以不需要一下判断 //提醒：嵌入式面板的加载方式和普通加载不一致，其不能为dialog类型
		/*private void CheckEmbedPanelIsNotDialog(int instanceID) {
			UIHolder holder = UILinker.instance.uiHolderDict.Val (instanceID);
			if (holder != null) {
				if (holder.isEmbed) 
					if (UILinker.instance.CheckPanelIsDialog ((int)holder.typeID)) 
						Debug.LogError (string.Format(LowoUN.Util.Log.Format.UI() + "a embed holder: {0} should not be the dialog type!!!", holder.typeID.ToString()));
			}
		}*/

		private void RemoveUI (int instanceID) {
			if (UILinker.instance.uiHolderDict.ContainsKey (instanceID)) {
				UIHolder holder = UILinker.instance.uiHolderDict[instanceID];

				UILinker.instance.RemoveHolderDictElement (instanceID);

				if (holder != null) {
					_UIBrowsing.Backup4BrowseUI (holder, false);
					holder.OnClose ();
				}

				//MAYBE: UINotifyMgr.Broadcast
			} else {
#if UNITY_EDITOR
				Debug.LogWarning ("uiHolderDict dosen't have instance: " + instanceID);
#endif
			}
		}

		private void RemoveUI4PopupAll (int instanceID) {
			if (UILinker.instance.uiHolderDict.ContainsKey (instanceID)) {
				UIHolder h = UILinker.instance.uiHolderDict[instanceID];

				UILinker.instance.RemoveHolderDictElement (instanceID);

				if (h != null)
					h.OnClose ();

				//MAYBE: UINotifyMgr.Broadcast
			}
		}

		public void LoadWaiting () {
			LoadUI (UIPanelType.Waiting);
		}

		//public void CloseWaiting (int instanceID) 
		public void CloseWaiting () {
			CloseUI (UIPanelType.Waiting);
		}

		private void Clear () {
			Dictionary<int, UIHolder> toRemoveHolder = new Dictionary<int, UIHolder> ();
			foreach (KeyValuePair<int, UIHolder> item in UILinker.instance.uiHolderDict) {
				toRemoveHolder.Add (item.Key, item.Value);
			}

			_UIBrowsing.OnReset ();

			foreach (var item in toRemoveHolder) {
				item.Value.OnClose ();
				UILinker.instance.RemoveHolderDictElement (item.Key);
			}
			//needn't do this
			//UILinker.instance.uiHolderDict.Clear ();
		}

		public void Clear4LoadUI (UIPanelType newPanel) {
			//ClearExceptSceneDefault ();
			NotifyMgr.Broadcast<UIPanelType> ("UI_LoadUIPanel4LobbyMainMenu", newPanel);
			LoadUI (newPanel);
		}

		public void ClearExceptSceneDefault () {
			_UIBrowsing.RollbackAll ();
		}

		public void ClearSceneAll () {
			//1, 不清除全局公用的对象，比如Loading
			//2, 不需要清除内嵌面板，因为统一由其宿主面板管理生命周期
			//3, 不需要dialogBg面板，因为也是由其主面板控制生命周期
			UILinker.instance.uiHolderDict
				.Where (i => !CheckGlobalPanels (i.Value.typeID) &&
					i.Value.typeID != UIPanelType.DialogBG &&
					!i.Value.isEmbed).ToList ()
				.ForEach (i => {
					UILinker.instance.RemoveHolderDictElement (i.Key);
					i.Value.OnClose ();
				});

			_UIBrowsing.OnReset ();
		}

		//UIGameObject support------------------------------------------------------------------
		public void ToggleItem (GameObject go, bool isActive) {
			UIGameObject.ToggleItem (go, isActive);
		}
		public void ShowPanel (int insID) {
			UIGameObject.ShowPanel (UILinker.instance.uiHolderDict, insID);
		}
		public void ShowPanel (UIPanelType uiPanel) {
			GetHolderInsIDs (uiPanel).ForEach (i => UIGameObject.ShowPanel (UILinker.instance.uiHolderDict, i));
		}
		public void HidePanel (int insID) {
			UIGameObject.HidePanel (UILinker.instance.uiHolderDict, insID);
		}
		public void HidePanel (UIPanelType uiPanel) {
			GetHolderInsIDs (uiPanel).ForEach (i => UIGameObject.HidePanel (UILinker.instance.uiHolderDict, i));
		}
		public void ShowAll () {
			UIGameObject.ToggleAll (UILinker.instance.uiHolderDict, true);
		}
		public void HideAll () {
			UIGameObject.ToggleAll (UILinker.instance.uiHolderDict, false);
		}

		//for uibrowsing------------------------------------------------------------------------
		public bool CheckSpecialPanel (UIPanelType panelTyp) {
			return UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Test ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Module ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Global ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Basic ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Notify ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Award ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Dlg ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Comn ||
				UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Comn_Map;
		}

		public bool CheckGlobalPanels (UIPanelType panelTyp) {
			//			if (panelTyp == UIPanelType.Loading
			//                || panelTyp == UIPanelType.ItemReward)
			//				return true;
			//
			//			return false;

			return UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Global;
		}

		public bool IsTheFirstOneOfScene () {
			return _UIBrowsing.IsTheFirstOne ();
		}

		private bool CheckSpecialPanel4NotUsingUI (UIPanelType panelType) {
			return UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Test ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Module ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Global ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Basic ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Notify ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Award ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Dlg;
		}

		public void CheckUsingUIByLoad (UIHolder holder) {
			if (holder == null)
				return;
			if (CheckSpecialPanel4NotUsingUI (holder.typeID))
				return;
			if (holder.isEmbed)
				return;
			if (UIScene.instance.IsSceneDefaultUI ((int) holder.typeID))
				return;

#if UNITY_EDITOR
			Debug.Log ("====== LowoUN-UI ===> Load: HolderToppest: " + UIHub.instance.GetHolderToppest ().ToString ());
#endif
			if (UIHub.instance.GetHolderToppest () == UIPanelType.None)
				NotifyMgr.Broadcast<bool> ("UI_IsUsingUI", false);
			else
				NotifyMgr.Broadcast<bool> ("UI_IsUsingUI", !UIScene.instance.IsSceneDefaultUI ((int) UIHub.instance.GetHolderToppest ()));
		}

		public void CheckUsingUIByUnload (UIHolder holder) {
			if (holder == null)
				return;
			if (CheckSpecialPanel4NotUsingUI (holder.typeID))
				return;
			if (UIScene.instance.IsSceneDefaultUI ((int) holder.typeID))
				return;

#if UNITY_EDITOR
			Debug.Log ("====== LowoUN-UI ===> Unload: HolderToppest: " + UIHub.instance.GetHolderToppest ().ToString ());
#endif
			if (UIHub.instance.GetHolderToppest () == UIPanelType.None)
				NotifyMgr.Broadcast<bool> ("UI_IsUsingUI", false);
			else
				NotifyMgr.Broadcast<bool> ("UI_IsUsingUI", !UIScene.instance.IsSceneDefaultUI ((int) UIHub.instance.GetHolderToppest ()));
		}

		public void RestartAllUI () {
			UILinker.instance.uiHolderDict
				.Where (i => !i.Value.isEmbed || i.Value.isSonGeneral)
				.Select (i => i.Value).ToList ()
				.ForEach (i => {
					if (i.GetBinder () != null)
						i.GetBinder ().OnStart ();
				});
		}

		//for common back-----------------------------------------------------------------------
		public void CommonBack () {
			if (IsGameGlobalEvents () || IsToppestCheckDlgTyp ())
				return;

			if (_UIBrowsing.Popup ())
				return;

			//没有可回退的面板（最初始场景），则退出当前场景
			if (!UIScene.instance.Logout ()) {

				//没有可退出的场景（最初始场景），则退出游戏
				if (Application.platform == RuntimePlatform.Android) {
					UIHub.instance.LoadCheckDialog_Notify (
						"Title" /*DataParserHelper.getMessage(10018)*/ ,
						new Dictionary<UIEnum_CheckDlgBtnType, string> () { { UIEnum_CheckDlgBtnType.Confirm, "Confirm" /*DataParserHelper.getMessage(4015)*/ } },
						btn => {
							if (btn == UIEnum_CheckDlgBtnType.Confirm) {
								Application.Quit ();
							}
						}
					);
				}
			}
		}

		private bool IsToppestCheckDlgTyp () {
			//当前场景没有可关闭的面板，则退出当前场景
			UIPanelType panelTyp = UIHub.instance.GetHolderToppest ();
			return UILinker.instance.GetPrefabClass (panelTyp) == UIPanelClass.Dlg;
		}

		private bool IsGameGlobalEvents () {
			//新手引导和剧情播放 等游戏全局事件状况下，无法回退
			//if ((LowoUN.Module.Guide.Module_Guide.instance!= null && LowoUN.Module.Guide.Module_Guide.instance.isPlaying)
			//	|| (LowoUN.Module.Cutscene.Module_Cutscene.instance != null && LowoUN.Module.Cutscene.Module_Cutscene.instance.isPlaying))
			//	return;

			return false;
		}
	}
}