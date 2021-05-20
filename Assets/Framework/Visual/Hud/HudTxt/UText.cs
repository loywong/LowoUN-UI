#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using LowoUN.Util;
using LowoUN.Module.UI.Com;

namespace LowoUN.Module.UI.HUDText
{
	public class UText : MonoBehaviour, IUTxt
    {
	    //public CanvasGroup layoutRoot = null;
		//public Text insText = null;
		public Text tObj = null;
		public Image imgObj = null;

		[SerializeField]
		private AnimationClip anim;
		[SerializeField]
		private AnimationClip anim_Open_Bubble;
		[SerializeField]
		private AnimationClip anim_Close_Bubble;
	    [HideInInspector]
	    public Color color;
	    [HideInInspector]
	    public UIEnum_Hud_UDriectionType movement;

	    //[HideInInspector]
	    //public float xOffset;
	    //[HideInInspector]
	    //public float yOffset;
		//[HideInInspector]
		//public float xAcceleration;
		//[HideInInspector]
		//public float yAcceleration;
		//[HideInInspector]
		//public float yQuicknessScaleFactor;

	    [HideInInspector]
	    public int size;
	    [HideInInspector]
	    public float speed;
//	    [HideInInspector]
//	    public string text;
	    [HideInInspector]
	    public Vector2 pos;

		[SerializeField]
		private Image stylePlayer;
		[SerializeField]
		private Image styleEnemy;
		[SerializeField]
		private UIProg progBar_Player;
		[SerializeField]
		private UIProg progBar_Enemy;

        //public Text Txt_SkillName;

        public System.Action<IUTxt> onAnimComplete;
		private int hashcode;

		void Awake () {
			hashcode = GetHashCode ();
		}

		public void SetInfo(string info) {
			tObj.color = color;

			//tObj.pos = pos;
			//item.text = text;
			//item.size = size;

			//THINKING
			tObj.fontSize = size;
			float temp = size * 1.28f;
			tObj.rectTransform.sizeDelta = new Vector2(temp * info.Length, temp);

			tObj.text = info;

			if (anim != null) 
				UIAnimPlayer.Play (gameObject, anim);
		}

        //for Damage??
		public void CompleteAnim () {
			if (onAnimComplete != null)
				onAnimComplete (this);
		}

		///////////////////////////////////////// Bubble //////////////////////////////////////////////
		public void ShowBubble () {
			if (TimeWatcher.instance.ContainKey ("UI_HUD_Bubble_DelayClose" + hashcode))
				TimeWatcher.instance.RemoveWatcher ("UI_HUD_Bubble_DelayClose" + hashcode);
			if (TimeWatcher.instance.ContainKey ("UI_HUD_Bubble_Close" + hashcode))
				TimeWatcher.instance.RemoveWatcher ("UI_HUD_Bubble_Close" + hashcode);

			if (anim_Open_Bubble != null) 
				UIAnimPlayer.Play (gameObject, anim_Open_Bubble);
		}
		public void HideBubble () {

            if (Time.timeScale != 1)
            {
                gameObject.SetActive(false);
            }
            else {
                if (TimeWatcher.instance.ContainKey("UI_HUD_Bubble_Close" + hashcode))
                    TimeWatcher.instance.RemoveWatcher("UI_HUD_Bubble_Close" + hashcode);
                if (TimeWatcher.instance.ContainKey("UI_HUD_Bubble_DelayClose" + hashcode))
                    TimeWatcher.instance.RemoveWatcher("UI_HUD_Bubble_DelayClose" + hashcode);

				TimeWatcher.instance.AddWatcher("UI_HUD_Bubble_DelayClose" + hashcode, (uint)(0.1f/*Module_Bubble.instance.delayToFadeout*/ * 1000), false, ()=> {
				TimeWatcher.instance.RemoveWatcher("UI_HUD_Bubble_DelayClose" + hashcode);

				if (this != null){
					if(gameObject != null){

						if(anim_Close_Bubble != null){
							UIAnimPlayer.Play (gameObject, anim_Close_Bubble);

							TimeWatcher.instance.AddWatcher("UI_HUD_Bubble_Close" + hashcode, (uint)(0.2/*Module_Bubble.instance.fadeoutAnimTime*/ * 1000), false, ()=> {
								TimeWatcher.instance.RemoveWatcher("UI_HUD_Bubble_Close" + hashcode);

								gameObject.SetActive(false);
							});
						}
						else
							gameObject.SetActive(false);
					}
				}
			});
            }
        }

		public void Init_Bubble(string text) {
			tObj.text = text;
			Vector2 txtSize = tObj.GetComponent<RectTransform> ().sizeDelta;
			imgObj.GetComponent<RectTransform> ().sizeDelta = new Vector2 (txtSize.x + 30f, tObj.preferredHeight + 25f);
		}

		///////////////////////////////////////// CD Bar //////////////////////////////////////////////
		private int styleID = -1;
		public void SetStyle (int styleID) {
			this.styleID = styleID;

			if (styleID == 0) 
			{
				stylePlayer.gameObject.SetActive (false);
				styleEnemy.gameObject.SetActive (true);

				progBar_Player.ShowOrHide (false);
				progBar_Enemy.ShowOrHide (true);
			}
			else if (styleID == 1) 
			{
				stylePlayer.gameObject.SetActive (true);
				styleEnemy.gameObject.SetActive (false);

				progBar_Player.ShowOrHide (true);
				progBar_Enemy.ShowOrHide (false);
			}
		}

		public void SetProgress (float rate) {
			//Debug.Log ("======== skill tip SetProgress: " + rate);
			if(styleID == 0)
				progBar_Enemy.SetValue (Mathf.FloorToInt(rate*10000), 10000);
			else if (styleID == 1) 
				progBar_Player.SetValue (Mathf.FloorToInt(rate*10000), 10000);
		}
	}
}