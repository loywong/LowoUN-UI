#pragma warning disable 0649//ignore default value null
using System;
using System.Collections.Generic;
using LowoUN.Business.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	public class UILst_Irreg : UIActionBase, ILst {
#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID; // = UIPanelClass.None;
#endif
		[SerializeField]
		private string typeIdx = UIPanelType.None.ToString ();

		[SerializeField]
		private GameObject scrollView;

		private bool isDynamic = true;

		[SerializeField]
		private UIPanelType itemPanelType;
		[SerializeField]
		private string itemPanelPrefab;

		//regular position for dynamic loaded items ----------------------
		[SerializeField]
		private bool isU;
		//		[SerializeField]
		//		private float rows = 1f;
		[SerializeField]
		private float uOffset;
		[SerializeField]
		private bool isV;
		//		[SerializeField]
		//		private float columns = 1f;
		[SerializeField]
		private float vOffset;

		private RectTransform rt;
		private Vector2 rtOriginSize;

		//for static loaded items ----------------------------------------
		[SerializeField]
		private List<GameObject> gameObjList;

		[SerializeField]
		private bool isURoll4Static;
		[SerializeField]
		private bool isVRoll4Static;

		//		[SerializeField]
		//		private bool isUCenterAlign;//!!!!!! use UICom_LstUVCenter //特例：1，为水平单行定制；2，以宽度中心为原点，两侧对其排列

		[SerializeField]
		private bool isPosByFocus = false; //如果是通过设置focus Idx来定位的列表

		public Action<RectTransform> onUpdateSize;

		// Use this for initialization
		void Awake () {
			if (scrollView == null) {
				if (transform.parent.parent != null && transform.parent.parent.GetComponent<ScrollRect> () != null)
					scrollView = transform.parent.parent.gameObject;
			}

			if (isDynamic) {
				NotifyScrollView ();
				CheckRectTransform ();
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

		private void NotifyScrollView () {
			if (isU) {
				if (scrollView != null && scrollView.GetComponent<UIScrollFixed> () != null)
					scrollView.GetComponent<UIScrollFixed> ().SetScrollDirection (UIScrollViewDir.U);
			} else if (isV) {
				if (scrollView != null && scrollView.GetComponent<UIScrollFixed> () != null)
					scrollView.GetComponent<UIScrollFixed> ().SetScrollDirection (UIScrollViewDir.V);
			}
		}

		private void CheckToLoadNewItems (int tempLength, int newListCount) {
			if (newListCount > tempLength) {
				for (int i = tempLength; i < newListCount; i++) {
					GameObject itemGameObj = null;

					//----------------- load item ui -----------------
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

					ResetNewLoadGameObj (itemGameObj);
					if (itemGameObj != null) {
						itemGameObj.name = itemPanelType.ToString () + i;

						itemGameObj.GetComponent<UIDynamicTxt> ().onUpateTxtCon = UpateTxtCon;
						itemGameObj.GetComponent<UIDynamicTxt> ().onUpateTxtConDy = UpateTxtCon_RefreshAll;
					}
				}
			}
		}

		private void ResetNewLoadGameObj (GameObject itemGameObj) {
			if (itemGameObj == null) {
				Debug.LogError ("====== LowoUN-UI ===> Don't forget to set list item reference with prefab name: " + itemPanelPrefab);
			} else {
				Transform itemTrans = itemGameObj.transform;
				itemTrans.SetParent (transform);

				//				InitNewLoadGameObjSize (itemTrans);
				itemTrans.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, 0, 0);
				itemTrans.localScale = Vector3.one;
				itemGameObj.layer = transform.gameObject.layer;

				gameObjList.Add (itemGameObj);
			}
		}

		private void ArrangeItems () {
			for (int i = 0; i < gameObjList.Count; i++) {
				//ArrangeItem (gameObjList[i].transform, i);
				ArrangeItem (i);
			}
		}

		float x;
		float y;
		//private void ArrangeItem (Transform itemT, int idx) {
		private void ArrangeItem (int idx) {
			Transform itemT = gameObjList[idx].transform;

			if (isV) {
				float itemHeight = itemT.GetComponent<RectTransform> ().sizeDelta.y;

				x = rtOriginSize.x / 2;
				y = -CalHeight (idx) - itemHeight / 2;
			}

			itemT.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		}

		private float CalHeight (int idx) {
			float heights = 0f;

			for (int i = 0; i < idx; i++) {
				float h = gameObjList[i].GetComponent<RectTransform> ().sizeDelta.y;
				heights += h;
			}

			return heights + vOffset * idx;
		}

		private void OnClickFromItem (int tempUIEventID, params object[] arr) {
			UpdateItemSelectedState (tempUIEventID);

			//if(onCallEvent != null)
			//	onCallEvent (currEventID, tempUIEventID, hostHolderInsID);
		}

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
			//			if (isUCenterAlign)
			//				return;

			CheckRectTransform ();

			if (isV) {
				newHeight = CalHeight (itemAmount);
				rt.sizeDelta = new Vector2 (rtOriginSize.x, newHeight);
			} else if (isU) {
				//				newWidth = (itemWidth + uOffset) * (itemAmount % rows == 0 ? Mathf.Floor (itemAmount / rows) : Mathf.Floor (itemAmount / rows) + 1) - uOffset;
				//				rt.sizeDelta = new Vector2 (newWidth, rtOriginSize.y);
			}

			if (onUpdateSize != null)
				onUpdateSize (rt);
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
			}
		}

		private void UpateTxtCon (int idx) {
			ArrangeItem (idx);
			UpdateSize (_realShowItemCount);
		}
		private void UpateTxtCon_RefreshAll (int idx) {
			ArrangeItems ();
			UpdateSize (_realShowItemCount);
		}

		private int objidOnHolder;
		public void SetObjidOnHolder (int objid) {
			objidOnHolder = objid;
		}

		private int _realShowItemCount;

		public List<int> SetItemList<T> (List<T> itemList) {
			itemList = itemList ?? new List<T> ();

			_realShowItemCount = 0;
			int _infoCount = itemList.Count;

			if (isDynamic) {
				_realShowItemCount = _infoCount;

				CheckToLoadNewItems (gameObjList.Count, _infoCount);

				//				ArrangeItems ();
				//				UpdateSize (_infoCount);
			}

			SetAllGameObjsVisible ();
			InitAllGameObjsAction ();
			SetAllGoDeselected ();
			UpdateItemsInfo (itemList);

			if (!isPosByFocus)
				CheckToSetCon (Vector2.zero);

			SetConActive ();

			return GetValidGameObjsHolderInsid ();
		}

		private void SetConActive () {
			//THINKING: set List container active state
			gameObject.SetActive (_realShowItemCount > 0);
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

		private int _curFocusID = -1;
		public void SetItemFocused (int idx) {
			_curFocusID = idx;

			UpdateItemSelectedState (idx);

			if (scrollView != null) {

				if (isDynamic) {
					if (isV) {
						CheckToSetCon (new Vector2 (0f, GetConPosByItemIdx (idx)));
					} else if (isU) {
						CheckToSetCon (new Vector2 (-GetConPosByItemIdx (idx), 0f));
					}
				}
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

		private float GetConPosByItemIdx4Static (int idx) {
			Vector2 scrollViewSize = scrollView.GetComponent<RectTransform> ().sizeDelta;

			if (idx != 0 && gameObjList.Count > 0) {
				if (isVRoll4Static) {
					//NOTEST reset static list container position
					float itemPosY = gameObjList[idx].GetComponent<RectTransform> ().anchoredPosition.y;
					float posY = -(Mathf.Abs (itemPosY) - scrollViewSize.y / 2);
					posY = Mathf.Clamp (posY, -(rt.sizeDelta.y - scrollViewSize.y / 2), 0);

					return posY;
				} else if (isURoll4Static) {
					float itemPosX = gameObjList[idx].GetComponent<RectTransform> ().anchoredPosition.x;
					float posX = -(Mathf.Abs (itemPosX) - scrollViewSize.x / 2);
					posX = Mathf.Clamp (posX, -(rt.sizeDelta.x - scrollViewSize.x / 2), 0);

					return posX;
				}
			}

			return 0f;
		}

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
				ToggleItemVisibility (gameObjList[i], true);
			}
		}

		private void ToggleItemVisibility (GameObject obj, bool isShow) {
			if (isDynamic)
				UIHub.instance.ToggleItem (obj, isShow);
		}

		private UIPanelType uiholdType;
		private int uiholdInstanceID;
		private void UpdateItemsInfo<T> (List<T> itemList) {
			if (gameObjList != null && gameObjList.Count > 0) {
				//SetAllGoDeselected ();

				for (int i = 0; i < itemList.Count && i < gameObjList.Count; i++) {
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
	}
}