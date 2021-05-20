using UnityEngine;
using LowoUN.Util;

namespace LowoUN.Module.UI.HUDText
{
	[System.Serializable]
	public class HudNotifyer
	{
		[SerializeField]
		private GameObject g;
		[HideInInspector]
		public UText_Notify go{get{return g.GetComponent<UText_Notify>();}}

		private bool _isComplete = true;
		public bool isComplete { get { return _isComplete; } }

		public System.Action<HudNotifyer> onComplete;

		private readonly Vector2 bgMargin = new Vector2(36f,0f); 
		private int hashcode;
		private uint duration;
		public HudNotifyer()
		{
			hashcode = GetHashCode();
		}

		public void Init(int idx){
			if (idx == 0)
				duration = UIHudText_Notify_CacheTwo.instance.duration;
			else if (idx == 1)
				duration = UIHudText_Notify_CacheTwo.instance.duration + 100u/*HACK: delay to handle the next one*/;
		}

		private string _msg = string.Empty;
		public string msg { get { return _msg; } }

		public void OnStart(string msg)
		{
			if (go == null)
				return;

			_isComplete = false;

			go.gameObject.SetActive(true);
			_msg = msg;
			go.tObj.text = msg;
			if(go.imgObj != null){
				var h = go.GetComponent<RectTransform>().sizeDelta.y;
				go.imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(go.tObj.preferredWidth + bgMargin.x, h-bgMargin.y*2);
			}

			if (TimeWatcher.instance.ContainKey("ui_hudtxt_nofify-cache-two" + msg + hashcode))
				TimeWatcher.instance.RemoveWatcher("ui_hudtxt_nofify-cache-two" + msg + hashcode);

			TimeWatcher.instance.AddWatcher("ui_hudtxt_nofify-cache-two" + msg + hashcode, duration, false, () => {
				End();
			}, false);
		}

		//public void ForceEnd () 

		public void Reset()
		{
			_isComplete = true;
			go.gameObject.SetActive (false);
			if (TimeWatcher.instance.ContainKey("ui_hudtxt_nofify-cache-two" + msg + hashcode))
				TimeWatcher.instance.RemoveWatcher("ui_hudtxt_nofify-cache-two" + msg + hashcode);
		}

		private void End()
		{
			Reset ();
			if (onComplete != null)
				onComplete(this);
		}
	}
}