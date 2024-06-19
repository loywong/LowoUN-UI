using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_WebView {
			None,
			Txt_Title,
			Con_Web,
		}

		public enum Evts_WebView {
			None,
			Btn_Close,
		}

		#region ---------------- for holder ui inspector ----------------

#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.WebView)]
		public List<string> SetInspectorObjectEnum4WebView () {
			return UILinker.instance.GetEnumNameList<Objs_WebView> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.WebView)]
		public List<string> SetInspectorEventEnum4WebView () {
			return UILinker.instance.GetEnumNameList<Evts_WebView> ();
		}
#endif
		#endregion

		#region ----------------- ui binder constructor -----------------

		[UIBinderAtt (UIPanelType.WebView)]
		public UIBinder GetUIData4WebView (int instanceID) {
			return new UIBinderWebView ((int) UIPanelType.WebView, instanceID);
		}

		#endregion

		#region ------ responce for the interactive ui components ------

		[UIActionAtt ((int) Evts_WebView.Btn_Close, UIPanelType.WebView)]
		public void CheckWebView (params object[] arr) {
			//UIHub.instance.CloseUI ((int)arr[0]);
			UIHub.instance.GetBinder<UIBinderWebView> ((int) arr[0]).OnBtnClose ();
		}

		#endregion

		#region ----------------- handle notify events ------------------

		#endregion
	}
}