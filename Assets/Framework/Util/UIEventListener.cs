using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//UIEventTriggerListener

namespace LowoUN.Module.UI 
{
	public class UIEventListener : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IUpdateSelectedHandler, ISelectHandler
	{
		public delegate void BoolDelegate(GameObject go, bool state);
		public delegate void FloatDelegate(GameObject go, float delta);
		public delegate void VectorDelegate(GameObject go, Vector2 delta);
		public delegate void ObjectDelegate(GameObject go, GameObject obj);
		public delegate void KeyCodeDelegate(GameObject go, KeyCode key);


		public delegate void VoidDelegate (GameObject go);
		public VoidDelegate onClick;
		public VoidDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public VoidDelegate onUp;
		public VoidDelegate onSelect;
		public VoidDelegate onUpdateSelect;

		static public UIEventListener Get (GameObject go)
		{
			UIEventListener listener = go.GetComponent<UIEventListener>();
			if (listener == null) listener = go.AddComponent<UIEventListener>();
			return listener;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if(onClick != null) onClick(gameObject);
		}
		public void OnPointerDown (PointerEventData eventData){
			if(onDown != null) onDown(gameObject);
		}
		public void OnPointerEnter (PointerEventData eventData){
			if(onEnter != null) onEnter(gameObject);
		}
		public void OnPointerExit (PointerEventData eventData){
			if(onExit != null) onExit(gameObject);
		}
		public void OnPointerUp (PointerEventData eventData){
			if(onUp != null) onUp(gameObject);
		}
		public void OnSelect (BaseEventData eventData){
			if(onSelect != null) onSelect(gameObject);
		}
		public void OnUpdateSelected (BaseEventData eventData){
			if(onUpdateSelect != null) onUpdateSelect(gameObject);
		}
	}
}