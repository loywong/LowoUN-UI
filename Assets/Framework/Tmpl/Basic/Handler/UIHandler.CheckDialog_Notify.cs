using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_CheckDialog_Notify {
			None,
			Btn_Confirm,
			Btn_Cancel,
			Txt_Des,
		}

		public enum Evts_CheckDialog_Notify {
			None,
			Btn_Confirm,
			Btn_Cancel,
			Btn_Close,
		}

		[UIBinderAtt (UIPanelType.CheckDialog_Notify)]
		public UIBinder GetUIData4CheckDialog_Notify (int instanceID) {
			return new UIBinderCheckDialog_Notify ((int) UIPanelType.CheckDialog_Notify, instanceID);
		}

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.CheckDialog_Notify)]
		public List<string> SetInspectorObjectEnum4CheckDialog_Notify () {
			return UILinker.instance.GetEnumNameList<Objs_CheckDialog_Notify> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.CheckDialog_Notify)]
		public List<string> SetInspectorEventEnum4CheckDialog_Notify () {
			return UILinker.instance.GetEnumNameList<Evts_CheckDialog_Notify> ();
		}
#endif

		[UIActionAtt ((int) Evts_CheckDialog_Notify.Btn_Close, UIPanelType.CheckDialog_Notify)]
		public void OnCloseCheckDialog_Notify (params object[] arr) {
			OnCancelCheckDialog_Notify (arr);
		}

		[UIActionAtt ((int) Evts_CheckDialog_Notify.Btn_Confirm, UIPanelType.CheckDialog_Notify)]
		public void OnConfirmCheckDialog_Notify (params object[] arr) {
			UIBinderCheckDialog_Notify b = UIHub.instance.GetBinder<UIBinderCheckDialog_Notify> ((int) arr[0]);
			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Confirm;

			UIHub.instance.CloseUI ((int) arr[0]);

			if (b != null)
				b.CallRegistMethod ();
		}

		[UIActionAtt ((int) Evts_CheckDialog_Notify.Btn_Cancel, UIPanelType.CheckDialog_Notify)]
		public void OnCancelCheckDialog_Notify (params object[] arr) {
			UIBinderCheckDialog_Notify b = UIHub.instance.GetBinder<UIBinderCheckDialog_Notify> ((int) arr[0]);
			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Cancel;

			UIHub.instance.CloseUI ((int) arr[0]);

			if (b != null)
				b.CallRegistMethod ();
		}
	}
}