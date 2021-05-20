#pragma warning disable 0649//ignore default value null
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LowoUN.Module.UI 
{
	public class UIStateVisual : MonoBehaviour
	{
		[SerializeField]
		private UIFrameAnimPlayer selectedFrameAnim;
		[SerializeField]
		private List<GameObject> selectedGoes;

		void Awake () {
			SetSelectedStateGoes (false);
		}

		private void SetSelectedStateGoes (bool isSelected) {
			if (selectedGoes != null && selectedGoes.Count > 0)
				selectedGoes.ForEach (i=>i.SetActive(isSelected));
		}

		public void SetState (UIStateType type) {
			if (type == UIStateType.Selected) {
				SetSelectedStateGoes (true);
			} else if (type == UIStateType.Deselected) {
				SetSelectedStateGoes (false);
			}
		}

		public void PlayFrameAnimEff(bool isPlay)
        {
			if (selectedFrameAnim != null)
            {
                if (isPlay)
                {
					//selectedFrameAnim.gameObject.SetActive(true);
					if (!selectedFrameAnim.isPlaying)
                    {
						selectedFrameAnim.StartPlay();
                    }
                }
                else
                {
					if (selectedFrameAnim.isPlaying)
                    {
						selectedFrameAnim.StopPlay();
                    }
					//selectedFrameAnim.gameObject.SetActive(false);
                }
            }
        }
	}
}