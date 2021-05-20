using UnityEngine;

namespace LowoUN.Module.UI 
{
	public class UIAnimMoveTrigger: MonoBehaviour {
		public bool trigger = false;
		void Update()
		{
			if (trigger) {
				UIAnimMoveBag.Play (gameObject.GetComponent<RectTransform> ());
				trigger = false;
			}
		}
	}
}