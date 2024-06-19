using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI {
	public static class UIAnimPlayer {
		public static void PlayAutoAnim (GameObject obj, AnimationClip animClip) {
			MakeSureAnimClipExist (obj, animClip);
			obj.GetComponent<Animation> ().clip = animClip;
			obj.GetComponent<Animation> ().playAutomatically = true;
			PlayAnim (obj, animClip);
		}

		public static void Play (GameObject obj, AnimationClip animClip) {
			if (animClip == null) {
				Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add on a animation clip.");
			} else {
				MakeSureAnimClipExist (obj, animClip);
				PlayAnim (obj, animClip);
			}
		}

		public static void StopAtFirstFrame (GameObject obj, AnimationClip animClip) {
			if (animClip == null) {
				Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add on a animation clip.");
			} else {
				MakeSureAnimClipExist (obj, animClip);
				StopAtFirstAnimFrame (obj, animClip);
			}
		}

		public static void StopAtLastFrame (GameObject obj, AnimationClip animClip) {
			if (animClip == null) {
				Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add on a animation clip.");
			} else {
				MakeSureAnimClipExist (obj, animClip);
				StopAtLastAnimFrame (obj, animClip);
			}
		}

		public static void Play (GameObject obj, string animStateName) {
			float currentTimeScale = Time.timeScale == 0 ? 1 : 1 / Time.timeScale;
			obj.GetComponent<Animator> ().speed = currentTimeScale;
			obj.GetComponent<Animator> ().Play (animStateName);
		}

		private static void PlayAnim (GameObject obj, AnimationClip animClip) {
			float currentTimeScale = Time.timeScale == 0 ? 1 : 1 / Time.timeScale;
			SetAnimClipSpeed (obj, animClip.name, currentTimeScale);
			obj.GetComponent<Animation> ().Play (animClip.name);
			SynAlpha4UGUI (obj);
		}

		private static void StopAtFirstAnimFrame (GameObject obj, AnimationClip animClip) {
			Animation anim = obj.GetComponent<Animation> ();
			anim[animClip.name].time = 0.0f;
			anim[animClip.name].enabled = true;
			anim[animClip.name].weight = 1;
			// Sample animations now.
			anim.Sample ();
			anim[animClip.name].enabled = false;
			SynAlpha4UGUI (obj);
		}

		private static void StopAtLastAnimFrame (GameObject obj, AnimationClip animClip) {
			Animation anim = obj.GetComponent<Animation> ();
			anim.Stop (animClip.name);
			anim[animClip.name].time = anim[animClip.name].length;
			anim[animClip.name].enabled = true;
			anim[animClip.name].weight = 1;
			// Sample animations now.
			anim.Sample ();
			anim[animClip.name].enabled = false;
			SynAlpha4UGUI (obj);
		}

		public static void Stop (GameObject obj, AnimationClip animClip) {
			if (animClip == null) {
				Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add on a animation clip.");
			} else {
				MakeSureAnimClipExist (obj, animClip);
				StopAnim (obj, animClip);
			}
		}

		private static void StopAnim (GameObject obj, AnimationClip animClip) {
			obj.GetComponent<Animation> ().Stop (animClip.name);
			SynAlpha4UGUI (obj);
		}
		//public static void Play (GameObject obj, string animClipName) {
		//	SetAnimClipSpeed (obj, animClipName, 1/Time.timeScale);
		//	obj.GetComponent<Animation> ().Play (animClipName);
		//	UIAnimation.SynAnimAlpha (obj);
		//}

		private static void SetAnimClipSpeed (GameObject obj, string animClipName, float speed) {
			if (obj != null)
				obj.GetComponent<Animation> () [animClipName].speed = speed;
		}

		private static void SynAlpha4UGUI (GameObject go) {
			//Debug.Log ("+++++++ image count: " + item.obj.GetComponentsInChildren<Image> ().Length);
			//deal with a bug about UGUI image's alpha transparency through legacy animation
			//if (go.GetComponentsInChildren<Image> ().Length > 0 || go.GetComponentsInChildren<Text> ().Length > 0) 
			if (go.GetComponentsInChildren<MaskableGraphic> ().Length > 0) {
				if (go.GetComponent<UIAnimLegacyForUGUI> () == null)
					go.AddComponent<UIAnimLegacyForUGUI> ();
				go.GetComponent<UIAnimLegacyForUGUI> ().CheckAlphaAnimation (go, true);
			}
		}

		private static void MakeSureAnimClipExist (GameObject obj, AnimationClip animClip) {
			if (obj == null)
				return;

			if (obj.GetComponent<Animation> () == null) {
				//Debug.Log ("Add animation component to the game object!");
				obj.AddComponent<Animation> ().playAutomatically = false;
			}

			if (obj.GetComponent<Animation> ().GetClip (animClip.name) == null) {
				//Debug.Log ("Add animation clip to the game object!!!!!" + animClip.name);
				obj.GetComponent<Animation> ().AddClip (animClip, animClip.name);
			}
		}

		public static void RevertPlayAnim (GameObject go, AnimationClip animClip) {
			//go.GetComponent<Animation> () [animClip.name].speed = -1;
			SetAnimClipSpeed (go, animClip.name, -1);
			go.GetComponent<Animation> () [animClip.name].time = go.GetComponent<Animation> () [animClip.name].length;
			go.GetComponent<Animation> ().Play (animClip.name);
			//if animation current clip is setted, then just Play() without any parameters
		}
	}
}