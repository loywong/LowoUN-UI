using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
    using HolderObjs = UIHandler.Objs_ItemReward;

	public class UIBinderItemReward : UIBinder {
		private int idx {
			set {
				onUpdateTxt ((int) HolderObjs.Txt_Idx, value.ToString ());
			}
		}

		public UIBinderItemReward (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart (object info) {
			base.OnStart (info);

			//data
			idx = curIdxInList;
		}

		protected override void OnEnd () {
			//throw new System.NotImplementedException ();
		}
	}
}