using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CheckDialog_Medal;

	public class UIBinderCheckDialog_Medal : LowoUN.Module.UI.UIBinder 
	{
		public UIBinderCheckDialog_Medal(int uiPanelType, int instanceID) : base(uiPanelType, instanceID)
		{

		}

		protected override void OnEnd ()
		{
			//throw new System.NotImplementedException ();
		}
	}
}