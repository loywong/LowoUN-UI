using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.UI {
	public static class UIGameObject {
		//static UIGameObject() {}

		public static void ToggleAll (Dictionary<int, UIHolder> gameObjDict, bool isActive) {
			List<UIHolder> holders = new List<UIHolder> (gameObjDict.Values);
			foreach (UIHolder h in holders) {
				if (h != null && !h.isEmbed)
					ToggleItem (h.gameObject, isActive);
			}
		}

		//		public static void ToggleAll(Dictionary<int, UIHolder> gameObjDict, bool isActive)
		//        {
		//            List<int> keys = new List<int>(gameObjDict.Keys);
		//            foreach (int k in keys) {
		//                  UIHolder h = gameObjDict[k];
		//
		//					//if (h != null && !h.isListItem && !h.isSonItem && !h.isSonGeneral)
		//					if (h != null && !h.isEmbed)
		//                      ToggleItem(h.gameObject, isActive);
		//            }
		//        }

		//when this is the first show, need to play the fadein animation
		public static void ShowPanel (Dictionary<int, UIHolder> gameObjDict, int instanceID) {
			UIHolder go = null;
			if (gameObjDict.TryGetValue (instanceID, out go)) {
				if (go != null && go.gameObject != null) {
					//HACK
					go.GetBinder ().Toggle3DModel (true);
					go.ToggleDialogBg (true);
					ToggleItem (go.gameObject, true);
				}
			}
		}

		public static void HidePanel (Dictionary<int, UIHolder> gameObjDict, int instanceID) {
			UIHolder go = null;
			if (gameObjDict.TryGetValue (instanceID, out go)) {
				if (go != null && go.gameObject != null) {
					//HACK
					go.GetBinder ().Toggle3DModel (false);
					go.ToggleDialogBg (false);
					ToggleItem (go.gameObject, false);
				}
			}
		}

		//		public static void ShowPanelElement(Dictionary<int, UIHolder> gameObjDict, int uiHolderInsID, int uiElementID) {
		//			UIHolder go = null;
		//
		//			if (gameObjDict.TryGetValue (uiHolderInsID, out go )) {
		//				if (go != null)
		//					//go.ToggleElement (uiElementID, true);
		//					ToggleItem (go.gameObject, true);
		//			}
		//		}
		//
		//		public static void HidePanelElement(Dictionary<int, UIHolder> gameObjDict, int uiHolderInsID, int uiElementID) {
		//			UIHolder go = null;
		//
		//			if (gameObjDict.TryGetValue (uiHolderInsID, out go )) {
		//				if (go != null)
		//					//go.ToggleElement (uiElementID, false);
		//					ToggleItem (go.gameObject, true);
		//			}
		//		}

		//Independent functions------------------------------------------------------------------
		public static void ToggleItem (GameObject go, bool isActive) {
			if (go != null) {
				if (go.activeSelf) {
					bool hasPlayedAnim = false;

					UIStateAnimator a = go.GetComponent<UIStateAnimator> ();
					if (a != null) { // && a.isActiveAndEnabled
						hasPlayedAnim = a.Play (isActive == true ? UIStateType.Show : UIStateType.Hide, () => {
							if (!isActive && go != null) {
								//if(go.GetComponent<UIStateAnimator> ().isActiveFalseWhenHide){
								go.SetActive (false);
								if (go.GetComponent<UIHolder> () != null)
									go.GetComponent<UIHolder> ().isLogicShow = false;
								//}
							}
						});
					}

					if (!hasPlayedAnim) {
						go.SetActive (isActive);
						if (go.GetComponent<UIHolder> () != null)
							go.GetComponent<UIHolder> ().isLogicShow = isActive;
					}
				} else {
					if (isActive) {
						go.SetActive (true);

						UIStateAnimator a = go.GetComponent<UIStateAnimator> ();
						if (a != null) // && a.isActiveAndEnabled
							a.Play (UIStateType.Show);

						if (go.GetComponent<UIHolder> () != null)
							go.GetComponent<UIHolder> ().isLogicShow = true;
					} else {
						//do nothing: keep inactive state!
						if (go.GetComponent<UIHolder> () != null)
							go.GetComponent<UIHolder> ().isLogicShow = false;
					}
				}
			} else {
#if UNITY_EDITOR
				Debug.LogError ("====== LowoUN-UI ===> No game object has been find!");
#endif
			}
		}
	}
}