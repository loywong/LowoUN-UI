using LowoUN.Module.UI;

namespace LowoUN.Business.UI {
	public class UIBinderDialogBG : UIBinder {
		public UIBinderDialogBG (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		private bool hasClickBtn;

		public override void OnStart () {
			base.OnStart ();

			hasClickBtn = false;
		}

		#region implemented abstract members of UIBinder

		protected override void OnEnd () {
			hasClickBtn = false;
		}

		#endregion

		public System.Action onTriggerHostPanel;

		public void CloseDialogByDarkBg () {
			if (!UISetting.instance.enableCloseByDarkBg)
				return;

			if (!hasClickBtn) {
				hasClickBtn = true;

				if (onTriggerHostPanel != null)
					onTriggerHostPanel (); //(hostInsid);
			}
		}
	}
}