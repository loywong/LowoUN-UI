#pragma warning disable 0649//ignore default value null
using System.Text;
using UnityEngine;

namespace LowoUN.Module.UI.HUDText {
	public class UIHudText : MonoBehaviour {
		private static UIHudText _instance = null;
		public static UIHudText instance {
			get {
				if (_instance == null)
					Debug.LogError ("====== LowoUN-UI ===> no ui hud text util found !");

				return _instance;
			}
		}

		[SerializeField]
		private GameObject _root_HP;
		//[SerializeField]
		//private GameObject _root_Bubble;

		void Awake () {
			_instance = this;

			HandleNotify (true);
		}
		void OnDestroy () {
			HandleNotify (false);
		}

		private void HandleNotify (bool isHandle) {
			if (isHandle) {
				//UINotifyMgr.Register<UnityEngine.Vector3, int, Pair<int, bool>>(UIHudText.instance.SetHP, "UW_Battle-HPInfo");
				//UINotifyMgr.Register<Vector3, int, PropType>(UIHpText.instance.Set, "w2u_prop-info");
				//UINotifyMgr.Register<Vector3, string, UIEnum_Module_Bubble_Say> (SetBubble, "UW_Bubble-Info");
			} else {
				//UINotifyMgr.Remove<Vector3, string, UIEnum_Module_Bubble_Say> (SetBubble);
			}
		}

		public Vector2 GetScreenPos (Vector3 v3) {
			if (LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera () != null) {
				Vector3 screenPoint = LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera ().WorldToScreenPoint (v3);
				return new Vector2 (screenPoint.x, screenPoint.y) * UIAdaptScreen.instance.GetScaleValue ();
			}

			return Vector2.zero;
		}

		//private GameObject _textConPrefab = null;
		//public int FontSize = 20;
		//public bool DestroyTextOnDeath = true;
		private void SetConPos (Transform ui, Vector2 pos) {
			//this._camera.WorldToViewportPoint(position);
			ui.GetComponent<RectTransform> ().anchoredPosition = pos;
		}
		//		private void OnAnimComplete (UText uiItem) {
		//
		//			//if (DestroyTextOnDeath) {
		//			if(uiItem != null){
		//				GameObject.Destroy(uiItem.gameObject);
		//				uiItem = null;
		//			}
		//		}

		//public GameObject CreateText(string text, Vector2 pos, Color color, int size, string asset)
		//{
		//	GameObject ui = UIAsset.instance.LoadPanelByName(asset, Enum_UIAsset.General) as GameObject;
		//	SetConPos (ui.transform, pos);

		//	UText item = ui.GetComponent<UText>();
		//	item.color = color;
		//	item.pos = pos;
		//	item.text = text;
		//	item.size = size;
		//	item.SetInfo (text);

		//	return ui;
		//}

		public GameObject SetHP (Vector3 goPos, string asset) //, int value, Pair<int, bool> objInfo
		{
			Vector2 uiPos = GetPopTxtPos (goPos);

			//UHUDText.instance.
			return CreateHPEff (
				uiPos,
				//				value,
				//				objInfo.first,
				//				objInfo.second,
				asset
			);
		}

		private GameObject CreateHPEff (Vector2 pos, string asset) //, int val, int harmLev, bool isEnemy, 
		{
			//GameObject tConObj = LowoUN.Module.Asset.Module_Asset.instance.LoadPanel(asset) as GameObject;
			//tConObj.transform.SetParent(GameHierarchy.instance.rootUI.transform, false);
			GameObject go = UIAsset.instance.LoadPanelByName (asset, Enum_UIAsset.Hud) as GameObject;
			go.transform.SetParent (_root_HP.transform);
			go.transform.localScale = Vector3.one;
			SetConPos (go.transform, pos);

			//Create new text info to instatiate 
			//			UTextAnim item = tConObj.GetComponent<UTextAnim>();
			//			item.onAnimComplete += OnAnimComplete;
			//			item.SetInfo (val, harmLev, isEnemy);
			//
			//			_hpConList.Add(item);

			return go;
		}

		//		public GameObject SetDamage(Vector3 goPos, int value, PropType propType)
		//		{
		//			string popupText = GetPopTxtFormat (value, propType);
		//			Vector2 uiPos    = GetPopTxtPos (goPos);//, value
		//			Color color      = GetPopTxtColor ();//value
		//			int fontSize     = GetPopTxtFontSize ();//value
		//
		//			//GameObject go = UHUDText.instance.CreateText(
		//			GameObject go = CreateText(
		//				popupText,
		//				uiPos,
		//				color,
		//				fontSize,
		//				"UI_UDamageText"
		//			);
		//
		//			return go;
		//		}

		//public GameObject SetBubble(Vector3 pos, string value, Enum_BubbleSay type)
		//private void SetBubble(Vector3 pos, string value, UIEnum_Module_Bubble_Say type)
		//{
		//	string popupText = value;//GetPopTxtFormat (value, propType);
		//	Vector2 uiPos    = GetPopTxtPos (pos);//, value
		//	Color color      = GetPopTxtColor ();//value
		//	int fontSize     = GetPopTxtFontSize ();//value

		//	//GameObject go = UHUDText.instance.CreateText(
		//	//			GameObject go = CreateText(
		//	//				popupText,
		//	//				uiPos,
		//	//				color,
		//	//				fontSize,
		//	//				"UI_UBubbleText"
		//	//			);

		//	string asset = "UI_UText_Bubble";

		//	GameObject go = UIAsset.instance.LoadPanelByName(asset, Enum_UIAsset.Hud) as GameObject;
		//	go.transform.SetParent (_root_Bubble.transform);
		//	go.transform.localScale = Vector3.one;
		//	SetConPos (go.transform, uiPos);

		//	UText item = go.GetComponent<UText>();
		//	item.color = color;
		//	item.pos = uiPos;
		//	//item.text = popupText;
		//	item.size = fontSize;
		//	item.SetInfo (popupText);

		//	//return go;
		//}

		private const float _ModelHeight = 1f;
		private const float _CorrectY = 1f;

		private Vector2 GetPopTxtPos (Vector3 goPos) { //, int value
			Vector3 newPos = goPos; // + Vector3.up * _ModelHeight + Vector3.up * _CorrectY;
			Vector3 screenPoint = LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera ().WorldToScreenPoint (newPos);
			Vector2 uiPos = new Vector2 (screenPoint.x, screenPoint.y) * UIAdaptScreen.instance.GetScaleValue ();

			return uiPos;
		}

		private int GetPopTxtFontSize () //int value
		{
			int fontSize = 20;
			return fontSize;
		}

		private Color GetPopTxtColor () //int value
		{
			Color color = Color.white;

			//TODO: set damage text color
			return color;
		}
		private string GetPopTxtFormat (int value, UIEnum_HudTxt_PropType type) {
			var popupText = new StringBuilder (); //string.Empty;

			//set value
			if (value < 0) {
				popupText.Append (value.ToString ());
			} else if (value > 0) {
				popupText.Append ("+" + value.ToString ());

				string typTxt = null;
				if (type == UIEnum_HudTxt_PropType.Def) {
					typTxt = "Defend";
				} else if (type == UIEnum_HudTxt_PropType.Addv) {
					typTxt = "Output";
				} else if (type == UIEnum_HudTxt_PropType.Hp) {
					typTxt = "Hp";
				}

				//popupText = typTxt + popupText;
				popupText.Insert (0, typTxt);
			} else {
				if (type == UIEnum_HudTxt_PropType.Def) {
					popupText.Append ("Defend state full\n");
				} else if (type == UIEnum_HudTxt_PropType.Addv) {
					popupText.Append ("Output state full\n");
				} else if (type == UIEnum_HudTxt_PropType.Hp) {
					popupText.Append ("Resist\n");
				}
			}

			return popupText.ToString ();
		}

		//public GameObject SetBubble_New (string asset) {
		//	GameObject go = UIAsset.instance.LoadPanelByName(asset, Enum_UIAsset.General) as GameObject;

		//	if (go != null && _root_Bubble != null) {
		//		go.transform.SetParent (_root_Bubble.transform);
		//		go.transform.localScale = Vector3.one;
		//	}
		//	else
		//		Debug.LogError ("====== LowoUN-UI ===> bubble ui need a ui specify root!");

		//	return go;
		//}
	}
}