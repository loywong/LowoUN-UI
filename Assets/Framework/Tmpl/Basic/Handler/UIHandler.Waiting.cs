using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	public partial class UIHandler 
	{
		public enum Objs_CommonWaiting 
		{
			None,
			Img_Mask,
			Txt_Desc,
			Txt_Protocol,
		}

		public enum Evts_CommonWaiting 
		{
			None,
		}

		[UIBinderAtt (UIPanelType.Waiting)]
		public UIBinder GetUIData4CommonWaiting (int instanceID) {
			return new UIBinderWaiting ((int)UIPanelType.Waiting, instanceID);
		}

	#if UNITY_EDITOR
		[ObjsAtt4UIInspector(UIPanelType.Waiting)]
		public List<string> SetInspectorObjectEnum4CommonWaiting() {
			return UILinker.instance.GetEnumNameList<Objs_CommonWaiting> ();
		}

		[EvtsAtt4UIInspector(UIPanelType.Waiting)]
		public List<string> SetInspectorEventEnum4CommonWaiting() {
			return UILinker.instance.GetEnumNameList<Evts_CommonWaiting> ();
		}
	#endif
	}
}