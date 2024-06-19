using System;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI {
	public class UIAnimLegacyForUGUI : MonoBehaviour {
		Color newColor = Color.clear;

		bool isStartCheck = false;
		Image[] imgArr;
		Text[] textArr;

		// Update is called once per frame
		void LateUpdate () {
			if (isStartCheck) {
				if (textArr.Length > 0) {
					foreach (Text txt in textArr) {
						//Vector4 v4= new Vector4 (r,g,b,a);
						newColor = txt.color;
						//Debug.Log ("img.color : " + img.color);
						//Debug.Log ("img.sprite name : " + img.sprite.name);
						txt.color = new Color (newColor.r, newColor.g, newColor.b, (float) (Math.Floor (newColor.a * 100) / 100));
					}
				}

				if (imgArr.Length > 0) {
					foreach (Image img in imgArr) {
						if (img == null)
							continue;

						//Vector4 v4= new Vector4 (r,g,b,a);
						//Debug.Log ("img.color: " + v4);
						newColor = img.color;
						//Debug.Log ("img.color : " + img.color);
						//Debug.Log ("img.sprite name : " + img.sprite.name);
						img.color = new Color (newColor.r, newColor.g, newColor.b, (float) (Math.Floor (newColor.a * 100) / 100));
					}
				}
			}
		}

		public void CheckAlphaAnimation (GameObject gameObj, bool isCheck) {
			if (gameObj.GetComponent<Animation> () != null) {
				imgArr = gameObj.GetComponentsInChildren<Image> ();
				textArr = gameObj.GetComponentsInChildren<Text> ();
			}

			isStartCheck = isCheck;
		}
	}
}