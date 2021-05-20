#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LowoUN.Util;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using LowoUN.Util.Notify;

namespace LowoUN.Module.UI.Com
{
	public enum Btn_LayoutTyp {
		None,
		Normal,
		Special, //such as dialog bg which has a fullscreen button
	}
	public enum Btn_EvtTyp {
		None,
		NormalClick,
		LongPress_Enter,
		LongPress_Exit,
	}

	[RequireComponent(typeof(Button))]
	public class UIButton : UIActionBase, ISelect, IName
	{
		[SerializeField]
		private GameObject btnObj;
		[SerializeField]
		private Text       btnName;
		[SerializeField]
		private Image      selectedState;//REFACTOR
		[SerializeField]
		private GameObject disabledState;

		[SerializeField]
		private bool _isLongPressEnable = false;
		[SerializeField]
		private float _longPressTime = 1f;

		[SerializeField]
		private Btn_LayoutTyp layoutTyp = Btn_LayoutTyp.Normal;


		private UIStateAnimator stateAnims;

		void SetDefaultAnimStates ()
		{
			UIStateObj stateDown = new UIStateObj () {
				animType = UIStateType.MouseDown,
				finishNotifyTime = UIAsset.instance.time_mouseDownAnim,
				group_filter = 0,
				objAnimList = new List<UIObjAnim> () {
					new UIObjAnim () {
						obj = gameObject,
						anim = UIAsset.instance.mouseDownAnim
					}
				}
			};
			UIStateObj stateUp = new UIStateObj () {
				animType = UIStateType.MouseUp,
				finishNotifyTime = UIAsset.instance.time_mouseUpAnim,
				group_filter = 0,
				objAnimList = new List<UIObjAnim> () {
					new UIObjAnim () {
						obj = gameObject,
						anim = UIAsset.instance.mouseUpAnim
					}
				}
			};

			if (stateAnims == null) {
				stateAnims = gameObject.AddComponent<UIStateAnimator> ();
				stateAnims.InitStateObjs (new List<UIStateObj> () { stateDown, stateUp });
			} else {
				stateAnims.AddStateObj (stateDown);
				stateAnims.AddStateObj (stateUp);
			}
		}

		void Awake () {
			//auto set mouse down and up animation effect
			if(layoutTyp == Btn_LayoutTyp.Normal)
				SetDefaultAnimStates ();

			if (btnObj == null) {
				Debug.LogError ("Don't forget to set 'btnObj' reference! / btn name : " + gameObject.name + "/host panel ins ID: " + hostHolderInsID);
			}
			else {
				//btnObj.GetComponent<Button> ().transition = Selectable.Transition.ColorTint;

				UIEventListener.Get(btnObj.gameObject).onDown = MouseDown;
				UIEventListener.Get(btnObj.gameObject).onExit = MouseOutside;
//				btnObj.GetComponent<Button> ().onClick.AddListener (delegate () {MouseUp ();});
				UIEventListener.Get (btnObj.gameObject).onUp = MouseUp;//MouseJustUp;

				////btnObj.GetComponent<Button> ().onClick.AddListener (delegate () {OnAction ();});
			}

			if(selectedState != null && !__isSelect){
				selectedState.gameObject.SetActive(false); 
			}
			if (disabledState != null) {
				Image img = disabledState.GetComponent<Image> ();
				Text txt = disabledState.GetComponent<Text> ();
				if (img != null)
					img.raycastTarget = false;
				if (txt != null)
					txt.raycastTarget = false;
			}
		}

		//private void OnActionWithState(int longPressState/*-1: no enter long press; 0: release form long press, 1: enter long press*/) {
		private void OnActionWithState(Btn_EvtTyp longPressState) {
			if (onCallEvent != null) {
				//Debug.LogError("onCallEvent - currEventID : " + currEventID);
				//Debug.LogError("onCallEvent - currInstanceID : " + currInstanceID);
				onCallEvent (curEventID, hostHolderInsID, (int)longPressState);//this.gameObject, //new int[]{objIdx}
			}
		}

		public void OnAction() {
//			if (isDragged)
//				return;

			if (onCallEvent != null) {
				//Debug.LogError("onCallEvent - currEventID : " + currEventID);
				//Debug.LogError("onCallEvent - currInstanceID : " + currInstanceID);

				NotifyMgr.Broadcast<int, string> ("UI_Com_Button", hostHolderInsID, btnObj.name);
//				if (AutomationRecord.is_recording) {
//					UIHolder _holder = UIHub.instance.GetHolder (hostHolderInsID);
//					string command_str = "Click " + _holder.typeID.ToString () + "," + btnObj.name;
//					if(_holder.curIdxInList != -1)
//						command_str += "," + _holder.curIdxInList.ToString();
//					//Debug.Log("Click " + (int)(_holder.typeID) + "," + currEventID + "    Click " + _holder.typeID.ToString() + "," + btnObj.name);
//					AutomationRecord.RecordCommand (command_str, "[Rec]UI :" + command_str, (int)ENUM_AUTOMATION_COMMAND.ENUM_AUTOMATION_COMMAND_COMMAND_UI_CLICK, 5f);
//				}	

				onCallEvent (curEventID, hostHolderInsID, Btn_EvtTyp.NormalClick);//this.gameObject, //new int[]{objIdx}
                
                //TODO
				//UINotifyMgr.Broadcast<string, string> ("UI_Com_Button_Sound", this.btnName, this.btnObj.name);
				//AudioManager.Instance.PlayUISE(UIFmodEventEnum.UI_FMODEVENT_TOUCH);
            }
		}


		private readonly float clickLimitDist = 5f;
		private Vector2 mouseDownPos;
		private Vector2 mouseUpPos;

		private bool isLongPress = false;
		private bool isMouseDown = false;
		private void MouseDown(GameObject go)
		{
//			if (!isEnable)
//				return;
			
			if (!isMouseDown) {
				mouseDownPos = Input.mousePosition;

				PlayAnims (UIStateType.MouseDown);
				isMouseDown = true;

				if (isEnable) {
					if (_isLongPressEnable) {
						TimeWatcher.instance.AddWatcher ("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID, (uint)(_longPressTime * 1000), false, () => {
							isLongPress = true;
							OnActionWithState (Btn_EvtTyp.LongPress_Enter);

							TimeWatcher.instance.RemoveWatcher("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID);
						});
						//StartCoroutine ("LongPressTimer");
					}
				}
			}
		}

//		IEnumerator LongPressTimer() {
//			yield return new WaitForSeconds(_longPressTime);
//			isLongPress = true;
//
//			OnActionWithState (1);
//			//onCallEvent (currEventID, hostHolderInsID, true/*enter long press state*/);
//			//isMouseDown = false;
//			//isLongPress = false;
//		}


//		private void MouseJustUp (GameObject go)
//		{
//			if (!isEnable)
//				return;
//
//			if (isMouseDown) 
//				PlayAnims (UIStateType.MouseUp);
//		}
		private void MouseUp(GameObject go)
//		private void MouseUp ()
		{
//			if (!isEnable)
//				return;
			
			if (isMouseDown) {
				isMouseDown = false;
				PlayAnims (UIStateType.MouseUp);

				if (isEnable) {
					// if dragged
					mouseUpPos = Input.mousePosition;
					float movedPos = Maths.TwoPointDistance2D (mouseDownPos ,mouseUpPos);
					if (movedPos >= clickLimitDist)
						return;

					if (_isLongPressEnable) {
						//StopCoroutine ("LongPressTimer");	
						if(TimeWatcher.instance.ContainKey("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID))
							TimeWatcher.instance.RemoveWatcher("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID);

						if (isLongPress) {
							isLongPress = false;
							OnActionWithState (Btn_EvtTyp.LongPress_Exit);
							//onCallEvent (currEventID, hostHolderInsID, false/*exit long press state*/);
						} else {
							OnAction ();
						}
					} else {
						OnAction ();
					}
				}
			}
		}

		private void MouseOutside (GameObject go) {
//			if (!isEnable)
//				return;
			
			if (isMouseDown) 
				PlayAnims (UIStateType.MouseUp);
		
			isMouseDown = false;

			if (isEnable) {
				if (_isLongPressEnable) {
					//StopCoroutine ("LongPressTimer");	
					if (TimeWatcher.instance.ContainKey ("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID))
						TimeWatcher.instance.RemoveWatcher ("UI_Compo_Button_LongPress_InsID" + hostHolderInsID + "_EvtID" + curEventID);

					if (isLongPress) {
						isLongPress = false;
						OnActionWithState (Btn_EvtTyp.LongPress_Exit);
						//onCallEvent (currEventID, hostHolderInsID, false/*exit long press state*/);
					} 
				}
			}
		}

		private bool isEnable = true;
		protected override void SetEnable (bool isEnable) {
			base.SetEnable (isEnable);

			btnObj.GetComponent<Button> ().interactable = isEnable;
			btnObj.GetComponent<Button> ().enabled = isEnable;

			if (disabledState != null) 
				disabledState.SetActive (!isEnable);

			this.isEnable = isEnable;
		}

		public override void UpdateSelectState (bool isSelect) {
			if (selectedState != null) 
				selectedState.gameObject.SetActive(isSelect);
		}
		
//		public override void SetSelectState (bool isSelect) {
//			if(__isSelect)
//			if (isSelect) {
//				if (!__isSelect) {
//					__isSelect = true;
//					if (selectedState != null) {
//						selectedState.gameObject.SetActive(true);
//					} else
//						Debug.LogWarning ("Don't forget to set a image for selected state!");
//
//					if (GetComponent<UIStateAnimator>() != null)
//						GetComponent<UIStateAnimator>().Play (UIStateType.Selected);
//				}
//			}
//			else {
//				if (__isSelect) {
//					__isSelect = false;
//					if (selectedState != null)
//						selectedState.gameObject.SetActive(false);
//					else
//						Debug.LogWarning ("Don't forget to set a image for selected state!");
//					
//					if (GetComponent<UIStateAnimator>() == null)
//						Debug.LogWarning ("no set deselect animation clip!");
//					else {
//						GetComponent<UIStateAnimator>().Play (UIStateType.Deselected);
//					}
//				}
//			}
//		}


		private void PlayAnims (UIStateType stateType) {
			UIStateAnimator a = btnObj.GetComponent<UIStateAnimator> ();
			if (a!= null)// && a.isActiveAndEnabled
				a.Play (stateType);
		}

		public void SetName (string name) {
			if (btnName != null) {
				btnName.text = name;
			} else {
				Debug.LogWarning ("====== LowoUI-UN ===> No button's name text component attached: " + btnObj.name);
			}
		}
	}
}
