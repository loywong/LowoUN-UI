using LowoUN.Module.UI;
using System.Collections.Generic;
using LowoUN.Util;
using LowoUN.Business.UI;
using LowoUN.Util.Notify;

namespace LowoUN.Business.UI
{
	public sealed class UILogic : UILogicBase
	{
		private static UILogic uiLogic;
		public static UILogic instance {
			get {
				if (uiLogic == null) 
					uiLogic = new UILogic ();
				
				return uiLogic;
			}
		}

        public UILogic() : base() {
			UIHub.instance.LoadUI((int)UIPanelType.DefaultInfo);
        }

		protected override void HandleBasicEvent (bool isAdd) {
			base.HandleBasicEvent (isAdd);

			if (isAdd) {
				NotifyMgr.AddListener<int>   ("UW_LoadScene", UIScene.instance.EnterScene);
				NotifyMgr.AddListener<int>   ("UI_LoadScene", UIScene.instance.EnterScene);
                NotifyMgr.AddListener        ("UW_EndScene", UIScene.instance.ExitScene);
                NotifyMgr.AddListener        ("UI_EndScene", UIScene.instance.ExitScene);
				NotifyMgr.AddListener         ("Module__Locale-Update", UIHub.instance.RestartAllUI);

			}
			else {
				NotifyMgr.RemoveListener<int> ("UW_LoadScene", UIScene.instance.EnterScene);
				NotifyMgr.RemoveListener<int> ("UI_LoadScene", UIScene.instance.EnterScene);
                NotifyMgr.RemoveListener      ("UW_EndScene", UIScene.instance.ExitScene);
                NotifyMgr.RemoveListener      ("UI_EndScene", UIScene.instance.ExitScene);
				NotifyMgr.RemoveListener      ("Module__Locale-Update", UIHub.instance.RestartAllUI);
			}
		}
	}
}