using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_ItemReward {
			None,
			Txt_Idx,
		}

		public enum Evts_ItemReward {
			None,
		}

		#region ---------------- for holder ui inspector ----------------
#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.ItemReward)]
		public List<string> SetInspectorObjectEnum4ItemReward () {
			return UILinker.instance.GetEnumNameList<Objs_ItemReward> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.ItemReward)]
		public List<string> SetInspectorEventEnum4ItemReward () {
			return UILinker.instance.GetEnumNameList<Evts_ItemReward> ();
		}
#endif
		#endregion

		#region ----------------- ui binder constructor -----------------
		[UIBinderAtt (UIPanelType.ItemReward)]
		public UIBinder GetUIBinder4ItemReward (int instanceID) {
			return new UIBinderItemReward ((int) UIPanelType.ItemReward, instanceID);
		}
		#endregion

		#region ----------------- handle notify events ------------------

		#endregion

		#region ------ responce for the interactive ui components ------

		#endregion
	}
}