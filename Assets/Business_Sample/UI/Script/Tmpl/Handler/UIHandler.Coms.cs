using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI
{
	public partial class UIHandler
	{
		public enum Objs_Coms
		{
			None,
			//nonaction
			Txt_Title,
			Img_Test,
			Prog_Test,
			SonItem_Test,
			SonGeneral_Test,
			Lst_Test,
			Lst_Mid_Test,
			//action
			Btn_Close,
			Group_Test,
			Togl_Test,
			Ipt_Test,
			Slider_Test,
		}

		public enum Evts_Coms
		{
			None,
			Btn_Close,
			Group_Test,
			Togl_Test,
			Ipt_Test,
			Slider_Test,
		}

		#region ---------------- for holder ui inspector ----------------
		#if UNITY_EDITOR
		[ObjsAtt4UIInspector (UIPanelType.Coms)]
		public List<string> SetInspectorObjectEnum4Coms ()
		{
			return UILinker.instance.GetEnumNameList<Objs_Coms> ();
		}

		[EvtsAtt4UIInspector (UIPanelType.Coms)]
		public List<string> SetInspectorEventEnum4Coms ()
		{
			return UILinker.instance.GetEnumNameList<Evts_Coms> ();
		}
		#endif
		#endregion

		#region ----------------- ui binder constructor -----------------
		[UIBinderAtt (UIPanelType.Coms)]
		public UIBinder GetUIBinder4Coms (int instanceID)
		{
			return new UIBinderComs ((int)UIPanelType.Coms, instanceID);
		}
		#endregion

		#region ----------------- handle notify events ------------------

		#endregion

		#region ------ responce for the interactive ui components ------
		/// <summary>
		/// Component Button params' meaning
		/// </summary>
		/// <param name="arr">[0]: uiholder instance id</param>
		[UIActionAtt((int)Evts_Coms.Btn_Close, UIPanelType.Coms)]
		public void Close_Coms(params object[] arr)
		{
			UIHub.instance.GetBinder<UIBinderComs> ((int)arr [0]).OnBtnClose ();
		}
		/// <summary>
		/// Component Group params' meaning
		/// </summary>
		/// <param name="arr">[0]: index id: int</param>
		/// <param name="arr">[1]: uiholder instance id</param>
		[UIActionAtt((int)Evts_Coms.Group_Test, UIPanelType.Coms)]
		public void Group_Test_Coms(params object[] arr)
		{
			UIHub.instance.GetBinder<UIBinderComs> ((int)arr [1]).SetGroup ((int)arr [0]);
		}
		/// <summary>
		///  Component Togl params' meaning
		/// </summary>
		/// <param name="arr">[0]: isSelected: bool</param>
		/// <param name="arr">[1]: uiholder instance id</param>
		[UIActionAtt((int)Evts_Coms.Togl_Test, UIPanelType.Coms)]
		public void Togl_Test_Coms(params object[] arr)
		{
			UIHub.instance.GetBinder<UIBinderComs> ((int)arr [1]).SetTogl ((bool)arr [0]);
		}
		/// <summary>
		/// InputFieald
		/// </summary>
		/// <param name="arr">[0]: value: string</param>
		/// <param name="arr">[1]: state: Ipt_EvtTyp</param>
		/// <param name="arr">[2]: uiholder instance id</param>
		[UIActionAtt((int)Evts_Coms.Ipt_Test, UIPanelType.Coms)]
		public void Ipt_Test_Coms(params object[] arr)
		{
			UIHub.instance.GetBinder<UIBinderComs> ((int)arr [2]).SetIpt (arr [0].ToString(), (LowoUN.Module.UI.Com.UIIpt.Ipt_EvtTyp)arr [1]);
		}
		/// <summary>
		/// Slider
		/// </summary>
		/// <param name="arr">[0] value: float</param>
		/// <param name="arr">[1] uiholder instance id</param>
		[UIActionAtt((int)Evts_Coms.Slider_Test, UIPanelType.Coms)]
		public void Slider_Test_Coms(params object[] arr)
		{
			UIHub.instance.GetBinder<UIBinderComs> ((int)arr [1]).SetSlider ((float)arr [0]);
		}
		#endregion
	}
}