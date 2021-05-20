using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.HUDText
{
	public class UText_Damage : MonoBehaviour//, IUTxt LowoUN.Module.UI.HUDText.UText
    {
        public System.Action<UText_Damage> onAnimComplete;

        public Animator infoAnimator;
		public Image bloodImg1;
		public Image bloodImg2;
		public Image bloodImg3;
		public Image bloodImg4;
		public Text infoTxt;
		public int maxValue;
		public int minValue;
		public int maxFontSize;
		public int minFontSize;
		public Transform bloodCon;
		public Transform bloodMirrorCon;
		private float bloodImageScale;

		// Use this for initialization
		void Start () {
			
		}

        public void CompleteAnim()
        {
            if (onAnimComplete != null)
                onAnimComplete(this);
        }

        //public override void SetInfo (int value, int advLev, bool isEnemy) {
        public void SetInfo (int value, int advLev, bool isEnemy) {
			//base.SetInfo (value, advLev, isEnemy);

			bloodImg1.gameObject.SetActive(false);
			bloodImg2.gameObject.SetActive(false);
			bloodImg3.gameObject.SetActive(false);
			bloodImg4.gameObject.SetActive(false);

			if (value == 0) {
				infoTxt.text = "RESIST";
				infoTxt.fontSize = 50;

			} else {
				int tempId = Mathf.FloorToInt (Random.Range (0, 4));
				switch (tempId) {
				case 0:
					bloodImg1.gameObject.SetActive(true);
					break;
				case 1:
					bloodImg2.gameObject.SetActive(true);
					break;
				case 2:
					bloodImg3.gameObject.SetActive(true);
					break;
				case 3:
					bloodImg4.gameObject.SetActive(true);
					break;
				default:
					bloodImg1.gameObject.SetActive(true);
					break;
				}

				bloodCon.localEulerAngles = new Vector3 (0, 0, Random.Range(-30f, 30f));

				switch (advLev) {
				case 0:
					bloodImageScale = 1f;
					infoAnimator.SetInteger ("statusId", 1);
					break;
				case 1:
					bloodImageScale = 1.1f;
					infoAnimator.SetInteger ("statusId", 2);
					break;
				case 2:
					bloodImageScale = 1.2f;
					infoAnimator.SetInteger ("statusId", 3);
					break;
				}

				if (isEnemy)
					bloodMirrorCon.localScale = new Vector3 (-bloodImageScale, bloodImageScale, 1);
				else
					bloodMirrorCon.localScale = new Vector3 (bloodImageScale, bloodImageScale, 1);
			
				infoTxt.text = value.ToString ();
	//			infoTxt.fontSize = Mathf.FloorToInt (maxFontSize + (maxFontSize - minFontSize) * (value - minValue) / (maxValue - minValue));
			}
		}
	}
}