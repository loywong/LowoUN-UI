using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	[RequireComponent (typeof (InputField))]
	public class UIIpt : UIActionBase {
		public enum Ipt_EvtTyp {
			ValueChange,
			EndEdit,
		}

		// Use this for initialization
		void Awake () {
			GetComponent<InputField> ().onValueChanged.AddListener (delegate (string str) { OnValueChanged (GetComponent<InputField> ().text); });
			GetComponent<InputField> ().onEndEdit.AddListener (delegate (string str) { OnEndEdit (GetComponent<InputField> ().text); });
		}

		private void OnValueChanged (string stringValue) {
			//Debug.Log("UIInputField, ValueChange: " + stringValue);
			if (onCallEvent != null)
				onCallEvent (curEventID, stringValue, (int) Ipt_EvtTyp.ValueChange, hostHolderInsID);
		}

		private void OnEndEdit (string stringValue) {
			Debug.Log ("UIInputField, EndEdit: " + stringValue);
			if (onCallEvent != null)
				onCallEvent (curEventID, stringValue, (int) Ipt_EvtTyp.EndEdit, hostHolderInsID);
		}

		// Update is called once per frame
		void Update () {

		}

		public void SetValue (string stringValue) {
			gameObject.GetComponent<InputField> ().text = stringValue;
		}

		public void SetLimit (int limit) {
			gameObject.GetComponent<InputField> ().characterLimit = limit;
		}

	}
}