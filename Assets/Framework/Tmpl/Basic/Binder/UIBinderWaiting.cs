using LowoUN.Module.UI;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Business.UI {
	using HolderObjs = UIHandler.Objs_CommonWaiting;

	public class UIBinderWaiting : UIBinder {

		public UIBinderWaiting (int uiPanelType, int instanceID) : base (uiPanelType, instanceID) {

		}

		private int hashcode;
		private string notifyKey;
		private string protocolStr;
		private int pointCount = 0;
		private float lastTime = 0;
		private float invertal = 0.4f;

		public override void OnStart () {
			base.OnStart ();

			hashcode = this.GetHashCode ();
			notifyKey = "UI_Waiting_ServerCallback" + hashcode;

			onUpdateState ((int) HolderObjs.Txt_Desc, UIStateType.Hide);
			//#if RELEASE
			//delay to show waiting dark bg
			TimeWatcher.instance.AddWatcher (notifyKey, 2000U, false, () => {
				onUpdateState ((int) HolderObjs.Img_Mask, UIStateType.Show);
				onUpdateState ((int) HolderObjs.Txt_Desc, UIStateType.Show);
#if !RELEASE
				onUpdateState ((int) HolderObjs.Txt_Protocol, UIStateType.Show);
#endif
				TimeWatcher.instance.RemoveWatcher (notifyKey + hashcode);
			});
			//#else
			//onUpdateState((int)HolderObjs.Img_Mask, UIStateType.Show);
			//#endif

#if RELEASE
			onUpdateState ((int) HolderObjs.Txt_Protocol, UIStateType.Hide);
#endif
		}

		public override void OnUpdate () {
			lastTime += Time.deltaTime;
			if (lastTime > invertal) {
				lastTime = 0;
				pointCount++;
				if (pointCount > 3)
					pointCount = 0;
				string text = "Waiting"; //DataParserHelper.getMessage(4616);
				for (int i = 0; i < 3; i++) {
					if (i < pointCount)
						text += ".";
					else
						text += " ";
				}
				onUpdateTxt ((int) HolderObjs.Txt_Desc, text);

				//				#if !RELEASE
				//				protocolStr = HttpManager.currentRequestPkg.protocol_name + "(";
				//				foreach(var s in HttpManager.currentRequestPkg.parameters.Values)
				//				{
				//					if(!protocolStr.EndsWith("("))
				//						protocolStr += ",";
				//					protocolStr += s;
				//				}
				//				protocolStr += ")";
				//				onUpdateTxt ((int)HolderObjs.Txt_Protocol, protocolStr);
				//				#endif
			}
		}

		protected override void OnEnd () {
			protocolStr = "";
			if (TimeWatcher.instance.ContainKey (notifyKey)) {
				TimeWatcher.instance.RemoveWatcher (notifyKey);
			}
		}
	}
}