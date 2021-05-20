using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CheckCostDialog;

	public class UIBinderCheckCostDialog : LowoUN.Module.UI.UIBinder 
	{
		public System.Action<UIEnum_CheckDlgBtnType> onBtnEvent;

		public string _CostTitle
        {
            set
            { 
				onUpdateTxt ((int)HolderObjs.Txt_CostTitle, value);
			}
		}

        public string costicon
        {
            set
            {
				onUpdateImg((int)HolderObjs.Img_CostIcon, value);
            }
        }

        public int cost
        {
            set
            {
				onUpdateTxt((int)HolderObjs.Txt_Cost, value.ToString());
            }
        }

        public string des
        {
            set
            {
                onUpdateTxt((int)HolderObjs.Txt_Des, value);
            }
        }

        private Dictionary<UIEnum_CheckDlgBtnType, string> _btnsTextDict;

		public UIEnum_CheckDlgBtnType buttonPressed { get; set; }

		public UIBinderCheckCostDialog (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

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

        public void SetInfo(string costtitle,string icon,int costNum,string Des, System.Action<UIEnum_CheckDlgBtnType> onBtnResponce = null, Dictionary<UIEnum_CheckDlgBtnType, string> btnsTextDict = null)
        {
            _CostTitle = costtitle;
            onBtnEvent = onBtnResponce;
            costicon = icon;
            cost = costNum;
            des = Des;
            buttonPressed = UIEnum_CheckDlgBtnType.Cancel;

			onUpdateState((int)HolderObjs.Btn_Confirm, UIStateType.Hide);
			onUpdateState((int)HolderObjs.Btn_Cancel, UIStateType.Hide);

            if (!(btnsTextDict == null || btnsTextDict.Count == 0))
            {
                if (btnsTextDict.ContainsKey(UIEnum_CheckDlgBtnType.Confirm))
                {
                    //onToggleVisual ((int)Objs_CheckCostDialog.Btn_Confirm, true);
					onUpdateState((int)HolderObjs.Btn_Confirm, UIStateType.Show);
					onUpdateName((int)HolderObjs.Btn_Confirm, btnsTextDict[UIEnum_CheckDlgBtnType.Confirm]);
                }
                if (btnsTextDict.ContainsKey(UIEnum_CheckDlgBtnType.Cancel))
                {
                    //onToggleVisual ((int)Objs_CheckCostDialog.Btn_Cancel, true);
					onUpdateState((int)HolderObjs.Btn_Cancel, UIStateType.Show);
					onUpdateName((int)HolderObjs.Btn_Cancel, btnsTextDict[UIEnum_CheckDlgBtnType.Cancel]);
                }
                this._btnsTextDict = btnsTextDict;

                //reset pos
                if (btnsTextDict.Count == 1)
                {
                    GameObject objbtnInfo = UILinker.instance.GetGameObject(insID, (int)HolderObjs.Btn_Confirm);
                    float y = 73.5f;
                    if (objbtnInfo != null)
                    {
                         y = objbtnInfo.GetComponent<RectTransform>().anchoredPosition.y;
                    }

                    if (btnsTextDict.ContainsKey(UIEnum_CheckDlgBtnType.Confirm))
						onUpdatePos((int)HolderObjs.Btn_Confirm, new Vector2(0f, y));
                    else if (btnsTextDict.ContainsKey(UIEnum_CheckDlgBtnType.Cancel))
						onUpdatePos((int)HolderObjs.Btn_Cancel, new Vector2(0f, y));
                }
            }
        }
    }
}