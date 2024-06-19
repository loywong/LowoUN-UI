using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_CheckDialog {
			None,
			Txt_Title,
			Txt_Des,
			Btn_Close,
			Btn_Confirm,
			Btn_Cancel,
		}

		public enum Evts_CheckDialog {
			None,
			Btn_Confirm,
			Btn_Cancel,
			Btn_Close,
		}

		[UIBinderAtt (UIPanelType.CheckDialog)]
		public UIBinder GetUIData4CheckDialog (int instanceID) {
			return new UIBinderCheckDialog ((int) UIPanelType.CheckDialog, instanceID);
		}

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.CheckDialog)]
		public List<string> SetInspectorObjectEnum4CheckDialog () {
			return UILinker.instance.GetEnumNameList<Objs_CheckDialog> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.CheckDialog)]
		public List<string> SetInspectorEventEnum4CheckDialog () {
			return UILinker.instance.GetEnumNameList<Evts_CheckDialog> ();
		}
#endif

		[UIActionAtt ((int) Evts_CheckDialog.Btn_Close, UIPanelType.CheckDialog)]
		public void OnCloseCheckDialog (params object[] arr) {
			OnCancelCheckDialog (arr);
		}

		[UIActionAtt ((int) Evts_CheckDialog.Btn_Confirm, UIPanelType.CheckDialog)]
		public void OnConfirmCheckDialog (params object[] arr) {
			UIBinderCheckDialog b = UIHub.instance.GetBinder<UIBinderCheckDialog> ((int) arr[0]);
			if (b != null)
				b.Confirm ();
		}

		[UIActionAtt ((int) Evts_CheckDialog.Btn_Cancel, UIPanelType.CheckDialog)]
		public void OnCancelCheckDialog (params object[] arr) {
			UIBinderCheckDialog b = UIHub.instance.GetBinder<UIBinderCheckDialog> ((int) arr[0]);
			if (b != null)
				b.Cancel ();
		}
	}
}