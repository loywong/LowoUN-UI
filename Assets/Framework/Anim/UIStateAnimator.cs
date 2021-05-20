#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace LowoUN.Module.UI 
{
	public enum UIStateType {
		None        = 0,
		ValueChange = 1,
		Selected    = 10,//
		Deselected  = 11,//
		Show        = 20,
		Hide        = 21,
		Open     	= 22,
		Close     	= 23,
		Enable      = 30,
		Disable     = 31,
		MouseDown   = 40,
		MouseUp     = 41,
		Action1    	= 51,//custom animation 1
		Action2    	= 52,//custom animation 2
		Action3    	= 53,//custom animation 3
	}

	public class UIStateAnimator : MonoBehaviour {
		[SerializeField]
		private AnimationClip autoPlayAnim;
		//[SerializeField]
		private float defaultAnimTime = 1f;
		[SerializeField]
		private List<UIStateObj> uiStatesObj;

		//public bool isActiveFalseWhenHide = true;

		[HideInInspector]
		public int current_group_filter = 0;

		private UIStateType animType = UIStateType.None;

		public void InitStateObjs(List<UIStateObj> states) {
			uiStatesObj = states;
		}

		public void AddStateObj(UIStateObj state) {
			if (uiStatesObj == null)
				Debug.LogError (Util.Log.Format.UI() + "turn to use func: InitStateObjs(List<UIStateObj> states)");
			
			if (uiStatesObj.Find (i => i.animType == state.animType) != null) {
				Debug.LogWarning (Util.Log.Format.UI() + "state: "+ state.animType.ToString() + " has customized!!! on the UIButton: " + gameObject.name);
				return; 
			}
			
			uiStatesObj.Add (state);
		}


		// Use this for initialization
		void Start () {
			PlayAutoAnim ();
		}

		void PlayAutoAnim () {
			if (autoPlayAnim != null) {
				UIAnimPlayer.PlayAutoAnim(gameObject, autoPlayAnim);
			}
		}

		// Update is called once per frame
		void Update () {
			if (uiStatesObj != null && uiStatesObj.Count > 0) {
				foreach (UIStateObj item in uiStatesObj) {
					foreach (UIObjAnim objItem in item.objAnimList) {
						if (objItem.obj != null) {
							objItem.OnUpdate();
						}
					}
				}
			}

			TimerForCallFinish ();
		}

		public bool isPlaying{get{ return isStartAnim;}}

		private float callFinishTime;
		private float timeGo;
		private bool isStartAnim = false;
		private void StartTimerForCallFinish (float finishToNotifyTime) {
			if (finishToNotifyTime <= 0) {
				//#if UNITY_EDITOR
				//Debug.LogWarning (string.Format ("Don't forget to set time for the animation play's complete notification on {0}!", gameObject.name));
				//#endif
				finishToNotifyTime = defaultAnimTime;
			}

			timeGo = 0;
			callFinishTime = finishToNotifyTime;
			isStartAnim = true;
		}

		private void TimerForCallFinish () {
			if (isStartAnim) {
				timeGo += Time.deltaTime;
				if (timeGo >= callFinishTime) {
					isStartAnim = false;
					if (this.onCompleteCallback != null)
						this.onCompleteCallback ();
				}
			}
		}
		
		private System.Action onCompleteCallback;
		public bool Play (UIStateType animType, System.Action callback = null) {
			if (this.animType != UIStateType.ValueChange && 
				animType != UIStateType.Action1 &&
				animType != UIStateType.Action2 &&
				animType != UIStateType.Action3)
				if (this.animType == animType)
					return true;


			if (this.animType != UIStateType.None)
				if (this.animType != animType)
					Stop ();
			
			this.animType = animType;
			this.onCompleteCallback = callback;
			return Play ();
		}

		//结束当前正在播放的动画，并停在最后一帧
		public void Skip (UIStateType animType) {
			if (uiStatesObj != null && uiStatesObj.Count > 0) {
				foreach (UIStateObj item in uiStatesObj) {
					if (item.animType == animType && current_group_filter == item.group_filter) {
						foreach (UIObjAnim objItem in item.objAnimList) {
							if (objItem.obj != null) {
								objItem.Skip();
							}
						}
					}
				}
			}
		}

		private bool Play () {
			bool hasPlayedAnim = false;

			if (uiStatesObj != null && uiStatesObj.Count > 0) {
				foreach (UIStateObj item in uiStatesObj) {
					if (item.animType == animType && current_group_filter == item.group_filter) {
						StartTimerForCallFinish (item.finishNotifyTime);

						foreach (UIObjAnim objItem in item.objAnimList) {
							if (objItem.obj != null) {
								objItem.Play();
								//StartCoroutine(StartObjPlay(objItem));
								hasPlayedAnim = true;
							}
						}
					}
				}
			}

			return hasPlayedAnim;
		}

		private void Stop () {
			if (uiStatesObj != null && uiStatesObj.Count > 0) {
				foreach (UIStateObj item in uiStatesObj) {
					if (item.animType == animType && current_group_filter == item.group_filter) {
						foreach (UIObjAnim objItem in item.objAnimList) {
							if (objItem.obj != null) {
								objItem.Stop();
							}
						}
					}
				}
			}
		}

		//HACK wait for one minute to play ui animation
//		private IEnumerator StartObjPlay (UIObjAnim objItem) {
//			yield return null;//下一帧执行
//			objItem.Play();
//		}

		//HACK for ui list
		public void SetStateParams (UIStateType animType, float delayAdd) {
			foreach (var item in uiStatesObj) {
				if (item.animType == animType) {
					foreach (var obj in item.objAnimList) {
						obj.setDelayAdd(delayAdd);
					} 
				}
			}
		}
	}

	[System.Serializable]
	public class UIStateObj {
		public UIStateType animType;
		public float finishNotifyTime = 0.5f;//just send notification,but do nothing to animation
		public int group_filter = 0;//group filter for different animation in same event
		public List<UIObjAnim> objAnimList;
		//public GameObject obj;
		//public AnimationClip anim;
	}

	[System.Serializable]
	public class UIObjAnim {
		public GameObject obj;
		public float delay;
		private float delayAdd = 0;
		public AnimationClip anim;
		
//		private UIStateType animType = UIStateType.None;
		private bool hasStartTime;
		private bool isStartTime;
		private float timeGo;
		
        //public UIObjAnim () {
        //}
		public void setDelayAdd (float add) {
			delayAdd = add;
		}
		public void OnUpdate () {
			if (isStartTime) {
				timeGo += Time.deltaTime;
				if (timeGo >= delay + delayAdd) {
					isStartTime = false;
					timeGo = 0;
					//Play ();
					UIAnimPlayer.Play(obj, anim);

					hasStartTime = false;
				}
			}
		}

		public void Play () {
			if (!hasStartTime) {
				//to run first frame
				UIAnimPlayer.StopAtFirstFrame(obj, anim);
				hasStartTime = true;
				
				timeGo = 0;
				isStartTime = true;
			}
		}

		public void Skip() {
			UIAnimPlayer.StopAtLastFrame(obj, anim);
		}

		public void Stop() {
			//UIAnimPlayer.StopAtFirstFrame(obj, anim);
			UIAnimPlayer.Stop(obj, anim);
		}
	}
}