using LowoUN.Module.UI;
using System.Collections.Generic;
using LowoUN.Util.Notify;
using LowoUN.Module.UI.HUDText;

namespace LowoUN.Business.UI 
{
    using HolderObjs = UIHandler.Objs_Samples;

    public class UIBinderSamples : LowoUN.Module.UI.UIBinder
	{

		public UIBinderSamples (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart ()
		{
			base.OnStart ();

            //1, layout
            onUpdateTxt((int)HolderObjs.Txt_Title, "Samples");
            onUpdateGroupNames((int)HolderObjs.Group_TestSamples, new List<string>() { 
				"Open Component Panel", 
				"Call CheckDialog", 
				"Show Notifies", 
				"GO GameState: Login"});

            //2, data

        }

        public override void OnBtnClose ()
		{
			base.OnBtnClose ();
			LowoUN.Module.UI.UIHub.instance.CloseUI (insID);
		}

		protected override void OnEnd ()
		{
			//throw new System.NotImplementedException ();
		}

        public void Test(int idx) {
            switch (idx)
            {
                case 0:
                    UIHub.instance.LoadUI(UIPanelType.Coms, "UI_Coms");
                    break;
				case 1:
					UIHub.instance.LoadCheckDialogUI (
						"Title: Dialog",//if set null, keep default
						"Description: This is a CheckDialog panel!",
						new Dictionary<UIEnum_CheckDlgBtnType, string> (){ { UIEnum_CheckDlgBtnType.Confirm, "Confirm" }, { UIEnum_CheckDlgBtnType.Cancel, "Cancel" } },
						btnType => {
							if (btnType == UIEnum_CheckDlgBtnType.Confirm) {
                                //Do Something
                                UIHudText_Notify_CacheTwo.instance.Show("Confirm the check of Dialog");
                            }
						}
					);
					break;
				case 2:
					UIHudText_Notify_CacheTwo.instance.Show ("msg111111111");
					UIHudText_Notify_CacheTwo.instance.Show ("msg222222222");
					UIHudText_Notify_CacheTwo.instance.Show ("msg333333333");
					UIHudText_Notify_CacheTwo.instance.Show ("msg444444444");
					UIHudText_Notify_CacheTwo.instance.Show ("msg555555555");
					break;
                case 3:
                    UIHudText_Notify_CacheTwo.instance.ClearAll();
                    NotifyMgr.Broadcast<int>("UI_LoadScene", (int)Global.Enum_GameState.Login);
                    break;
                default:
                    break;
            }
        }
	}
}
