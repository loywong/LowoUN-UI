using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LowoUN.Module.UI.Com
{
	[RequireComponent(typeof(Slider))]
	public class UISlider : UIActionBase 
	{
		//[SerializeField]
		private Slider _slider;

		// Use this for initialization
		void Awake () {
			_slider = GetComponent<Slider>();
			#if UNITY_EDITOR
			if(_slider == null){
				Debug.LogError ("======= LowoUN-UI ===> Don't forget to set slider component reference!");
			}
			#endif

			_slider.onValueChanged.AddListener (OnAction);
		}

		//private readonly float beginRate = 0.2f; 
		private void OnAction(float value){
			#if UNITY_EDITOR
			Debug.Log("====== LowoUN-UI ===> onCallEvent : UISlider" + value);
			#endif

//			float fakeVal = 0f;
//
//			if (value == 0f)
//				fakeVal = 0f;
//			else if (value == 1f)
//				fakeVal = 1f;
//			else
//				fakeVal = beginRate + (1f-beginRate) * value;
			
			if(onCallEvent != null)
				onCallEvent (curEventID, value, hostHolderInsID);
		}

		// Update is called once per frame
		void Update () {

		}

		public void SetValue (float percent) {
			if(_slider != null)
				_slider.value = percent;
		}

        public void SetMaxAndMinValue(int Max,int Min)
        {
            if (_slider != null)
            {
                _slider.maxValue = Max;
                _slider.minValue = Min;
            }
        }

        //		public void SetValue (float cur, float min, float max) {
        //			slider.maxValue = max;
        //			slider.minValue = min;
        //			slider.value = cur;
        //		}
    }
}