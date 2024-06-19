using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_Login {
			None,
			Txt_Title,
			Btn_Close,
		}

		public enum Evts_Login {
			None,
			Btn_Close,
		}

		#region ---------------- for holder ui inspector ----------------
#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.Login)]
		public List<string> SetInspectorObjectEnum4Login () {
			return UILinker.instance.GetEnumNameList<Objs_Login> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.Login)]
		public List<string> SetInspectorEventEnum4Login () {
			return UILinker.instance.GetEnumNameList<Evts_Login> ();
		}
#endif
		#endregion

		#region ----------------- ui binder constructor -----------------
		[UIBinderAtt (UIPanelType.Login)]
		public UIBinder GetUIBinder_Login (int instanceID) {
			return new UIBinderLogin ((int) UIPanelType.Login, instanceID);
		}
		#endregion

		#region ----------------- handle notify events ------------------

		#endregion

		#region ------ responce for the interactive ui components ------
		[UIActionAtt ((int) Evts_Login.Btn_Close, UIPanelType.Login)]
		public void Close_Login (params object[] arr) {
			UIHub.instance.GetBinder<UIBinderLogin> ((int) arr[0]).OnBtnClose ();
		}
		#endregion
	}
}