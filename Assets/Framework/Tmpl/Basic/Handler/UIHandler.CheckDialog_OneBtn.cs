using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	public partial class UIHandler {
		
		public enum Objs_CheckDialog_OneBtn {
			None,
			Txt_Title,
			Txt_ConfirmBtn,
			
			Txt_Des,
			SonItem_Item,
		}
		
		public enum Evts_CheckDialog_OneBtn {
			None,
			Btn_Confirm,
			Btn_Close,
		}
		
		[UIBinderAtt (UIPanelType.CheckDialog_OneBtn)]
		public UIBinder GetUIData4CheckDialog_OneBtn (int instanceID) {
			return new UIBinderCheckDialog_OneBtn ((int)UIPanelType.CheckDialog_OneBtn, instanceID);
		}

		#if UNITY_EDITOR
		[ObjsAtt4UIInspector(UIPanelType.CheckDialog_OneBtn)]
		public List<string> SetInspectorObjectEnum4CheckDialog_OneBtn() {
			return UILinker.instance.GetEnumNameList<Objs_CheckDialog_OneBtn> ();
		}

		[EvtsAtt4UIInspector(UIPanelType.CheckDialog_OneBtn)]
		public List<string> SetInspectorEventEnum4CheckDialog_OneBtn() {
			return UILinker.instance.GetEnumNameList<Evts_CheckDialog_OneBtn> ();
		}
		#endif

		[UIActionAtt((int)Evts_CheckDialog_OneBtn.Btn_Close, UIPanelType.CheckDialog_OneBtn)]
		public void OnCloseCheckDialog_OneBtn (params object[] arr) {
//			UIBinderCheckDialog_OneBtn uiData = UIHub.instance.GetCurDialog<UIBinderCheckDialog_OneBtn>();
			UIBinderCheckDialog_OneBtn b = UIHub.instance.GetBinder<UIBinderCheckDialog_OneBtn>((int)arr[0]);
			if(b != null) b.CallCancelRegistMethod ();
		}

		[UIActionAtt((int)Evts_CheckDialog_OneBtn.Btn_Confirm, UIPanelType.CheckDialog_OneBtn)]
		public void OnConfirmCheckDialog_OneBtn (params object[] arr) {
//			UIBinderCheckDialog_OneBtn uiData = UIHub.instance.GetCurDialog<UIBinderCheckDialog_OneBtn>();
			UIBinderCheckDialog_OneBtn b = UIHub.instance.GetBinder<UIBinderCheckDialog_OneBtn>((int)arr[0]);
			if(b != null) b.CallConfirmRegistMethod ();
		}
	}
}