#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using LowoUN.Business.UI;
using LowoUN.Util.Log;

namespace LowoUN.Module.UI.Com
{
	public class UILst_Mid : UIActionBase, ILst
	{
		#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID;
		#endif
		[SerializeField]
		private string       typeIdx = UIPanelType.None.ToString();

		[SerializeField]
		private UIPanelType itemPanelType;
		[SerializeField]
		private string itemPanelPrefab;

		//regular position for dynamic loaded items ----------------------
		[SerializeField]
		private bool isU;
//		[SerializeField]
		private float rows = 1f;
		[SerializeField]
		private float uOffset;
		[SerializeField]
		private bool isV;
//		[SerializeField]
		private float columns = 1f;
		[SerializeField]
		private float vOffset;
		[SerializeField]
		private float itemWidth = -1;
		[SerializeField]
		private float itemHeight = -1;

//		[SerializeField]
//		private bool isUCenterAlign = true;//!!!!!! use UICom_LstUVCenter //特例：1，为水平单行定制；2，以宽度中心为原点，两侧对其排列

		private RectTransform rt;
		private Vector2 rtOriginSize;
		private List<GameObject> gameObjList = new List<GameObject>();

		// Use this for initialization
		void Awake () {
			CheckRectTransform ();
		}

		private void CheckRectTransform ()
		{
			if (rt == null) 
				rt = GetComponent<RectTransform> ();
			
			rtOriginSize = rt.sizeDelta;
		}

		void Update () {
		}

		private void CheckToLoadNewItems (int tempLength, int newListCount) {
			if(newListCount > tempLength) {
				for (int i = tempLength; i < newListCount; i++) {
					GameObject itemGameObj = null;

					//----------------- load item ui -----------------
					//GameObject itemGameObj = UIHandler.instance.LoadUI(itemType.GetComponent<UIHolder>().typeID) as GameObject;
					//GameObject itemGameObj = UIHandler.instance.LoadUI(itemPanelType) as GameObject;
					if (!string.IsNullOrEmpty(itemPanelPrefab)) {
						bool match = false;
						foreach( string name in UILinker.instance.GetPrefabNames(itemPanelType)) {
							if (string.Equals(itemPanelPrefab, name, StringComparison.OrdinalIgnoreCase))
								match = true;
						}
						if (match)
							itemGameObj = UILinker.instance.LoadUI4ChildHolder(itemPanelPrefab) as GameObject;//, false, Enum_UIAsset.Son
						else
							itemGameObj = UILinker.instance.LoadUI4ChildHolder(itemPanelType) as GameObject;//, Enum_UIAsset.Son
					}
					else
						itemGameObj = UILinker.instance.LoadUI4ChildHolder(itemPanelType) as GameObject;//, Enum_UIAsset.Son
					//----------------- load item ui -----------------

					if(itemGameObj != null)
						itemGameObj.name = itemPanelType.ToString() + i;

					ResetNewLoadGameObj (itemGameObj);
				}
			}
		}

		private void ResetNewLoadGameObj (GameObject itemGameObj) {
			if (itemGameObj == null) {
				Debug.LogError ("====== LowoUI-UN ===> Don't forget to set list item reference with prefab name: " + itemPanelPrefab);
			}
			else {
				Transform itemTrans = itemGameObj.transform;
				itemTrans.SetParent (transform);

				InitNewLoadGameObjSize (itemTrans);
				itemTrans.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, 0, 0);
				itemTrans.localScale = Vector3.one;
				itemGameObj.layer = transform.gameObject.layer;

				gameObjList.Add (itemGameObj);
			}
		}

		private void InitNewLoadGameObjSize (Transform itemTrans)
		{
			if (itemWidth > 0f && itemHeight > 0f) {
				itemTrans.GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemWidth, itemHeight);
			}
			else {
				//#if UNITY_EDITOR
				//Debug.LogWarning ("====== LowoUI-UN ===> use the item loaded default size.");
				//#endif
				itemWidth = itemTrans.GetComponent<RectTransform> ().sizeDelta.x;
				itemHeight = itemTrans.GetComponent<RectTransform> ().sizeDelta.y;
			}
		}

		//TODO: UILstMotionTyp
		//[SerializeField]
		//private UILstMotionTyp motionTyp = UILstMotionTyp.Single;
		[SerializeField]
		private float interval4Anim = 0.2f;
		[SerializeField]
		private float delay4Anim = 0.0f;
//		[SerializeField]
//		private int group4Anim = 0;

		private UIStateAnimator stateAnimator;
		private void ArrangeItems () {
			for (int i = 0; i < gameObjList.Count; i++) {
				ArrangeItem (gameObjList[i].transform, i);
			}
		}

		private void OpenItems () {
			//////////////////////////////////////////////////
			/// play item animation by order.
			//1,set delay time
			for (int i = 0; i < gameObjList.Count; i++) {
				stateAnimator = gameObjList [i].GetComponent<UIStateAnimator> ();
				if (stateAnimator != null) {
					//stateAnimator.current_group_filter = group4Anim;
					stateAnimator.SetStateParams (UIStateType.Open, delay4Anim + interval4Anim * (float)i + Time.deltaTime/*HACK: delay for one frame time*/);

					//2,update anim state: open
					gameObjList [i].GetComponent<UIHolder> ().OnOpen ();
				}
			}
		}


		float x;
		float y;
		private void ArrangeItem (Transform itemT, int idx) {
			if (isU) {
//				if (isUCenterAlign) {
					float w = rt.sizeDelta.x;
					if (_realShowItemCount % 2 == 0) {
						//rt.sizeDelta
						if(idx <_realShowItemCount / 2)
							x = w/2 - (itemWidth/2 + (_realShowItemCount/2 - idx - 1)* itemWidth) - uOffset * (_realShowItemCount / 2 - idx);
						else
							x = w/2 + (itemWidth/2 + (idx - _realShowItemCount/2)* itemWidth) + uOffset * (idx - _realShowItemCount / 2);
					}
					else{
						if((float)idx < (float)_realShowItemCount / 2f)
							x = w/2 - (_realShowItemCount/2 - idx)* itemWidth - uOffset * (_realShowItemCount / 2 - idx);
						else
							x = w/2 + (idx - _realShowItemCount/2)* itemWidth + uOffset * (idx - _realShowItemCount / 2);
					}
//				}
//				else {
//					//x = offsetEdge.x + (itemWidth + uOffset) * Mathf.Floor (idx / rows) + itemWidth / 2;
//					x = (itemWidth + uOffset) * Mathf.Floor (idx / rows) + itemWidth / 2;
//				}

				float spaceY = (rtOriginSize.y - itemHeight * rows)/(rows + 1);
				y = -(spaceY * (idx % rows + 1) + (idx % rows) * itemHeight + itemHeight / 2);// + rtOriginSize.y / 2;
			} 
			else if (isV) {
				float spaceX = (rtOriginSize.x - itemWidth * columns) / (columns + 1);
				x = spaceX * (idx % columns + 1) + (idx % columns) * itemWidth + itemWidth / 2;// - rtOriginSize.x / 2;
				//y = -offsetEdge.y - (itemHeight+vOffset) * Mathf.Floor (idx / columns) - itemHeight / 2;
				y = - (itemHeight+vOffset) * Mathf.Floor (idx / columns) - itemHeight / 2;
			}

			itemT.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		}

		private void OnClickFromItem(int tempUIEventID, params object[] arr) {
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
			idxIdInList = Mathf.Min (idxIdInList, _realShowItemCount-1);
			if(idxIdInList >= 0 && idxIdInList < gameObjList.Count)
			{
				UIStateVisual v = gameObjList [idxIdInList].GetComponent<UIStateVisual> ();
				if(v != null)// && v.isActiveAndEnabled
					v.SetState(UIStateType.Selected);
			}
		}
		private void SetAllGoDeselected(){
			if (_realShowItemCount > 0) {
				for (int i = 0; i < Mathf.Min(_realShowItemCount,gameObjList.Count); i++) {
					UIStateVisual v = gameObjList [i].GetComponent<UIStateVisual> ();
					if(v != null)// && v.isActiveAndEnabled
						v.SetState(UIStateType.Deselected);
				}
			}
		}

		private void InitAllGameObjsAction ()
		{
			//for (int i = 0; i < gameObjList.Count; i++) {
			for (int i = 0; i < Mathf.Min(_realShowItemCount,gameObjList.Count); i++) {
				if (gameObjList [i].GetComponent<UIHolder> () != null) {
					gameObjList [i].GetComponent<UIHolder> ().curIdxInList = i;
					gameObjList [i].GetComponent<UIHolder> ().hostLstObjid = objidOnHolder;
					gameObjList [i].GetComponent<UIHolder> ().parentHolderInsID = hostHolderInsID;
					gameObjList[i].GetComponent<UIHolder>().onCallEvent4List = OnClickFromItem;
				}
				else {
					#if UNITY_EDITOR
					Debug.LogWarning ("====== LowoUI-UN ===> Dont forget add UIHolder for list item!!! on holder with ID: " + hostHolderInsID);
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

		private int objidOnHolder;
		public void SetObjidOnHolder (int objid) {
			objidOnHolder = objid;
		}

		public void SetItemFocused (int idx) {
		}

		private int _realShowItemCount;

		public List<int> SetItemList<T> (List<T> itemList) {
			itemList = itemList ?? new List<T> ();

			_realShowItemCount = 0;
			int _infoCount = itemList.Count;

			//if (isDynamic) {
			_realShowItemCount = _infoCount;

			CheckToLoadNewItems(gameObjList.Count, _infoCount);
			//UpdateSize (_infoCount);
			ArrangeItems ();
			OpenItems ();

			SetAllGameObjsVisible();
			InitAllGameObjsAction ();
			SetAllGoDeselected ();
			UpdateItemsInfo(itemList);

			gameObject.SetActive (_realShowItemCount > 0);

			return GetValidGameObjsHolderInsid ();
		}

		private List<int> GetValidGameObjsHolderInsid () {
			List<int> lstIds = null;

			if (_realShowItemCount > 0) {
				lstIds = new List<int> ();
				for (int i = 0; i < Mathf.Min(_realShowItemCount,gameObjList.Count); i++) {
					lstIds.Add (gameObjList[i].GetComponent<UIHolder>().insID);
				}
			}

			return lstIds;
		}

		private void SetAllGameObjsVisible () {
			foreach (GameObject obj in gameObjList) {
				ToggleItemVisibility (obj, false);
				UIFunc_Lst.ResetItem (obj.GetComponent<UIHolder>());
			}

			for (int i = 0; i < Mathf.Min(_realShowItemCount,gameObjList.Count) && i < gameObjList.Count; i++) {
				//ToggleItemAction (gameObjList [i], true);
				ToggleItemVisibility (gameObjList [i], true);
			}
		}

		private void ToggleItemVisibility (GameObject obj, bool isShow)
		{
			UIHub.instance.ToggleItem (obj, isShow);
		}

		private UIPanelType uiholdType;
		private int uiholdInstanceID;
		private void UpdateItemsInfo<T> (List<T> itemList) {
			if (gameObjList != null && gameObjList.Count > 0) {
				//SetAllGoDeselected ();

				for (int i = 0; i < itemList.Count && i < gameObjList.Count; i++) {
					if (gameObjList [i].GetComponent<UIHolder> () == null) {
						Debug.LogWarning ("====== LowoUI-UN ===> Don't forget to add UIHolder to the list component on the panel with holder id: " + hostHolderInsID);
					} else {
						uiholdType = gameObjList [i].GetComponent<UIHolder> ().typeID;
						uiholdInstanceID = gameObjList [i].GetComponent<UIHolder> ().insID;
						UILinker.instance.SetHolderItemInfo (uiholdType, uiholdInstanceID, itemList[i]);
					}
				}
			}
		}
	}
}