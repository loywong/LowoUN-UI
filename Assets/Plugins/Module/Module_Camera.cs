using UnityEngine;

namespace LowoUN.Module.Cameras {
	public class Module_Camera : MonoBehaviour {

		private static Module_Camera _instance = null;
		public static Module_Camera instance {
			get {
				if (_instance == null) {
					Debug.LogError ("====== LowoUN / Module / Camera ===> hasn't found this module.");
				}
				return _instance;
			}
		}

		[SerializeField]
		private Camera defaultCam;

		void Awake () {
			_instance = this;
		}

		public Camera GetCurCamera () {
			return curCamera ?? defaultCam;
		}

		Camera _curCamera;
		Camera curCamera {
			get { return _curCamera; }
			set { _curCamera = value; }
		}
	}
}