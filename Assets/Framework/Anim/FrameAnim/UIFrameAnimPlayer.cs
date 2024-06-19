using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI {
	[RequireComponent (typeof (Image))]
	public class UIFrameAnimPlayer : MonoBehaviour {
		[SerializeField]
		private bool isAutoPlay = false;

		public bool isPlaying {
			get;
			private set;
		}

		public string movieName;
		public List<Sprite> lSprites;
		public float fSep = 0.05f;
		public float showerWidth {
			get {
				if (shower == null) {
					return 0;
				}
				return shower.rectTransform.rect.width;
			}
		}
		public float showerHeight {
			get {
				if (shower == null) {
					return 0;
				}
				return shower.rectTransform.rect.height;
			}
		}
		void Awake () {
			CheckShower ();
			//gameObject.SetActive(false);
		}

		private void CheckShower () {
			if (shower == null) {
				shower = GetComponent<Image> ();
				shower.raycastTarget = false;

				if (string.IsNullOrEmpty (movieName)) {
					movieName = "movieName";
				}
			}
		}

		void Start () {
			if (isAutoPlay) {
				StartPlay ();
			}
		}

		public void StopPlay () {
			isPlaying = false;
			//gameObject.SetActive (false);
		}

		public void StartPlay () {
			//gameObject.SetActive (true);
			CheckShower ();
			isPlaying = true;
			Play (0);
		}

		private void Play (int iFrame) {
			if (iFrame >= FrameCount) {
				iFrame = 0;
			}
			if (iFrame < lSprites.Count) {
#if UNITY_EDITOR
				//Debug.Log("iFrame: " + iFrame);
				//Debug.Log("lSprites: " + lSprites);
				//Debug.Log("lSprites length: " + lSprites.Count);
				//Debug.Log("lSprites [iFrame]: " + lSprites[iFrame]);
				//Debug.Log("shower: " + shower);
#endif
				shower.sprite = lSprites[iFrame];
				curFrame = iFrame;
				//shower.SetNativeSize();
				if (dMovieEvents.ContainsKey (iFrame)) {
					foreach (delegateMovieEvent del in dMovieEvents[iFrame]) {
						del ();
					}
				}
			} else {
				//Error
			}
		}
		private Image shower;
		int curFrame = 0;
		public int FrameCount {
			get {
				return lSprites.Count;
			}
		}
		float fDelta = 0;
		void Update () {
			if (isPlaying) {
				fDelta += Time.deltaTime;
				if (fDelta > fSep) {
					fDelta = 0;
					curFrame++;
					Play (curFrame);
				}
			}
		}
		public delegate void delegateMovieEvent ();
		private Dictionary<int, List<delegateMovieEvent>> dMovieEvents = new Dictionary<int, List<delegateMovieEvent>> ();
		public void RegistMovieEvent (int frame, delegateMovieEvent delEvent) {
			if (!dMovieEvents.ContainsKey (frame)) {
				dMovieEvents.Add (frame, new List<delegateMovieEvent> ());
			}
			dMovieEvents[frame].Add (delEvent);
		}
		public void UnregistMovieEvent (int frame, delegateMovieEvent delEvent) {
			if (!dMovieEvents.ContainsKey (frame)) {
				return;
			}
			if (dMovieEvents[frame].Contains (delEvent)) {
				dMovieEvents[frame].Remove (delEvent);
			}
		}
	}
}