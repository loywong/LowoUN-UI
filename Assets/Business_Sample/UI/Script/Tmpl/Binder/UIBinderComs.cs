using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_Coms;

	public class UIBinderComs : LowoUN.Module.UI.UIBinder
	{

		public UIBinderComs (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart ()
		{
			base.OnStart ();

            //1, layout
			onUpdateTxt((int)HolderObjs.Txt_Title, "Components(Coms)");
			onUpdateName((int)HolderObjs.Btn_Close, "Close");
			onUpdateGroupNames((int)HolderObjs.Group_Test, new List<string>(){"Tab1", "Tab2", "Tab3"});
			onUpdateTogl((int)HolderObjs.Togl_Test, true);
			onUpdateIptInitStr((int)HolderObjs.Ipt_Test, "www.loywong.com");
			onUpdateSlider((int)HolderObjs.Slider_Test, 0.5f);

            //2, data
			onUpdateImg((int)HolderObjs.Img_Test, "Close");
			onUpdateProg((int)HolderObjs.Prog_Test, 20, 100);
			//sonGeneral will loaded uiholder by theirself
			onUpdateSonItem((int)HolderObjs.SonItem_Test, new object());
			onUpdateLst((int)HolderObjs.Lst_Mid_Test, new List<object>{new object(),new object()});
			onUpdateLst((int)HolderObjs.Lst_Test, new List<object>{new object(),new object(),new object(),new object(),new object(),new object(),new object()});
        
			onUpdateGroupIdx((int)HolderObjs.Group_Test, 1);
			onUpdateLstFocus((int)HolderObjs.Lst_Test, 6);
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

		public void SetGroup (int idx) {
			Debug.Log("SetGroup idx: " + idx);
		}
		public void SetTogl (bool isSelect) {
			Debug.Log("SetTogl isSelect: " + isSelect);
		}
		public void SetIpt (string str, LowoUN.Module.UI.Com.UIIpt.Ipt_EvtTyp state) {
			Debug.Log("SetIpt str: " + str + "/ state: " + state.ToString());
		}
		public void SetSlider (float val) {
			Debug.Log("SetSlider val: " + val);
		}
	}
}
