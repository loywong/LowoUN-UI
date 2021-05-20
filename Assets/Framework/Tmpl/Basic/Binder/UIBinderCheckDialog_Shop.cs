using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CheckDialog_Shop;

	public class UIBinderCheckDialog_Shop : LowoUN.Module.UI.UIBinder 
	{
		public System.Action<UIEnum_CheckDlgBtnType> onBtnEvent;
        public UIEnum_CheckDlgBtnType buttonPressed { get; set; }

		public UIBinderCheckDialog_Shop (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart ()
		{
			base.OnStart ();

		}

		#region implemented abstract members of UIBinder

		protected override void OnEnd ()
		{
			//throw new System.NotImplementedException ();
		}

		#endregion

        public void CallRegistMethod()
        {
            if (onBtnEvent != null)
                onBtnEvent(buttonPressed);
        }

		public void SetInfo (string ReawrdTitle,string Count,string FirstRewardDes, int mode = 0, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, System.Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {

            onUpdateTxt((int)HolderObjs.Txt_ReWardTitle, ReawrdTitle);
            onUpdateTxt((int)HolderObjs.Txt_Count, Count);
            onUpdateTxt((int)HolderObjs.Txt_VIPTitle, ReawrdTitle);
            onUpdateTxt((int)HolderObjs.Txt_VIPDes, Count);
            onUpdateTxt((int)HolderObjs.Txt_FisrtReWardDes, FirstRewardDes);
            onUpdateState_ShowOrHide((int)HolderObjs.Con_ModeDiamond, mode == 0);
            onUpdateState_ShowOrHide((int)HolderObjs.Con_ModeVip, mode == 1);
            onUpdateState_ShowOrHide((int)HolderObjs.Txt_FisrtReWardDes, FirstRewardDes!="");
            onBtnEvent = onBtnResponce;
			buttonPressed = UIEnum_CheckDlgBtnType.Cancel;
            if (!(btnsTextDict == null || btnsTextDict.Count == 0))
                onUpdateName((int)HolderObjs.Btn_Confirm, btnsTextDict[UIEnum_CheckDlgBtnType.Confirm]);
        }
	}
}