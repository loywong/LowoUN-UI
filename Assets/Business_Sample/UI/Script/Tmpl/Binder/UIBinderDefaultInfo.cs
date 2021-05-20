namespace LowoUN.Business.UI
{
    using Module.UI;
    using HolderObjs = UIHandler.Objs_DefaultInfo;

    public class UIBinderDefaultInfo : LowoUN.Module.UI.UIBinder
    {
        private string curSceneName {
            set {
                onUpdateTxt((int)HolderObjs.Txt_CurScene, string.Format("Current Scene: {0}", value));
            }
        }

        public UIBinderDefaultInfo(int uiPanelType, int instanceID) : base(uiPanelType, instanceID)
        {

        }

        public override void OnStart()
        {
            base.OnStart();

            //data
            UpdateCurScene(UIScene.instance.curSceneStateID);
        }

        protected override void OnEnd()
        {
            //throw new System.NotImplementedException ();
        }

        public void UpdateCurScene(int stateid) {
            curSceneName =  ((Global.Enum_GameState)stateid).ToString();
        }
    }
}
