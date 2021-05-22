#pragma warning disable 0649
using System;
using UnityEngine;
using System.Collections.Generic;

namespace LowoUN.Module.UI 
{
	public class UIDynamicCon : MonoBehaviour, IDynamicSize
	{
		[SerializeField]
		private GameObject con;
//		[SerializeField]
//		private float paddingY = 120f;
		[SerializeField]
		private List<float> sizeLst;

		public Action<int> onUpateTxtConDy;

		private int curIdxInLst = -1;
		private RectTransform conRT;

		void Awake () {
			if (con == null) 
				Debug.LogError ("====== LowoUN-UI ===> no container set onto the UIDynamicTxt!");
			else
				conRT = con.transform.GetComponent<RectTransform> ();
		}

		public void SetSize (int sizelv, bool isUorV = false/*default: isV*/) {
			curIdxInLst = con.GetComponent<UIHolder> ().curIdxInList;
			Vector2 olssize = conRT.sizeDelta;
			conRT.sizeDelta = new Vector2(olssize.x, sizeLst[sizelv]);

			ResetCon ();
		}

		public void ResetCon () {
			//SetSize (txt.preferredHeight);
			if(onUpateTxtConDy != null) onUpateTxtConDy (curIdxInLst);
		}
	}
}