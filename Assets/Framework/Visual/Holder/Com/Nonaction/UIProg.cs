#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	public class UIProg : MonoBehaviour, IProgress {
		[SerializeField]
		private UIFrameAnimPlayer frameAnim;
		[SerializeField]
		private bool isUseSlider = true;
		[SerializeField]
		private Slider progBar_Slider;
		[SerializeField]
		private Image progBar_Img;

		[SerializeField]
		private Text txtInfo;
		[SerializeField]
		private bool isRateLayout;

		private UIStateAnimator animEffect;
		//private UIFrameAnim frameAnimEffect;

		void Awake () {

			if (isUseSlider) {
				if (progBar_Slider == null) {
#if UNITY_EDITOR
					Debug.LogWarning (" ====== LowoUN-UI ===> Don't forget to set progerss bar _ Slider Type!");
#endif
				}
			} else {
				if (progBar_Img == null) {
#if UNITY_EDITOR
					Debug.LogWarning ("  ====== LowoUN-UI ===> Don't forget to set progerss bar _ Imgae Type!");
#endif
				} else {
					progBar_Img.type = Image.Type.Filled;
				}
			}

			if (GetComponent<UIStateAnimator> () != null)
				animEffect = GetComponent<UIStateAnimator> ();

			//SetValue (0,0);
		}

		// Update is called once per frame
		void Update () {

		}

		//		public void SetValue (float cur, float max) {
		//		
		//		}

		public void SetValue (int cur, int max) {
			float rate;
			if (max == 0) {
#if UNITY_EDITOR
				Debug.LogWarning ("====== LowoUN-UI ===> [progress bar] max value should not be 0 !!! ");
#endif
				cur = 0;
				rate = 0;
			} else {
				rate = (float) cur / (float) max;
			}

			if (txtInfo != null) {
				if (!isRateLayout) {
					txtInfo.text = cur + " / " + max;
				} else {
					if (rate > 1f)
						rate = 1f; //zhao添加，超过100%时，仍显示100%
					txtInfo.text = Mathf.Round (rate * 100) + "%";
				}
			}

			if (isUseSlider) {
				if (progBar_Slider != null)
					progBar_Slider.value = rate;
			} else {
				if (progBar_Img != null)
					progBar_Img.fillAmount = rate;
			}

			if (rate == 1) {
				PlayNormalAnimEffect (true);
				PlayFrameAnimEffect (true);
			} else {
				PlayNormalAnimEffect (false);
				PlayFrameAnimEffect (false);
			}
		}

		private void PlayNormalAnimEffect (bool isPlay) {
			if (animEffect != null) // && animEffect.isActiveAndEnabled
				animEffect.Play (isPlay == true ? UIStateType.Enable : UIStateType.Disable);
		}

		private void PlayFrameAnimEffect (bool isPlay) {
			if (frameAnim != null) {
				if (isPlay) {
					frameAnim.gameObject.SetActive (true);
					if (!frameAnim.isPlaying) {
						frameAnim.StartPlay ();
					}
				} else {
					if (frameAnim.isPlaying) {
						frameAnim.StopPlay ();
					}
					frameAnim.gameObject.SetActive (false);
				}
			}
		}

		public void ShowOrHide (bool isShow) {

			if (txtInfo != null)
				txtInfo.gameObject.SetActive (isShow);

			if (isUseSlider) {
				if (progBar_Slider != null)
					progBar_Slider.gameObject.SetActive (isShow);
			} else {
				if (progBar_Img != null)
					progBar_Img.gameObject.SetActive (isShow);
			}
		}
	}
}