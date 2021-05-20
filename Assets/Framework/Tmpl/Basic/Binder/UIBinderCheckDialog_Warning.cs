using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CheckDialog_Warning;

	public class UIBinderCheckDialog_Warning : LowoUN.Module.UI.UIBinder 
	{
		public System.Action<UIEnum_CheckDlgBtnType> onBtnEvent;

		public string _des
        {
            set
            { 
				onUpdateTxt ((int)HolderObjs.Txt_Des, value);
			}
		}

		private Dictionary<UIEnum_CheckDlgBtnType, string> _btnsTextDict;

        public UIEnum_CheckDlgBtnType buttonPressed { get; set; }

		public UIBinderCheckDialog_Warning (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

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

		public void SetInfo (string des, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null, System.Action<UIEnum_CheckDlgBtnType> onBtnResponce = null) {
			_des = des;

            onBtnEvent = onBtnResponce;
			buttonPressed = UIEnum_CheckDlgBtnType.Cancel;
			
			if (!(btnsTextDict == null || btnsTextDict.Count == 0)) {
					onUpdateName ((int)HolderObjs.Btn_Confirm, btnsTextDict[UIEnum_CheckDlgBtnType.Confirm]);
				}
			}
		}
}