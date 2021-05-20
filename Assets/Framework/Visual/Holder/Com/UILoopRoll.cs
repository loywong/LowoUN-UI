#pragma warning disable 0649//ignore default value null
using LowoUN.Module.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Module.UI.Com
{
	public class UILoopRoll : MonoBehaviour {
	    [SerializeField]
	    private float interval;//如果不用恒定的时间，也可以用Time.deltaTime实现
	    [SerializeField]
	    private int speed = 3;//每单位时间滚动像素
	    [SerializeField]
	    private bool isUorV = false;
	    [SerializeField]
	    private UIList lst1;
	    [SerializeField]
	    private UIList lst2;

		private RectTransform lst1RT;
		private RectTransform lst2RT;
	    private float lstH;

	    private bool isStartPlay = false;
	    public void SetInfo<T>(List<T> lst) {
	        lst1.SetItemList(lst);
	        lst2.SetItemList(lst);

	        if (isUorV == false) {
	            lstH = lst1RT.sizeDelta.y;

	            //reset lsts pos
	            lst1RT.anchoredPosition = Vector2.zero;
	            lst2RT.anchoredPosition = Vector2.zero + new Vector2(0f, -lstH);

	            newPos1 = lst1RT.anchoredPosition;
	            newPos2 = lst2RT.anchoredPosition;
	        }

	        isStartPlay = true;
	        //StartPlay();
	    }



	    // Use this for initialization
	    void Awake () {
	        if (lst1 != null)
	            lst1RT = lst1.GetComponent<RectTransform>();
	        else
	            Debug.LogError("Lst1 is null");
	        if (lst2 != null)
	            lst2RT = lst2.GetComponent<RectTransform>();
	        else
	            Debug.LogError("Lst2 is null");
	    }

		private Vector2 newPos1 = Vector2.zero;
		private Vector2 newPos2 = Vector2.zero;
	    // Update is called once per frame
	    void Update () {
	        if (isStartPlay) {
	            if (isUorV == false) {
	                 newPos1 += new Vector2(0f, speed);
	                 newPos2 += new Vector2(0f, speed);
	                 
	                 lst1RT.anchoredPosition = newPos1;
	                 lst2RT.anchoredPosition = newPos2;
	                 
	                 //reset
	                 if (lst1RT.anchoredPosition.y >= lstH) {
	                     lst1RT.anchoredPosition = new Vector2(0, -lstH);
	                     newPos1 = lst1RT.anchoredPosition;
	                 }
	                 
	                 if (lst2RT.anchoredPosition.y >= lstH) {
	                     lst2RT.anchoredPosition = new Vector2(0, -lstH);
	                     newPos2 = lst2RT.anchoredPosition;
	                 }
	             }
	        }
	    }
	}
}