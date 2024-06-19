using System.Collections.Generic;
using LowoUN.Business.UI;
using LowoUN.Util;

namespace LowoUN.Module.UI {
	public sealed class UIBrowsing {
		private int 							_curUIClass;
		private Stack<int> 						_uiClassStack;
		private Dictionary<int, List<UIHolder>> _uiBrowseData;

		public UIBrowsing () {
			_curUIClass = -1;
			_uiClassStack = new Stack<int> ();
			_uiBrowseData = new Dictionary<int, List<UIHolder>> ();
		}

		private bool CheckSpecialPanel4Browsing (UIPanelType panelType) {
			return UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Test ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Module ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Global ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Basic ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Notify ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Award ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.Dlg ||
				UILinker.instance.GetPrefabClass (panelType) == UIPanelClass.ComnInfo;
		}

		public void Backup4BrowseUI (UIHolder uiholder, bool isLoad) {
			//[start] handle those needn't recode holders---------------
			if (uiholder == null)
				return;
			if (CheckSpecialPanel4Browsing (uiholder.typeID))
				return;
			if (uiholder.isEmbed)
				return;
			if (UIScene.instance.IsSceneDefaultUI ((int) uiholder.typeID))
				return;
			//[start] end-----------------------------------------------

			int uiClass = (int) UILinker.instance.GetPrefabClass (uiholder.typeID);

			if (isLoad) {
				if (uiClass != _curUIClass) {
					LeavePrevUIClass (_curUIClass);

					_curUIClass = uiClass;
					_uiClassStack.Push (uiClass);
					_uiBrowseData[uiClass] = new List<UIHolder> ();
					_uiBrowseData[uiClass].Add (uiholder);
				} else {
					if (!_uiBrowseData.ContainsKey (_curUIClass))
						_uiBrowseData[_curUIClass] = new List<UIHolder> ();

					_uiBrowseData[_curUIClass].Add (uiholder);
				}
			} else {
				if (_uiBrowseData.ContainsKey (uiClass) && _uiBrowseData[uiClass] != null) {
					if (_uiBrowseData[uiClass].Count >= 1)
						_uiBrowseData[uiClass].Remove (uiholder);

					if (_uiBrowseData[uiClass].Count == 0) {
						//Clear current class layer
						_uiBrowseData.Remove (uiClass);
						_uiClassStack.Pop ();
						_curUIClass = -1;

						Recover4BrowseUI (uiClass);
					}
				} else {
					//UnityEngine.Debug.LogWarning ("no ui class data for current panel type : " + uiholder.typeID.ToString());
				}
			}
		}

		private void Recover4BrowseUI (int oldUIClass) {
			//			if (_uiClassStack.Contains (oldUIClass)) {
			//				_uiClassStack.Pop ();
			//
			//				if (_uiClassStack.Count > 0) {
			//					BackToPrevUIClass (_uiClassStack.Peek ());
			//				}
			//			}

			if (_uiClassStack.Count > 0)
				BackToPrevUIClass (_uiClassStack.Peek ());
		}

		private void LeavePrevUIClass (int uiClass) {
			if (_uiBrowseData.ContainsKey (uiClass) && _uiBrowseData[uiClass] != null) {
				foreach (var item in _uiBrowseData[uiClass]) {
					UIHub.instance.HidePanel (item.insID);
				}
			}
		}

		private void BackToPrevUIClass (int uiClass) {
			_curUIClass = uiClass;

			if (_uiBrowseData.ContainsKey (uiClass) && _uiBrowseData[uiClass] != null) {
				foreach (var item in _uiBrowseData[uiClass]) {
					UIHub.instance.ShowPanel (item.insID);
				}
			}
		}

		public void OnReset () {
			_curUIClass = -1; //UIPanelClass.None;
			_uiBrowseData.Clear ();
			_uiClassStack.Clear ();
		}

		/// <summary>
		/// Rollbacks to the previous class.
		/// </summary>
		public void RollbackPrevClass (int clas) {

		}
		/// <summary>
		/// Rollbacks to the target class.
		/// </summary>
		public void Rollback2Class (int clas) {

		}
		/// <summary>
		/// Rollbacks to the target panel type.
		/// </summary>
		public void Rollback2Typ (int Typ) {
			int uiClass = UILinker.instance.GetPrefabClass (Typ);

			if (_uiClassStack.Peek () != uiClass) {
				List<UIHolder> TypIDs = _uiBrowseData[uiClass];
				int idx = TypIDs.FindIndex (i => (int) i.typeID == Typ);
				if (idx != -1) {
					List<UIHolder> collectTypIDs = TypIDs.GetRange (idx + 1, TypIDs.Count - 1 - idx);
					collectTypIDs.Reverse ();
					collectTypIDs.ForEach (i => UIHub.instance.CloseUI (i.insID));
				}

				_uiClassStack.Pop ();
				Rollback2Typ (Typ);
			}
		}

		public void RollbackAll () {
			if (_uiClassStack.Count > 0) {
				_curUIClass = _uiClassStack.Peek ();

				List<UIHolder> hs = _uiBrowseData.Val (_curUIClass);
				if (hs != null) {
					hs.Reverse ();
					hs.ForEach (i => UIHub.instance.CloseUI4PopupAll (i.insID));

					_uiClassStack.Pop ();
					_uiBrowseData.Remove (_curUIClass);

					RollbackAll ();
				} else {
					UnityEngine.Debug.LogError (Util.Log.Format.UI () + "_uiBrowseData doesn't has a panel class type key: " + _curUIClass);
				}
			} else {
				_curUIClass = -1;
			}
		}

		public bool IsTheFirstOne () {
			//return _uiBrowseData.Count == 1 && _uiBrowseData.Select(i=>i.Value).FirstOrDefault().Count == 1;
			return _uiBrowseData.Count == 0;
		}

		public bool Popup () {
			if (_uiBrowseData.Count == 0 || _uiClassStack.Count == 0)
				return false;

			//Get the toppest panel
			int _curClas = _uiClassStack.Peek ();
			List<UIHolder> hs = _uiBrowseData[_curClas];
			UIHolder h = hs[hs.Count - 1];
			if (!UIScene.instance.current.IsMainMenuDefaultPanel ((int) h.typeID)) {
				h.GetBinder ().OnBtnClose ();
				return true;
			}

			return false;
		}
	}
}