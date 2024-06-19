#pragma warning disable 0649//ignore default value null
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.UI.HUDText {
	[ExecuteInEditMode]
	public class UIHudText_Notify_CacheTwo : MonoBehaviour {
		private static UIHudText_Notify_CacheTwo _instance = null;
		public static UIHudText_Notify_CacheTwo instance {
			get {
				if (_instance == null)
					Debug.LogError ("====== LowoUN-UI ===> no UIHudText_CacheTwo ins found !");

				return _instance;
			}
		}

		[SerializeField]
		private bool isEnable = true;

		[SerializeField]
		private float _duration = 3f;
		public uint duration { get { return (uint) (_duration * 1000); } }

		[SerializeField]
		private List<HudNotifyer> notifyList;
		private Queue<string> _msgs = new Queue<string> ();

		void Awake () {
			_instance = this;
			_msgs = new Queue<string> ();

			if (notifyList.Count <= 0) {
				Debug.LogWarning (" ====== wangliang ===> no notify tips find!!!");
				return;
			}

			for (int i = 0; i < notifyList.Count; i++) {
				notifyList[i].Init (i);
				notifyList[i].onComplete += CompleteNotify;
			}
		}

		void OnDestroy () {
			if (_msgs != null)
				_msgs.Clear ();
		}

		private void CompleteNotify (HudNotifyer n) {
			//当前机制，相当于不需要处理第二个容器的结束事件
			if (n == notifyList[1])
				return;

			if (!notifyList[1].isComplete) {
				string msg = notifyList[1].msg;
				notifyList[0].OnStart (msg);
				notifyList[1].Reset ();
			}

			CheckUIQueue ();
		}

		//		private static bool isTest = true;
		public void Show (string msg) {
			//Debug.LogWarning ("@@@@@@@@@@@@@@ test notify !!!!!!");
			_msgs.Enqueue (msg);

			//1, if the first container is free, add to it
			//2, if the second container is free, add to it

			//3, if the second is busy, force end the first container
			//4, set the second container info to the fist containter
			//5, the new info set to the second container

			if (isEnable)
				CheckUIQueue ();
		}

		public void SetEnable (bool isEnable) {
			this.isEnable = isEnable;
			if (this.isEnable)
				CheckUIQueue ();
		}

		private void CheckUIQueue () {
			//两个都在被使用中
			if (!notifyList[0].isComplete && !notifyList[1].isComplete)
				return;

			//1被使用 2空闲
			if (!notifyList[0].isComplete && notifyList[1].isComplete) {
				if (_msgs.Count > 0)
					notifyList[1].OnStart (_msgs.Dequeue ());

				return;
			}
			//12都处于空闲状态
			if (_msgs.Count > 0)
				notifyList[0].OnStart (_msgs.Dequeue ());
			if (_msgs.Count > 0)
				notifyList[1].OnStart (_msgs.Dequeue ());
		}

		public void ClearAll () {
			notifyList[0].Reset ();
			notifyList[1].Reset ();

			_msgs.Clear ();
		}
	}
}