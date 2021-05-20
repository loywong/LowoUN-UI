#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com
{
	public class UILocker : MonoBehaviour 
	{
		[SerializeField]
		private GameObject   _con;
		[SerializeField]
		private Text         _txt;
		[SerializeField]
		private UIActionBase _lockedObj;
		//MaskableGraphic
		[SerializeField]
		private GameObject _typ_locker;
		[SerializeField]
		private GameObject _typ_clocker;
		[SerializeField]
		private GameObject _lockedTxt;

		public void SetLock (bool isLock) {
			if (isLock) {
				if (!_con.activeSelf)
					_con.SetActive (true);
				
				_lockedObj.SetStateType (UIStateType.Disable);
				if(_lockedTxt != null)
					_lockedTxt.SetActive (false);
			}
			else {
				if (_con.activeSelf)
					_con.SetActive (false);

				_lockedObj.SetStateType (UIStateType.Enable);
				if(_lockedTxt != null)
					_lockedTxt.SetActive (true);
			}
		}

		//THINKING: SetDesc for UILocker component
//		public void SetDesc (string desc) {
//			_txt.text = desc;
//		}

		public void SetLockTyp (int mask_type) {
			if(_typ_clocker != null)
				_typ_clocker.SetActive (mask_type == 1);
			if(_typ_locker != null)
				_typ_locker.SetActive (mask_type == 0);
		}
	}
}