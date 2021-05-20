#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;


namespace LowoUN.Module.UI.HUDText
{
    public class UText_Notify : MonoBehaviour, IUTxt
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

        //public Text Txt_SkillName;

        public System.Action<IUTxt> onAnimComplete;
        private int hashcode;

        void Awake()
        {
            hashcode = GetHashCode();
        }

        //public void SetInfo(string info)
        //{
        //    tObj.color = color;

        //    //tObj.pos = pos;
        //    //item.text = text;
        //    //item.size = size;

        //    //THINKING
        //    tObj.fontSize = size;
        //    float temp = size * 1.28f;
        //    tObj.rectTransform.sizeDelta = new Vector2(temp * info.Length, temp);

        //    tObj.text = info;

        //    if (anim != null)
        //        UIAnimPlayer.Play(gameObject, anim);
        //}

        public void CompleteAnim()
        {
            if (onAnimComplete != null)
                onAnimComplete(this);
        }
    }
}