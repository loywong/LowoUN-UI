#pragma warning disable 0649//ignore default value null
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LowoUN.Business.UI;
using LowoUN.Util;
using UnityEngine;
using UnityEngine.UI;
using LowoUN.Module.UI.Com;

namespace LowoUN.Module.UI 
{
	public class UIHolder : MonoBehaviour 
	{
		public System.Action<UIHolder, UIStateType>      onStateAnimComplete;
		public System.Action<UIPanelType, int, object[]> onInvokeHandler;

		#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID;
		#endif
		public UIPanelType typeID  = UIPanelType.None;
		public string      typeIdx = UIPanelType.None.ToString();
		public int         insID   = -1;

		public int   parentHolderInsID = -1;
		public int   hostLstObjid = -1;
		public int   curIdxInList = -1;
		public bool  isSonItem{ set; private get;}
		private bool isLstItem { get { return curIdxInList != -1; } }
		public bool  isSonGeneral { get { return parentHolderInsID != -1; } }
		public bool  isEmbed { get { return isLstItem || isSonItem || isSonGeneral; } }

		public int  dialogBg = -1;
		public bool isLogicShow = true;

		//shenmo add for optimize
		RectTransform _rectTransform = null;
		public RectTransform rectTransform {
			get {
				if (_rectTransform == null)
					_rectTransform = gameObject.GetComponent<RectTransform>();
				
				return _rectTransform;
			}
		}

		[SerializeField]
		private List<UIObject> objectList;
		[SerializeField]
		private List<UIEvent> eventList;

		protected Dictionary<int, GameObject> objDict;
		protected bool started = false;

		void Awake () {
			stateAnimator = GetComponent<UIStateAnimator> ();
			
			insID = UIHolder.GetUIInstanceID();
			InitObjLst ();
			InitEvtLst();

			UILinker.instance.AwakeUIHolder (this);
		}

		void Start () {
			if(!started) {
				started = true;
				UILinker.instance.StartUIHolder(this);
			}
		}

		public void OnReset () {
			//reset holder self
			//Destruct///////////////////////////////////////////////
			//onStateAnimComplete = null;
			//onInvokeHandler = null;

//			#if UNITY_EDITOR
//			classID = UIPanelClass.None;
//			#endif
//			typeID = UIPanelType.None;
//			typeIdx = string.Empty;

			insID = -1;
			isLogicShow = true;
			dialogBg = -1;
			parentHolderInsID = -1;
			hostLstObjid = -1;
			curIdxInList = -1;
			isSonItem = false;
			_rectTransform = null;
			objectList.Clear ();
			eventList.Clear ();
			objDict.Clear();
			started = false;

			UILinker.instance.ResetUIHolder (this);
		}

		public void SetInScreen(bool b) {
			bool bActive = b;
			if (bActive) 
				bActive = isLogicShow;

			if(gameObject.activeSelf != bActive)
				gameObject.SetActive (bActive);
		}

		private static int _instanceID = 0;
		//(int (min)-2147438648 ~ (max)+2147438647)(uint (min)0 ~ (max)+4294967295)
		private static readonly int _maxHolderInsID = 2147438647;
		private static int GetUIInstanceID () {
			//RecycleInsid ();
			_instanceID =  (_instanceID % _maxHolderInsID) + 1;
			if (_instanceID == _maxHolderInsID) {
				Debug.LogWarning ("====== Lowo ===> THE DACKNESS AGE IS COMING......\n " +
					"BUT IT SHOULD BE NEVER HAPPEN! \n " +
					"(FRIENDLY ATTENTION: FORCE RELOAD THE GAME)");
				Application.Quit ();
			}
			return _instanceID;
			//return _instanceID ++ % _maxHolderInsID;
		}

		public static void RecycleInsid () {
			//new List<int> (UILinker.instance.uiHolderDict.Keys).ForEach (i=>Debug.Log("curid: " + i + "\n"));
			_instanceID = UILinker.instance.uiHolderDict.Select (i => i.Key).OrderByDescending (i => i).FirstOrDefault ();
			//Debug.LogError ("_instanceID: " + _instanceID);
		}

		void Update () {
			if (_binder != null) 
				_binder.OnUpdate ();
		}

		private UIBinder _binder;
		public void SetBinder (UIBinder binder) {
			if (binder != null)
				_binder = binder;
			else {
				#if UNITY_EDITOR
				Debug.LogWarning("====== LowoUI-UN ===> no binder instance created for current holder type: " + typeID.ToString());
				#endif
			}
		}

        public UIBinder GetBinder() {
            return _binder;
        }

        public T GetBinder<T>() where T : UIBinder {
            return _binder as T;
        }

		private void EndBinder () {
			if (_binder != null) {
				_binder.OnEndBinder ();
				_binder = null;
			}
		}

	    private void InitObjLst() {
	        objDict = new Dictionary<int, GameObject>();

	        foreach (UIObject obj in objectList) {
	            int id = (int)obj.id;

				UIActionBase uac = null;
				if (obj.obj != null && (uac = obj.obj.GetComponent<UIActionBase> ()) != null) {
					uac.hostHolderInsID = insID;
				}
					
				if (objDict.ContainsKey (id))
					Debug.LogError ("====== LowoUI-UN ===> holder object ID has existed: " + id);
				else
	                objDict.Add(id, obj.obj);
	        }
	    }

		private void InitEvtLst () {
            foreach (UIEvent item in eventList) {
                UIActionBase uac = null;
                if (item.obj != null && (uac = item.obj.GetComponent<UIActionBase>()) != null) {
					uac.curEventID = item._eventID;
					uac.hostHolderInsID = insID;
					uac.onCallEvent = CallEvent;

//                    Dictionary<int, MethodInfo> info;
//					if(UILinker.instance.CacheEvent4AllHolder.TryGetValue(typeID, out info)) {
//                        if(info!= null && info.ContainsKey(item._eventID)) {
//                            uac.curEventID = item._eventID;
//                            uac.hostHolderInsID = insID;
//                            uac.onCallEvent = CallEvent;
//                        }
//                    }
//                    else {
//                        //Debug.LogWarning("Don't forget to add game object to the serilized variable! / typeID : " + typeID);
//                    }
                }
                else {
                    //Debug.LogWarning("Don't forget to add game object to the serilized variable! / typeID : " + typeID);
                }
            }
		}

		public GameObject GetObj (int uiObjectID) {
			GameObject obj = null;
			SafeObjSet(uiObjectID, (GameObject go) => {
				obj = go;
			});

			return obj;
		}

		public GameObject GetEvt (int uiEventID) {
			GameObject obj = null;
			SafeEvtSet(uiEventID, (GameObject go) => {
				obj = go;
			});

			return obj;
		}

		public void SetElementPos (int uiObjectID, Vector2 pos) {
			SafeObjSet(uiObjectID, (GameObject go) => {
                go.GetComponent<RectTransform>().anchoredPosition = pos;// * UIAdaptScreen.instance.GetScaleValue();
			});
		}

		public void SetElementSize (int uiObjectID, Vector2 size) {
			SafeObjSet(uiObjectID, (GameObject go) => {
				go.GetComponent<RectTransform>().sizeDelta = size;
			});
		}

		private void PlayAnimation (UIStateType animType) {
			if(this == null || gameObject == null || !gameObject.activeSelf)
				CompleteAnim ();
			
			bool isAnimPlayed = false;
			//CheckStateAnimator ();

			if (stateAnimator != null)// && animsManager.isActiveAndEnabled
				isAnimPlayed = stateAnimator.Play (animType, CompleteAnim);

            if (!isAnimPlayed) 
				CompleteAnim ();
		}

		private UIStateAnimator stateAnimator;
		public void OnOpen() {
			gameObject.SetActive (true);

			animType = UIStateType.Open;
			PlayAnimation (animType);
//			StartCoroutine (PlayOpenAnim());
		}
//		IEnumerator PlayOpenAnim() {
//			yield return null;//等待一帧/下一帧执行
//			animType = UIStateType.Open;
//			PlayAnimation (animType);
//		}

		public void OnClose() {
			if (animType != UIStateType.Close) {
				//THINKING On Close UI holder
				//1, unload lists' items
				//2, unload son item panels
				// top thing handled in the binder

				EndBinder ();
				//OnReset ();

				animType = UIStateType.Close;
				PlayAnimation (animType);
			}
		}

		public void ToggleDialogBg (bool isShow) {
			if (dialogBg != -1) {
				if(isShow)
					UIHub.instance.ShowPanel (dialogBg);
				else
					UIHub.instance.HidePanel (dialogBg);
			}
		}

		public void OnEnd () {
			if(dialogBg != -1)
				UIHub.instance.CloseUI (dialogBg);

			//if (UILinker.instance.CheckPanelIsDialog ((int)typeID)) 
			//	UIHub.instance.RemoveHolderDialogBg ();

            DestroyWebView();
            //UIAsset.instance.UnloadPanel (this);
            if (this != null && gameObject != null)
				Destroy (gameObject);
			
			OnReset ();
		}

		//for list
		public delegate void Call4ListEvent (int tempUIEventID, params object[] arr);
		public Call4ListEvent onCallEvent4List;

		object[] p;
		private void CallEvent(int tempUIEventID, params object[] arr) {
			//for list
			if (curIdxInList != -1)
				onCallEvent4List (curIdxInList, arr);

			p = null;
			if (arr != null && arr.Length > 0) {
				if (curIdxInList != -1) {
					object[] parameters = new object[arr.Length + 1];

					for (int i = 0; i < arr.Length; i++) {
						parameters [i] = arr [i];
						//Debug.LogError (" arr [i] : " + arr [i]);
					}
					parameters [arr.Length] = curIdxInList;
					//Debug.LogError ("currIdxInList : " + currIdxInList);

					p = new object[]{ parameters };
				} else {
					p = new object[]{arr};
				}
			}

			if(onInvokeHandler != null)
				onInvokeHandler (typeID, tempUIEventID, p);
		}
	    
	    protected delegate void SafeSetCallback(GameObject go);
	    protected bool SafeObjSet(int uiObjectID, SafeSetCallback callback) {
	        GameObject obj = null;
	        if (objDict.TryGetValue(uiObjectID, out obj)) {
	            if (obj != null) {
	                callback(obj);
	                return true;
	            }
	        }

			//Debug.LogWarning("====== LowoUI-UN ===> no game object set to the serialized variable!");
	        return false;
	    }

		protected bool SafeEvtSet(int uiEventID, SafeSetCallback callback) {
			GameObject obj = null;
			if (eventList.Count > uiEventID) {
				obj = eventList[uiEventID].obj;

				if (obj != null) {
					callback(obj);
					return true;
				}
			}
			
			return false;
		}

		//find ui object per call is waste too much, we can build relationship in one timezuod
	    public bool SetText(int uiObjectID, string textValue) {
	        return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<Text>() != null) 
					go.GetComponent<Text>().text = textValue;
				else
					Debug.LogWarning(string.Format("====== LowoUI-UN ===>[func: SetText] holder type:{0} / objID:{1} / val:{2}, no a text type component!!!", typeID.ToString(), uiObjectID, textValue));
	        });
		}

        public bool SetParent(GameObject uiObject, int uiParentObjectID) {
            return SafeObjSet(uiParentObjectID, (GameObject go) => {
				if (uiObject != null)
               	 	uiObject.transform.parent = go.transform;
            });
        }

        public bool SetTextAlign(int uiObjectID, TextAnchor align) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<Text>() != null) 
					go.GetComponent<Text>().alignment = align;
				else 
					Debug.LogWarning(string.Format("====== LowoUI-UN ===>[func: SetTextAlign] holder type:{0} / objID:{1} / val:{2}, no a text type component!!!", typeID.ToString(), uiObjectID, align.ToString()));
			});
		}

		public bool SetImg(int uiObjectID, string uiTextureName, bool isWeb) {
	        return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<Image>() != null) {
					if (string.IsNullOrEmpty (uiTextureName)) {
						go.SetActive(false);
						
						//#if UNITY_EDITOR
						//Debug.LogWarning("====== LowoUI-UN ===> mistake image name: empty or null/ holder:  " + insID + "/ objID: " + uiObjectID);
						//#endif
					}
					else {
						if(isWeb){
							UIAsset.instance.LoadWebImg(go.GetComponent<Image>(), uiTextureName);
						}
						else {
							Sprite s = UIAsset.instance.LoadSprite(uiTextureName);
							
							if (s != null) {
								go.SetActive(true);
								go.GetComponent<Image>().sprite = s;
							} else{
								Sprite ss = UIAsset.instance.LoadSprite("Icon_Dummy");
								if(ss != null)
									go.GetComponent<Image>().sprite = ss;
								else
									go.SetActive(false);
							}
						}
					}
				}
				else {
					//Debug.LogError("====== LowoUI-UN ===> No Image component found: " + insID + "/ objID: " + uiObjectID);
					Debug.LogWarning(string.Format("====== LowoUI-UN ===>[func: SetImg] holder type:{0} / objID:{1} / val:{2}, No a image type component!!!", typeID.ToString(), uiObjectID, isWeb.ToString()));
				}
	        });
		}

		private WebViewObject webViewObject;
		public GameObject SetWebView (int uiObjectID, string Url) {//, int charaID = -1
			//return SafeSet(uiObjectID, (GameObject go) => {
			SafeObjSet(uiObjectID, (GameObject go) => {
                RectTransform rt = go.GetComponent<RectTransform>();
                Pair<Vector2, Vector2> margins = new Pair<Vector2, Vector2>(Vector2.zero, Vector2.zero);
                float ToEdgetLeft = 0;
                float ToEdgetRight = 0;
                float ToEdgetTop = 0;
                float ToEdgetBottom = 0;
                Vector3 WorldPosition = new Vector3();
                Vector3 LocalPosition = new Vector3();
                float webHeight = 0;
                float webWidth = 0;
                LocalPosition =  rt.localPosition;
				WorldPosition = transform.InverseTransformPoint(LocalPosition) / UIAdaptScreen.instance.GetFixRate4TransformUICamera ();
				WorldPosition.x = (WorldPosition.x * UIHub.instance.ScreenWidthUsedByWeb / 720);
				WorldPosition.y = (WorldPosition.y * UIHub.instance.ScreenWidthUsedByWeb / 720);
                webWidth = rt.rect.width * rt.parent.localScale.x;
                webHeight = rt.rect.height * rt.parent.localScale.y;
				webWidth = webWidth * UIHub.instance.ScreenWidthUsedByWeb / 720;
				webHeight = webHeight * UIHub.instance.ScreenWidthUsedByWeb / 720;
				ToEdgetLeft = (UIHub.instance.ScreenWidthUsedByWeb / 2 - (webWidth/2-WorldPosition.x)) ;
				ToEdgetRight = (UIHub.instance.ScreenWidthUsedByWeb / 2- (webWidth/2+WorldPosition.x));
				ToEdgetTop = (UIHub.instance.ScreenHeightUsedByWeb / 2 - (webHeight / 2 +WorldPosition.y));
				ToEdgetBottom = (UIHub.instance.ScreenHeightUsedByWeb / 2 - (webHeight / 2 - WorldPosition.y));
                margins.first = new Vector2(ToEdgetLeft, ToEdgetRight);// U [left, right]
                margins.second = new Vector2(ToEdgetTop, ToEdgetBottom);// V [top, bottom]
				StartCoroutine(SetWebViewInfo(Url, margins));//charaID, 
            });

			//TODO: set webViewObject's parent
			//webViewObject.transform.SetLayer(UIAsset.instance.root_General2.gameObject.layer);
			webViewObject.gameObject.layer = (UIAsset.instance.root_General2.gameObject.layer);
			webViewObject.transform.SetParent (UIAsset.instance.root_General2);
			webViewObject.transform.localScale = Vector3.one;
			return webViewObject.gameObject;
        }

        public void DestroyWebView() {
            if (webViewObject != null)
                Destroy(webViewObject.gameObject);
        }

        //public Vector2 scrollPosition = Vector2.zero;
        //public int button_line = 0;
        //public int button_line1 = 0;
        //void OnGUI()
        //{
        //    return;
        //    if (gameObject.name == "UI_WebView(Clone)")
        //    {
        //        scrollPosition = GUI.BeginScrollView(new Rect(10, 10, 300, 400), scrollPosition, new Rect(0, 0, 300, 800));
        //        button_line = 0;

        //        GUI.Label(new Rect(10, (button_line++) * 40, 300, 40), margins.first.x.ToString() + " " + margins.second.x.ToString() + " " + margins.first.y.ToString() + " " + margins.second.y.ToString());
        //        GUI.Label(new Rect(10, (button_line++) * 40, 300, 40), UIAdaptScreen.instance.GetScaleRate().ToString());
        //        GUI.Label(new Rect(10, (button_line++) * 40, 300, 40), UIHub.instance.ScreenWidthUsedByWeb.ToString() + " " + UIHub.instance.ScreenHeightUsedByWeb.ToString());
        //        GUI.Label(new Rect(10, (button_line++) * 40, 300, 40), GameSystem.GameManager.ScreenWidthUsedByWeb.ToString() + " " + GameSystem.GameManager.ScreenHeightUsedByWeb.ToString());
        //        GUI.Label(new Rect(10, (button_line++) * 40, 300, 40), WorldPosition.x.ToString() + " " + WorldPosition.y.ToString());
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins  0"))
        //        {
        //            webViewObject.SetMargins(0, 0, 0,0);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 "))
        //        {
        //            webViewObject.SetMargins((int)360, (int)margins.second.x, (int)360, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 480 "))
        //        {
        //            webViewObject.SetMargins((int)margins.first.x, (int)480, (int)margins.first.y, (int)480);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360  1"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 2 "))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 3 "))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 4"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 5"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 6"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360  7"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 8"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 9 "))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 10"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 11"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 12"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 13"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 14"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 15"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 16"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        if (GUI.Button(new Rect(10, (button_line++) * 40, 300, 40), "set margins 360 17"))
        //        {
        //            webViewObject.SetMargins((int)360 + button_line1++ * 10, (int)margins.second.x, (int)360 + button_line1 * 10, (int)margins.second.y);
        //        }
        //        GUI.EndScrollView();
        //    }
        //}

		private System.Collections.IEnumerator SetWebViewInfo (string Url, Pair<Vector2/*U*/, Vector2/*V*/> margins = null)//int charaID = -1, 
		{
			webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
			webViewObject.Init(
				cb: (msg) =>
				{
					Debug.Log(string.Format("CallFromJS[{0}]", msg));
					//status.text = msg;
					//status.GetComponent<Animation>().Play();
				},
				err: (msg) =>
				{
					Debug.Log(string.Format("CallOnError[{0}]", msg));
					//status.text = msg;
					//status.GetComponent<Animation>().Play();
				},
				ld: (msg) =>
				{
					Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
				},
				enableWKWebView: true);

			//(int left, int top, int right, int bottom)
			if(margins != null) webViewObject.SetMargins((int)margins.first.x, (int)margins.second.x, (int)margins.first.y, (int)margins.second.y);
			else                webViewObject.SetMargins(5, 200, 5, Screen.height / 4);
			webViewObject.SetVisibility(true);

			#if !UNITY_WEBPLAYER
			if (Url.StartsWith("http")) {
				webViewObject.LoadURL(Url.Replace(" ", "%20"));
			} else {
				var exts = new string[]{
					".jpg",
					".html"  // should be last
				};
				foreach (var ext in exts) {
					var url = Url.Replace(".html", ext);
					var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
					var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
					byte[] result = null;
					if (src.Contains("://")) {  // for Android
						var www = new WWW(src);
						yield return www;
						result = www.bytes;
					} else {
						result = System.IO.File.ReadAllBytes(src);
					}
					System.IO.File.WriteAllBytes(dst, result);
					if (ext == ".html") {
						webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
						break;
					}
				}
			}
			#if !UNITY_ANDROID
			webViewObject.EvaluateJS(
			"window.addEventListener('load', function() {" +
			"   window.Unity = {" +
			"       call:function(msg) {" +
			"           var iframe = document.createElement('IFRAME');" +
			"           iframe.setAttribute('src', 'unity:' + msg);" +
			"           document.documentElement.appendChild(iframe);" +
			"           iframe.parentNode.removeChild(iframe);" +
			"           iframe = null;" +
			"       }" +
			"   }" +
			"}, false);");
			#endif
			#else
			if (Url.StartsWith("http")) {
			webViewObject.LoadURL(Url.Replace(" ", "%20"));
			} else {
			webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
			}
			webViewObject.EvaluateJS(
			"parent.$(function() {" +
			"   window.Unity = {" +
			"       call:function(msg) {" +
			"           parent.unityWebView.sendMessage('WebViewObject', msg)" +
			"       }" +
			"   };" +
			"});");
			#endif
			yield break;

			//!!!???
			//if (charaID != -1)
			//	webViewObject.CallFromJS (charaID.ToString());
		}

        public bool SetColor(int uiObjectID, Color uiColor) {
            return SafeObjSet(uiObjectID, (GameObject go) => 
				{
                    Graphic g = go.GetComponent<Graphic>();
                    if (g == null)
						Debug.LogWarning("====== LowoUI-UN ===> No graphic ui component attached!");
                    else
                    {
                        g.color = uiColor;
                    }
                });
        }

		public GameObject SetRenderTexture_New(int uiObjectID, object data, string assetName) {
			GameObject chara3D = null;

			SafeObjSet(uiObjectID, (GameObject go) => {
				UIRImg r = go.GetComponent<UIRImg>();

				if(r != null){
					chara3D = r.SetInfo(data, assetName);
				}
				else {
					if(go.GetComponent<RawImage>() == null)
						Debug.LogWarning("====== LowoUI-UN ===> No RawImage ui component attached on panel: " + uiObjectID +" / obj id: "+ uiObjectID);
					else 
					{
						RawImage img = go.GetComponent<RawImage>();
						RenderTexture m_Texture = CreateTexture(img.GetComponent<RectTransform>().sizeDelta);
						img.texture = m_Texture;

						chara3D = LowoUN.Module.Asset.Module_Asset.instance.LoadModel (assetName);
						//CharacterStage chara = chara3D.GetComponent<CharacterStage>();
						//if(chara != null)
						//	chara.CreateModel (data as Character, m_Texture);
						IUI3DModel chara = chara3D.GetComponent<IUI3DModel>();
						if(chara != null)
							chara.CreateModel (data, m_Texture);
					}
				}
			});

			return chara3D;
		}
		public GameObject SetRenderTexture_New(int uiObjectID, int typid, string assetName){
			GameObject chara3D = null;

			SafeObjSet(uiObjectID, (GameObject go) => {
				UIRImg r = go.GetComponent<UIRImg>();

				if(r != null){
					chara3D = r.SetInfo(typid, assetName);
				}
				else {
					if(go.GetComponent<RawImage>() == null)
						Debug.LogWarning("====== LowoUI-UN ===> No RawImage ui component attached on panel: " + uiObjectID +" / obj id: "+ uiObjectID);
					else
					{
						RawImage img = go.GetComponent<RawImage>();
						RenderTexture m_Texture = CreateTexture(img.GetComponent<RectTransform>().sizeDelta);
						img.texture = m_Texture;

						chara3D = LowoUN.Module.Asset.Module_Asset.instance.LoadModel (assetName);
						//CharacterStage chara = chara3D.GetComponent<CharacterStage>();
						//if(chara != null)
						//	chara.CreateModel (typid, m_Texture);
						IUI3DModel chara = chara3D.GetComponent<IUI3DModel>();
						if(chara != null)
							chara.CreateModelByID (typid, m_Texture);
					}
				}
			});

			return chara3D;
		}

		private RenderTexture CreateTexture(Vector2 v2)//int _width, int _height
		{
			RenderTexture m_Texture = null;
			m_Texture = new RenderTexture((int)v2.x, (int)v2.y, 16,RenderTextureFormat.ARGB32);
			m_Texture.autoGenerateMips = false;
			m_Texture.Create();

			return m_Texture;
		}

		public bool SetCheckbox (int uiObjectID, bool isTriggle) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UITogl>() != null)
					go.GetComponent<UITogl>().SetSelectState(isTriggle);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UICheckBox component attached!");
			});
		}

		public bool SetGroupIdx (int uiObjectID, int selectIdx) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<IGroup>() != null)
					go.GetComponent<IGroup>().SetSelectIdx(selectIdx);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UITabGroup component attached!");
			});
		}

		public bool SetTabGroupNames (int uiObjectID, List<string> names) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UIGroup>() != null)
					go.GetComponent<UIGroup>().SetNames(names);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UITabGroup component attached!");
			});
		}

		public bool SetObjName (int uiObjectID, string name) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<IName>() != null)
					go.GetComponent<IName>().SetName(name);
				else
					Debug.LogWarning("====== LowoUI-UN ===> this component doesn't implement IName interface: " + go.name);
			});
		}

//		public bool SetProgressBar (int uiObjectID, float currentValue, float maxValue) {
		public bool SetProgressBar (int uiObjectID, int currentValue, int maxValue) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<IProgress>() != null)
					go.GetComponent<IProgress>().SetValue(currentValue, maxValue);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No ProcessBar component attached!");
			});
		}

		public bool SetImoji (int uiObjectID, string texture) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<Image>() == null)
					Debug.LogWarning("====== LowoUI-UN ===> No imoji image component: " + go.name);
				else 
				{
					if(!string.IsNullOrEmpty(texture)) {
						SetStateEff(uiObjectID, UIStateType.Show);

						Sprite s = UIAsset.instance.LoadSprite(texture);
						if(s != null)
							go.GetComponent<Image>().overrideSprite = s;
						else
							Debug.LogError("====== LowoUI-UN ===> No imoji texture found: " + go.name + "/ texture : " + texture);

//						Texture2D t = LowoUN.Module.Asset.Module_Asset.instance.LoadTexture(texture) as Texture2D;
//						if(t != null){
//							Material mat = new Material(Resources.Load("UIFrameAnim") as Shader);
//							mat.SetTexture("Texture", t);
//							go.GetComponent<Image>().material = mat;//.SetTexture("Texture", t);
//						}
//						else
//							Debug.LogError("====== LowoUI-UN ===> No imoji texture found: " + go.name + "/ texture : " + texture);
					}
					else {
						SetStateEff(uiObjectID, UIStateType.Hide);
					}
				}
			});
		}

		//public bool SetSlider (int uiObjectID, float currentValue, float minValue, float maxValue) {
		public bool SetSlider (int uiObjectID, float percent) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UISlider>() != null)
					go.GetComponent<UISlider>().SetValue(percent);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UISlider component attached!");
			});
		}

        public bool SetSliderMaxAndMinValue(int uiObjectID, int Max,int Min)
        {
            return SafeObjSet(uiObjectID, (GameObject go) =>
            {
                if (go.GetComponent<UISlider>() != null)
                    go.GetComponent<UISlider>().SetMaxAndMinValue(Max,Min);
                else
                    Debug.LogWarning("====== LowoUI-UN ===> No UISlider component attached!");
            });
        }

        public bool SetInput (int uiObjectID, string stringValue) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UIIpt>() != null)
					go.GetComponent<UIIpt>().SetValue(stringValue);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UIInputField component attached!");
			});
		}

		public bool SetInput (int uiObjectID, int limit) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UIIpt>() != null)
					go.GetComponent<UIIpt>().SetLimit(limit);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UIInputField component attached!");
			});
		}

		public int SetSonItemCon<T> (int uiObjectID, T itemInfo) {
			int sonHolderInsID = -1;

			SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<UISonItem>() != null)
					sonHolderInsID = go.GetComponent<UISonItem>().SetItem(itemInfo);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UI son item type con component attached!");
			});

			return sonHolderInsID;
		}

		public List<int> SetLstCon<T> (int uiObjectID, List<T> listInfo) {
			List<int> lstIds = null;

			SafeObjSet(uiObjectID, (GameObject go) =>{
				if(go.GetComponent<ILst>() != null) {
					go.GetComponent<ILst>().SetObjidOnHolder(uiObjectID);
					lstIds = go.GetComponent<ILst>().SetItemList(listInfo);
				}
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UIListGeneral component attached!");
			});

			return lstIds;
		}

        public void SetLoopRollCon<T>(int uiObjectID, List<T> listInfo)
        {
            SafeObjSet(uiObjectID, (GameObject go) =>
            {
                if (go.GetComponent<UILoopRoll>() != null)
                {
                    go.GetComponent<UILoopRoll>().SetInfo(listInfo);
                }
                else
                    Debug.LogWarning("====== LowoUI-UN ===> No UIListGeneral component attached!");
            });
        }

        public List<GameObject> GetLstPosObjects(int uiObjectID)
        {
            List<GameObject> objects = null;

            SafeObjSet(uiObjectID, (GameObject go) => {
                if (go.GetComponent<ILst>() != null)
					objects = go.GetComponent<UIList>().GetPosList();
                else
					Debug.LogWarning("====== LowoUI-UN ===> No UIListGeneral component attached!");
            });

            return objects;
        }

//		public bool SetLstFold (int uiObjectID, int idx, bool isFold) {
//			return SafeObjSet(uiObjectID, (GameObject go) =>{
//				if(go.GetComponent<ILst>() != null) {
////					go.GetComponent<ILst>().SetItemFocused(idx);
//					go.GetComponent<UILst_Fold>().SetItemFold(idx, isFold);
//				}
//				else {
//					Debug.LogWarning("====== LowoUI-UN ===> UIListGeneral component attached!");
//				}
//			});
//		}

        public bool SetLstPos (int uiObjectID, int idx) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(go.GetComponent<ILst>() != null)
					go.GetComponent<ILst>().SetItemFocused(idx);
				else
					Debug.LogWarning("====== LowoUI-UN ===> UIListGeneral component attached!");
			});
		}

        public bool SetLstPos(int uiObjectID,Vector2 vec) {
            return SafeObjSet(uiObjectID, (GameObject go) => {
                if (go.GetComponent<UIList>() != null)
					go.GetComponent<UIList>().CheckToSetCon(vec);
                else
					Debug.LogWarning("====== LowoUI-UN ===> UIListGeneral component attached!");
            });
        }

        public List<GameObject> GetLstPosGameObjects(int uiObjectID) {
            List<GameObject> posObjects = null;
            SafeObjSet(uiObjectID, (GameObject go) => {
                if (go.GetComponent<UIList>() != null)
					posObjects =go.GetComponent<UIList>().GetPosList();
                else
					Debug.LogWarning("====== LowoUI-UN ===> UIListGeneral component attached!");
            });

            return posObjects;
        }

        public bool SetStateEff(int uiObjectID, UIStateType stateType) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				if(stateType == UIStateType.Show || stateType == UIStateType.Hide){
					//ToggleElement(uiObjectID, stateType == UIStateType.Show ? true : false);
					UIHub.instance.ToggleItem(go, stateType == UIStateType.Show ? true : false);
				}
				else {
					if(go.activeSelf) {
						if(go.GetComponent<UIActionBase>() != null)
							go.GetComponent<UIActionBase>().SetStateType(stateType);
						else {
							UIStateAnimator a = go.GetComponent<UIStateAnimator>();
							if (a!= null)// && a.isActiveAndEnabled
								a.Play(stateType);
						}
					}
				}
			});
		}

		public bool PlayEfx(int uiObjectID) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				IEfx ep = go.GetComponent<IEfx>();
				if(ep != null)
					ep.Play();
				else
					Debug.LogError("====== LowoUI-UN ===> No IEffect object found! on panel: " + typeID.ToString() + "/ uiObjectID: " + uiObjectID);
			});
		}

//		public bool SetEventEff(int uiObjectID, UIEventType eventAnimType) {
//			return SafeObjSet(uiObjectID, (GameObject go) => {
//				if(go.activeSelf){
//					if(go.GetComponent<UIEventAnimator>() != null)
//						go.GetComponent<UIEventAnimator>().Play(eventAnimType);
//					else
//						Debug.LogWarning("====== LowoUI-UN ===> No UIEventAnimator component attached!");
//				}
//			});
//		}

		public bool SetLocker(int uiObjectID, bool isLock) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				//if(go.activeSelf){
				if(go.GetComponent<UILocker>() != null)
					go.GetComponent<UILocker>().SetLock(isLock);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UIEventAnimator component attached!");
				//}
			});
		}

		public bool SetLockerTyp(int uiObjectID, int mask_type) {
			return SafeObjSet(uiObjectID, (GameObject go) => {
				//if(go.activeSelf){
				if(go.GetComponent<UILocker>() != null)
					go.GetComponent<UILocker>().SetLockTyp(mask_type);
				else
					Debug.LogWarning("====== LowoUI-UN ===> No UIEventAnimator component attached!");
				//}
			});
		}

		private UIStateType animType;
		public UIStateType animState {
			get{ return animType;}
		}

		private void CompleteAnim() {
			//Debug.Log ("OnAnimMotionComplete : " + animType + " / holder type: " + typeID);
			if (onStateAnimComplete == null){
				//Debug.LogError (Comment.UI () + "holder typ: " + typeID + "onStateAnimComplete is null !!!");
			}
			else 
				onStateAnimComplete (this, animType);
			
			animType = UIStateType.None;
		}

//		//TEMP for the static embed ui panel
//		void OnDestroy () {
//			UIHub.instance.CloseUI (insID);
//		}

		public bool IsPlayingAnim () {
			if (stateAnimator == null)
				return false;
			else
				return stateAnimator.isPlaying;
		}
	}
}