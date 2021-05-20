#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using LowoUN.Util.Notify;

namespace LowoUN.Module.UI
{
	[RequireComponent(typeof(UIGraphicCollider))]
	public class UIEventRedirect : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField]
		private string EventNameClick;
		[SerializeField]
		private string EventNameDrag;
		[SerializeField]
		private string EventNameDragEnd;


		public void SetEvtsName (string click, string drag, string dragEnd) {
			EventNameClick = click;
			EventNameDrag = drag;
			EventNameDragEnd = dragEnd;
		}

		private bool isDragged = false;

	    public void OnPointerClick(PointerEventData eventData)
	    {
			if(!isDragged)
	        	if (!string.IsNullOrEmpty(EventNameClick))
	           		NotifyMgr.Broadcast<PointerEventData>(EventNameClick, eventData);
	    }

	    public void OnDrag(PointerEventData eventData)
	    {
			isDragged = true;

	        if (!string.IsNullOrEmpty(EventNameDrag))
				NotifyMgr.Broadcast<PointerEventData>(EventNameDrag, eventData);
	    }

	    public void OnEndDrag(PointerEventData eventData)
	    {
			isDragged = false;

	        if (!string.IsNullOrEmpty(EventNameDragEnd))
				NotifyMgr.Broadcast<PointerEventData>(EventNameDragEnd, eventData);
	    }
	}
}