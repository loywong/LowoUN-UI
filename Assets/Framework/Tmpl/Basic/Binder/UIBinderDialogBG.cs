namespace LowoUN.Business.UI 
{
	public class UIBinderDialogBG : LowoUN.Module.UI.UIBinder
	{
		public UIBinderDialogBG (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}


		private bool hasClickBtn;

		public override void OnStart ()
		{
			base.OnStart ();

			hasClickBtn = false;
		}

		#region implemented abstract members of UIBinder

		protected override void OnEnd ()
		{
			hasClickBtn = false;
		}

		#endregion


		public System.Action onTriggerHostPanel;

		public void CloseDialogByDarkBg () {
			if (!LowoUN.Entry.INI.UISetting.instance.enableCloseByDarkBg)
				return;
			
			if (!hasClickBtn) {
				hasClickBtn = true;

				if (onTriggerHostPanel != null)
					onTriggerHostPanel ();//(hostInsid);
			}
		}
	}
}