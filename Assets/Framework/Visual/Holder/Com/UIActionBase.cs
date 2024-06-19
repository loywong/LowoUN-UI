using System;
using UnityEngine;

namespace LowoUN.Module.UI.Com {
	public enum UIActionType {
		None,
		PressDown,
		MoveEnter,
		MoveExit,
		PointerUp,
		Click,
	}
	//	public enum Base_EvtTyp {
	//		OnSubmit,
	//		OnClick,
	//		OnDoubleClick,
	//		OnHover,
	//		OnPress,
	//		OnSelect,
	//		OnScroll,
	//		OnInput,
	//		OnPreClick,
	//	}

	public class UIActionBase : MonoBehaviour {

		private int _curEventID;
		public int curEventID {
			get { return _curEventID; }
			set { _curEventID = value; }
		}

		private int _currHolderInsID;
		public int hostHolderInsID {
			get { return _currHolderInsID; }
			set { _currHolderInsID = value; }
		}

		//        private int _currIdxInList = -1;
		//        public int currIdxInList
		//        {
		//            get { return _currIdxInList; }
		//            set
		//            {
		//                //Debug.LogError ("holder name : " + gameObject.name + "/ idx in list : " + value);
		//                _currIdxInList = value;
		//            }
		//        }

		public delegate void CallEvent (int tempUIEventID, params object[] arr); //GameObject gameObj, 
		public CallEvent onCallEvent;

		protected bool __isSelect = false;
		public void SetSelectState (bool isSelect) {
			if (isSelect) {
				if (!__isSelect) {
					__isSelect = true;

					UIStateAnimator a = GetComponent<UIStateAnimator> ();
					if (a != null) // && a.isActiveAndEnabled
						a.Play (UIStateType.Selected);

					UIStateVisual v = GetComponent<UIStateVisual> ();
					if (v != null) // && v.isActiveAndEnabled
						v.SetState (UIStateType.Selected);

					UpdateSelectState (true);
				}
			} else {
				if (__isSelect) {
					__isSelect = false;

					UIStateAnimator a = GetComponent<UIStateAnimator> ();
					if (a != null) // && a.isActiveAndEnabled
						a.Play (UIStateType.Deselected);

					UIStateVisual v = GetComponent<UIStateVisual> ();
					if (v != null) // && v.isActiveAndEnabled
						v.SetState (UIStateType.Deselected);

					UpdateSelectState (false);
				}
			}
		}

		public virtual void UpdateSelectState (bool isSelect) { }

		protected Action<GameObject> onSubmit;
		protected Action<GameObject> onClick;
		protected Action<GameObject> onDoubleClick;
		protected Action<GameObject, bool> onHover;
		protected Action<GameObject, bool> onPress;
		protected Action<GameObject, bool> onSelect;
		protected Action<GameObject, float> onScroll;
		//protected Action<GameObject, Vector2> onDrag;
		// Action<GameObject, GameObject> onDrop;//dragged object
		protected Action<GameObject, string> onInput;

		protected Action<GameObject> onPreClick;

		[HideInInspector]
		public bool PreClickValid;

		public virtual void OnSubmit () { if (onSubmit != null) onSubmit (gameObject); }
		public virtual void OnClick () {
			PreClickValid = true;
			if (onPreClick != null) {
				onPreClick (gameObject);
			}
			if (!PreClickValid) {
				return;
			}
			if (onClick != null) onClick (gameObject);
		}
		public virtual void OnDoubleClick () { if (onDoubleClick != null) onDoubleClick (gameObject); }
		public virtual void OnHover (bool isOver) { if (onHover != null) onHover (gameObject, isOver); }
		public virtual void OnPress (bool isPressed) { if (onPress != null) onPress (gameObject, isPressed); }
		public virtual void OnSelect (bool selected) { if (onSelect != null) onSelect (gameObject, selected); }
		public virtual void OnScroll (float delta) { if (onScroll != null) onScroll (gameObject, delta); }
		//public virtual void OnDrag(Vector2 delta) { if (onDrag != null) onDrag(gameObject, delta); }
		//public virtual void OnDrop(GameObject go) { if (onDrop != null) onDrop(gameObject, go); }
		//public virtual void OnInput(string text) { if (onInput != null) onInput(gameObject, text); }
		public virtual void OnInputText (string text) { if (onInput != null) onInput (gameObject, text); }

		public virtual void AddPreResponder (Action<GameObject> del) {
			onPreClick += del;
		}

		public virtual void RemovePreResponder (Action<GameObject> del) {
			onPreClick -= del;
		}

		public virtual void AddSubmitDelegate (Action<GameObject> del) {
			onSubmit += del;
		}
		public virtual void RemoveSubmitDelegate (Action<GameObject> del) {
			onSubmit -= del;
		}

		/// <summary>
		/// Adds a method to be called when the value of a control changes (such as a checkbox changing from false to true, or a slider being moved).
		/// </summary>
		public virtual void AddClickDelegate (Action<GameObject> del) {
			onClick += del;
		}

		/// <summary>
		/// Removes a method added with AddClickDelegate().
		/// </summary>
		public virtual void RemoveClickDelegate (Action<GameObject> del) {
			onClick -= del;
		}

		public virtual void AddHoverDelegate (Action<GameObject, bool> del) {
			onHover += del;
		}

		/// <summary>
		/// Removes a method added with AddHoverDelegate().
		/// </summary>
		public virtual void RemoveHoverDelegate (Action<GameObject, bool> del) {
			onHover -= del;
		}

		public virtual void AddPressDelegate (Action<GameObject, bool> del) {
			onPress += del;
		}

		/// <summary>
		/// Removes a method added with AddPressDelegate().
		/// </summary>
		public virtual void RemovePressDelegate (Action<GameObject, bool> del) {
			onPress -= del;
		}

		public void SetStateType (UIStateType stateType) {
			if (stateType == UIStateType.Enable)
				SetEnable (true);
			else if (stateType == UIStateType.Disable)
				SetEnable (false);

			UIStateAnimator a = GetComponent<UIStateAnimator> ();
			if (a != null) // && a.isActiveAndEnabled
				a.Play (stateType);

		}

		protected virtual void SetEnable (bool isEnable) { }
	}
}