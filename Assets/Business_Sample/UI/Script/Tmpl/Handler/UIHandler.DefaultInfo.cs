using System.Collections.Generic;
using LowoUN.Module.UI;
using LowoUN.Util.Notify;

namespace LowoUN.Business.UI
{
	public partial class UIHandler
	{
		public enum Objs_DefaultInfo
        {
			None,
			Txt_CurScene,
		}

		public enum Evts_DefaultInfo
        {
			None,
		}

		#region ---------------- for holder ui inspector ----------------
		#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.DefaultInfo)]
		public List<string> SetInspectorObjectEnum4DefaultInfo()
		{
			return UILinker.instance.GetEnumNameList<Objs_DefaultInfo> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.DefaultInfo)]
		public List<string> SetInspectorEventEnum4DefaultInfo()
		{
			return UILinker.instance.GetEnumNameList<Evts_DefaultInfo> ();
		}
		#endif
		#endregion

		#region ----------------- ui binder constructor -----------------
		[UIBinderAtt (UIPanelType.DefaultInfo)]
		public UIBinder GetUIBinder4DefaultInfo(int instanceID)
		{
			return new UIBinderDefaultInfo((int)UIPanelType.DefaultInfo, instanceID);
		}
        #endregion

        #region ----------------- handle notify events ------------------
        [Events4NotifyAtt(true)]
        public void AddNotifyEvts_DefaultInfo()
        {
			NotifyMgr.AddListener<int>( "UW_LoadScene",UpdateScene_DefaultInfo);
			NotifyMgr.AddListener<int>( "UI_LoadScene",UpdateScene_DefaultInfo);
        }
        [Events4NotifyAtt(false)]
        public void RemoveNotifyEvts_DefaultInfo()
        {
			NotifyMgr.RemoveListener<int>( "UW_LoadScene",UpdateScene_DefaultInfo);
			NotifyMgr.RemoveListener<int>( "UI_LoadScene",UpdateScene_DefaultInfo);
        }
        private void UpdateScene_DefaultInfo(int gamestateid) {
            var binders = UIHub.instance.GetBinders<UIBinderDefaultInfo>(UIPanelType.DefaultInfo);
            binders.ForEach(i => i.UpdateCurScene(gamestateid));
        }
        #endregion

        #region ------ responce for the interactive ui components ------

        #endregion
    }
}