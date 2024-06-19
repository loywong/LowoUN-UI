using System.Collections.Generic;
using LowoUN.Module.UI;
using UnityEngine;

namespace LowoUN.Business.UI {
	using HolderObjs = UIHandler.Objs_CheckDialog_Notify;

	public class UIBinderCheckDialog_Notify : UIBinder {
		public System.Action<UIEnum_CheckDlgBtnType> onBtnEvent;
		public string description {
			set {
				onUpdateTxt ((int) HolderObjs.Txt_Des, value);
			}
		}

		public UIEnum_CheckDlgBtnType buttonPressed { get; set; }

		public UIBinderCheckDialog_Notify (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart () {
			base.OnStart ();

		}

		protected override void OnEnd () {
			//throw new System.NotImplementedException ();
		}

		public void CallRegistMethod () {
			if (onBtnEvent != null)
				onBtnEvent (buttonPressed);
		}

		public void SetInfo (string des, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, System.Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			description = des;

			onBtnEvent = onBtnResponce;
			buttonPressed = UIEnum_CheckDlgBtnType.Cancel;

			onUpdateState ((int) HolderObjs.Btn_Confirm, UIStateType.Hide);
			onUpdateState ((int) HolderObjs.Btn_Cancel, UIStateType.Hide);

			if (!(btnsTextDict == null || btnsTextDict.Count == 0)) {
				if (btnsTextDict.ContainsKey (UIEnum_CheckDlgBtnType.Confirm)) {
					onUpdateState ((int) HolderObjs.Btn_Confirm, UIStateType.Show);
					onUpdateName ((int) HolderObjs.Btn_Confirm, btnsTextDict[UIEnum_CheckDlgBtnType.Confirm]);
				}
				if (btnsTextDict.ContainsKey (UIEnum_CheckDlgBtnType.Cancel)) {
					onUpdateState ((int) HolderObjs.Btn_Cancel, UIStateType.Show);
					onUpdateName ((int) HolderObjs.Btn_Cancel, btnsTextDict[UIEnum_CheckDlgBtnType.Cancel]);
				}

				//reset pos
				if (btnsTextDict.Count == 1) {
					GameObject objbtnInfo = UILinker.instance.GetGameObject (insID, (int) HolderObjs.Btn_Confirm);
					float y = 88f;
					if (objbtnInfo != null) {
						y = objbtnInfo.GetComponent<RectTransform> ().anchoredPosition.y;
					}
					if (btnsTextDict.ContainsKey (UIEnum_CheckDlgBtnType.Confirm))
						onUpdatePos ((int) HolderObjs.Btn_Confirm, new Vector2 (0f, y));
					else if (btnsTextDict.ContainsKey (UIEnum_CheckDlgBtnType.Cancel))
						onUpdatePos ((int) HolderObjs.Btn_Cancel, new Vector2 (0f, y));
				}
			}
		}
	}
}