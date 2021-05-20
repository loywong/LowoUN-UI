using UnityEngine;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	public partial class UIHandler 
	{
		public enum Objs_CommonSystemNotify {
			None,
			Txt_Notify,
			Con_Notify,
		}
		
		public enum Evts_CommonSystemNotify {
			None,
		}
		
		[UIBinderAtt (UIPanelType.SystemNotify)]
		public UIBinder GetUIData4CommonSystemNotify (int instanceID) {
			return new UIBinderSystemNotify ((int)UIPanelType.SystemNotify, instanceID);
		}

	#if UNITY_EDITOR
		[ObjsAtt4UIInspector(UIPanelType.SystemNotify)]
		public List<string> SetInspectorObjectEnum4CommonSystemNotify() {
			return UILinker.instance.GetEnumNameList<Objs_CommonSystemNotify> ();
		}

		[EvtsAtt4UIInspector(UIPanelType.SystemNotify)]
		public List<string> SetInspectorEventEnum4CommonSystemNotify() {
			return null;
		}
	#endif
	}
}