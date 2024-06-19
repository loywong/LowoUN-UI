#pragma warning disable 0649//ignore default value null
using System;
using LowoUN.Business.UI;
using UnityEngine;

/// <summary>
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!! load and unload automatically !!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
namespace LowoUN.Module.UI.Com {
	public class UISonGeneral : MonoBehaviour {
		[SerializeField]
		private GameObject _parentHolder;
#if UNITY_EDITOR
		[SerializeField]
		private UIPanelClass classID; // = UIPanelClass.None;
#endif
		[SerializeField]
		private UIPanelType _sonPanelType = UIPanelType.None;
		[SerializeField]
		private string typeIdx = UIPanelType.None.ToString ();
		[SerializeField]
		private string _sonPanelPrefab;

		private UIHolder _curPanel;

		void Awake () { }

		void Start () {
			//Debug.LogError ("ui son general component start on host: " + _parentHolder.GetComponent<UIHolder>().typeID.ToString());
			//SetLoad (true);
			SetLoad ();
		}
		//		void OnDestroy () {
		//			//Debug.LogError ("ui son general component end");
		//			SetLoad (false);
		//		}

		//		public void SetLoad (bool isLoad) {
		public void SetLoad () {
			//			if (!isLoad) {
			//				//Debug.LogError ("_sonPanel : " + _sonPanel);
			//				if (_curPanel != null) {
			//					UIHub.instance.CloseUI (_curPanel.insID);
			//					//_curPanel = null;
			//				}
			//			}
			//			else {
			if (_sonPanelType != UIPanelType.None) {
				GameObject itemGameObj = null;

				//----------------- load item ui -----------------
				if (!string.IsNullOrEmpty (_sonPanelPrefab)) {
					bool match = false;
					foreach (string name in UILinker.instance.GetPrefabNames (_sonPanelType)) {
						if (string.Equals (_sonPanelPrefab, name, StringComparison.OrdinalIgnoreCase))
							match = true;
					}
					if (match)
						itemGameObj = UILinker.instance.LoadUI4ChildHolder (_sonPanelPrefab) as GameObject; //, false, Enum_UIAsset.Son
					else
						itemGameObj = UILinker.instance.LoadUI4ChildHolder (_sonPanelType) as GameObject; //, Enum_UIAsset.Son
				} else
					itemGameObj = UILinker.instance.LoadUI4ChildHolder (_sonPanelType) as GameObject; //, Enum_UIAsset.Son
				//----------------- load item ui -----------------

				if (itemGameObj == null) {
					Debug.LogError ("====== LowoUN-UI ===> doesn't find the prefab with name: " + _sonPanelPrefab);
				} else {
					itemGameObj.transform.SetParent (transform);
					itemGameObj.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
					itemGameObj.GetComponent<RectTransform> ().sizeDelta = Vector3.zero;
					itemGameObj.GetComponent<RectTransform> ().localScale = Vector3.one;
					itemGameObj.SetActive (true);

					_curPanel = itemGameObj.GetComponent<UIHolder> ();
					if (_parentHolder != null) {
						_curPanel.parentHolderInsID = _parentHolder.GetComponent<UIHolder> ().insID;
						_parentHolder.GetComponent<UIHolder> ().GetBinder ().sonGenerals.Add (_curPanel.insID);
					} else {
						Debug.LogError ("don't forget to set host panel for _sonPanelType: " + _sonPanelType.ToString ());
					}
				}
			} else
				Debug.LogWarning ("don't forget to set children panel info");
		}
	}
	//	}
}