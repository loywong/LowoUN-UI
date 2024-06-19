using UnityEngine;

namespace LowoUN.Module.UI {
	[ExecuteInEditMode]
	public class UISetting : MonoBehaviour {
		public bool enableCloseByDarkBg = true;

		private static UISetting _instance = null;
		public static UISetting instance {
			get {
				if (_instance == null)
					Debug.LogWarning ("====== LowoUN-UI ===> no settings GameObject for ui!");

				return _instance;
			}
		}

		void Awake () {
			_instance = this;
		}
	}
}