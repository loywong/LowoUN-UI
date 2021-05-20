using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI
{
    public partial class UIHandler
    {
        public enum Objs_CheckDialog_Shop
        {
            None,
            Txt_ReWardTitle,
            Txt_Count,
            Txt_FisrtReWardDes,
            Btn_Confirm,
            Con_ModeDiamond,
            Con_ModeVip,
            Txt_VIPTitle,
            Txt_VIPDes,
        }

        public enum Evts_CheckDialog_Shop
        {
            None,
            Btn_Confirm,
        }

        [UIBinderAtt(UIPanelType.CheckDialog_Shop)]
        public UIBinder GetUIData4CheckDialog_Shop(int instanceID)
        {
            return new UIBinderCheckDialog_Shop((int)UIPanelType.CheckDialog_Shop, instanceID);
        }

#if UNITY_EDITOR
        [ObjsAtt4UIInspector(UIPanelType.CheckDialog_Shop)]
        public List<string> SetInspectorObjectEnum4CheckDialog_Shop()
        {
            return UILinker.instance.GetEnumNameList<Objs_CheckDialog_Shop>();
        }

        [EvtsAtt4UIInspector(UIPanelType.CheckDialog_Shop)]
        public List<string> SetInspectorEventEnum4CheckDialog_Shop()
        {
            return UILinker.instance.GetEnumNameList<Evts_CheckDialog_Shop>();
        }
#endif
        [UIActionAtt((int)Evts_CheckDialog_Shop.Btn_Confirm, UIPanelType.CheckDialog_Shop)]
        public void OnConfirmCheckDialog_Shop(params object[] arr)
        {
            UIBinderCheckDialog_Shop b = UIHub.instance.GetBinder<UIBinderCheckDialog_Shop>((int)arr[0]);
            if (b != null)
                b.buttonPressed = UIEnum_CheckDlgBtnType.Confirm;

            UIHub.instance.CloseUI((int)arr[0]);

            if (b != null)
                b.CallRegistMethod();
        }
    }
}