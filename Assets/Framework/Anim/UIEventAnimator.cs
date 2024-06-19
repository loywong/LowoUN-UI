#pragma warning disable 0649//ignore default value null
using System.Collections.Generic;
using LowoUN.Util.Notify;
using UnityEngine;

namespace LowoUN.Module.UI {
	public enum UIEventType {
		None,
		GoldOnTarget,
		DiamondOnTarget,
		MeltingEquip,
	}

	public class UIEventAnimator : MonoBehaviour {
		[SerializeField]
		private List<UIEventAnim> uiEventAnimList;

		private System.Action onCompleteEvent;

		void Start () {
			NotifyMgr.AddListener<UIEventType> ("UIEventAnim", OnUIEventAnim);
			NotifyMgr.AddListener<UIEventType> ("UIEventAnim_Stop", OnUIStopEventAnim);
		}

		void OnDestroy () {
			NotifyMgr.RemoveListener<UIEventType> ("UIEventAnim", OnUIEventAnim);
			NotifyMgr.RemoveListener<UIEventType> ("UIEventAnim_Stop", OnUIStopEventAnim);
		}

		void Update () {
			foreach (UIEventAnim n in uiEventAnimList) {
				if (n.obj != null) {
					n.OnUpdate ();
				}
			}
		}

		public void Play (UIEventType eventAnimType) {
			OnUIEventAnim (eventAnimType);
		}

		public void OnUIEventAnim (UIEventType eventName) {
			foreach (UIEventAnim n in uiEventAnimList) {
				if (n.obj != null) {
					if (n.EventName == eventName) {
						n.Play ();
					}
				}
			}
		}
		public void OnUIStopEventAnim (UIEventType eventName) {
			foreach (UIEventAnim n in uiEventAnimList) {
				if (n.obj != null) {
					if (n.EventName == eventName) {
						n.Stop ();
					}
				}
			}
		}

	}

	[System.Serializable]
	public class UIEventAnim {
		public UIEventType EventName;
		public float delay;
		public float finishTime = 0;
		public GameObject obj;
		public AnimationClip anim;
		private bool started = false;
		private bool played = false;
		private float timeGo;

		public void OnUpdate () {
			timeGo += Time.deltaTime;
			if (started) {
				if (timeGo >= delay) {
					timeGo = 0;
					UIAnimPlayer.Play (obj, anim);
					started = false;
					played = true;
				}
			} else if (played) {
				if (finishTime > 0 && timeGo >= finishTime) {
					Stop ();
				}
			}
		}

		public void Stop () {
			started = false;
			played = false;

			UIAnimPlayer.Stop (obj, anim);
			NotifyMgr.Broadcast<UIEventType> ("UIEventAnim_Finish", EventName);
		}

		public void Play () {
			started = true;
			timeGo = 0;
		}
	}
}