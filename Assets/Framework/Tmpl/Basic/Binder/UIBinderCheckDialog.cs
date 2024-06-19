using System.Collections.Generic;
using LowoUN.Module.UI;
using UnityEngine;

namespace LowoUN.Business.UI {
	using HolderObjs = UIHandler.Objs_CheckDialog;

	public class UIBinderCheckDialog : UIBinder {
		public System.Action<UIEnum_CheckDlgBtnType> onBtnEvent;

		public string title {
			set {
				onUpdateTxt ((int) HolderObjs.Txt_Title, value);
			}
		}
		public string description {
			set {
				onUpdateTxt ((int) HolderObjs.Txt_Des, value);
			}
		}

		private IDictionary<UIEnum_CheckDlgBtnType, string> _btnsTextDict;

		public UIEnum_CheckDlgBtnType buttonPressed { get; set; }

		public UIBinderCheckDialog (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart () {
			base.OnStart ();

			title = "Dialog";
		}

		#region implemented abstract members of UIBinder

		protected override void OnEnd () {
			//throw new System.NotImplementedException ();
		}

		#endregion

		public override void OnBtnClose () {
			if (onBtnEvent == null) {
				base.OnBtnClose ();
				Cancel ();
			}
		}

		public void Confirm () {
			UIHub.instance.CloseUI (insID);
			buttonPressed = UIEnum_CheckDlgBtnType.Confirm;
			CallRegistMethod ();
		}

		public void Cancel () {
			UIHub.instance.CloseUI (insID);
			buttonPressed = UIEnum_CheckDlgBtnType.Cancel;
			CallRegistMethod ();
		}

		public void CallRegistMethod () {
			if (onBtnEvent != null)
				onBtnEvent (buttonPressed);
		}

		public void SetInfo (string title, string des, IDictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, System.Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			if (title != null) this.title = title;
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
				this._btnsTextDict = btnsTextDict;

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