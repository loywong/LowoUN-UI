#pragma warning disable 0649//ignore default value null
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LowoUN.Util;

namespace LowoUN.Module.UI.Com
{
	public class UIGroup : UIActionBase , IGroup
	{
		[SerializeField]
		private List<GameObject> _actionObjList;
		[SerializeField]
		private Transform _dragItemCon;


		private bool isSwapEnable = false;
		private bool hasInit = false;

		// Use this for initialization
		void Awake () {
			CheckInit ();
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void CheckInit ()
		{
			if (!hasInit) {
				InitAllGameObjs ();
				hasInit = true;
			}
		}

		public void SetNames (List<string> names) {
			for (int i = 0; i < _actionObjList.Count; i++) {
				_actionObjList [i].GetComponent<IName> ().SetName (names[i]);
			}
		}

		public void SetSelectIndex (int idx, params object[] arr) {
			//Debug.Log ("On Set Tab, currHolderInsID: " + currHolderInsID);
			CheckInit ();

			if (_actionObjList [idx].GetComponent<UIActionBase> () != null) {
				for (int i = 0; i < _actionObjList.Count; i++) {
					if(i != idx)
						_actionObjList [i].GetComponent<UIActionBase> ().SetSelectState (false);
				}
				_actionObjList [idx].GetComponent<UIActionBase> ().SetSelectState (true);
			}

			if (onCallEvent != null) {
				if (isSwapEnable) {
					if(CheckSwap(arr))
						onCallEvent (curEventID, idx, hostHolderInsID, (int)arr[1]);//, (int)arr[1]
					else
						onCallEvent (curEventID, idx, hostHolderInsID);
				}
				else
					onCallEvent (curEventID, idx, hostHolderInsID);
			}
		}

		private bool CheckSwap (params object[] paramArr) {
			return paramArr.Length > 1;
		}

		private void InitAllGameObjs ()
		{
			for (int i = 0; i < _actionObjList.Count; i++) {
				if (_actionObjList [i] == null) {
					Debug.LogError ("====== LowoUN-UI ===> _actionObjList is null!");
					continue;
				}
				if (_actionObjList [i].GetComponent<UIActionBase> () == null) {
					if (_actionObjList [i].GetComponent<UIActionGetter> () != null) {
						
						//------------------------------
						isSwapEnable = true;
						if (_dragItemCon == null)
							Debug.LogWarning ("====== LowoUN-UI ===> No containter for dragged item!");
						_actionObjList [i].GetComponent<UIActionGetter> ()._dragItemCon = _dragItemCon;
						//------------------------------

						_actionObjList [i].GetComponent<UIActionGetter> ().curIdxID = i;
						_actionObjList [i].GetComponent<UIActionGetter> ().currHolderInsID = hostHolderInsID;
						_actionObjList [i].GetComponent<UIActionGetter> ().onCallEvent = OnSelectTab;
					} else {
						//Debug.LogWarning ($"====== LowoUN-UI ===> Don't forget to operate callback script on ui group component with id: {currEventID}, on holder panel: {hostHolderInsID}");
						Debug.LogWarning (string.Format("====== LowoUN-UI ===> Don't forget to operate callback script on ui group component with id: {0}, on holder panel: {1}", curEventID,hostHolderInsID));
					}
				}
				else {
					isSwapEnable = false;

		            _actionObjList[i].GetComponent<UIActionBase>().curEventID = i;
					_actionObjList[i].GetComponent<UIActionBase>().hostHolderInsID = hostHolderInsID;
					_actionObjList[i].GetComponent<UIActionBase>().onCallEvent = OnSelectTab;
				}

			}
		}

		private void OnSelectTab(int tempIdxID, params object[] arr) {
			//Debug.Log ("OnSelectTab, tab_idx: " + tempIdxID);
			//Debug.Log ("====== LowoUN-UI / UIGroup ===> OnSelectTab, arr length: " + arr.Length);
			//Debug.Log ("OnSelectTab, (int)arr[0]: " + (int)arr[0]);//panel holder id
			//Debug.Log ("OnSelectTab, (int)arr[1]: " + (int)arr[1]);//???

			if (!isSwapEnable) {
				SetSelectIndex (tempIdxID, arr);

				//if(onCallEvent != null)
				//	onCallEvent (currEventID, tempUIEventID, (int)arr[0]);
			} else {
				//TODO:
				//Pair<int, int> orderInfo = UpdateSwapState((UIActionType)arr[0], (int)arr[1]);
				Pair<int, int> orderInfo = UpdateSwapState((UIActionType)arr[1], (int)arr[0]);



				Debug.Log ("OnSelectTab, (int)parameters[0]: " + orderInfo.first);
				Debug.Log ("OnSelectTab, (int)parameters[1]: " + orderInfo.second);

				if (orderInfo.first == -1 || orderInfo.second == -1) {
					if((UIActionType)arr[1] == UIActionType.Click)
						SetSelectIndex (tempIdxID, arr);
				}
				else {
					object[] parameters = new object[2];
					parameters[0] = orderInfo.first;
					parameters[1] = orderInfo.second;

					SetSelectIndex (tempIdxID, parameters);
					ClearSwapPlayerActorState ();
				}
			}
		}

		public void SetSelectIdx(int idx){
			//Debug.Log ("On Set Tab, currHolderInsID: " + currHolderInsID);
			CheckInit ();
			SetSelectState (idx);
			if (onCallEvent != null) {
				onCallEvent (curEventID, idx, hostHolderInsID);
			}
		}

		private void SetSelectState (int idx)
		{
			if (_actionObjList [idx].GetComponent<UIActionBase> () != null) {
				for (int i = 0; i < _actionObjList.Count; i++) {
					if (i != idx)
						_actionObjList [i].GetComponent<UIActionBase> ().SetSelectState (false);
				}
				_actionObjList [idx].GetComponent<UIActionBase> ().SetSelectState (true);
			}
		}


		//===============================================================================
		int originalIdx;
		int newIdx = -1;
		//public Pair<int, int> UpdateSwapState (UIActionType actionType, int curEnterID) {
		public Pair<int, int> UpdateSwapState (UIActionType actionType, int curEnterID) {
			//ClearSwapPlayerActorState ();

			UnityEngine.Debug.LogWarning ("ModifySequence action type: " + actionType);
			UnityEngine.Debug.LogWarning ("ModifySequence curEnterID: " + curEnterID);


			if (actionType == UIActionType.PressDown) {
				originalIdx = curEnterID;
				//isDown = true;

				//DragGO ();
			}
			else if (actionType == UIActionType.MoveEnter) {
				newIdx = curEnterID;

				//				if (isDown) {
				//					DragGO ();
				//				}
			}
			else if (actionType == UIActionType.MoveExit) {
				newIdx = -1;
			}
			else if (actionType == UIActionType.PointerUp) {
				if (newIdx != -1 && newIdx != originalIdx) {
					//isDown = false;
					UnityEngine.Debug.LogError("change the order:" + originalIdx + "to new : " + newIdx);

					//SwapActorsSequence(originalIdx, newIdx);
					return new Pair<int, int>(originalIdx, newIdx);
				}

				//if (hasShowDraggedItem) {
				//	DragEnd ();
				//}

				newIdx = -1;
			}

			return new Pair<int, int>(-1, -1);
		}


		private void ClearSwapPlayerActorState () {
			//isDown = false;
			originalIdx = -1;
			newIdx = -1;
			//DragEnd ();
		}


		//=====================================================================================
//		[UIActionAtt ((int)Evts_Test.Group_Test, UIPanelType.Test)]
//		public void Group_Test (params object[] arr)
//		{
//			UnityEngine.Debug.LogWarning (" ====== LowoUN-UI ===> Group_Test: " + arr.Length);
//
//			if (arr.Length >= 3) {
//				UnityEngine.Debug.LogWarning (" ====== LowoUN-UI ===> Group_Test 0: " + (int)arr[1]);
//				UnityEngine.Debug.LogWarning (" ====== LowoUN-UI ===> Group_Test 1: " + (int)arr[0]);
//				UnityEngine.Debug.LogWarning (" ====== LowoUN-UI ===> Group_Test 2: " + (int)arr[2]);
//
//				//UIHub.instance.GetBinder<UIBinderTest> ((int)arr [0]).SwapActorsSequence ((UIActionType)(int)arr [1], (int)arr [2]);
//			}
//		}
	}
}