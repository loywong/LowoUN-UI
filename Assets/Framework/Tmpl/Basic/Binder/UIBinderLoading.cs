using LowoUN.Module.UI;
using LowoUN.Util.Notify;

namespace LowoUN.Business.UI 
{
	using HolderObjs = UIHandler.Objs_Loading;

	public class UIBinderLoading : LowoUN.Module.UI.UIBinder 
	{
		private float _progress;
		public float progress {
			set{
				if (_progress != value) {
					//#if UNITY_EDITOR
					//UnityEngine.Debug.Log ("====== LowoUN-UI ======> loading progress : " + value);
					//#endif
					_progress = value;
					onUpdateProg ((int)HolderObjs.Prog_Loader, value * 100f, 100f);
				}
			}
		}
//		public string tipText {
//			set{
//				onUpdateTxt ((int)HolderObjs.Txt_TipDesc, value);
//			}
//		}

		public string desc {
			set{
				onUpdateTxt ((int)HolderObjs.Txt_Desc, value);
			}
		}

		public UIBinderLoading(int uiPanelType, int instanceID) : base(uiPanelType, instanceID) {

		}


		private string loadServerDesc;
		private string loadMapDesc;

		public override void OnAwake () {
			loadServerDesc = "更新服务器资源中...";//DataParserHelper.getMessage(4614);
			loadMapDesc = "关卡进入中...";//DataParserHelper.getMessage(4615);
			SetType (0);
			SetProg (0);
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

		public void SetType (UIEnum_LoadingTyp typ) {
			if(typ == UIEnum_LoadingTyp.FromServer)      desc = loadServerDesc;
			else if(typ == UIEnum_LoadingTyp.FromClient) desc = loadMapDesc; 
		}

		public void SetProg (float progress) {
			if (progress <= 1f) {
				this.progress = progress;
				//SetVisual (false);
				//SystemStatistic.SetLoadingTime ();
				NotifyMgr.Broadcast("UI__UpdateLoadingTime");
			}
		}

		public void SetVisual (bool isShow) {
			if (isShow) {
				onUpdateState ((int)HolderObjs.Con_Bg, UIStateType.Show);
				onUpdateState ((int)HolderObjs.Con_Logo, UIStateType.Show);
				onUpdateState ((int)HolderObjs.Con_Loader, UIStateType.Show);
			}
			else {
				SetProg (0);
				onUpdateState ((int)HolderObjs.Con_Bg, UIStateType.Hide);
				onUpdateState ((int)HolderObjs.Con_Logo, UIStateType.Hide);
				onUpdateState ((int)HolderObjs.Con_Loader, UIStateType.Hide);
			}
		}
	}
}