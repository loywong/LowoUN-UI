#pragma warning disable 0649//ignore default value null
using System;
using System.Collections.Generic;
using System.Linq;

using LowoUN.Business.UI;
using LowoUN.Module.Asset;
using LowoUN.Util;

using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI
{
	public enum Enum_UIAsset {
		None = -1,
		Son,//include: 1/item, 2/son item, 3/son general
		General,
		General2,
       	GeneralTop,
		CheckDlg,
		Reward,
		Test,
		Hud,//TEMP
		Notify,
	}

	[ExecuteInEditMode]
	public sealed class UIAsset : MonoBehaviour
	{
//		[SerializeField]
//		private bool isForceUILocalScale = true;

		[SerializeField]
		private GameObject _root_UI;
		[SerializeField]
		private GameObject _root_General;
        [SerializeField]
		private GameObject _root_General2;
		[SerializeField]
        private GameObject _root_GeneralTop;
		[SerializeField]
		private GameObject _root_Guide;
        [SerializeField]
		private GameObject _root_Cutscene;
		[SerializeField]
		private GameObject _root_CheckDlg;
		[SerializeField]
		private GameObject _root_Reward;
		[SerializeField]
		private GameObject _root_Test;
		[SerializeField]
		private GameObject _root_Notify;

		//for component common anim
		public AnimationClip mouseDownAnim;
		public float time_mouseDownAnim = 0.2f;
		public AnimationClip mouseUpAnim;
		public float time_mouseUpAnim = 0.1f;

		public Transform root{
			get{
				if(_root_UI == null){
					Debug.Assert(_root_UI == null);
					Debug.LogError("ui root must be exist!");
					return null;
				}

				return _root_UI.transform; 
			}
		}

		public Transform root_General{
			get{ return _root_General==null ? root :_root_General.transform;}
		}
		public Transform root_General2{
			get{ return _root_General2==null ? root :_root_General2.transform;}
		}
		public Transform root_GeneralTop{
			get{ return _root_GeneralTop==null ? root :_root_GeneralTop.transform;}
		}

		#region particularly roots
		public Transform root_Guide{
			get{ return _root_Guide==null ? root :_root_Guide.transform;}
		}
		public Transform root_Cutscene{
			get{ return _root_Cutscene==null ? root :_root_Cutscene.transform;}
		}
		#endregion

		public Transform root_CheckDlg{
			get{ return _root_CheckDlg==null ? root :_root_CheckDlg.transform;}
		}
		public Transform root_Reward{
			get{ return _root_Reward==null ? root :_root_Reward.transform;}
		}
		public Transform root_Test{
			get{ return _root_Test==null ? root :_root_Test.transform;}
		}
		public Transform root_Notify{
			get{ return _root_Notify==null ? root :_root_Notify.transform;}
		}

		[SerializeField]
		private string _setting_LayerPanels = "uilayerpanel";
		[SerializeField]
		private string _setting_SceneDefaultPanels = "uiscenedefault";
		//[SerializeField]
		//private string _txt_uiAtlas = "uiatlas";

		//public string txt_uiAtlas{get{ return _txt_uiAtlas;}}
		//public string txt_uiLayerPanel{get{ return _txt_uiLayerPanel;}}
		public string setting_sceneDefaultPanels{get{ return _setting_SceneDefaultPanels;}}

		private Dictionary<string, List<string>> uiAssetTypDict = new Dictionary<string, List<string>> ();
		private Dictionary<Enum_UIAsset, Transform> uiRootTypDict = new Dictionary<Enum_UIAsset, Transform> ();

        private static UIAsset _instance;
		public static UIAsset instance { 
			get {
				if (_instance == null) {
					Debug.LogError ("====== LowoUI-UN ===> no UIAsset found !");
				}
				return _instance;
			}
		}


		void Awake() {
			_instance = this;

			uiRootTypDict = new Dictionary<Enum_UIAsset, Transform> ()
			{
				{Enum_UIAsset.General, root_General},
				{Enum_UIAsset.General2, root_General2},
				{Enum_UIAsset.GeneralTop, root_GeneralTop},
				{Enum_UIAsset.CheckDlg, root_CheckDlg},
				{Enum_UIAsset.Reward, root_Reward},
				{Enum_UIAsset.Test, root_Test},
				{Enum_UIAsset.Notify, root_Notify},
			};
			uiAssetTypDict = GetClassifyPanels (Module_Asset.LoadText_Custom (_setting_LayerPanels));
		}

		public Dictionary<string/*enum name*/, List<string>> GetSceneDefault () {
			return GetClassifyPanels (Module_Asset.LoadText_Custom (setting_sceneDefaultPanels));
		}

		private Dictionary<string/*enum name*/, List<string>> GetClassifyPanels (string[] strs) {
			var dict = new Dictionary<string, List<string>>();
			string key = string.Empty;
			//Enum_UIAsset keyEnum = Enum_UIAsset.None;

			foreach (var item in strs) {
				if (!string.IsNullOrEmpty (key)) {
					if(!item.Contains("["))
						dict [key].Add (item);
				}

				if (item.Contains ("[")) {
					key = item.Replace ("[", string.Empty).Replace ("]", string.Empty);
					//keyEnum = (Enum_UIAsset)Enum.Parse (typeof(Enum_UIAsset), "UI_"+key);
					dict[key] = new List<string>();
				}

				//UnityEngine.Debug.LogError (item + "\n");
			}

			return dict;
		}

		public Sprite LoadSprite (string imgName) {
			return Module_Asset.instance.LoadSprite (imgName);
		}

		/// <summary>
		/// 以WWW方式进行加载 Image from network or local storage
		/// </summary>
		public void LoadWebImg(Image img, string url) {
			StartCoroutine(LoadNetImg(img, url));
		}

		private System.Collections.IEnumerator LoadNetImg(Image img, string url) {
			double startTime = (double)Time.time;
			//请求WWW url
			WWW www = new WWW(url);//"file://D:\\test.jpg"
			yield return www;        
			if(www != null && string.IsNullOrEmpty(www.error)) {
				//获取Texture
				Texture2D texture=www.texture;

				//创建Sprite
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
				img.sprite = sprite;

				startTime = (double)Time.time - startTime;
				Debug.Log("====== LowoUI-UN ===> WWW加载图片用时:" + startTime);

				//清理游离资源
				yield return new WaitForSeconds(0.01f);
				Resources.UnloadUnusedAssets(); 
			}
		}

		public GameObject LoadPanelByType (int uiPanelTypeID, Enum_UIAsset assetType, string name = null) {// = Enum_AssetType.UI_General
			//string uiName = string.Empty;
			if(string.IsNullOrEmpty(name))
				name = UILinker.instance.GetPrefabName (uiPanelTypeID);
			//uiName = name
			return LoadPanelByName(name, assetType);
		}

		public GameObject LoadPanelByName(string uiName, Enum_UIAsset uiTyp)// = Enum_AssetType.UI_General
		{
			GameObject go = null;

			if (!string.IsNullOrEmpty (uiName)) {
				go = Module_Asset.instance.LoadPrefab (uiName, Module_Asset.Enum_PrefabTyp.UI);

				Transform t = go.transform;
				SetRoot (uiTyp, t);

				//Re Init
                t.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, 0, 0);
				//if(isForceUILocalScale)
				t.localScale = Vector3.one;
			}

			return go;
		}

		private void SetRoot (Enum_UIAsset uiTyp, Transform t) {
			bool hasSet = false;

			Transform r = uiRootTypDict.Val (uiTyp);
			if (r != null) {
				t.SetParent (r);
				hasSet = true;
			}

			if (hasSet) 
				t.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
			else
				t.SetParent (root);
		}

		public Enum_UIAsset GetUIRootTyp(int typ) {
			string enumName = uiAssetTypDict.Where (i=>i.Value.Contains(System.Enum.GetName(typeof(UIPanelType), typ))).Select(i=>i.Key).FirstOrDefault();
			if(string.IsNullOrEmpty(enumName))
				return Enum_UIAsset.General;

			return (Enum_UIAsset)EnumParse.GetEnumID (enumName,typeof(Enum_UIAsset));
		}

		public void SetDialogBgRoot (UIPanelType typ, Transform bg) {
			SetRoot (UIAsset.instance.GetUIRootTyp ((int)typ), bg);
		}

//		private int GetEnumID (string name) {
//			foreach (int intValue in System.Enum.GetValues(typeof(Enum_UIAsset))) {
//				if(name == System.Enum.GetName(typeof(Enum_UIAsset), intValue))
//					return intValue;
//			}
//
//			return -2147483648;//int range(-2147483648～+2147483647)
//		}
	}
}