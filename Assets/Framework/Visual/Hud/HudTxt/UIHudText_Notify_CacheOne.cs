using System.Collections.Generic;
using LowoUN.Business.UI;
using UnityEngine;

namespace LowoUN.Module.UI.HUDText {
	[ExecuteInEditMode]
	public class UIHudText_Notify_CacheOne : MonoBehaviour {
		private static UIHudText_Notify_CacheOne _instance = null;
		public static UIHudText_Notify_CacheOne instance {
			get {
				if (_instance == null)
					Debug.LogError ("====== LowoUN-UI ===> no UIHudText_CacheOne ins found !");

				return _instance;
			}
		}

		private readonly float _INTERVAL = 0.5f;
		private Queue<GameObject> _goes;
		private bool _isEnableNext;
		private float _timer;
		private bool _isWork;

		public UIHudText_Notify_CacheOne () {
			_goes = new Queue<GameObject> ();
			_isWork = true;
		}

		public void Setinfo (GameObject go, string des, float time, int sysNotifyType) {
			UIBinderSystemNotify data = go.GetComponent<UIHolder> ().GetBinder () as UIBinderSystemNotify;
			data.SetInfo (des, time);
			_goes.Enqueue (go);

			if (!_isEnableNext)
				_isEnableNext = true;
		}

		private GameObject _curGo;
		public void OnUpdate () {
			if (_isWork) {
				if (_goes.Count > 0) {
					if (_isEnableNext) {
						_curGo = _goes.Dequeue ();
						UIBinderSystemNotify data = _curGo.GetComponent<UIHolder> ().GetBinder () as UIBinderSystemNotify;
						data.StartShow ();

						_isEnableNext = false;
						_timer = 0f;
					}

					if (_timer >= _INTERVAL)
						_isEnableNext = true;
					else
						_timer += Time.deltaTime;
				}
			}
		}

		public void OnEnd () {
			_isWork = false;
			foreach (var item in _goes) {
				UIBinderSystemNotify data = item.GetComponent<UIHolder> ().GetBinder () as UIBinderSystemNotify;
				data.SetDisableAnim ();
			}
		}
	}
}