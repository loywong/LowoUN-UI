#pragma warning disable 0649//ignore default value null
using LowoUN.Util.Notify;
using UnityEngine;

namespace LowoUN.Module.UI.HUDText {
    public class UIHudText_Bubble : MonoBehaviour {
        private static UIHudText_Bubble _instance = null;
        public static UIHudText_Bubble instance {
            get {
                if (_instance == null)
                    Debug.LogError ("====== LowoUN-UI ===> no ui hud text util found !");

                return _instance;
            }
        }

        [SerializeField]
        private GameObject _root_Bubble;

        void Awake () {
            _instance = this;

            HandleNotify (true);
        }
        void OnDestroy () {
            HandleNotify (false);
        }

        private void HandleNotify (bool isHandle) {
            if (isHandle) {
                NotifyMgr.AddListener<Vector3, string, UIEnum_Module_Bubble_Say> ("UW_Bubble-Info", SetBubble);
            } else {
                NotifyMgr.AddListener<Vector3, string, UIEnum_Module_Bubble_Say> ("UW_Bubble-Info", SetBubble);
            }
        }

        public Vector2 GetScreenPos (Vector3 v3) {
            if (LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera () != null) {
                Vector3 screenPoint = LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera ().WorldToScreenPoint (v3);
                return new Vector2 (screenPoint.x, screenPoint.y) * UIAdaptScreen.instance.GetScaleValue ();
            }

            return Vector2.zero;
        }

        private void SetConPos (Transform ui, Vector2 pos) {
            //this._camera.WorldToViewportPoint(position);
            ui.GetComponent<RectTransform> ().anchoredPosition = pos;
        }

        private void SetBubble (Vector3 pos, string value, UIEnum_Module_Bubble_Say type) {
            string popupText = value; //GetPopTxtFormat (value, propType);
            Vector2 uiPos = GetPopTxtPos (pos); //, value
            Color color = GetPopTxtColor (); //value
            int fontSize = GetPopTxtFontSize (); //value

            string asset = "UI_UText_Bubble";

            GameObject go = UIAsset.instance.LoadPanelByName (asset, Enum_UIAsset.Hud) as GameObject;
            go.transform.SetParent (_root_Bubble.transform);
            go.transform.localScale = Vector3.one;
            SetConPos (go.transform, uiPos);

            UText_Bubble item = go.GetComponent<UText_Bubble> ();
            item.color = color;
            item.pos = uiPos;
            //item.text = popupText;
            item.size = fontSize;
            item.SetInfo (popupText);

            //return go;
        }

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
            return "";
        }

        public GameObject SetBubble_New (string asset) {
            GameObject go = UIAsset.instance.LoadPanelByName (asset, Enum_UIAsset.General) as GameObject;

            if (go != null && _root_Bubble != null) {
                go.transform.SetParent (_root_Bubble.transform);
                go.transform.localScale = Vector3.one;
            } else
                Debug.LogError ("====== LowoUN-UI ===> bubble ui need a ui specify root!");

            return go;
        }
    }
}