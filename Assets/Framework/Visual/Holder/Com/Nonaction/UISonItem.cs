#pragma warning disable 0649//ignore default value null
using System;
using System.Collections.Generic;
using LowoUN.Business.UI;
using UnityEngine;

namespace LowoUN.Module.UI.Com {
	public class UISonItem : UIActionBase {
#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID; // = UIPanelClass.None;
#endif
		[SerializeField]
		private string typeIdx = UIPanelType.None.ToString ();

		[SerializeField]
		private UIPanelType itemPanelType;
		[SerializeField]
		private string itemPanelPrefab;

		//regular position for dynamic loaded items ----------------------
		[SerializeField]
		private float itemWidth = -1f;
		[SerializeField]
		private float itemHeight = -1f;

		//free position for dynamic loaded items -------------------------
		private List<GameObject> posStaticList = new List<GameObject> ();
		private List<GameObject> gameObjList = new List<GameObject> ();

		// Use this for initialization
		void Awake () {
			posStaticList.Add (gameObject);
		}

		// void Start () { }

		// // Update is called once per frame
		// void Update () {

		// }

		Transform itemTrans;
		private void CheckToLoadNewItem () {
			if (gameObjList.Count == 0) {
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

				if (itemGameObj == null) {
					Debug.LogError ("====== LowoUN-UI ===> doesn't find the prefab with name: " + itemPanelPrefab);
				} else {
					itemGameObj.name = itemPanelType.ToString ();

					itemTrans = itemGameObj.transform;
					itemTrans.SetParent (transform);

					//TODO: set Trans.localScale
					//if(_isForceUILocalScale)
					itemTrans.localScale = Vector3.one;

					if (itemWidth > 0 && itemHeight > 0) {
						itemTrans.GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemWidth, itemHeight);
					} else {
						itemWidth = itemTrans.GetComponent<RectTransform> ().sizeDelta.x;
						itemHeight = itemTrans.GetComponent<RectTransform> ().sizeDelta.y;
					}

					itemTrans.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (itemWidth / 2, -itemHeight / 2);
					itemGameObj.layer = transform.gameObject.layer;

					gameObjList.Add (itemGameObj);

					_holderInsID = itemGameObj.GetComponent<UIHolder> ().insID;

					//itemGameObj.GetComponent<UIHolder> ().hostHolderInsID = 0;
					itemGameObj.GetComponent<UIHolder> ().isSonItem = true;
				}
			}
		}

		private int _holderInsID = -1;

		public int SetItem<T> (T item) {
			if (item != null) {
				CheckToLoadNewItem ();
				UpdateItemsInfo (item);
			} else {
				Debug.LogWarning ("====== LowoUN-UI ===> son item data exception");
			}

			return _holderInsID;
		}

		private UIPanelType uiholdType;
		private int uiholdInstanceID;
		private void UpdateItemsInfo<T> (T item) {
			if (gameObjList != null && gameObjList.Count > 0) {
				if (gameObjList[0].GetComponent<UIHolder> () == null) {
					Debug.LogWarning ("====== LowoUN-UI ===> Don't forget to add UIHolder component to the list item game object.");
				} else {
					uiholdType = gameObjList[0].GetComponent<UIHolder> ().typeID;
					uiholdInstanceID = gameObjList[0].GetComponent<UIHolder> ().insID;
					UILinker.instance.SetHolderItemInfo (uiholdType, uiholdInstanceID, item);
				}
			} else {
				Debug.LogWarning ("====== LowoUN-UI ===> no gameObj for son item.");
			}
		}
	}
}