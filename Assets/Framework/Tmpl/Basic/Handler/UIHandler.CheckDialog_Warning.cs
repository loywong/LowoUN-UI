using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_CheckDialog_Warning {
			None,
			Btn_Confirm,
			Txt_Des
		}

		public enum Evts_CheckDialog_Warning {
			None,
			Btn_Confirm,
			Btn_Close,
		}

		[UIBinderAtt (UIPanelType.CheckDialog_Warning)]
		public UIBinder GetUIData4CheckDialog_Warning (int instanceID) {
			return new UIBinderCheckDialog_Warning ((int) UIPanelType.CheckDialog_Warning, instanceID);
		}

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.CheckDialog_Warning)]
		public List<string> SetInspectorObjectEnum4CheckDialog_Warning () {
			return UILinker.instance.GetEnumNameList<Objs_CheckDialog_Warning> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.CheckDialog_Warning)]
		public List<string> SetInspectorEventEnum4CheckDialog_Warning () {
			return UILinker.instance.GetEnumNameList<Evts_CheckDialog_Warning> ();
		}
#endif

		[UIActionAtt ((int) Evts_CheckDialog_Warning.Btn_Close, UIPanelType.CheckDialog_Warning)]
		public void OnCloseCheckDialog_Warning (params object[] arr) {
			UIBinderCheckDialog_Warning b = UIHub.instance.GetBinder<UIBinderCheckDialog_Warning> ((int) arr[0]);
			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Cancel;

			UIHub.instance.CloseUI ((int) arr[0]);

			if (b != null)
				b.CallRegistMethod ();
		}

		[UIActionAtt ((int) Evts_CheckDialog_Warning.Btn_Confirm, UIPanelType.CheckDialog_Warning)]
		public void OnConfirmCheckDialog_Warning (params object[] arr) {
			UIBinderCheckDialog_Warning b = UIHub.instance.GetBinder<UIBinderCheckDialog_Warning> ((int) arr[0]);
			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Confirm;

			UIHub.instance.CloseUI ((int) arr[0]);

			if (b != null)
				b.CallRegistMethod ();
		}
	}
}