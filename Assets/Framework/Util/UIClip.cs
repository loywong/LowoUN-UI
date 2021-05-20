using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clip out screen UIHolder, Author by shenmo
/// Be careful with the UIHolder Active status
/// </summary>
namespace LowoUN.Module.UI
{
	public class UIClip
	{
		static private bool _enable = true;

		static private Vector2 _resolution = Vector2.zero;

		static public Vector2 resolution{
			get{
				if (_resolution == Vector2.zero) {
					if (UIAdaptScreen.instance != null) {
						_resolution = UIAdaptScreen.instance.gameObject.GetComponent<CanvasScaler> ().referenceResolution;
					} else {
						_enable = false;
					}
				}
				return _resolution;
			}
		}
		static public void setEnable(bool b)
		{
			_enable = b;
			if (!b) {
				foreach (var n in UILinker.instance.uiHolderDict.Values) {
					n.SetInScreen(true);
				}
			}
		}

		static public void DoClip()
		{
			if (_enable) {
				foreach (var n in UILinker.instance.uiHolderDict.Values) {
					if (n != null) {
						RectTransform rt = n.rectTransform;
						Vector3[] corners = new	Vector3[4];
						rt.GetWorldCorners (corners);
						float camscale = UIAdaptScreen.instance.GetFixRate4TransformUICamera ();
						bool inScreen = true;
						if (corners [2].x * camscale < -resolution.x / 2 || corners [1].x  * camscale > resolution.x / 2 || corners [2].y  * camscale  < -resolution.y / 2 || corners [3].y  * camscale > resolution.y / 2)
							inScreen = false;
						else
							inScreen = true;
						//hide or unhide
						n.SetInScreen(inScreen);
					}
				}
			}
		}
	}
}