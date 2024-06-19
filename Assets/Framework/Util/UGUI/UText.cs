using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.UGUI {
	public class UText : MonoBehaviour {
		public CanvasGroup layoutRoot = null;
		public Text insText = null;
		public RectTransform rect;
		[HideInInspector]
		public Color color;
		//    [HideInInspector]
		//    public UDriectionType movement;
		[HideInInspector]
		public float xOffset;
		[HideInInspector]
		public float yOffset;
		[HideInInspector]
		public int size;
		[HideInInspector]
		public float speed;
		[HideInInspector]
		public string text;
		[HideInInspector]
		public Vector3 pos;
		[HideInInspector]
		public float xAcceleration;
		[HideInInspector]
		public float yAcceleration;
		[HideInInspector]
		public float yQuicknessScaleFactor;
	}
}