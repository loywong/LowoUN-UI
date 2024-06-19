using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LowoUN.Module.UI {
    public abstract class UIBinder {
		protected Action<int>                       onPlayEfx; //Efx_Particle // particular for efx object to just play
		//protected Action<int, string>             onPlayAnim;//Efx_Anim     // just play anim effect object ------> by using custom UIStateType 1,2,3 

        protected Action<int, UIStateType>          onUpdateState;
		protected Action<int, bool>                 onUpdateState_ShowOrHide;
        protected Action<int, UIEventType>          onUpdateEvent;
		protected Action<int, Vector2>              onUpdatePos;
		protected Action<int, Vector2>              onUpdateSize;
		protected Action<int, string>               onUpdateTxt; //----------------- To integrate
		protected Action<int, string>               onUpdateName; //---------------- To integrate
		protected Action<int, List<string>>         onUpdateGroupNames; //---------- To integrate
		protected Action<int, TextAnchor>           onUpdateTxtAlign;
		protected Action<int, string>               onUpdateImg;//net image surpport
		protected Func<int, string, GameObject>     onUpdateWebView;//, int
		protected Action<int, ValueType, ValueType> onUpdateProg;
        protected Action<int, float>                onUpdateSlider;
        protected Action<int, int,int>              onUpdateSliderMaxAndMinValue;
        protected Action<int, bool>                 onUpdateTogl;
		protected Action<int, int>                  onUpdateGroupIdx;
		protected Action<int, string>               onUpdateIptInitStr;
		protected Action<int, int>                  onUpdateIptLimit;
		protected Action<int, Color>                onUpdateColor;
		protected Action<int, string>               onUpdateEmoji;

		protected Action<int, int>                  onUpdateLstFocus;//for lst self
        protected Action<int, Vector2>              onUpdateLstPos;
        protected Action<GameObject, int>           onSetParent;

		//protected Action<int, int, int, bool>       onUpdateLstFold;//for lst's member /*1,host holder insid; 2,host lst com obj id on whose'holder 3,cur item idx in list 4,fold state*/

        //HACK:
//		protected void                              onUpdateLst<T>(int objID, List<T> listInfo) { _lstItems.Add (UILinker.instance.UpdateList (insID, objID, listInfo));}
//		protected void                              onUpdatelstInfi<T>(int objID, List<T> listInfo) { UILinker.instance.UpdateListInfi (insID, objID, listInfo);}
        protected List<int>/*ins ids*/              onUpdateLst<T>(int objID, List<T> listInfo) {
			var ids = UILinker.instance.UpdateList (insID, objID, listInfo);
			if (ids != null) lstItems["Ins"+insID+"Obj"+objID] = ids;
			return ids;
		}
        protected void                              onUpdateLoopRoll<T>(int objID, List<T> listInfo) { UILinker.instance.UpdateLoopRoll(insID, objID, listInfo); }
        protected void                              onUpdateSonItem<T>(int objID, T itemInfo) {
			int holderID = UILinker.instance.LoadSonItem (insID, objID, itemInfo);
			if(holderID != -1) sonItems.Add (holderID);
		}
//		private void                                onUnloadSonItem(int objID) { UILinker.instance.UnloadSonItem (insID, objID);}
//		protected void                              onLoadSonGeneral(int objID){UILinker.instance.LoadSonGeneral(insID, objID);}
		
		protected Func<int, object, string, GameObject> onUpdateRImgByT;
		protected Func<int, int, string, GameObject>    onUpdateRImgByID;
//		protected Func<int, int, IUI3DModel>            updateRImg;

		protected int typID{ get; private set;}
		protected int insID{ get; private set;}
		protected int curIdxInList { get { return UIHub.instance.GetHolder (insID).curIdxInList; } set { ;} }
		protected int hostLstObjid { get{ return UIHub.instance.GetHolder (insID).hostLstObjid; } set { ;} }
		protected int hostHolderInsID { get{ return UIHub.instance.GetHolder (insID).parentHolderInsID; } set { ;} }

		private bool hasInit;
		//private bool hasClose;

		private Dictionary<string, List<int>>     lstItems = new Dictionary<string, List<int>>();
		private List<int>                         sonItems = new List<int>();
		private List<int>                         _sonGenerals = new List<int> ();
		public List<int>                          sonGenerals{ set{ _sonGenerals = value;} get{ return _sonGenerals;}}
		private List<IUI3DModel>                  model3Ds = new List<IUI3DModel>();

		public UIBinder (int typID, int insID) {
			this.typID = typID;
			this.insID = insID;

			hasInit = false;
			//hasClose = false;

			if (!hasInit) {
				hasInit = true;
				InitEvents ();
			}
		}

		private void InitEvents () {
			onUpdateState_ShowOrHide = (objID, isShowOrHide) => {
				if(isShowOrHide) UILinker.instance.UpdateState (insID, objID, UIStateType.Show);
				else             UILinker.instance.UpdateState (insID, objID, UIStateType.Hide);
			};

			onPlayEfx          				= (objID) => UILinker.instance.PlayEfx(insID, objID);
			onUpdateState      				= (objID, stateAnim) => UILinker.instance.UpdateState(insID, objID, stateAnim);
//			onUpdateEvent       			= (objID, evtAnim) => UILinker.instance.UpdateEvent(insID, objID, evtAnim);
			onUpdatePos        				= (objID, pos) => UILinker.instance.UpdatePos(insID, objID, pos);
			onUpdateSize       				= (objID, size) => UILinker.instance.UpdateSize(insID, objID, size);
			onUpdateTxt        				= (objID, info) => UILinker.instance.UpdateTxt(insID, objID, info);
			onUpdateTxtAlign   				= (objID, align) => UILinker.instance.UpdateTextAlign(insID, objID, align);
			onUpdateImg        				= (objID, name) => UILinker.instance.UpdateImg(insID, objID, name);
			onUpdateWebView    				= (objID, url) => UILinker.instance.UpdateWebView(insID, objID, url);//, dataID , dataID
			onUpdateColor      				= (objID, newColor) => UILinker.instance.UpdateColor(insID, objID, newColor);
			onUpdateProg   					= (objID, curValue, maxValue) => UILinker.instance.UpdateProg(insID, objID, curValue, maxValue);
			onUpdateSlider     				= (objID, percent) => UILinker.instance.UpdateSlider(insID, objID, percent);
            onUpdateSliderMaxAndMinValue 	= (objID, Max,Min) => UILinker.instance.UpdateSliderMaxAndMinValue(insID, objID, Max,Min);
            onUpdateTogl     				= (objID, isTriggle) => UILinker.instance.UpdateTogl(insID, objID, isTriggle);
			onUpdateGroupIdx   				= (objID, selectIdx) => UILinker.instance.UpdateGroupIdx(insID, objID, selectIdx);
			onUpdateGroupNames 				= (objID, selectIdx) => UILinker.instance.UpdateGroupNames(insID, objID, selectIdx);
			onUpdateName       				= (objID, stringValue) => UILinker.instance.UpdateName(insID, objID, stringValue);
			onUpdateIptInitStr 				= (objID, stringValue) => UILinker.instance.UpdateIptInitStr(insID, objID, stringValue);
			onUpdateIptLimit   				= (objID, limit) => UILinker.instance.SetIptLimit(insID, objID, limit);
			onUpdateLstFocus   				= (objID, isLoad) => UILinker.instance.UpdateLstFocus(insID, objID, isLoad);
			//onUpdateLstFold    			= (holdinsid, lstobjID, itemidx, isFold) => UILinker.instance.UpdateLstFold(holdinsid, lstobjID, itemidx, isFold);

			//especially for Cutscene ui
			onUpdateEmoji 					= (objID, emojiTexture) => UILinker.instance.UpdateEmoji(insID, objID, emojiTexture);

			//especially for PVE Map
            onUpdateLstPos     				= (objID, vet2) => UILinker.instance.UpdateLstPos(insID, objID, vet2);
            onSetParent        				= (obj, parentObjId)=> UILinker.instance.SetParent(insID,obj,parentObjId);

            
            onUpdateRImgByT = (objID, data, assetName) => {
				GameObject go = UILinker.instance.UpdateRImgByT(insID, objID, data, assetName);
				if(go != null) 
					model3Ds.Add(go.GetComponent<IUI3DModel>());
				
				return go;
			};
			onUpdateRImgByID = (objID, dataid, assetName) => {
				GameObject go = UILinker.instance.UpdateRImgByID(insID, objID, dataid, assetName);
				if(go != null) 
					model3Ds.Add(go.GetComponent<IUI3DModel>());
				
				return go;
			};
        }

		public virtual void OnAwake (){}
		public virtual void OnStart (){}
		public virtual void OnStart (object info){}
		public virtual void OnOpen () {}
		public virtual void OnBtnClose (){}//for closing by fullscreen bg.
		public void OnEndBinder () {
			OnEndSon ();
			OnEnd ();
			//reset all variables
		}
		protected abstract void OnEnd ();//should reset all variables
		private void OnEndSon (){
			//1, unload lists' items
			//TODO [LowoUN-UI]: clear list items' id when holder on end.
			foreach (var insIDs in lstItems.Values) {
				if(insIDs != null && insIDs.Count > 0)
					insIDs.ForEach(i => UIHub.instance.CloseUI (i));
			}
			lstItems.Clear();

			//2, unload son item holders
			sonItems.ForEach(i => UIHub.instance.CloseUI (i));
			sonItems.Clear();

			//3, sonGenerals
			sonGenerals.ForEach(i => UIHub.instance.CloseUI (i));
			sonGenerals.Clear();

            //4, unload ui 3d model
            model3Ds.ForEach(i => {i.OnEnd();});
			model3Ds.Clear();

			//5, something else by override
		}
		public virtual void OnUpdate () {
			if (isTestModeEnabled) {
				curTimer += Time.fixedDeltaTime;
				if (curTimer >= intervalTime) {
					curTimer = 0;

					OnTest ();
				}
			}
		}

		/// <summary>
		/// ���ر�����Լ�Ƕ�׵�lstItem��sonItem��sonGeneral3����������������?model.
		/// </summary>
		public void Toggle3DModel (bool isShoworHide) {
			model3Ds.ForEach (i=>{if(i != null){i.SetActive(isShoworHide);}});
			//list item
			//lstItems.Select(i=>i.Value).ToList().ForEach(i=>i.ForEach(j=>UIHub.instance.GetHolder(j).GetBinder().Toggle3DModel(isShoworHide)));
			lstItems.Select(i=>i.Value).ToList().ForEach(i=>i.ForEach(j=>{
				UIHolder h = UIHub.instance.GetHolder(j);
				if(h!= null){
					UIBinder b = h.GetBinder();
					if(b != null)
					b.Toggle3DModel(isShoworHide);
				}
			}));
			//sonitems
			sonItems.ForEach(i=>UIHub.instance.GetHolder(i).GetBinder().Toggle3DModel(isShoworHide));
			//sonGeneral: handle son general holders' 3d modle show when toggle holder active state
			sonGenerals.ForEach(i=>UIHub.instance.GetHolder(i).GetBinder().Toggle3DModel(isShoworHide));
		}


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
		public virtual void OnTest () {}

		protected float intervalTime = 1f;
		private float   curTimer = 0f;
		private bool    isTestModeEnabled = false;
	}
}