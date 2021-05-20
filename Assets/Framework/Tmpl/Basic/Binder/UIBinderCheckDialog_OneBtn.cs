using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CheckDialog_OneBtn;

	public class UIBinderCheckDialog_OneBtn : LowoUN.Module.UI.UIBinder 
	{
		public System.Action onBtnEvent_Confirm;
		public System.Action onBtnEvent_Cancel;

		public string title{
			set{ 
				onUpdateTxt ((int)HolderObjs.Txt_Title, value);
			}
		}

		public UIBinderCheckDialog_OneBtn (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart ()
		{
			base.OnStart ();

            //1, layout
            //onUpdateTxt ((int)HolderObjs.Txt_ConfirmBtn,DataParserHelper.getMessage(4015));
        }

		#region implemented abstract members of UIBinder

		protected override void OnEnd ()
		{
			//throw new System.NotImplementedException ();
		}

		#endregion

//		public override void OnBtnClose ()
//		{
//			base.OnBtnClose ();
//			UIHub.instance.CloseUI(insID);
//		}

		public void CallConfirmRegistMethod()
		{
			UIHub.instance.CloseUI (insID);

			if (onBtnEvent_Confirm != null)
				onBtnEvent_Confirm ();
		}

		public void CallCancelRegistMethod()
		{
			UIHub.instance.CloseUI (insID);

			if (onBtnEvent_Cancel != null)
				onBtnEvent_Cancel ();
		}

		public void SetInfo (string title, object obj, System.Action onBtnResponceConfirm = null,  System.Action onBtnResponceCancel = null, string btnConfirmName ="") {
			this.title = title;
            if (btnConfirmName == "")
				btnConfirmName = "Confirm";/*DataParserHelper.getMessage(4015)*/

            onUpdateState ((int)HolderObjs.Txt_Des, UIStateType.Hide);
			onUpdateState ((int)HolderObjs.SonItem_Item, UIStateType.Hide);

			if (obj is string) {
				onUpdateState ((int)HolderObjs.Txt_Des, UIStateType.Show);
				onUpdateTxt ((int)HolderObjs.Txt_Des, obj.ToString());
			} else {
				onUpdateState ((int)HolderObjs.SonItem_Item, UIStateType.Show);
				onUpdateSonItem ((int)HolderObjs.SonItem_Item, obj);
			}

			onUpdateTxt ((int)HolderObjs.Txt_ConfirmBtn, btnConfirmName);

			onBtnEvent_Confirm = onBtnResponceConfirm;
			onBtnEvent_Cancel = onBtnResponceCancel;
		}
	}
}