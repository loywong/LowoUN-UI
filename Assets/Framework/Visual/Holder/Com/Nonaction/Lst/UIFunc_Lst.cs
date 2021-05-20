using System;
using UnityEngine;

namespace LowoUN.Module.UI.Com
{
	public static class UIFunc_Lst
	{
		public static void CheckPrevNextBtnVisble (
			GameObject btnNext, 
			GameObject btnPrev, 
			GameObject scrollView, 
			int _realShowItemCount, 
			float itemWidth, 
			float itemHeight, 
			Transform transform, 
			bool isV, 
			bool isU
		){
			if (btnNext == null && btnPrev == null)
				return;

			if (scrollView == null)
				return;

			if (isU) {
				if (_realShowItemCount * itemWidth <= scrollView.GetComponent<RectTransform>().sizeDelta.x) {
					if (btnNext != null)
						btnNext.SetActive(false);
					if (btnPrev != null)
						btnPrev.SetActive(false);
				}
				else {
					if (transform.GetComponent<RectTransform>().anchoredPosition.x>= 0) {
						if (btnPrev != null)
							btnPrev.SetActive(false);
					}
					else {
						if (btnPrev != null)
							btnPrev.SetActive(true);
					}

					if (transform.GetComponent<RectTransform>().anchoredPosition.x <= scrollView.GetComponent<RectTransform>().sizeDelta.x-_realShowItemCount * itemWidth + itemWidth) {
						if (btnNext != null)
							btnNext.SetActive(false);
					}
					else {
						if (btnNext != null)
							btnNext.SetActive(true);
					}
				}
			}
			else if (isV) {
				if (_realShowItemCount *itemHeight<= scrollView.GetComponent<RectTransform>().sizeDelta.y) {
					if (btnNext != null)
						btnNext.SetActive(false);
					if (btnPrev != null)
						btnPrev.SetActive(false);
				}
				else {
					if (transform.GetComponent<RectTransform>().anchoredPosition.y >= 0) {
						if (btnPrev != null)
							btnPrev.SetActive(false);
					}
					else {
						if (btnPrev != null)
							btnPrev.SetActive(true);
					}

					if (transform.GetComponent<RectTransform>().anchoredPosition.y<= scrollView.GetComponent<RectTransform>().sizeDelta.y- _realShowItemCount * itemHeight+ itemHeight) {
						if (btnNext != null)
							btnNext.SetActive(false);
					}
					else {
						if (btnNext != null)
							btnNext.SetActive(true);
					}
				}
			}
		}

		public static void ResetItem (UIHolder h) {
			if (h != null) {
				//h.OnReset ();
				h.curIdxInList = -1;
			}
		}
	}
}