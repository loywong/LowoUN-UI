using LowoUN.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LowoUN.Module.UI.UGUI {
	public class UGUIUtil {
		/// <summary>
		/// create canvas
		/// </summary>
		/// <returns></returns>
		public static GameObject CreateCanvas () {
			GameObject canvas = new GameObject ("CanvasHud", typeof (RectTransform));
			RectTransform rect = canvas.GetComponent<RectTransform> ();
			rect.position = new Vector3 (Screen.width / 2, Screen.height / 2, 0);
			rect.sizeDelta = new Vector2 (Screen.width, Screen.height);
			Canvas can = canvas.AddComponent<Canvas> ();
			can.renderMode = RenderMode.ScreenSpaceCamera;
			can.sortingOrder = -1;
			CanvasScaler cs = canvas.AddComponent<CanvasScaler> ();
			cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			cs.referenceResolution = new Vector2 (Screen.width, Screen.height);
			canvas.AddComponent<GraphicRaycaster> ();
			if (GameObject.FindObjectOfType<EventSystem> () == null) {
				GameObject eventSystem = new GameObject ("EventSystem", typeof (EventSystem));
				eventSystem.AddComponent<StandaloneInputModule> ();
				//eventSystem.AddComponent<TouchInputModule>();//no need in the latest version
			}
			canvas.layer = LayerMask.NameToLayer ("UI");
			return canvas;
		}

		/// <summary>
		/// create label
		/// </summary>
		/// <returns></returns>
		public static Text CreateLabel () {
			Text text = new GameObject ("Text").AddComponent<Text> ();
			text.rectTransform.sizeDelta = new Vector2 (100, 50);
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.anchoredPosition = new Vector2 (.5f, .5f);
			text.text = "";
			text.font = Resources.FindObjectsOfTypeAll<Font> () [0];
			text.fontSize = 14;
			text.color = Color.yellow;
			text.alignment = TextAnchor.MiddleCenter;
			UText utext = text.gameObject.AddComponent<UText> ();
			utext.insText = text;
			utext.rect = text.GetComponent<RectTransform> ();
			Outline outline = text.gameObject.AddComponent<Outline> ();
			outline.effectColor = new Color (0f, 0f, 0f, 62f / 255f);
			text.gameObject.layer = LayerMask.NameToLayer ("UI");
			return text;
		}

		public static Image CreateImage () {
			return null;
		}

		public static Pair<Vector2, Vector2> GetTranMargins (GameObject go) {
			return null;
		}
	}
}