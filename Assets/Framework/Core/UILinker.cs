using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;
using System.Reflection;

using LowoUN.Module.UI.Com;
using LowoUN.Util;
using LowoUN.Util.Notify;
using LowoUN.Util.Log;

using UIHandler    = LowoUN.Business.UI.UIHandler;
using UIPanelClass = LowoUN.Business.UI.UIPanelClass;
using UIPanelType  = LowoUN.Business.UI.UIPanelType;

namespace LowoUN.Module.UI
{
	public sealed class UILinker 
	{
		private static UILinker _instance;
		public static UILinker instance {
			get{ 
				if (_instance == null) 
					_instance = new UILinker ();

				return _instance;
			}
		}

		public Dictionary<int, UIHolder>                            uiHolderDict = new Dictionary<int, UIHolder> ();

		public Dictionary<string, UIPanelClass>                     cachePanelClassDict = new Dictionary<string, UIPanelClass> ();
//		public Dictionary<string, bool>                             cachePanelIsDialogDict = new Dictionary<string, bool> ();
		public Dictionary<string, UIEnum_DlgBG>                     cachePanelDialogTypDict = new Dictionary<string, UIEnum_DlgBG> ();
		public Dictionary<string, List<string>>                     cachePanelAssetDict = new Dictionary<string, List<string>>();

		public Dictionary<UIPanelType, Dictionary<int, MethodInfo>> CacheEvent4AllHolder = new Dictionary<UIPanelType, Dictionary<int, MethodInfo>>();
		private Dictionary<UIPanelType, MethodInfo>                 cachePanelBinderDict = new Dictionary<UIPanelType, MethodInfo>();

		private UILinker () {
			CachePanelAsset ();
			CachePanelInfo ();
			//CachePanelTypeName ();
		}

		#if UNITY_EDITOR
		public List<string> GetEnumNameList<T>() {//Enum enumElement
			List<string> nameList = new List<string>();

			foreach (int  myCode in Enum.GetValues(typeof(T))) {
				string strName = Enum.GetName(typeof(T), myCode);
				//Debug.LogError ("objects strName : " + strName);
				nameList.Add (strName);
			}

			return nameList;
		}
		#endif

		public void OnStart () {
			RegistNotify ();
		}

		public void OnUpdate () {
			UIClip.DoClip ();
		}

		public void OnEnd () {
			RemoveNotify();
        }


		public Type t = typeof(UIHandler);
		//TODO: setup a cache
		//regist notifies for all panel types
		private void RegistNotify () {
			//collect partial class Events to register for notify
			foreach (MethodInfo info in t.GetMethods (BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance)) {
				foreach (Events4NotifyAtt att in info.GetCustomAttributes (typeof(Events4NotifyAtt), true)) {
					if (att.isRegister) {
						info.Invoke (UIHandler.instance, null);
					}
				}
			}
		}

		private void RemoveNotify () {
			//collect partial class Events to remove for notify
			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (Events4NotifyAtt att in info.GetCustomAttributes (typeof(Events4NotifyAtt), false)) {
					if (!att.isRegister) {
						info.Invoke (UIHandler.instance, null);
					}
				}
			}
		}
			
		private void OnHolderEventHandle (UIPanelType typeID, int tempUIEventID,object[] p) {
			MethodInfo methodInfo = null;
			if (CacheEvent4AllHolder.ContainsKey (typeID)) {
				var infos = CacheEvent4AllHolder [typeID];
				if(infos.ContainsKey(tempUIEventID))
					methodInfo = infos[tempUIEventID];
				else
					Debug.LogError (string.Format("{2} No event binded to this action component which enum id: {0}, on holder: {1}", tempUIEventID, typeID, Format.UI()));
			} else {
				Debug.LogError (string.Format("{1} No event cache for current ui holder which holder type is : {0}", typeID, Format.UI()));
			}

			if (methodInfo != null)
				methodInfo.Invoke (UIHandler.instance, p);
		}

		//private void OnHolderAnimComplete (int uiHolderInsID, UIStateType animType) {
		private void OnHolderAnimComplete (UIHolder uiHolder, UIStateType animType) {
			if (animType == UIStateType.Close) {
				//RemoveHolderDictElement (uiHolder.insID);
				EndUIHolder (uiHolder);
			}
			else if (animType == UIStateType.Open) {
				UIBinder binder = uiHolder.GetBinder ();
				if (binder != null) 
					binder.OnOpen();
			}
		}

		public void AwakeUIHolder(UIHolder holder) {
			HandleHolderDelegate (holder, true);

			uiHolderDict[holder.insID] = holder;
			holder.SetBinder (GetUIBinder (holder.typeID, holder.insID));

			if(holder.GetBinder () != null)
				holder.GetBinder ().OnAwake ();

			//#if UNITY_EDITOR
			//Debug.Log(LowoUN.Comment.UI() + "Type: " + holder.typeID.ToString() + " / insID: " + holder.insID.ToString());
			//#endif
		}

		public void StartUIHolder (UIHolder holder) {
			if(holder.GetBinder () != null)
				holder.GetBinder ().OnStart ();

			holder.OnOpen ();

			UIHub.instance.CheckUsingUIByLoad (holder);
		}

		private void EndUIHolder (UIHolder uiHolder) {
			NotifyMgr.Broadcast<int>("UI_UnloadUIPanel", uiHolder.insID);

			HandleHolderDelegate (uiHolder, false);
			uiHolder.OnEnd();
			uiHolder = null;
		}

		public void ResetUIHolder (UIHolder uiHolder) {
			//TODO [LowoUI-UN]:set ui Holder Default layout by it's binder!!!
		}

		private void HandleHolderDelegate(UIHolder holder, bool isAdd) {
			if (isAdd) {
//				holder.onReset             += OnHolderReset;
				holder.onStateAnimComplete += OnHolderAnimComplete;
				holder.onInvokeHandler     += OnHolderEventHandle;
			} else {
//				holder.onReset             -= OnHolderReset;
				holder.onStateAnimComplete -= OnHolderAnimComplete;
				holder.onInvokeHandler     -= OnHolderEventHandle;
			}
		}

		private UIBinder GetUIBinder(UIPanelType uiType, int holderID) {
			UIBinder uiBinder = null;

			MethodInfo info = cachePanelBinderDict.Val(uiType);
            if (info != null)
				uiBinder = info.Invoke(UIHandler.instance, new object[] { holderID }) as UIBinder;

			return uiBinder;
		}

		public int GetPrefabClass(int p) {
			return (int)GetPrefabClass ((UIPanelType)p);
		}

		public UIPanelClass GetPrefabClass(UIPanelType p) {
			if (cachePanelClassDict != null && cachePanelClassDict.Count > 0) {
				if (cachePanelClassDict.ContainsKey (p.ToString ()))
					return cachePanelClassDict[p.ToString()];
			}

			return UIPanelClass.None;
		}

		public string GetPrefabName(int p) {
			return GetPrefabNames((UIPanelType)p)[0];
		}

		public string[] GetPrefabNames(UIPanelType p) {
			if (cachePanelAssetDict != null && cachePanelAssetDict.Count > 0) {
				if (cachePanelAssetDict.ContainsKey(p.ToString()))
					return cachePanelAssetDict[p.ToString()].ToArray();
			}

			Debug.LogWarning("====== LowoUI-UN ===> No such enum element : " + p);
			return new string[]{""};
		}

		public UIPanelType GetPanelTypeByName(string name) {
			Type t = typeof(UIPanelType);
			foreach (string s in Enum.GetNames(t)) {
				if (s == name) {
					return (UIPanelType)Enum.Parse (t, s);
				}
			}
			foreach (UIPanelType n in Enum.GetValues(t)) {
				string[] prefabNames = GetPrefabNames (n);
				foreach (var s in prefabNames) {
					if (s == name) {
						return n;
					}
				}
			}
			return UIPanelType.None;
		}

		private delegate void SafeSetCallback(UIHolder h);
		private void SafeSet(int holderID, SafeSetCallback callback) {
			if (uiHolderDict.ContainsKey (holderID)) {
				UIHolder holder = uiHolderDict [holderID];
				if (holder != null)
					callback (holder);
				else {
					////uiHolderDict.Remove (holderID);
					//RemoveHolderDictElement (holderID);
				}
			} else {
				#if UNITY_EDITOR
				Debug.LogWarningFormat ("====== LowoUI-UN ===> holder id: {0} had been removed !", holderID);
				#endif
			}
		}

		public void PlayEfx(int holderID, int objID) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.PlayEfx (objID);
			});
		}

		public void UpdateState(int holderID, int objID, UIStateType stateAnim) {
			//Debug.LogError ("OnSelectedStateChange : uiHolderDict.len : " + uiHolderDict.Count + "/ holderID : " + holderID);// + uiObjctsEnumDict [uiType].GetType ());
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetStateEff (objID, stateAnim);
			});
		}

//		public void UpdateEvent	(int holderID, int objID, UIEventType eventAnim){
//			SafeSet(holderID, (UIHolder holder) => {
//				holder.SetEventEff(objID, eventAnim);
//			});
//		}

		public void UpdatePos(int holderID, int objID, Vector2 pos) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetElementPos (objID, pos);
			});
		}

		public void UpdateSize(int holderID, int objID, Vector2 size) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetElementSize (objID, size);
			});
		}

        public void SetParent(int holderID,GameObject obj,int objID) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetParent(obj, objID);
			});
        }

        public void UpdateTxt(int holderID, int objID, string info) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetText(objID, info);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

		public void UpdateTextAlign(int holderID, int objID, TextAnchor align) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetTextAlign (objID, align);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

		public void UpdateImg(int holderID, int objID, string nameOrUrl) {
			SafeSet(holderID, (UIHolder holder) => {
				bool isweb = nameOrUrl.Contains("http")||nameOrUrl.Contains("https");
				holder.SetImg (objID, nameOrUrl, isweb);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
        }

		public GameObject UpdateWebView(int holderID, int objID, string url) {//, int dataID
			GameObject w = null;
			SafeSet(holderID, (UIHolder holder) => {
				w = holder.SetWebView (objID, url);//, dataID
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});

			return w;
		}

        public void UpdateColor(int holderID, int objID, Color newColor) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetColor (objID, newColor);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
        }

		public GameObject UpdateRImgByID(int holderID, int objID, int charaTypID, string assetName) {
			GameObject go = null;
			SafeSet(holderID, (UIHolder holder) => {
				go = holder.SetRenderTexture_New (objID, charaTypID, assetName);
			});

			return go;
		}

		public GameObject UpdateRImgByT(int holderID, int objID, object data, string assetName) {
			GameObject go = null;
			SafeSet(holderID, (UIHolder holder) => {
				go = holder.SetRenderTexture_New (objID, data, assetName);
			});

			return go;
		}

		public void UpdateProg(int holderID, int objID, ValueType currentValue, ValueType maxValue) {
			SafeSet(holderID, (UIHolder holder) => {
				if (currentValue is float)
					holder.SetProgressBar (objID, Mathf.FloorToInt((float)currentValue * 100f), Mathf.FloorToInt((float)maxValue * 100f));
				else if (currentValue is int)
					holder.SetProgressBar (objID, (int)currentValue, (int)maxValue);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
	    }

		public void UpdateSlider(int holderID, int objID, float percent) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetSlider (objID, percent);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

        public void UpdateSliderMaxAndMinValue(int holderID, int objID, int Max,int Min) {
            SafeSet(holderID, (UIHolder holder) =>
            {
                holder.SetSliderMaxAndMinValue(objID, Max, Min);
                UpdateState(holderID, objID, UIStateType.ValueChange);
            });
        }

        public void UpdateTogl(int holderID, int objID, bool isTriggle) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetCheckbox (objID, isTriggle);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

		public void UpdateGroupIdx(int holderID, int objID, int selectIdx) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetGroupIdx (objID, selectIdx);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

		public void UpdateGroupNames(int holderID, int objID, List<string> names) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetTabGroupNames (objID, names);
			});
		}

		public void UpdateName(int holderID, int objID, string name) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetObjName (objID, name);
			});
		}

		public void UpdateIptInitStr (int holderID, int objID, string stringValue) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetInput (objID, stringValue);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});
		}

		public void SetIptLimit (int holderID, int objID, int limit) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetInput (objID, limit);
			});
		}

		public int LoadSonItem<T>(int holderID, int objID, T itemInfo) {
			int sonHolderInsID = -1;
			SafeSet(holderID, (UIHolder holder) => {
				sonHolderInsID = holder.SetSonItemCon (objID, itemInfo);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});

			return sonHolderInsID;
		}

		public List<int> UpdateList<T>(int holderID, int objID, List<T> listInfo) {
			List<int> lstIds = null;
			SafeSet(holderID, (UIHolder holder) => {
				lstIds = holder.SetLstCon (objID, listInfo);
				UpdateState (holderID, objID, UIStateType.ValueChange);
			});

			return lstIds;
		}

        public void UpdateLoopRoll<T>(int holderID, int objID, List<T> listInfo) {
            SafeSet(holderID, (UIHolder holder) => {
                holder.SetLoopRollCon(objID, listInfo);
                UpdateState(holderID, objID, UIStateType.ValueChange);
            });
        }

        public List<GameObject> GetLstPosObjects(int holderID, int objID) {
            List<GameObject> objects = null;
            SafeSet(holderID, (UIHolder holder) => {
                objects = holder.GetLstPosGameObjects(objID);
            });

            return objects;
        }

        public GameObject GetGameObject(int holderID, int objID) {
            GameObject obj = null;
            SafeSet(holderID, (UIHolder holder) => {
                obj = holder.GetObj(objID);
            });
            return obj;
        }

        public void UpdateLstInfi<T>(int holderID, int objID, List<T> listInfo) {
			//TODO [LowoUI-UN]: implement the infinite list
			//uiHolderDict[holderID].SetListCon(objID, listInfo??new List<T>());
			//UpdateStateEff (holderID, objID, UIStateType.ValueChange);
		}

		public void UpdateLstFocus(int holderID, int objID, int idx) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetLstPos (objID, idx);
			});
		}

		//public void UpdateLstFold(int holderID, int objID, int idx, bool isfold) {
		//	SafeSet(holderID, (UIHolder holder) => {
		//		holder.SetLstFold (objID, idx, isfold);
		//	});
		//}
        
		public void UpdateLstPos(int holderID, int objID, Vector2 vec) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetLstPos(objID, vec);
			});
        }

		public void UpdateEmoji(int holderID, int objID, string texture) {
			SafeSet(holderID, (UIHolder holder) => {
				holder.SetImoji(objID, texture);
			});
		}

		public void SetHolderItemInfo<T> (UIPanelType uiholdType, int holderID, T itemInfo) {
			SafeSet(holderID, (UIHolder holder) => {
				if(holder.GetBinder () != null)
					holder.GetBinder ().OnStart (itemInfo as object);
			});
		}

        private void CachePanelAsset() {
			foreach (FieldInfo field in typeof(UIPanelType).GetFields()) {
                object[] objs = field.GetCustomAttributes(typeof(UIPrefabDesc), true);
				if (objs != null && objs.Length > 0) {
					List<string> prefabs = new List<string> ();
					foreach (UIPrefabDesc desc in objs) {
						prefabs.Add (desc.prefabName);
					}
					cachePanelAssetDict [field.Name] = prefabs;
					//cachePanelIsDialogDict [field.Name] = ((UIPrefabDesc)objs [0]).isDialog;
					cachePanelDialogTypDict [field.Name] = ((UIPrefabDesc)objs [0]).dlgBgTyp;
					cachePanelClassDict [field.Name] = ((UIPrefabDesc)objs [0]).prefabClass;
				} else {
					#if UNITY_EDITOR
					Debug.Log("====== LowoUI-UN ===> no panel found for UIPanelType: " + field.Name + " when Cache Panel Asset!");
					#endif
				}
            }
        }

		private void CachePanelInfo () {
			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (UIBinderAtt att in info.GetCustomAttributes(typeof(UIBinderAtt), false)) {
					cachePanelBinderDict[att.panelType] = info;
				}

                foreach (UIActionAtt att in info.GetCustomAttributes(typeof(UIActionAtt), false)) {
                    if (!CacheEvent4AllHolder.ContainsKey(att.uiPanel))
                        CacheEvent4AllHolder.Add(att.uiPanel, new Dictionary<int, MethodInfo>());

                    CacheEvent4AllHolder[att.uiPanel][att.eventID] = info;
                }
			}
		}

		// = false, = LowoUN.Module.Asset.Enum_AssetType.UI_General
		//和UIHub.instance.LoadUI 不一样，不需要判断是dialog就要加载darkBG
		public GameObject LoadUI4ChildHolder(string uiPanelPrefab) {//, bool isDialog, Enum_UIAsset assetType
//			if (isDialog) {
//				Debug.LogError ("List item can't be dialog type!!!");
//				return null;
//			}

//			return UIAsset.instance.LoadPanelByName(uiPanelPrefab, assetType);
			return UIAsset.instance.LoadPanelByName(uiPanelPrefab, Enum_UIAsset.Son);
		}

		// = LowoUN.Module.Asset.Enum_AssetType.UI_General
		//和UIHub.instance.LoadUI 不一样，不需要判断是dialog就要加载darkBG
		public GameObject LoadUI4ChildHolder (UIPanelType uiPanelType) {//, Enum_UIAsset assetType
//			bool isDialog = CheckPanelIsDialog ((int)uiPanelType);
//
//			if (isDialog) {
//				Debug.LogError ("List item can't be dialog type!!!");
//				return null;
//			}

//			return UIAsset.instance.LoadPanelByType ((int)uiPanelType, assetType);
			return UIAsset.instance.LoadPanelByType ((int)uiPanelType, Enum_UIAsset.Son);
		}

		public bool CheckPanelIsDialog(int pid) {
			return (int)GetPaneDialogBgTyp(pid) >= 0;
		}

		public UIEnum_DlgBG GetPaneDialogBgTyp(int pid) {
			UIPanelType p = (UIPanelType)pid;
			if (cachePanelDialogTypDict != null && cachePanelDialogTypDict.Count > 0) {
				if (cachePanelDialogTypDict.ContainsKey (p.ToString ()))
					return cachePanelDialogTypDict[p.ToString()];
			}

			return UIEnum_DlgBG.None;
		}

		public List<string> GetObjectsNameList (UIPanelType panelTypeID) {
			List<string> objNameList = new List<string> ();

			//object[] parameters;
			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (ObjsAtt4UIInspector att in info.GetCustomAttributes (typeof(ObjsAtt4UIInspector), false)) {
					if (panelTypeID == att.uiPanelType) {
						//parameters = new object[]{ item };

						if (info != null)
							objNameList = info.Invoke (UIHandler.instance, null) as List<string>;
					}
				}
			}

			return objNameList;
		}

		//SerializedProperty item, 
		public List<string> GetEventsNameList (UIPanelType panelTypeID) {		
			List<string> evtNameList = new List<string> ();

			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (EvtsAtt4UIInspector att in info.GetCustomAttributes (typeof(EvtsAtt4UIInspector), false)) {
					if (panelTypeID == att.uiPanelType) {
						if (info != null)
							evtNameList = info.Invoke (UIHandler.instance, null) as List<string>;
					}
				}
			}

			return evtNameList;
		}

		public UIButton GetUIButtonIns (string panelName, string btnName) {
			UIButton uibutton = null;

			int panelInt = -1;
			int btnInt = -1;
			int panelIdxInList = -1;

			UIPanelType panelTypeID = UIPanelType.None;
			if (int.TryParse (panelName,out panelInt))
				panelTypeID = (UIPanelType)panelInt;
			else
				panelTypeID = GetPanelTypeByName (panelName);
			List<string> EventsNameList = GetEventsNameList(panelTypeID);

			//btn
			int btnEventid = -1;
			if (int.TryParse (btnName,out btnInt))
				btnEventid = btnInt;
			else
				btnEventid = EventsNameList.IndexOf(btnName);

			List<UIButton> btns = new List<UIButton> ();

			if (btnEventid >= 0) {// find child event
				List<UIHolder> panelInsList = UIHub.instance.GetHolders (panelTypeID);
				foreach (var item in panelInsList) {
					if (panelIdxInList != -1) {
						if (item.curIdxInList != panelIdxInList)
							continue;
					}
					GameObject btn = item.GetEvt (btnEventid);
					if(btn!=null)
						btns.Add (btn.GetComponent<UIButton> ());
				}
			}
			else {//otherwise try to find child object by name
				List<UIHolder> panelInsList = UIHub.instance.GetHolders (panelTypeID);
				foreach (var item in panelInsList) {
					if (panelIdxInList != -1) {
						if (item.curIdxInList != panelIdxInList)
							continue;
					}
					UIButton[] _btn = item.GetComponentsInChildren<UIButton> (item);
					foreach (var n in _btn) {
						if(n.gameObject.name == btnName)
							btns.Add (n);
					}
				}
			}

			foreach (var b in btns) {
				if (b.isActiveAndEnabled) {
					//ret = true;
					//b.OnAction();
					uibutton = b;
					break;
				}
			}

			return uibutton;
		}

		//!!!!!!!!!!!!!!!!!!!!!!!!!!
		public void RemoveHolderDictElement (int hinsid) {
			if (uiHolderDict.ContainsKey (hinsid)) {
				UIHolder h = uiHolderDict [hinsid];
				uiHolderDict.Remove (hinsid);
				UIHolder.RecycleInsid ();

				UIHub.instance.CheckUsingUIByUnload (h);
			} else {
				#if UNITY_EDITOR
				Debug.LogWarning ("uiHolderDict dosen't have instance: " + hinsid);
				#endif
			}
		}
	}
}