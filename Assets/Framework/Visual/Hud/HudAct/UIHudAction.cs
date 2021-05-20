using UnityEngine;
using System.Collections;

namespace LowoUN.Module.UI.HudAction
{
	public class UIHudAction : MonoBehaviour 
	{
		private static UIHudAction _instance = null;
		public static UIHudAction instance	{
			get {
				//if (_instance == null) {
				//	_instance = GameObject.FindObjectOfType<UIHudAction>();
				//
				//	if(_instance == null)
				//		_instance = new GameObject("UIActionEffect").AddComponent<UIHudAction>();
				//}
				if (_instance == null) 
					Debug.LogError ("====== LowoUI-UN ===> no ui hud act util found !");
				
				return _instance;
			}
		}

		public float speed = 5f;
		public float xStartAcce = 1f;
		public float yStartAcce = 0f;
		public float yAcceScaleFactor = 1f;//2.2f;
		private string uiSpriteName = string.Empty;


		void Awake () {
			_instance = this;

			HandleNotify (true);
		}
		void OnDestroy () {
			HandleNotify (false);
		}

		private void HandleNotify (bool isHandle)
		{
			if (isHandle) {
				//UINotifyMgr.Register<UIEffectType, int>(UIActionEffect.instance.Set, ui_msg.w2u_onUIEffect4Action.ToString());
			} else {
			}
		}

		// Update is called once per frame
		void Update () {
		
		}

		public void Set(UIEnum_HudAct_Typ actionType, int agentId)//, Vector3 actionWorldPos
		{
			if (actionType == UIEnum_HudAct_Typ.Boom) {
				uiSpriteName = "ui_effect_boom";
			} else if (actionType == UIEnum_HudAct_Typ.Hong) {
				uiSpriteName = "ui_effect_hong";
			} else if (actionType == UIEnum_HudAct_Typ.Pa) {
				uiSpriteName = "ui_effect_poop";
			} else {
				return;
			}

//	        float biasX = 3f;
//	        float biasY = 1.5f;
			UHUDAction.instance.CreateImage (
				uiSpriteName,
				Vector3.one,//MainGameUnity.instance.agentDict [agentId].transform.position + Vector3.up * MainGameUnity.instance.agentDict [agentId].ModelHeight + Vector3.up * Random.Range (-biasY, biasY) + Vector3.right * Random.Range (-biasX, biasX), //agent world pos
				this.speed, //speed
				this.xStartAcce, 
				this.yStartAcce, 
				this.yAcceScaleFactor, 
				UIEnum_Hud_UDriectionType.Right
			);
		}
	}
}