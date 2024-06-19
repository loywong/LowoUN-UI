using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI {
	public class UIAdaptScreen : MonoBehaviour {
		private CanvasScaler canvasScaler;
		private CanvasScaler.ScaleMode scaleMode;
		private Vector2 referenceResolution;
		private float match;

		private static UIAdaptScreen _instance = null;
		public static UIAdaptScreen instance {
			get {
				//_instance = GameObject.FindObjectOfType<UIAdaptScreen>();
				//if (_instance == null)
				//	_instance = new GameObject("UIAdaptScreen").AddComponent<UIAdaptScreen>();
				if (_instance == null)
					Debug.LogError ("====== LowoUN-UI ===> don't forget add script 'UIAdaptScreen' !!!");

				return _instance;
			}
		}

		void Awake () {
			if (_instance != null) {
				Debug.LogError ("====== LowoUN-UI ===> UIAdaptScreen - add to gameobject repeatedly is Forbidden");
			} else {
				_instance = this;

				canvasScaler = gameObject.GetComponent<CanvasScaler> ();
				scaleMode = canvasScaler.uiScaleMode;
				referenceResolution = canvasScaler.referenceResolution;
				match = canvasScaler.matchWidthOrHeight;
			}
		}

		// Update is called once per frame
		void Update () {

		}

		public float GetScaleValue () {
			float val = 1f;

			if (referenceResolution.x > referenceResolution.y) {
				val = referenceResolution.y / Screen.height;
			} else {
				val = referenceResolution.x / Screen.width;
			}

			return val;
		}

		public bool isWidthTendencyWhenVScreen {
			get {
				/*~= 3:4*/
				float proportion = Screen.width / (float) Screen.height;
				return proportion > 0.7f;
			}
		}

		public float GetScaleRate () {
			float rateX = 1f;
			float rateY = 1f;
			float rate = 1f;

			rateX = referenceResolution.x / Screen.width;
			rateY = referenceResolution.y / Screen.height;

			if (referenceResolution.x > referenceResolution.y) {
				rate = rateX / rateY;
			} else {
				rate = rateY / rateX;
			}

			return rate;
		}

		public float GetFixRate4TransformUICamera () {
			return referenceResolution.y / 2 / CalMainCameraSize ();
			//if(referenceResolution.x >= referenceResolution.y)
			//	return referenceResolution.y / 2 / (referenceResolution.x / referenceResolution.y);
			//else 
			//	return referenceResolution.y / 2 / (referenceResolution.y / referenceResolution.x);
		}

		private float CalMainCameraSize () {
			float s = 0f;
			//HACK for partical effect on ui
			if (referenceResolution.x >= referenceResolution.y) {
				s = referenceResolution.x / referenceResolution.y;
				//should be
				//s = referenceResolution.x / UIAsset.instance.mainCanvas.planeDistance / 2;
			} else {
				s = referenceResolution.y / referenceResolution.x;
			}

			//cam.orthographicSize = s;
			return s;
		}
	}
}