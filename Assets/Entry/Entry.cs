using System;
using System.Collections.Generic;
using System.Linq;
using LowoUN.Business.UI;
using UnityEngine;

namespace LowoUN.Entry
{
	public class Entry : MonoBehaviour
    {
		private static Entry _instance;
		public static Entry  instance { get { return _instance;}}

		[SerializeField] private Global.Enum_GameState _startGameState = Global.Enum_GameState.Login;
		[SerializeField] private float _timeSpeed = 1;

		void Awake() {
			_instance = this;

			InitDebug ();
		}

		private void InitDebug () {
			//#if DEVELOPMENT_BUILD
			#if UNITY_EDITOR
			Debug.unityLogger.logEnabled = true;
			#else
			Debug.logger.logEnabled = false;
			#endif

			Time.timeScale = _timeSpeed;
		}

        void Start() {
			//[3]ui
			UILogic.instance.OnStart();

			LowoUN.Util.Notify.NotifyMgr.Broadcast<int>("UW_LoadScene", (int)_startGameState/*gamestateid*/);
        }

		void Update() {
			//[3]ui
			UILogic.instance.OnUpdate();
		}

		//OnDestroy later
		void OnApplicationQuit() {
			//[3]ui
			UILogic.instance.OnEnd();
		}
    }
}