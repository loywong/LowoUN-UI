using UnityEngine;
//using UnityEngine.UI;
using System.Collections.Generic;
//using GameSystem.ItemSystem;

namespace LowoUN.Module.UI 
{
	public class UIAnimMoveBag {
		public static UIAnimCloneAndMove _instance = null;
		public static void Play(RectTransform rt)
		{
			if (_instance != null) {
				_instance.AddItem(rt);
			}
		}
	}
		
	public class UIAnimCloneAndMove: MonoBehaviour {
		private float speed = 1.0f;
		public RectTransform ownerRt = null;
		private List<RectTransform> _curPanelRTs = new List<RectTransform>();
		private List<RectTransform> _tempRemoveRTs = new List<RectTransform>();
		void Start()
		{
			ownerRt = gameObject.GetComponent<RectTransform> ();
			UIAnimMoveBag._instance = this;
		}

		public void AddItem(RectTransform rt)
		{
			_curPanelRTs.Add (rt);
			//rt.SetParent (ownerRt);
		}

		void Update()
		{
			_tempRemoveRTs.Clear ();
			foreach (var o in _curPanelRTs) {
				if (o != null) {
					var delta = ownerRt.position - o.position;
					if (delta.magnitude <= speed * Time.deltaTime) {
						_tempRemoveRTs.Add (o);
						continue;
					}
					delta.Normalize ();
					o.position += delta * speed * Time.deltaTime;
				} else {
					_tempRemoveRTs.Add (o);
				}
			}
			foreach (var o in _tempRemoveRTs) {
				_curPanelRTs.Remove (o);
				if(o!=null)
					Destroy(o.gameObject);
			}
		}

		void OnGUI()
		{	
			//GUI.Box (new Rect (Screen.width - 200, 45, 120, 350), "");
			//if (GUI.Button (new Rect (Screen.width - 190, 100, 100, 40), "Spawn")) {
			//	foreach(var n in DataStorage.Instance.my_item_info_arr)
			//	{
			//		AddItem(n, Random.Range(-300.0f,300.0f),800.0f);
			//		break;
			//	}
			//}
		}
	}
}