using System.Collections.Generic;
using LowoUN.Util.Notify;
using UnityEngine;

namespace LowoUN.Module.UI.HUDText {
	public class UIHudText_Damage : MonoBehaviour {
		private static UIHudText_Damage _instance;
		public static UIHudText_Damage instance {
			get {
				if (_instance == null)
					Debug.LogError ("====== LowoUN-UI ===> UIHudText_Damage has not been initialized!");

				return _instance;
			}
		}

		[SerializeField]
		private bool _isEnable = true;
		public bool isEnable {
			get { return _isEnable; }
			set { _isEnable = value; }
		}

		[SerializeField]
		private string _uiAsset = "UI_UText_Damage";
		public string uiAsset {
			get { return _uiAsset; }
		}
		//HACK: offset for text show
		public Vector2 uiOffset = new Vector2 (0, 222f);
		public float delayToFadeout = 1f;
		public float fadeoutAnimTime = 0.5f;

		[SerializeField]
		private GameObject testModel;

		private List<UText_Damage> hpConList = new List<UText_Damage> ();

		void Awake () {
			_instance = this;

			NotifyMgr.AddListener<Vector3, int, Pair<int, bool>> ("UW_Battle-HPInfo", SetHpInfo);
		}

		private void SetHpInfo (Vector3 goPos, int value, Pair<int, bool> objInfo) {
			GameObject go = UIHudText.instance.SetHP (goPos, _uiAsset); //,value, objInfo
			UText_Damage u = go.GetComponent<UText_Damage> ();

			//value,
			//objInfo.first,
			//objInfo.second,

			u.onAnimComplete += OnAnimComplete;
			//u.SetInfo (val, harmLev, isEnemy);
			u.SetInfo (value, objInfo.first, objInfo.second);

			hpConList.Add (u);
		}

		private void OnAnimComplete (UText_Damage uiItem) {
			if (uiItem != null) {
				//if (DestroyTextOnDeath) {
				GameObject.Destroy (uiItem.gameObject);
				//}
				hpConList.Remove (uiItem as UText_Damage);
				uiItem = null;
			}
		}

		void OnDestroy () {
			NotifyMgr.RemoveListener<Vector3, int, Pair<int, bool>> ("UW_Battle-HPInfo", SetHpInfo);

			hpConList.ForEach (i => GameObject.Destroy (i.gameObject));
			hpConList.Clear ();
		}
	}
}