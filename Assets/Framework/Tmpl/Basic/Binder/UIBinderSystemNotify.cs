using System.Collections.Generic;
using LowoUN.Module.UI;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_CommonSystemNotify;

	public class UIBinderSystemNotify : LowoUN.Module.UI.UIBinder
	{
		private string _info{ 
			set{
				onUpdateTxt ((int)HolderObjs.Txt_Notify, value);

//				if(onUpdateAnimTip != null)
//					onUpdateAnimTip ((int)Objs_CommonSystemNotify.Tip_SystemNotify, _info);
			}
		}

		public UIBinderSystemNotify (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		public override void OnStart ()
		{
			base.OnStart ();

			//onUpdateState ((int)HolderObjs.Tip_SystemNotify,LowoUN.Module.UI.UIStateType.Hide);
		}

		#region implemented abstract members of UIBinder
		
		protected override void OnEnd ()
		{
			
		}
		
		#endregion

		public override void OnUpdate ()
		{
			base.OnUpdate ();

			if (_isEnableAnim) {
				_timer += UnityEngine.Time.deltaTime;
				if (_timer >= _delayTime) {
					_timer = 0;
					_isEnableAnim = false;

					//close the game object instance.
					LowoUN.Module.UI.UIHub.instance.CloseUI(insID);
				}
			}
		}


//		//TODO: info list
//		private List<string> testInfoList = new List<string> () {
//			"hello world 001!",
//			"hello world 002!",
//			"hello world 003!"
//		};
//		private int testInfoListIdx = 0;
//
//		public override void OnTest ()
//		{
//			if (testInfoListIdx >= (testInfoList.Count - 1))
//				testInfoListIdx = testInfoList.Count - 1;
//	
//			_info = testInfoList [testInfoListIdx];
//			info = _info;
//			testInfoListIdx += 1;
//		}

		private float _delayTime;
		private float _timer;
		public void SetInfo (string desc, float delayTime) {
			_info = desc;
			_delayTime = delayTime;

			onUpdateState ((int)HolderObjs.Con_Notify, UIStateType.Hide);
			//UIHub.instance.ToggleItem(UIHub.instance.GetHolder(insID).gameObject, false);
		}


		private bool _isEnableAnim;
		public void StartShow () {
			_timer = 0f;
			_isEnableAnim = true;

			onUpdateState ((int)HolderObjs.Con_Notify, UIStateType.Show);
			//UIHub.instance.ToggleItem(UIHub.instance.GetHolder(insID).gameObject, true);
		}

		public void SetDisableAnim () {
			_isEnableAnim = false;
		}
	}
}