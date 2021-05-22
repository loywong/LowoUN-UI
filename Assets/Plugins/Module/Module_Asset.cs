#pragma warning disable 0649//ignore default value null
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//using LowoUN.Module.Asset.Patch;
using LowoUN.Util;

using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LowoUN.Module.Asset
{
	public sealed class Module_Asset : MonoBehaviour
	{
		public enum Enum_LoadMode {
			DirectReference,
			ResourceLoad,
			PersistentDataPath,//WWWAsync
		}

		public enum Enum_PrefabTyp {
			None,
			Model,
			UI,
			//Sprite,
			//Txt,
			//Texture,
		}

		private Dictionary<string/*asset name*/, object> prefabRefCacheDict = new Dictionary<string, object> ();
		private Dictionary<string/*sprite name*/, object> spriteRefCacheDict = new Dictionary<string, object> ();

		private static Module_Asset _instance;
		public static Module_Asset instance { 
			get {
				if (_instance == null) 
					Debug.LogError ("====== LowoUN / Moduel / Asset ===> No Asset Module  found !");
				
				return _instance;
			}
		}

		//[SerializeField]private GameObject rootUI;
		[SerializeField]private GameObject rootUW;
		[SerializeField]private Enum_LoadMode assetLoadMode = Enum_LoadMode.ResourceLoad;
//		[SerializeField]private bool isUsePatchServer = false;
		[SerializeField]private bool isUseAtlas = true;
		[SerializeField]private string _txt_atlas = "atlas";

		//_isForceLayer = false;
		//_isRefCount = false;

//		public bool CheckPatch (System.Action callback) {
//			if (isUsePatchServer) {
//				PatchMgr.AddPatchDoneDelegate(callback);
//				PatchMgr.ins.Sync ();
//			}
//
//			return isUsePatchServer;
//		}

		void Awake(){
			_instance = this;
		}

		private void Start() {
//			if (isUsePatchServer)
//				Patch.PatchMgr.ins.Init ();
		}

		public static string LoadText(string fileName) {
			string s = string.Empty;

			TextAsset item = (TextAsset)Resources.Load (fileName, typeof(TextAsset));
			if (item != null) {
				s = item.text;
				Resources.UnloadAsset (item);
			}
			else {
				//LowoUN.Util.LogMgr.Log_Error ("Failed to load file: " + name);
				#if UNITY_EDITOR
				UnityEngine.Debug.LogError ("Failed to load txt file: " + fileName);
				#endif
			}

			return s;
		}

		public static string[] LoadText_Custom(string fileName) {
			int hash = 0;
			return LoadText_Custom (fileName, ref hash);
		}
		public static string[] LoadText_Custom(string fileName, ref int hashcode) {
			string[] itemRowsList = null;

			TextAsset item = (TextAsset)Resources.Load (fileName, typeof(TextAsset));
			if (item != null) {
				byte[] buf = Encoding.UTF8.GetBytes (item.text);
				hashcode = CRC32.GetCRC32 (buf);
				itemRowsList = item.text.Split (new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
				Resources.UnloadAsset (item);
			} else {
				#if UNITY_EDITOR
				UnityEngine.Debug.LogError ("Failed to load txt file: " + fileName);
				#endif
			}

	        return itemRowsList;
	    }

		//public static void LoadTexture(MeshRenderer mr, string fileName)
		public Texture LoadTexture(string fileName) {
			return (Texture)Resources.Load(fileName);
	    }

		public GameObject LoadModel (string fileName) {
			return LoadPrefab (fileName, Enum_PrefabTyp.Model);
		}

		public T LoadModel<T> (string fileName) where T : Component {
			GameObject go = LoadPrefab (fileName, Enum_PrefabTyp.Model);
			if (go.GetComponent<T> () == null)
				go.AddComponent<T> ();
			return go.GetComponent<T>();
		}

		//---------------------------------------------------------
		//load sprite in asset mgr
		//item.obj.GetComponent<Image>().sprite = Resources.Load("UITest/" + uiTextureName, typeof(Sprite))as Sprite;//overrideSprite
		//Sprite s = Sprite.Create(AssetMgr.Instance.m_UISourceImage, new Rect(0, 0, 56/*img.width*/, 56/*img.height*/), Vector2.zero) as Sprite;
		public Sprite LoadSprite(string imgName) {
			Sprite sprite = null;

			if (assetLoadMode == Enum_LoadMode.DirectReference) {
//				List<Sprite> uiSprites = DirectRefCollector.instance.uiSprites;
//				if (uiSprites != null && uiSprites.Count > 0) {
//					foreach (Sprite item in uiSprites) {
//						if (item.name.Equals (imgName, StringComparison.OrdinalIgnoreCase)) {
//							sprite = item;
//						}
//					}
//				}
			}
			else if (assetLoadMode == Enum_LoadMode.ResourceLoad) {
				if (isUseAtlas) {
					//1, use atlas
					sprite = GetSpriteFromAllAtlas(imgName);
				} 

				if (sprite == null) {
					//2, not use atlas
					//sprite = Resources.Load(imgName) as Sprite;
					sprite = Resources.Load<Sprite>(imgName);
				}

				if (sprite == null)
					sprite = Resources.Load<Sprite>("UI_Dummy");
			}
			else if (assetLoadMode == Enum_LoadMode.PersistentDataPath) {
				Debug.LogError ("sprite Assets load mode WWW has not been implemented!");
			}

			return sprite;
		}

		private Dictionary<string, Object[]> allAtlasDict = new Dictionary<string, Object[]>();
		private Sprite GetSpriteFromAllAtlas (string imgName) {
			Sprite sp = null;

			if(allAtlasDict.Count == 0){
				allAtlasDict = new Dictionary<string, Object[]> ();
				CacheAllAtlas ();
				CacheAllSprites ();
			}

			if (spriteRefCacheDict.ContainsKey (imgName)) 
				return spriteRefCacheDict[imgName] as Sprite;
			else
				Debug.LogWarning ("====== LowoUN / Module / Asset ===> Sprite: "+imgName+", hasn't found in all atlases"); 

			return sp;
		}

		private string[] GetAllAtlasPath () {
			return LoadText_Custom (_txt_atlas);
		}

		private void CacheAllAtlas () {
			foreach (var item in GetAllAtlasPath()) {
				Object[] _atlas = Resources.LoadAll (item);  
				allAtlasDict.Add (item, _atlas);  
			}
		}

		private void CacheAllSprites () {
			foreach (var item in allAtlasDict) { 
				Object[] atlas = item.Value;
				for (int i = 0; i < atlas.Length; i++) {  
					if (atlas [i].GetType () == typeof(UnityEngine.Sprite)) {  
						spriteRefCacheDict[atlas [i].name] = atlas [i] as Sprite;//(Sprite)
					}  
				} 
			}
		}

		public GameObject GetPrefabRef(string prefabName, Enum_PrefabTyp type) {
			GameObject prefabRef = null;

			if (prefabRefCacheDict.ContainsKey (prefabName))
				prefabRef = prefabRefCacheDict [prefabName] as GameObject;
			else {
				if (assetLoadMode == Enum_LoadMode.DirectReference) {
//					prefabRef = GetPrefab_DirectRef (prefabName, type);
				}
				else if (assetLoadMode == Enum_LoadMode.ResourceLoad) {
					//LowoUN.Util.Pool.ObjectsPool.GetPooledObject (prefabName);
					prefabRef = Resources.Load(prefabName) as GameObject;
				}
				else if (assetLoadMode == Enum_LoadMode.PersistentDataPath) {
					#if UNITY_EDITOR
					Debug.LogError ("====== LowoUN-UI ===> Assets load mode WWW has not been implemented!");
					#endif
				}

				prefabRefCacheDict[prefabName] = prefabRef;
			}

			return prefabRef;
		}

		private GameObject GetGameObj(string prefabName, Enum_PrefabTyp type) {
			GameObject gameObj = null;

			GameObject prefabRef = GetPrefabRef (prefabName, type);
			if (prefabRef == null) {
				#if UNITY_EDITOR
				Debug.LogError ("====== LowoUN-UI ===> Asset has not been find!" + prefabName);
				#endif
			} 
			else {
				gameObj = GameObject.Instantiate (prefabRef) as GameObject;
			}

			return gameObj;
		}

		public GameObject LoadPrefab(string prefabName, Enum_PrefabTyp type) {
			GameObject gameObj = GetGameObj (prefabName, type);

			if (gameObj != null) {
				Transform t = gameObj.transform;
				if (type == Enum_PrefabTyp.Model) {
					t.SetParent (rootUW.transform);
					t.GetComponent<Transform> ().localRotation = Quaternion.identity;
					t.localScale = Vector3.one;
				}

				//if (_pData.instance) {
				//	genSeq.Add (_name);
				//	_pData.instance.name = _name;
				//	_pData.obj = _pData.instance.GetComponent<ObjType> ();
				//	_pData.rc++;
				//
				//	pool [_name] = _pData;

				//	//LogManager.Log_Error("Gen obj: " + p.prefab.name);
				//	//LogManager.Log_Error("Cur size: " + poolSize);
				//}
			}
			else {
				#if UNITY_EDITOR
				Debug.LogWarning ("====== LowoUN-UI ===> Instantiate failed with : " + prefabName);
				#endif
			}

			return gameObj;
		}

		//It's better to init a dict for ui prefabs' reference
//		private GameObject GetPrefab_DirectRef(string prefabName, Enum_PrefabTyp type){
//			GameObject prefabRef = null;
//
//			if (type == Enum_PrefabTyp.UI) {
//				for (int i = 0; i < DirectRefCollector.instance.uiPanels.Count; ++i) {
//					if (prefabName == DirectRefCollector.instance.uiPanels [i].name) {
//						prefabRef = DirectRefCollector.instance.uiPanels [i];
//					}
//				}
//			}
//			else if (type == Enum_PrefabTyp.Model) {
//				for (int i = 0; i < DirectRefCollector.instance.uwModels.Length; ++i) {
//					if (prefabName == DirectRefCollector.instance.uwModels [i].name) {
//						prefabRef = DirectRefCollector.instance.uwModels [i];
//					}
//				}
//			}
//
//			return prefabRef;
//		}

		void OnDestroy () {
			prefabRefCacheDict.Clear ();
			allAtlasDict.Clear ();
			spriteRefCacheDict.Clear ();
		}


		///////////////////////////////////////////////////////////////////////////////////
		/// save and load local persistentDataPath config texts
		private void SaveConfig (string content, string path) {
			if (string.IsNullOrEmpty (content) || string.IsNullOrEmpty (path))
				return;

			StreamWriter st = File.CreateText (Application.persistentDataPath + path/*"/uiAtlasPath.ini"*/);
			st.Write (content);
			st.Close ();
		}
		private string GetConfig (string path) {
			if (string.IsNullOrEmpty (path)) {
				Debug.LogError (Util.Log.Format.Module("Asset") + "error path!");
				return string.Empty;
			}

			return File.ReadAllText (Application.persistentDataPath + path);
		}
	}
}