#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com
{
	[RequireComponent(typeof(Button))]
	public class UITogl : UIActionBase, ISelect, IName
	{

		[SerializeField]
		private bool _isSelete;//initial value
		[SerializeField]
		private GameObject _selectedGo;
		[SerializeField]
		private Text _name;
		[SerializeField]
		private GameObject _deselectedGo;
		[SerializeField]
		private GameObject _disabledGo;

		private Button _btn;
		// Use this for initialization
		void Awake () {
			_btn = GetComponent<Button> ();
			_btn.onClick.AddListener (delegate () {OnAction ();});

			__isSelect = _isSelete;
			ToggleEffect (__isSelect);
		}

		private void OnAction(){
			//Debug.Log("onCallEvent : check box");
			SetSelectState (!__isSelect);

			if(onCallEvent != null)
				onCallEvent (curEventID, __isSelect, hostHolderInsID);
		}

		public override void UpdateSelectState (bool isSelect) {
			ToggleEffect (isSelect);
		}

		private bool _isEnable = true;
		protected override void SetEnable (bool isEnable) {
			_isEnable = isEnable;
			base.SetEnable (isEnable);
			if (_disabledGo != null) {
				_disabledGo.SetActive (!isEnable);

				if (isEnable)
					ToggleEffect (__isSelect);
				else {
					__isSelect = true;
					if (_deselectedGo != null) {
						UIHub.instance.ToggleItem (_deselectedGo, false);
					}
					if (_selectedGo != null) {
						UIHub.instance.ToggleItem (_selectedGo, false);
					}
				}
			}
		}

//		public override void SetSelectState (bool isSelect) {
//			if (isSelect != _isSelete) {
//				_isSelete = isSelect;
//
//				ToggleEffect (_isSelete);
//			}
//		}

		private void ToggleEffect (bool isSelete) {
			if (_selectedGo == null) {// || _deselectedGo == null
				Debug.LogError ("====== LowoUN-UI ===> Don't forget to set seleted flag!" + "obj id: " + curEventID + "/ host holder id: " + hostHolderInsID);
			} 
			else {
				UIHub.instance.ToggleItem(_selectedGo, isSelete);
				if(_deselectedGo != null)
					UIHub.instance.ToggleItem (_deselectedGo, !isSelete);
				//_selectedGo.SetActive (isSelete);
				//_deselectedGo.SetActive (!isSelete);
			}
		}

		public void SetName (string name) {
			if (_name != null) {
				_name.text = name;
			} else {
				Debug.LogWarning ("No toggle's name component has been found!");
			}
		}
	}
}
