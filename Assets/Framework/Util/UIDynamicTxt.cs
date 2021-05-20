#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
//using LowoUN.Business.UI;
using System.Text;

namespace LowoUN.Module.UI 
{
	[System.Serializable]
	public class DynamicTxtConState {
		public int style;
		public List<GameObject> elements; 
	}

	[ExecuteInEditMode]
	public class UIDynamicTxt : MonoBehaviour, IDynamicSize
	{
		//simulating type writing
		[SerializeField]
		private Text txt;
		[SerializeField]
		private float interval = 0.1f;
		[SerializeField]
		private float fastRate = 10;

		private char[] cacheChars;
		private bool isStartTexting = false;
		private StringBuilder curText = new StringBuilder();
		private int curLine = 0;
		private int curIdx = 0;
		private float timeGo = 0f;
		private float curTxtHight = 0f;


		[SerializeField]
		private GameObject con;
		[SerializeField]
		private float paddingY = 120f;
		[SerializeField]
		private float paddingY4Aside = 60f;

		public Action<int> onUpateTxtCon;
		public Action<int> onUpateTxtConDy;
		public Action<int> onUpateTxtConDone;

		void Update () {
			if (isStartTexting) {
				timeGo += Time.deltaTime;

				if (timeGo >= interval) {
					timeGo = 0f;

					if (curIdx <= cacheChars.Length - 1) {
						curText.Append (cacheChars [curIdx]);
						txt.text = curText.ToString ();
						curIdx++;

						//SetContainerSize_NEW ();
						if (txt.preferredHeight != curTxtHight) {
							curTxtHight = txt.preferredHeight;
							ResetConSize (curTxtHight);
							if(onUpateTxtConDy != null) onUpateTxtConDy (curIdxInLst);
						}
					} else {
						isStartTexting = false;
						if(onUpateTxtConDone != null) onUpateTxtConDone (curIdxInLst);
					}
				}
			}
		}

		public bool FastTyping () {
			if (isStartTexting) {
				interval /= fastRate;
				return true;
			}
			return false;
		}

		//public void SetTxt (string c, bool isTpying, Enum_Cutscene_Dialogue _frameStyle) {
		public void SetTxt (string c, bool isTpying, int _frameStyle) {
			curStyle = _frameStyle;
			curIdxInLst = con.GetComponent<UIHolder> ().curIdxInList;

			if (isTpying) {
				//Reset
				txt.text = string.Empty;

				interval = 0.1f;
				curLine = 0;
				curIdx = 0;
				curText = new StringBuilder ();
				timeGo = 0;

				cacheChars = c.ToCharArray ();
				isStartTexting = true;
			} else {
				txt.text = c;
			}
		}

		void Awake () {
			if (con == null) 
				Debug.LogError ("====== LowoUI-UN ===> no container set onto the UIDynamicTxt!");
			else
				conRT = con.transform.GetComponent<RectTransform> ();
			
			if (txt == null) 
				Debug.LogError ("====== LowoUI-UN ===> no text component attached onto the UIDynamicTxt!");
		}

		private int curIdxInLst = -1;
		//private Enum_Cutscene_Dialogue curStyle;
		private int curStyle;
		private RectTransform conRT;
		public void ResetCon () {//Enum_Cutscene_Dialogue style
			//curStyle = style;
			ResetConSize (txt.preferredHeight);
			if(onUpateTxtCon != null) onUpateTxtCon (curIdxInLst);
		}
		private void ResetConSize (float tNewHeight) {
			// set item holder height
			Vector2 size = conRT.sizeDelta;
			if(curStyle == 0/*Enum_Cutscene_Dialogue.Aside*/)
				conRT.sizeDelta = new Vector2(size.x, tNewHeight + paddingY4Aside);
			else
				conRT.sizeDelta = new Vector2(size.x, tNewHeight + paddingY);
		}


		///////////////////////////////////////////////////////////////////////////
		//HACK for the inactive state of container
		[SerializeField]
		private List<DynamicTxtConState> inactiveMasks;
		[SerializeField]
		private Color activeTxtColor;
		[SerializeField]
		private Color inactiveTxtColor;

		public void SetConState (int style, bool active) {//int holderInsid, 
			//if (con != null && con.GetComponent<UIHolder> () != UIHub.instance.GetHolder(holderInsid))
			//	return;
			
			foreach (var item in inactiveMasks) {
				if (item.style == style) {
					item.elements.ForEach (i => i.SetActive (!active));
				} else {
					item.elements.ForEach (i => i.SetActive (false));
				}
			}

			if (txt != null) {
				txt.color = active ? activeTxtColor : inactiveTxtColor;
				if (style == 0/*(int)Enum_Cutscene_Dialogue.Aside*/)
					txt.alignment = TextAnchor.MiddleCenter;
				else
					txt.alignment = TextAnchor.MiddleLeft;
			}
		}
	}
}