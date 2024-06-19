using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {

		public enum Objs_CheckCostDialog {
			None,
			Btn_Close,
			Btn_Confirm,
			Btn_Cancel,
			Txt_CostTitle,
			Img_CostIcon,
			Txt_Cost,
			Txt_Des,
		}

		public enum Evts_CheckCostDialog {
			None,
			Btn_Confirm,
			Btn_Cancel,
			Btn_Close,
		}

		[UIBinderAtt (UIPanelType.CheckCostDialog)]
		public UIBinder GetUIData4CheckCostDialog (int instanceID) {
			return new UIBinderCheckCostDialog ((int) UIPanelType.CheckCostDialog, instanceID);
		}

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.CheckCostDialog)]
		public List<string> SetInspectorObjectEnum4CheckCostDialog () {
			return UILinker.instance.GetEnumNameList<Objs_CheckCostDialog> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.CheckCostDialog)]
		public List<string> SetInspectorEventEnum4CheckCostDialog () {
			return UILinker.instance.GetEnumNameList<Evts_CheckCostDialog> ();
		}
#endif

		[UIActionAtt ((int) Evts_CheckCostDialog.Btn_Close, UIPanelType.CheckCostDialog)]
		public void OnCloseCheckCostDialog (params object[] arr) {
			UIHub.instance.CloseUI ((int) arr[0]);
		}

		[UIActionAtt ((int) Evts_CheckCostDialog.Btn_Confirm, UIPanelType.CheckCostDialog)]
		public void OnConfirmCheckCostDialog (params object[] arr) {
			UIBinderCheckCostDialog b = UIHub.instance.GetBinder<UIBinderCheckCostDialog> ((int) arr[0]);

			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Confirm;

			UIHub.instance.CloseUI ((int) arr[0]);
			b.CallRegistMethod ();
		}

		[UIActionAtt ((int) Evts_CheckCostDialog.Btn_Cancel, UIPanelType.CheckCostDialog)]
		public void OnCancelCheckCostDialog (params object[] arr) {
			UIBinderCheckCostDialog b = UIHub.instance.GetBinder<UIBinderCheckCostDialog> ((int) arr[0]);

			if (b != null)
				b.buttonPressed = UIEnum_CheckDlgBtnType.Cancel;

			UIHub.instance.CloseUI ((int) arr[0]);
			b.CallRegistMethod ();
		}
	}
}