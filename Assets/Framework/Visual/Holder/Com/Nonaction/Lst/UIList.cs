#pragma warning disable 0649//ignore default value null
using System;
using System.Collections.Generic;
using LowoUN.Business.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	/// <summary>
	/// 1, normal(dynamic)
	/// 2, mid 以中心两侧扩展
	/// 3, Irreg 动态高度，主要针对对于文本成员列表
	/// 
	/// !!! remove bellow
	/// 4, Fold 可折叠(只有一级折叠)
	/// 5, Mess 不固定高度(Fold的扩展，可多级折叠)
	/// 6, normal(static)
	/// </summary>
	public enum UILstMotionTyp {
		None,
		Single,
		Row,
		Column,
	}
	public enum UIListDir {
		IsU,
		IsV,
	}

	public class UIList : UIActionBase, ILst {
#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID; // = UIPanelClass.None;
#endif
		[SerializeField]
		private string typeIdx = UIPanelType.None.ToString ();

		[SerializeField]
		private GameObject scrollView;
		//[SerializeField]
		private bool isDynamic = true;
		[SerializeField]
		private bool isUseStaticPos;

		[SerializeField]
		private UIPanelType itemPanelType;
		[SerializeField]
		private string itemPanelPrefab;

		//regular position for dynamic loaded items ----------------------
		[SerializeField]
		private bool isU;
		[SerializeField]
		private float rows = 1f;
		[SerializeField]
		private float uOffset;
		[SerializeField]
		private bool isV;
		[SerializeField]
		private float columns = 1f;
		[SerializeField]
		private float vOffset;
		//[SerializeField]
		//private int maxItem;
		//private GameObject itemType;
		[SerializeField]
		private float itemWidth = -1;
		[SerializeField]
		private float itemHeight = -1;

		[SerializeField]
		private Vector2 offsetEdge = new Vector2 (0f, 0f);
		//private float uOffsetEdge = 10f;

		//free position for dynamic loaded items -------------------------
		[SerializeField]
		private List<GameObject> posStaticList;

		private RectTransform rt;
		private Vector2 rtOriginSize;

		//for static loaded items ----------------------------------------
		//		[SerializeField]
		private List<GameObject> gameObjList = new List<GameObject> ();
		//		[SerializeField]
		//		private bool isStillShow = false;//------------------------------------------------------- TODO: toremove
		//		[SerializeField]
		//		private bool isArrange4Static = false;

		//		[SerializeField]
		//		private bool isURoll4Static;//------------------------------------------------------- TODO: toremove
		//		[SerializeField]
		//		private bool isVRoll4Static;//------------------------------------------------------- TODO: toremove

		[SerializeField]
		private GameObject btnPrev;
		[SerializeField]
		private GameObject btnNext;
		[SerializeField]
		private int showNum;

		private bool isPaging;
		private bool isShowPagesText;

		[SerializeField]
		private UIList embedLst;
		[SerializeField]
		private float embedOffset;

		[SerializeField]
		private bool isUCenterAlign; //特例：1，为水平单行定制；2，以宽度中心为原点，两侧对其排列//------------------------------------------------------- TODO: toremove

		[SerializeField]
		private bool isPosByFocus = false; //如果是通过设置focus Idx来定位的列表

		//HACK: 对于加载的item内部有遮罩，且List容器有可能移除屏幕再移入导致Active状态的变化时遮罩无效的情况，采取每次加载都强制清理缓存的gameobject方式
		[SerializeField]
		private bool isForceReloadItems = false;

		public Action<RectTransform> onUpdateSize;

		public bool is_U { get { return isU; } }
		public bool is_V { get { return isV; } }

		// Use this for initialization
		void Awake () {
			//if (!isDynamic) {
			//	Debug.LogWarning ("<color=#ff0000>" + Format.UI()+ string.Format("static list holdertyp: {0} is no longer supported by normal ui list component", typeIdx) + "</color>");
			//	Debug.LogWarning ("<color=#ff0000>" + Format.UI()+ "!!!!!!!!!!!! use UICom_LstStatic instead" + "</color>");
			//}

			InitScrollBar ();

			if (isDynamic) {
				NotifyScrollView ();
				CheckRectTransform ();

				//init for paging
				Init4Paging ();

				//init for son lst
				Init4EmbedLst ();
			}
			//else {
			//	NotifyScrollView4Static ();
			//}
		}

		private UIScrollFixed scrollFixed;
		private void InitScrollBar () {
			if (scrollView == null) {
				if (transform.parent.parent != null && transform.parent.parent.GetComponent<ScrollRect> () != null)
					scrollView = transform.parent.parent.gameObject;
			}
			if (scrollView != null) {
				if (scrollView.GetComponent<UIScrollFixed> () == null)
					scrollView.AddComponent<UIScrollFixed> ();
				scrollFixed = scrollView.GetComponent<UIScrollFixed> ();
			}
		}

		private void Init4Paging () {
			if (scrollView != null) {
				if (btnPrev != null && btnNext != null) {
					UIEventListener.Get (btnPrev).onClick = Prev;
					UIEventListener.Get (btnNext).onClick = Next;
				}
			}
		}

		private void Init4EmbedLst () {
			if (embedLst != null) {
				embedLst.onUpdateSize += RefreshByEmbedCon;

				Vector2 size = embedLst.GetComponent<RectTransform> ().sizeDelta;
				if (isU) {
					embedLst.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0f, size.y);
				} else if (isV) {
					embedLst.GetComponent<RectTransform> ().sizeDelta = new Vector2 (size.x, 0f);
				}
			}
		}

		private void CheckRectTransform () {
			if (rt == null) {
				rt = GetComponent<RectTransform> ();

				if (scrollView != null) {
					rtOriginSize = rt.sizeDelta = scrollView.GetComponent<RectTransform> ().sizeDelta; // - new Vector2(17f,17f);
				} else {
					rtOriginSize = rt.sizeDelta;
				}
			}
		}

		// Update is called once per frame
		void Update () {
			//CheckPrevNextBtnVisble();
			UIFunc_Lst.CheckPrevNextBtnVisble (
				btnNext,
				btnPrev,
				scrollView,
				_realShowItemCount,
				itemWidth,
				itemHeight,
				transform,
				isV,
				isU
			);
		}

		private void NotifyScrollView () {
			if (isU) {
				//if(onSetScrollDirection != null) onSetScrollDirection (UIScrollViewDir.U);
				if (scrollView != null && scrollFixed != null)
					scrollFixed.SetScrollDirection (UIScrollViewDir.U);
			} else if (isV) {
				if (scrollView != null && scrollFixed != null)
					scrollFixed.SetScrollDirection (UIScrollViewDir.V);
			}
		}

		//private void NotifyScrollView4Static () {
		//	if (scrollView != null && scrollFixed != null) {
		//		if (!isURoll4Static && !isVRoll4Static) {
		//			scrollFixed.SetScrollDirection (UIScrollViewDir.None);
		//		} else {
		//			if (isURoll4Static) {
		//				scrollFixed.SetScrollDirection (UIScrollViewDir.U);
		//			}
		//			if (isVRoll4Static) {
		//				scrollFixed.SetScrollDirection (UIScrollViewDir.V);
		//			}
		//		}
		//	}
		//}

		private void CheckToLoadNewItems (int tempLength, int newListCount) {
			if (newListCount > tempLength) {
				for (int i = tempLength; i < newListCount; i++) {
					GameObject itemGameObj = null;

					//----------------- load item ui -----------------
					//GameObject itemGameObj = UIHandler.instance.LoadUI(itemType.GetComponent<UIHolder>().typeID) as GameObject;
					//GameObject itemGameObj = UIHandler.instance.LoadUI(itemPanelType) as GameObject;
					if (!string.IsNullOrEmpty (itemPanelPrefab)) {
						bool match = false;
						foreach (string name in UILinker.instance.GetPrefabNames (itemPanelType)) {
							if (string.Equals (itemPanelPrefab, name, StringComparison.OrdinalIgnoreCase))
								match = true;
						}
						if (match)
							itemGameObj = UILinker.instance.LoadUI4ChildHolder (itemPanelPrefab) as GameObject; //, false, Enum_UIAsset.Son
						else
							itemGameObj = UILinker.instance.LoadUI4ChildHolder (itemPanelType) as GameObject; //, Enum_UIAsset.Son
					} else
						itemGameObj = UILinker.instance.LoadUI4ChildHolder (itemPanelType) as GameObject; //, Enum_UIAsset.Son
					//----------------- load item ui -----------------

					if (itemGameObj != null)
						itemGameObj.name = itemPanelType.ToString () + i;

					ResetNewLoadGameObj (itemGameObj);
				}
			}
		}

		private void ResetNewLoadGameObj (GameObject itemGameObj) {
			if (itemGameObj == null) {
				Debug.LogError ("====== LowoUN-UI ===> Don't forget to set list item reference with prefab name: " + itemPanelPrefab);
			} else {
				Transform itemTrans = itemGameObj.transform;
				itemTrans.SetParent (transform);

				InitNewLoadGameObjSize (itemTrans);
				itemTrans.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, 0, 0);
				itemTrans.localScale = Vector3.one;
				itemGameObj.layer = transform.gameObject.layer;

				gameObjList.Add (itemGameObj);
			}
		}

		private void InitNewLoadGameObjSize (Transform itemTrans) {
			if (itemWidth > 0f && itemHeight > 0f) {
				itemTrans.GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemWidth, itemHeight);
			} else {
				//#if UNITY_EDITOR
				//Debug.LogWarning ("====== LowoUN-UI ===> use the item loaded default size.");
				//#endif
				itemWidth = itemTrans.GetComponent<RectTransform> ().sizeDelta.x;
				itemHeight = itemTrans.GetComponent<RectTransform> ().sizeDelta.y;
			}
			//t.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
		}

		//TODO:
		//[SerializeField]
		//private UILstMotionTyp motionTyp = UILstMotionTyp.Single;
		[SerializeField]
		private float interval4Anim = 0.2f;
		[SerializeField]
		private float delay4Anim = 0.0f;
		[SerializeField]
		private int group4Anim = 0;

		private UIStateAnimator stateAnimator;
		private void ArrangeItems () {
			if (isUseStaticPos) {
				if (posStaticList.Count < gameObjList.Count)
					Debug.LogWarning ("Not enough position gameobjects for items!");

				int minLength = Mathf.Min (gameObjList.Count, posStaticList.Count);
				for (int i = 0; i < minLength; i++) {
					//check position gameobject is enough ??
					gameObjList[i].transform.GetComponent<RectTransform> ().anchoredPosition = posStaticList[i].transform.GetComponent<RectTransform> ().anchoredPosition;
				}
			} else {
				for (int i = 0; i < gameObjList.Count; i++) {
					ArrangeItem (gameObjList[i].transform, i);

					//					////////////////////////////////////////////////////
					//					/// play item animation by order.
					//					//1,set delay time
					//					stateAnimator = gameObjList [i].GetComponent<UIStateAnimator> ();
					//					if (stateAnimator != null) {
					//						stateAnimator.current_group_filter = group4Anim;
					//						stateAnimator.SetStateParams (UIStateType.Open, delay4Anim + interval4Anim * (float)i + Time.deltaTime/*HACK: delay for one frame time*/);
					//
					//						//2,update anim state: open
					//						gameObjList[i].GetComponent<UIHolder>().OnOpen();
					//					}
				}
			}
		}

		public void SkipItemsAnimation (UIStateType type) {
			for (int i = 0; i < gameObjList.Count; i++) {
				stateAnimator = gameObjList[i].GetComponent<UIStateAnimator> ();
				if (stateAnimator != null) {
					stateAnimator.Skip (type);
				}
			}
		}

		private void OpenItems () {
			//////////////////////////////////////////////////
			/// play item animation by order.
			//1,set delay time
			for (int i = 0; i < gameObjList.Count; i++) {
				stateAnimator = gameObjList[i].GetComponent<UIStateAnimator> ();
				if (stateAnimator != null) {
					stateAnimator.current_group_filter = group4Anim;
					stateAnimator.SetStateParams (UIStateType.Open, delay4Anim + interval4Anim * (float) i + Time.deltaTime /*HACK: delay for one frame time*/ );
					//delay for close
					stateAnimator.SetStateParams (UIStateType.Close, interval4Anim * (float) i + Time.deltaTime /*HACK: delay for one frame time*/ );
					//2,update anim state: open
					gameObjList[i].GetComponent<UIHolder> ().OnOpen ();
				}
			}
		}

		public void UpdateAnimationInterval () {
			for (int i = 0; i < gameObjList.Count; i++) {
				stateAnimator = gameObjList[i].GetComponent<UIStateAnimator> ();
				if (stateAnimator != null) {
					stateAnimator.current_group_filter = group4Anim;
					stateAnimator.SetStateParams (UIStateType.Open, delay4Anim + interval4Anim * (float) i + Time.deltaTime /*HACK: delay for one frame time*/ );
					stateAnimator.SetStateParams (UIStateType.Close, interval4Anim * (float) i + Time.deltaTime /*HACK: delay for one frame time*/ );
				}
			}
		}

		float x;
		float y;
		private void ArrangeItem (Transform itemT, int idx) {
			if (isU) {
				if (isUCenterAlign) {
					float w = rt.sizeDelta.x;
					if (_realShowItemCount % 2 == 0) {
						//rt.sizeDelta
						if (idx < _realShowItemCount / 2)
							x = w / 2 - (itemWidth / 2 + (_realShowItemCount / 2 - idx - 1) * itemWidth) - uOffset * (_realShowItemCount / 2 - idx);
						else
							x = w / 2 + (itemWidth / 2 + (idx - _realShowItemCount / 2) * itemWidth) + uOffset * (idx - _realShowItemCount / 2);
					} else {
						if ((float) idx < (float) _realShowItemCount / 2f)
							x = w / 2 - (_realShowItemCount / 2 - idx) * itemWidth - uOffset * (_realShowItemCount / 2 - idx);
						else
							x = w / 2 + (idx - _realShowItemCount / 2) * itemWidth + uOffset * (idx - _realShowItemCount / 2);
					}
				} else {
					//x = uOffset + (itemWidth + uOffset) * Mathf.Floor (idx / rows) + itemWidth / 2;// - rt.sizeDelta.x / 2;
					x = offsetEdge.x + (itemWidth + uOffset) * Mathf.Floor (idx / rows) + itemWidth / 2;
				}
				float spaceY = (rtOriginSize.y - itemHeight * rows) / (rows + 1);
				y = -(spaceY * (idx % rows + 1) + (idx % rows) * itemHeight + itemHeight / 2); // + rtOriginSize.y / 2;
			} else if (isV) {
				float spaceX = (rtOriginSize.x - itemWidth * columns) / (columns + 1);
				x = spaceX * (idx % columns + 1) + (idx % columns) * itemWidth + itemWidth / 2; // - rtOriginSize.x / 2;
				//y = -vOffset - (itemHeight+vOffset) * Mathf.Floor (idx / columns) - itemHeight / 2;// + rt.sizeDelta.y / 2;
				y = -offsetEdge.y - (itemHeight + vOffset) * Mathf.Floor (idx / columns) - itemHeight / 2;
			}

			itemT.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		}

		private void OnClickFromItem (int tempUIEventID, params object[] arr) {
			UpdateItemSelectedState (tempUIEventID);

			//if(onCallEvent != null)
			//	onCallEvent (currEventID, tempUIEventID, hostHolderInsID);
		}
		//disable button event directly from list component
		/*private void OnClickButton(int tempUIEventID, params object[] arr) {
			UpdateItemSelectedState (tempUIEventID);

            if (onCallEvent != null)
                onCallEvent(currEventID, tempUIEventID, (int)arr[0], currHolderInsID);
        }*/

		//show selected flag
		private void UpdateItemSelectedState (int idxIdInList) {
			SetAllGoDeselected ();
			idxIdInList = Mathf.Min (idxIdInList, _realShowItemCount - 1);
			if (idxIdInList >= 0 && idxIdInList < gameObjList.Count) {
				UIStateVisual v = gameObjList[idxIdInList].GetComponent<UIStateVisual> ();
				if (v != null) // && v.isActiveAndEnabled
					v.SetState (UIStateType.Selected);
			}
		}
		private void SetAllGoDeselected () {
			if (_realShowItemCount > 0) {
				for (int i = 0; i < Mathf.Min (_realShowItemCount, gameObjList.Count); i++) {
					UIStateVisual v = gameObjList[i].GetComponent<UIStateVisual> ();
					if (v != null) // && v.isActiveAndEnabled
						v.SetState (UIStateType.Deselected);
				}
			}
		}

		private void SetListConPos (Vector2 vec2) {
			if (rt == null) {
				rt = GetComponent<RectTransform> ();
			}
			rt.anchoredPosition = vec2;
		}

		private float newHeight;
		private float newWidth;
		private void UpdateSize (int itemAmount) {
			if (isUCenterAlign)
				return;

			CheckRectTransform ();
			//Reminder_UVRange ();

			if (isV) {
				newHeight = (itemHeight + vOffset) * (itemAmount % columns == 0 ? Mathf.Floor (itemAmount / columns) : Mathf.Floor (itemAmount / columns) + 1) - vOffset;
				newHeight += offsetEdge.y * 2;
				rt.sizeDelta = new Vector2 (rtOriginSize.x, newHeight);
			} else if (isU) {
				newWidth = (itemWidth + uOffset) * (itemAmount % rows == 0 ? Mathf.Floor (itemAmount / rows) : Mathf.Floor (itemAmount / rows) + 1) - uOffset;
				newWidth += offsetEdge.x * 2;
				rt.sizeDelta = new Vector2 (newWidth, rtOriginSize.y);
			}

			if (embedLst != null && embedLst.gameObject.activeSelf)
				RefreshByEmbedCon (embedLst.GetComponent<RectTransform> ());
			if (onUpdateSize != null)
				onUpdateSize (rt);
		}

		private void Reminder_UVRange () {
			if (isDynamic) {
				if (isV && columns == 0) {
#if UNITY_EDITOR
					Debug.LogWarning ("====== LowoUN-UI ===> columns value can't be 0! for the list of holder: " + hostHolderInsID);
#endif
					columns = 1;
				}
				if (isU && rows == 0) {
#if UNITY_EDITOR
					Debug.LogWarning ("====== LowoUN-UI ===> rows value can't be 0! for the list of holder: " + hostHolderInsID);
#endif
					rows = 1;
				}
			}
		}

		private void RefreshByEmbedCon (RectTransform rtf) {
			Vector2 conSize = rtf.sizeDelta;

			if (isV) {
				rt.sizeDelta = new Vector2 (rtOriginSize.x, newHeight + conSize.y + embedOffset);
				//tf.GetComponent<IPos> ().SetY (newHeight);
				rtf.anchoredPosition = new Vector2 (0, -newHeight - embedOffset);
			} else if (isU) {
				rt.sizeDelta = new Vector2 (newWidth + conSize.x + embedOffset, rtOriginSize.y);
				//tf.GetComponent<IPos> ().SetX (newWidth);
				rtf.anchoredPosition = new Vector2 (newWidth + embedOffset, 0);
			}
		}

		private void InitAllGameObjsAction () {
			//for (int i = 0; i < gameObjList.Count; i++) {
			for (int i = 0; i < Mathf.Min (_realShowItemCount, gameObjList.Count); i++) {
				if (gameObjList[i].GetComponent<UIHolder> () != null) {
					gameObjList[i].GetComponent<UIHolder> ().curIdxInList = i;
					gameObjList[i].GetComponent<UIHolder> ().hostLstObjid = objidOnHolder;
					gameObjList[i].GetComponent<UIHolder> ().parentHolderInsID = hostHolderInsID;
					gameObjList[i].GetComponent<UIHolder> ().onCallEvent4List = OnClickFromItem;
				} else {
#if UNITY_EDITOR
					Debug.LogWarning ("====== LowoUN-UI ===> Dont forget add UIHolder for list item!!! on holder with ID: " + hostHolderInsID);
#endif
				}

				/*if (gameObjList[i].GetComponent<UIActionBase>() != null)
				{
					//gameObjList [i].GetComponent<UIActionBase> ().currEventID = i;
					gameObjList[i].GetComponent<UIActionBase>().currHolderInsID = currHolderInsID;
					gameObjList[i].GetComponent<UIActionBase>().currIdxInList = i;
					gameObjList[i].GetComponent<UIActionBase>().onCallEvent = OnClickButton;
				}*/
			}
		}

		/*private void ResetShow ()
		{
			if (isDynamic) {
				if (gameObjList.Count > 0) {
					foreach (GameObject item in gameObjList) {
						if (item.GetComponent<UIHolder> () == null)
							Debug.LogError ("Don't forget to add UIHolder component to the list item.");
						UIHandlerHub.instance.CloseUI (item.GetComponent<UIHolder> ().instanceID);
					}
				}
				gameObjList.Clear ();
			}
			else {
				if (gameObjList.Count > 0) {
					foreach (GameObject item in gameObjList) {
						if (!isStillShow) {
							ToggleItemVisibility (item, false);
						}
						else {
							//TODO: game object clear data
							item.GetComponent<UIHolder> ().OnSetDefaultShow ();
						}
					}
				}
			}
		}*/

		private int objidOnHolder;
		public void SetObjidOnHolder (int objid) {
			objidOnHolder = objid;
		}

		private int _realShowItemCount;

		//////////////////////////////////////
		private void ClearCacheGameObjects () {
			if (gameObjList.Count > 0) {
				gameObjList.ForEach (i => UIHub.instance.CloseUI (i.GetComponent<UIHolder> ().insID));
				gameObjList.Clear ();
			}
		}
		//////////////////////////////////////

		public List<int> SetItemList<T> (List<T> itemList) {
			//HACK
			if (isForceReloadItems)
				ClearCacheGameObjects ();

			itemList = itemList ?? new List<T> ();

			_realShowItemCount = 0;
			int _infoCount = itemList.Count;

			if (isDynamic) {
				_realShowItemCount = _infoCount;

				CheckToLoadNewItems (gameObjList.Count, _infoCount);
				UpdateSize (_infoCount);
				ArrangeItems ();
				OpenItems ();
			}
			//else {
			//	_realShowItemCount = Mathf.Min(_infoCount, gameObjList.Count);
			//	if (gameObjList.Count < _infoCount) {
			//		#if UNITY_EDITOR
			//		Debug.LogWarning ("====== LowoUN-UI ===> no enough item containers for the static list!");
			//		#endif
			//	}
			//}

			SetAllGameObjsVisible ();
			InitAllGameObjsAction ();
			SetAllGoDeselected ();
			UpdateItemsInfo (itemList);

			//if(!_hasManuallySetScrollBarValue)
			//	CheckToResetCon (Vector2.zero);
			if (!isPosByFocus)
				CheckToSetCon (Vector2.zero);

			//CheckPrevNextBtnVisble ();
			UIFunc_Lst.CheckPrevNextBtnVisble (
				btnNext,
				btnPrev,
				scrollView,
				_realShowItemCount,
				itemWidth,
				itemHeight,
				transform,
				isV,
				isU
			);

			//TODO: set List container active state
			gameObject.SetActive (_realShowItemCount > 0);

			return GetValidGameObjsHolderInsid ();
		}

		private List<int> GetValidGameObjsHolderInsid () {
			List<int> lstIds = null;

			if (_realShowItemCount > 0) {
				lstIds = new List<int> ();
				for (int i = 0; i < Mathf.Min (_realShowItemCount, gameObjList.Count); i++) {
					lstIds.Add (gameObjList[i].GetComponent<UIHolder> ().insID);
				}
			}

			return lstIds;
		}

		//private bool _hasManuallySetScrollBarValue = false;
		//private int _curFocusID = -1;
		public void SetItemFocused (int idx) {
			//_curFocusID = idx;
			_curShowMinIdx = idx;
			UpdateItemSelectedState (idx);

			//CheckScrollView ();
			if (scrollView != null) {
				//_hasManuallySetScrollBarValue = false;

				if (isDynamic) {
					UIList list = scrollView.GetComponentInChildren<UIList> ();
					if (list != null) {
						if (isV) {
							if (list.GetComponent<RectTransform> ().sizeDelta.y > scrollView.GetComponent<RectTransform> ().sizeDelta.y)
								CheckToSetCon (new Vector2 (0f, GetConPosByItemIdx (idx)));
						} else if (isU) {
							if (list.GetComponent<RectTransform> ().sizeDelta.x > scrollView.GetComponent<RectTransform> ().sizeDelta.x)
								CheckToSetCon (new Vector2 (-GetConPosByItemIdx (idx), 0f));
						}
					} else {
						if (isV) {
							CheckToSetCon (new Vector2 (0f, GetConPosByItemIdx (idx)));
						} else if (isU) {
							CheckToSetCon (new Vector2 (-GetConPosByItemIdx (idx), 0f));
						}
					}
				}
				//else {                  
				//                    if (isV)
				//                    {
				//                        CheckToSetCon(new Vector2(0, GetConPosByItemIdx4Static(idx)));//scrollViewSize.x
				//                    }
				//                    else if (isU)
				//                    {
				//                        CheckToSetCon(new Vector2(GetConPosByItemIdx4Static(idx), 0));//scrollViewSize.y
				//                    }
				//}

				//if(!_hasManuallySetScrollBarValue)
				//	_hasManuallySetScrollBarValue = true;
			}
		}

		private float GetConPosByItemIdx (int idx) {
			if (idx != 0 && gameObjList.Count > 0) {
				idx = Mathf.Min (idx, _realShowItemCount - 1);

				if (isV) {
					float itemPosY = gameObjList[idx].GetComponent<RectTransform> ().anchoredPosition.y;
					float deltaY = Mathf.Abs (itemPosY) - rtOriginSize.y / 2;
					deltaY = Mathf.Clamp (deltaY, 0, rt.sizeDelta.y - rtOriginSize.y);

					return deltaY;
				} else if (isU) {
					//NOTEST: reset dynamic list container position
					float itemPosX = gameObjList[idx].GetComponent<RectTransform> ().anchoredPosition.x;
					float deltaX = Mathf.Abs (itemPosX) - rtOriginSize.x / 2;
					deltaX = Mathf.Clamp (deltaX, 0, rt.sizeDelta.x - rtOriginSize.x);

					return deltaX;
				}
			}

			return 0f;
		}

		//private float GetConPosByItemIdx4Static (int idx) {
		//	Vector2 scrollViewSize = scrollView.GetComponent<RectTransform>().sizeDelta;

		//	if (idx != 0 && gameObjList.Count > 0) {
		//		if (isVRoll4Static) {
		//			//NOTEST reset static list container position
		//			float itemPosY = gameObjList [idx].GetComponent<RectTransform> ().anchoredPosition.y;
		//			float posY = -(Mathf.Abs (itemPosY) - scrollViewSize.y / 2);
		//			posY = Mathf.Clamp (posY, -(rt.sizeDelta.y - scrollViewSize.y / 2), 0);

		//			return posY;
		//		}
		//		else if (isURoll4Static) {
		//			float itemPosX = gameObjList [idx].GetComponent<RectTransform> ().anchoredPosition.x;
		//			float posX = -(Mathf.Abs (itemPosX) - scrollViewSize.x / 2);
		//			posX = Mathf.Clamp (posX, -(rt.sizeDelta.x - scrollViewSize.x / 2), 0);

		//			return posX;
		//		}
		//	}

		//	return 0f;
		//}

		//		private bool IsNeedResetCon () {
		//			return scrollView != null;
		//		}

		public void CheckToSetCon (Vector2 vec2) {
			if (scrollView != null)
				SetListConPos (vec2);
		}

		private void SetAllGameObjsVisible () {
			foreach (GameObject obj in gameObjList) {
				ToggleItemVisibility (obj, false);
				UIFunc_Lst.ResetItem (obj.GetComponent<UIHolder> ());
			}

			for (int i = 0; i < Mathf.Min (_realShowItemCount, gameObjList.Count) && i < gameObjList.Count; i++) {
				//ToggleItemAction (gameObjList [i], true);
				ToggleItemVisibility (gameObjList[i], true);
			}
		}

		/*private void ToggleItemAction (GameObject obj, bool isEnable)
		{
			if (isInteractive) {
				if (isNeedToggleBtnEvent) {
					if(obj.GetComponent<UIButton> () != null)
						obj.GetComponent<UIButton> ().SetEnable (isEnable);
				}
			}
		}*/

		private void ToggleItemVisibility (GameObject obj, bool isShow) {
			if (isDynamic) {
				UIHub.instance.ToggleItem (obj, isShow);
			}
			//else {
			//	if (!isStillShow) {
			//		UIHub.instance.ToggleItem (obj, isShow);
			//	}
			//}
		}
		//		private void ResetItem (UIHolder h) {
		//			if (h != null) {
		//				//h.OnReset ();
		//				h.curIdxInList = -1;
		//			}
		//		}

		private UIPanelType uiholdType;
		private int uiholdInstanceID;
		private void UpdateItemsInfo<T> (List<T> itemList) {
			if (gameObjList != null && gameObjList.Count > 0) {
				//SetAllGoDeselected ();

				for (int i = 0; i < itemList.Count && i < gameObjList.Count; i++) {
					//ToggleItemAction (gameObjList [i], true);
					//ToggleItemVisibility (gameObjList [i], true);

					if (gameObjList[i].GetComponent<UIHolder> () == null) {
						Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add UIHolder to the list component on the panel with holder id: " + hostHolderInsID);
					} else {
						uiholdType = gameObjList[i].GetComponent<UIHolder> ().typeID;
						uiholdInstanceID = gameObjList[i].GetComponent<UIHolder> ().insID;
						UILinker.instance.SetHolderItemInfo (uiholdType, uiholdInstanceID, itemList[i]);
					}
				}
			}
		}

		//for one row and one Column
		private int _curShowMinIdx = 0;
		private void Next (GameObject go) {
			Debug.Log ("====== LowoUN-UI ===> list press btnNext: " + go.name);

			if (transform.GetComponent<RectTransform> ().anchoredPosition.x <= scrollView.GetComponent<RectTransform> ().sizeDelta.x - _realShowItemCount * itemWidth + itemWidth)
				return;
			_curShowMinIdx++;
			_curShowMinIdx = Mathf.Min (_curShowMinIdx, _realShowItemCount);
			if (_curShowMinIdx <= _realShowItemCount - 1)
				MoveCon (1);
		}
		private void Prev (GameObject go) {
			Debug.Log ("====== LowoUN-UI ===> list press btnPrev: " + go.name);
			if (transform.GetComponent<RectTransform> ().anchoredPosition.x >= 0)
				return;
			_curShowMinIdx--;
			_curShowMinIdx = Mathf.Max (_curShowMinIdx, 0);

			if (_curShowMinIdx >= 0)
				MoveCon (-1);
		}
		private void MoveCon (int direction = 0) {
			float pos = 0f;

			if (isU) {
				pos = -_curShowMinIdx * itemWidth;
				transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (pos, 0);
			} else if (isV) {
				pos = _curShowMinIdx * itemHeight;
				transform.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, pos);
			}
		}

		public List<GameObject> GetPosList () {
			return posStaticList;
		}
	}
}