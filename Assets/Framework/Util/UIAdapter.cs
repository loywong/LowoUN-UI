using UnityEngine;

namespace LowoUN.Module.UI 
{
	[ExecuteInEditMode]
	public class UIAdapter : MonoBehaviour {

		[SerializeField]
        private float Refit = 1f;
		[SerializeField]
		private bool isVScreen = true;

		public float RRefit {get{ return Refit;}}

		private float curRefit;
		void Awake () {
			curRefit = RRefit;
			Reset ();
		}
		
		// Update is called once per frame
		void Update () {
			#if UNITY_STANDALONE_WIN
			transform.GetComponent<RectTransform>().localScale = Refit * Vector3.one/UIAdaptScreen.instance.GetScaleRate();
			#endif

			#if UNITY_EDITOR
			if(curRefit != RRefit){
				curRefit = RRefit;
				Reset();
			}
			#endif
		}

		private void Reset () {
			//For UGUI
			if(transform.GetComponent<RectTransform>() != null){
				if (isVScreen) {
					if (UIAdaptScreen.instance.isWidthTendencyWhenVScreen) 
						transform.GetComponent<RectTransform>().localScale = Refit * Vector3.one/UIAdaptScreen.instance.GetScaleRate();
					else
						transform.GetComponent<RectTransform>().localScale = Vector3.one/UIAdaptScreen.instance.GetScaleRate();
				}
				else
					transform.GetComponent<RectTransform>().localScale = Vector3.one/UIAdaptScreen.instance.GetScaleRate();
			}

			//if (transform.GetComponent<Transform> () != null) {
			//	transform.GetComponent<Transform>().localScale = Vector3.one/UIAdaptScreen.instance.GetScaleRate();
			//}
		}
	}
}