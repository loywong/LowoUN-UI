using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {

		public enum Objs_CheckDialog_Medal {
			None,
			Txt_Title,
			Txt_ConfirmBtn,

			Txt_Des,
			SonItem_Item,
		}

		public enum Evts_CheckDialog_Medal {
			None,
			Btn_Confirm,
			Btn_Close,
		}

		[UIBinderAtt (UIPanelType.CheckDialog_Modal)]
		public UIBinder GetUIData4CheckDialog_Medal (int instanceID) {
			return null; //new UIBinderCheckDialog_Medal ((int)UIPanelType.CheckDialog_Medal, instanceID);
		}

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.CheckDialog_Modal)]
		public List<string> SetInspectorObjectEnum4CheckDialog_Medal () {
			return UILinker.instance.GetEnumNameList<Objs_CheckDialog_Medal> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.CheckDialog_Modal)]
		public List<string> SetInspectorEventEnum4CheckDialog_Medal () {
			return UILinker.instance.GetEnumNameList<Evts_CheckDialog_Medal> ();
		}
#endif
	}
}