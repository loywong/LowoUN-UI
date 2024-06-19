#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	public class UIActionGetter : MonoBehaviour {
		[SerializeField]
		private GameObject btnObj;
		//[SerializeField]
		//private bool isCopy;

		public Transform _dragItemCon;

		private int _curIdxID;
		public int curIdxID {
			get { return _curIdxID; }
			set { _curIdxID = value; }
		}

		private int _currHolderInsID;
		public int currHolderInsID {
			get { return _currHolderInsID; }
			set { _currHolderInsID = value; }
		}

		void Awake () {
			if (btnObj == null) {
				Debug.LogError ("Don't forget to set game object to 'btnObj' reference! / game object name : " + gameObject.name);
			} else {
				//EventTriggerListener.Get(btnObj.gameObject).onClick = OnButtonClick;
				UIEventListener.Get (btnObj.gameObject).onDown = MouseDown;
				UIEventListener.Get (btnObj.gameObject).onUp = MouseUp;
				UIEventListener.Get (btnObj.gameObject).onClick = Click;

				UIEventListener.Get (btnObj.gameObject).onEnter = Enter;
				UIEventListener.Get (btnObj.gameObject).onExit = Exit;
			}
		}

		public delegate void CallEvent (int tempIdxID, params object[] arr);
		public CallEvent onCallEvent;

		//		private void OnAction() {
		//			if (onCallEvent != null) {
		//				//Debug.LogError("onCallEvent - currEventID : " + currEventID);
		//				//Debug.LogError("onCallEvent - currInstanceID : " + currInstanceID);
		//				onCallEvent (currEventID, currHolderInsID);//this.gameObject, //new int[]{objIdx}
		//			}
		//		}

		//private void OnButtonClick(GameObject go) {
		//	Debug.Log (":::::: MouseDown: " + go.name);
		//}

		//		private bool isMouseDown = false;
		private bool isStartDrag = false;
		private RectTransform dragItem = null;

		private void BeginDrag () {
			if (dragItem == null) {
				dragItem = Instantiate<GameObject> (btnObj.gameObject).GetComponent<RectTransform> ();
				dragItem.transform.SetParent (_dragItemCon);
				dragItem.anchoredPosition = Vector2.zero;
				//dragItem.GetComponent<UIActionGetter> ().enabled = false;
				dragItem.GetComponent<Image> ().raycastTarget = false;

				isStartDrag = true;
			}
		}
		private void EndDrag () {
			if (dragItem != null) {
				GameObject.Destroy (dragItem.gameObject);
				isStartDrag = false;
			}
		}

		private void MouseDown (GameObject go) {
			//if (!isMouseDown) {
			//	isMouseDown = true;
			BeginDrag ();

			//Debug.Log (":::::: MouseDown: " + go.name);
			onCallEvent (curIdxID, currHolderInsID, (int) UIActionType.PressDown);

			//}
		}

		private void MouseUp (GameObject go) {
			//			if (isMouseDown) {
			//				isMouseDown = false;
			EndDrag ();

			//Debug.Log (":::::: MouseUp: " + go.name);
			onCallEvent (curIdxID, currHolderInsID, (int) UIActionType.PointerUp);
			//			}
			//			else if (isEnter) {
			//				isEnter = false;
			//				onCallEvent (curIdxID, currHolderInsID, (int)UIActionType.PointerUp);
			//			}
		}

		private void Click (GameObject go) {
			onCallEvent (curIdxID, currHolderInsID, (int) UIActionType.Click);
		}

		//private bool isEnter = false;
		private void Enter (GameObject go) {
			//isEnter = true;

			Debug.Log (":::::: Enter: " + go.name);
			onCallEvent (curIdxID, currHolderInsID, (int) UIActionType.MoveEnter);
		}
		private void Exit (GameObject go) {
			//isEnter = false;

			//Debug.Log (":::::: Enter: " + go.name);
			onCallEvent (curIdxID, currHolderInsID, (int) UIActionType.MoveExit);
		}

		Vector3 mousePos;
		// Update is called once per frame
		void Update () {
			if (isStartDrag) {
				if (_dragItemCon != null) {
					mousePos = Input.mousePosition;
					_dragItemCon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (mousePos.x, mousePos.y);
				}
			}
		}
	}
}