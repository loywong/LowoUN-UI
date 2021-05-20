using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public enum UIObjects4CommonDialogBG {
		None,
		//Btn_FullScreen,
	}

	public enum UIEvents4CommonDialogBG {
		None,
		Btn_ClickBG,
	}

	public partial class UIHandler 
	{
		[UIBinderAtt (UIPanelType.DialogBG)]
		public UIBinder GetUIData4CommonDialogBG (int instanceID) {
			return new UIBinderDialogBG ((int)UIPanelType.DialogBG, instanceID);
		}

	#if UNITY_EDITOR
		[ObjsAtt4UIInspector(UIPanelType.DialogBG)]
		public List<string> SetInspectorObjectEnum4CommonDialogBG() {
			return UILinker.instance.GetEnumNameList<UIObjects4CommonDialogBG> ();
		}

		[EvtsAtt4UIInspector(UIPanelType.DialogBG)]
		public List<string> SetInspectorEventEnum4CommonDialogBG() {
			return UILinker.instance.GetEnumNameList<UIEvents4CommonDialogBG> ();
		}
	#endif

		[UIActionAtt((int)UIEvents4CommonDialogBG.Btn_ClickBG, UIPanelType.DialogBG)]
		public void OnCloseDialog (params object[] arr) {
			UIBinderDialogBG b = UIHub.instance.GetBinder<UIBinderDialogBG> ((int)arr [0]);
			if(b != null) b.CloseDialogByDarkBg();
		}
	}
}