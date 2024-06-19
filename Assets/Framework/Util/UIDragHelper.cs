using UnityEngine;
using UnityEngine.EventSystems;

namespace LowoUN.Module.UI {
    public class UIDragHelper : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler {
        private Vector2 curDragPos = new Vector2 (-1f, -1f);
        private Vector2 newDragPos;
        // Use this for initialization
        void Start () {

        }

        // Update is called once per frame
        //void Update () {
        //          if (curDragPos == new Vector2(-1f, -1f))
        //              curDragPos = newDragPos;

        //          if (curDragPos != newDragPos) {
        //              GetComponent<RectTransform>().anchoredPosition += newDragPos - curDragPos;
        //              curDragPos = newDragPos;
        //          }
        //      }

        public void OnDrag (UnityEngine.EventSystems.PointerEventData eventData) {
            //GetComponent<RectTransform>().anchoredPosition = eventData.position;// *UIAdaptScreen.instance.GetScaleValue();
            //Debug.Log (eventData.position);
            newDragPos = eventData.position;

            if (curDragPos == new Vector2 (-1f, -1f))
                curDragPos = newDragPos;

            if (curDragPos != newDragPos) {
                Vector2 offset = newDragPos - curDragPos;
                offset *= UIAdaptScreen.instance.GetScaleValue ();
                GetComponent<RectTransform> ().anchoredPosition += offset;
                curDragPos = newDragPos;
            }
        }
        public void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData) {
            //Debug.Log (eventData.position);
            curDragPos = new Vector2 (-1f, -1f);
        }
        public void OnDrop (UnityEngine.EventSystems.PointerEventData eventData) {
            //Debug.Log (eventData.position);
        }
    }
}