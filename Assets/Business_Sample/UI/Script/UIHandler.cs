using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public enum UIPanelClass {
		None,

		Test = 0,
		Global = 1,
		Basic = 2,
		Module = 3,
		Notify = 4,
		Award = 5,
		Dlg = 6,

		ComnInfo = 9999,
		Comn = 10000,
		Comn_Map = 20000,

		Login = 30000,
	}
	public enum UIPanelType {
		/// <summary>
		/// Common = 1
		/// Map = 1001
		/// Battle = 2001
		/// </summary>

		None = 0,

		//Basic ---------------------------------------------------
		[UIPrefabDesc ("UI_DialogBG_A_0", UIPanelClass.Basic)]
		[UIPrefabDesc ("UI_DialogBG_A_25", UIPanelClass.Basic)]
		[UIPrefabDesc ("UI_DialogBG_Nomal_75", UIPanelClass.Basic)]
		DialogBG = 51, [UIPrefabDesc ("UI_CheckDialog", UIPanelClass.Dlg, UIEnum_DlgBG.Normal)]
		CheckDialog = 52, [UIPrefabDesc ("UI_Waiting", UIPanelClass.Basic)]
		Waiting = 53, [UIPrefabDesc ("UI_Loading", UIPanelClass.Global)]
		Loading = 54, [UIPrefabDesc ("UI_SysNotify", UIPanelClass.Notify)]
		SystemNotify = 55, [UIPrefabDesc ("UI_WebView", UIPanelClass.Basic, UIEnum_DlgBG.Normal)]
		WebView = 56, [UIPrefabDesc ("UI_CheckCostDialog", UIPanelClass.Dlg, UIEnum_DlgBG.Normal)]
		CheckCostDialog = 57, [UIPrefabDesc ("UI_Tip_ValsChange", UIPanelClass.Notify)]
		Tip_ValsChange = 58, [UIPrefabDesc ("UI_Tip_StrsChange", UIPanelClass.Notify)]
		Tip_StrsChange = 59,

		[UIPrefabDesc ("UI_CheckDialog_OneBtn", UIPanelClass.Dlg)]
		CheckDialog_OneBtn = 80, [UIPrefabDesc ("UI_CheckDialog_Modal", UIPanelClass.Dlg)]
		CheckDialog_Modal = 81, [UIPrefabDesc ("UI_CheckConfirmDialog", UIPanelClass.Dlg)]
		CheckConfirmDialog = 82, [UIPrefabDesc ("UI_CheckDialog_Warning", UIPanelClass.Dlg)]
		CheckDialog_Warning = 83, [UIPrefabDesc ("UI_CheckDialog_Shop", UIPanelClass.Dlg)]
		CheckDialog_Shop = 84, [UIPrefabDesc ("UI_CheckDialog_Notify", UIPanelClass.Dlg)]
		CheckDialog_Notify = 85,

		[UIPrefabDesc ("UI_Samples", UIPanelClass.Test)]
		Samples = 90, [UIPrefabDesc ("UI_DefaultInfo", UIPanelClass.Global)]
		DefaultInfo = 91,

		[UIPrefabDesc ("Holder_GeneralTyp", UIPanelClass.Test)]
		[UIPrefabDesc ("UI_Coms", UIPanelClass.Test, UIEnum_DlgBG.Normal)]
		Coms = 101, [UIPrefabDesc ("UI_ItemReward", UIPanelClass.Test)]
		[UIPrefabDesc ("Holder_ItemTyp", UIPanelClass.Test)]
		ItemReward = 102,

		[UIPrefabDesc ("UI_Login", UIPanelClass.Login, UIEnum_DlgBG.Normal)]
		Login = 201,
	}

	public partial class UIHandler {

		private static UIHandler _uiHandler;
		public static UIHandler instance {
			get {
				if (_uiHandler == null) {
					_uiHandler = new UIHandler ();
				}

				return _uiHandler;
			}
		}

		private UIHandler () { }
	}
}