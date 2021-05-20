#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using LowoUN.Module.UI.Com;

namespace LowoUN.Module.UI.HUDText
{
    public class UText_Prog : MonoBehaviour, IUTxt
    {
        //public CanvasGroup layoutRoot = null;
        //public Text insText = null;
        public Text tObj = null;
        public Image imgObj = null;

        [SerializeField]
        private AnimationClip anim;

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

        void Awake()
        {
            hashcode = GetHashCode();
        }

        public void CompleteAnim()
        {
            if (onAnimComplete != null)
                onAnimComplete(this);
        }

        ///////////////////////////////////////// CD Bar //////////////////////////////////////////////
        private int styleID = -1;
        public void SetStyle(int styleID)
        {
            this.styleID = styleID;

            if (styleID == 0)
            {
                stylePlayer.gameObject.SetActive(false);
                styleEnemy.gameObject.SetActive(true);

                progBar_Player.ShowOrHide(false);
                progBar_Enemy.ShowOrHide(true);
            }
            else if (styleID == 1)
            {
                stylePlayer.gameObject.SetActive(true);
                styleEnemy.gameObject.SetActive(false);

                progBar_Player.ShowOrHide(true);
                progBar_Enemy.ShowOrHide(false);
            }
        }

		public void SetProgress(float rate)
        {
            //Debug.Log ("======== skill tip SetProgress: " + rate);
            if (styleID == 0)
                progBar_Enemy.SetValue(Mathf.FloorToInt(rate * 10000), 10000);
            else if (styleID == 1)
                progBar_Player.SetValue(Mathf.FloorToInt(rate * 10000), 10000);
        }
    }
}