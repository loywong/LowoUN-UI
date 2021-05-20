//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//
//namespace LowoUN.Module.UI.HUDText
//{
//	public class UHUDText
//	{
//	    private static UHUDText _instance = null;
//		public static UHUDText instance
//		{
//			get {
//				if (_instance == null)
//					_instance = new UHUDText();
//				return _instance;
//			}
//		}
//
//		private UHUDText () {
//		
//		}
//
//		//private GameObject _textConPrefab = null;
//	    //public int FontSize = 20;
//	    //public bool DestroyTextOnDeath = true;
//
//		private List<UText> _textConList = new List<UText>();
//
//	    /// <summary>
//	    /// set label font
//
//	    /// <summary>
//	    /// set outline color
//
//	    /// <summary>
//	    /// Set the outline distance
//
//		private void SetConPos (GameObject uiCon, Vector2 pos)
//		{
//			//this._camera.WorldToViewportPoint(position);
//			uiCon.GetComponent<RectTransform>().anchoredPosition = pos;
//		}
//
//		private void OnAnimComplete (UText uiItem) {
//
//			//if (DestroyTextOnDeath) {
//			if(uiItem != null){
//				_textConList.Remove(uiItem);
//
//				GameObject.Destroy(uiItem.gameObject);
//				uiItem = null;
//			}
//		}
//
//
//	    /// <summary>
//	    /// Disable all text.
//	    /// </summary>
//	    public void OnEnd()
//	    {
//			_textConList.ForEach (i => {
//				if (i != null) GameObject.Destroy(i.gameObject);
//				_textConList.Remove(i);
//			});
//
//			_textConList.Clear();
//	    }
//
//		public GameObject CreateText(string text, Vector2 pos, Color color, int size, string asset)
//	    {
//			GameObject tConObj = LowoUN.Module.Asset.Module_Asset.instance.LoadPanel(asset) as GameObject;
//			SetConPos (tConObj, pos);
//
//	        //Create new text info to instatiate 
//			UText item = tConObj.GetComponent<UText>();
//			item.onAnimComplete += OnAnimComplete;
//
//	        item.color = color;
//	        item.pos = pos;
//	        item.text = text;
//	        item.size = size;
//			item.SetInfo (text);
//
//	        _textConList.Add(item);
//
//			return tConObj;
//	    }
//
//		public GameObject CreateText_Bubble(string text, Vector2 pos, float duration, string asset)
//		{
//			GameObject tConObj = LowoUN.Module.Asset.Module_Asset.instance.LoadPanel(asset) as GameObject;
//
//			//Create new text info to instatiate 
//			UText item = tConObj.GetComponent<UText>();
//			item.onAnimComplete += OnAnimComplete;
//
//			SetConPos (tConObj, pos);
//			item.SetInfo_Bubble (text);
//
//			_textConList.Add(item);
//
//			return tConObj;
//		}
//
//
//		public GameObject CreateText_SkillTip(Vector2 pos, bool isEnemy, string asset)
//		{
//			GameObject tConObj = LowoUN.Module.Asset.Module_Asset.instance.LoadPanel(asset) as GameObject;
//            if (tConObj != null)
//            {
//                UText item = tConObj.GetComponent<UText>();
//                item.SetStyle(isEnemy ? 0 : 1);
//                tConObj.GetComponent<RectTransform>().anchoredPosition = pos;
//            }
//			return tConObj;
//		}
//	}
//}