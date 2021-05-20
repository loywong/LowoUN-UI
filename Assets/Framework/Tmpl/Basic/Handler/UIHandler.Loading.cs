using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	public partial class UIHandler 
	{
		public enum Objs_Loading {
			None,
			Txt_Desc,
			Prog_Loader,
			Txt_TipTitle,
			Txt_TipDesc,

			Con_Loader,
			Con_Bg,
			Con_Logo,
		}
		
//		public enum Evts_Loading {
//			None,
//		}
		
		[UIBinderAtt(UIPanelType.Loading)]
		public UIBinder GetUIData4Loading (int instanceID) {
			return new UIBinderLoading ((int)UIPanelType.Loading, instanceID);
		}

	#if UNITY_EDITOR
		[ObjsAtt4UIInspector(UIPanelType.Loading)]
		public List<string> SetInspectorObjectEnum4Loading() {
			return UILinker.instance.GetEnumNameList<Objs_Loading> ();
		}

//		[EvtsAtt4UIInspector(UIPanelType.Loading)]
//		public List<string> SetInspectorEventEnum4Loading() {
//			return UILinker.instance.GetEnumNameList<Evts_Loading> ();
//		}
	#endif
	}
}
