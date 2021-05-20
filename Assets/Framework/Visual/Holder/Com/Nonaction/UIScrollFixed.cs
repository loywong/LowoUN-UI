using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LowoUN.Module.UI.Com
{
	public enum UIScrollViewDir 
	{
		None,
		UV,
		U,
		V,
	}

	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollFixed : UIActionBase
	{
		[SerializeField]
		private bool isShowScrollbar = true;

		private ScrollRect sRect;


		// Use this for initialization
		void Awake () {
			sRect = GetComponent<ScrollRect> ();

		}
		void Start () {
			SetScrollbarVisibility ();
		}
		// Update is called once per frame
		void Update () {

		}

		private void SetScrollbarVisibility () {
			if(transform.GetComponent<Image>() != null)
				transform.GetComponent<Image>().enabled = isShowScrollbar;

			//if (sRect.horizontalScrollbar != null && sRect.horizontalScrollbar.gameObject.activeSelf)
	        if (sRect.horizontalScrollbar != null && sRect.horizontalScrollbar.gameObject.activeSelf)
	        {
				if(sRect.horizontalScrollbar.gameObject.GetComponent<Image>() != null)
					sRect.horizontalScrollbar.gameObject.GetComponent<Image>().enabled = isShowScrollbar && sRect.horizontal;
				
				sRect.horizontalScrollbar.handleRect.gameObject.SetActive(isShowScrollbar && sRect.horizontal);
				//sRect.horizontalScrollbar.targetGraphic.enabled = isShowScrollbar;
			}

			//if (sRect.verticalScrollbar != null && sRect.verticalScrollbar.gameObject.activeSelf)
	        if (sRect.verticalScrollbar != null && sRect.verticalScrollbar.gameObject.activeSelf)
	        {
                if (sRect.verticalScrollbar.gameObject.GetComponent<Image>() != null)
					sRect.verticalScrollbar.gameObject.GetComponent<Image>().enabled = isShowScrollbar && sRect.vertical;
				
				sRect.verticalScrollbar.handleRect.gameObject.SetActive(isShowScrollbar && sRect.vertical);
				//sRect.verticalScrollbar.targetGraphic.enabled = isShowScrollbar;
			}
		}

		public void SetScrollDirection (UIScrollViewDir dirType) {
			if(sRect==null)
				sRect = GetComponent<ScrollRect> ();

			sRect.horizontal = (dirType == UIScrollViewDir.U || dirType == UIScrollViewDir.UV);
			sRect.vertical = (dirType == UIScrollViewDir.V || dirType == UIScrollViewDir.UV);
		}
	}
}