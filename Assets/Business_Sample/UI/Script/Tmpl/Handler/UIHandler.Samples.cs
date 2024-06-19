using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public partial class UIHandler {
		public enum Objs_Samples {
			None,
			Txt_Title,
			Group_TestSamples,
		}

		public enum Evts_Samples {
			None,
			Group_TestSamples,
		}

		#region ---------------- for holder ui inspector ----------------
#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.Samples)]
		public List<string> SetInspectorObjectEnum4Samples () {
			return UILinker.instance.GetEnumNameList<Objs_Samples> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.Samples)]
		public List<string> SetInspectorEventEnum4Samples () {
			return UILinker.instance.GetEnumNameList<Evts_Samples> ();
		}
#endif
		#endregion

		#region ----------------- ui binder constructor -----------------
		[UIBinderAtt (UIPanelType.Samples)]
		public UIBinder GetUIData4Samples (int instanceID) {
			return new UIBinderSamples ((int) UIPanelType.Samples, instanceID);
		}
		#endregion

		#region ----------------- handle notify events ------------------

		#endregion

		#region ------ responce for the interactive ui components ------
		[UIActionAtt ((int) Evts_Samples.Group_TestSamples, UIPanelType.Samples)]
		public void Test_Samples (params object[] arr) {
			UIHub.instance.GetBinder<UIBinderSamples> ((int) arr[1]).Test ((int) arr[0]);
		}
		#endregion
	}
}