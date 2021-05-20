using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com
{
	[RequireComponent(typeof(Text))]
	public class UITxt : MonoBehaviour, IName {
		[SerializeField]
		private bool isShadow = false;

		private Shadow shadow;

		void Avake () {
			GetComponent<Text> ().raycastTarget = false;

			if (isShadow) {
				shadow = GetComponent<Shadow> ();
				if (shadow == null)
					shadow = gameObject.AddComponent<Shadow> ();
			}
		}

		public void SetName (string name) {
			GetComponent<Text> ().text = name;
		}
	}
}