using LowoUN.Util.Notify;

namespace LowoUN.Business.UI
{
    using HolderObjs = UIHandler.Objs_Login;

    public class UIBinderLogin : LowoUN.Module.UI.UIBinder
    {

        public UIBinderLogin(int uiPanelType, int instanceID) : base(uiPanelType, instanceID)
        {

        }

        public override void OnStart()
        {
            base.OnStart();

            //1, layout
            onUpdateTxt((int)HolderObjs.Txt_Title, "Login");
            onUpdateName((int)HolderObjs.Btn_Close, "Quit");

            //2, data

        }

        public override void OnBtnClose()
        {
            base.OnBtnClose();

            //LowoUN.Module.UI.UIHub.instance.CloseUI(insID);
			NotifyMgr.Broadcast<int>("UI_LoadScene", (int)Enum_GameState.Test);
        }

        protected override void OnEnd()
        {
            //throw new System.NotImplementedException ();
        }
    }
}
