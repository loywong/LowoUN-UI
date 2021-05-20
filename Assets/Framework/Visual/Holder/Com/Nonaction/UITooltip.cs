#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LowoUN.Module.UI.Com
{
	public class UITooltip : UIActionBase {
		[SerializeField]
		private GameObject obj;

		void Awake () {
			if (obj == null) {
				Debug.LogError ("Don't forget to set game object to 'btnObj' reference! / game object name : " + gameObject.name);
			} else {
				UIEventListener.Get(obj.gameObject).onDown = MouseDown;
				UIEventListener.Get(obj.gameObject).onUp = MouseUp;
			}
		}

		private void OnAction(bool isDown) {
			if (onCallEvent != null) {
				//Debug.LogError("onCallEvent - currEventID : " + currEventID);
				//Debug.LogError("onCallEvent - currInstanceID : " + currInstanceID);
				onCallEvent (curEventID, hostHolderInsID, isDown, obj.gameObject);//this.gameObject, //new int[]{objIdx}
			}
		}

		private void MouseDown(GameObject go)
		{
			SetStateType (UIStateType.MouseDown);
			OnAction (true);
		}

		private void MouseUp(GameObject go)
		{
			SetStateType (UIStateType.MouseUp);
			OnAction (false);
		}

		protected override void SetEnable (bool isEnable) {
			base.SetEnable (isEnable);
			obj.GetComponent<Button> ().enabled = isEnable;
		}

		// Update is called once per frame
		void Update () {
			//TODO [LowoUI-UN]: timer for short/long press operation
		}
	}
}
