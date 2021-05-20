using System.Collections.Generic;
using UnityEngine;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI
{
	using HolderObjs = UIHandler.Objs_WebView;

	public class UIBinderWebView : LowoUN.Module.UI.UIBinder
	{
		private string _title {
			set {
				onUpdateTxt ((int)HolderObjs.Txt_Title, value);
			}
		}

		public UIBinderWebView (int uiPanelType, int instanceID) : base (uiPanelType, instanceID)
		{

		}
			
		public override void OnStart ()
		{
			base.OnStart ();

		}

		protected override void OnEnd ()
		{
			//UINotifyMgr.Broadcast<int> ("UI_CloseWebView", insID);
			//LowoUN.Module.Data.TD.RmvVal("StartUpHaveSystemMailToShow");
		}

		public override void OnBtnClose ()
		{
			base.OnBtnClose ();
			UIHub.instance.CloseUI(insID);
		}

		public void SetInfo (string title, string url, Dictionary<Vector2, float> poses = null/*, int charaID = -1*/) {
			_title = title;
			onUpdateWebView ((int)HolderObjs.Con_Web , url);//, charaID
		}
	}
}
