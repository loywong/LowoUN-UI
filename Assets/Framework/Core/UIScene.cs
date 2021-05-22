using System.Collections.Generic;
using System.Linq;

namespace LowoUN.Module.UI
{
	public sealed class UIScene
	{
		private static UIScene _uiScene;
		public static UIScene instance{
			get{
				if (_uiScene == null) {
					_uiScene = new UIScene ();
				}
				return _uiScene; }
		}

		private Dictionary<int, List<int>> sceneDefaultPanels = new Dictionary<int, List<int>>();
		//private Dictionary<int, List<int>> preloadPanels = new Dictionary<int, List<int>>();

		public IScene current = null;

		private UIScene(){}

		public void OnInit (Dictionary<int, List<int>> defaultPanels, Dictionary<int, List<int>> preloadPanels = null) {
			this.sceneDefaultPanels = defaultPanels;
			//this.preloadPanels = preloadPanels??this.preloadPanels;
		}

		private int curSceneID = -1;
		public int curSceneStateID {
            get { return curSceneID; }
        }

        public bool HasEntered (int sceneTypeID) {			
			return sceneTypeID == curSceneID;
		}


		public void EnterScene(int sceneID) {
			//UnityEngine.Debug.LogError ("load scene: " + (LowoUN.Entry.INI.GameState)sceneID);
			if (sceneID == -1) {
				UnityEngine.Debug.LogError ("====== LowoUN-UI ===> Error game scene entered !");
				return;
			}

			if(HasEntered(sceneID))
				return;
				
			if (curSceneID != -1) 
				ExitScene ();
			
			curSceneID = sceneID;

			if (sceneDefaultPanels.ContainsKey (sceneID)) {
				foreach (var item in sceneDefaultPanels[sceneID]) 
					UIHub.instance.LoadUI (item);
			}

//			if (preloadPanels.ContainsKey (sceneID)) {
//				foreach (var item in preloadPanels[sceneID])
//					UIHub.instance.PreloadUI (item);
//			}
		}

		public void ExitScene () {
			UIHub.instance.ClearSceneAll ();

			curSceneID = -1;
		}

		public void OnEnd () { 
			ExitScene ();
		}

		public bool IsSceneDefaultUI (int panelTypID) {
			bool isContain = false;
			var all = sceneDefaultPanels.Select (i => i.Value).ToList ();
			foreach (var item in all) {
				if (item.Contains (panelTypID)) {
					isContain = true;
					break;
				}
			}
			return isContain;
		}

		public int GetSceneDefaultToppestUI (int sceneID) {
			return sceneDefaultPanels[sceneID].Last();
		}

		//TOREFACTOR
		public bool Logout () {
//			//1,if the original scene
//			UIlogic.instance.IsTheOriginalScene();
//			//if ((GameState)curSceneID == GameState.Login||(GameState)curSceneID == GameState.LobbyToLogin)
//			//	return false;
//			
//			//2,exit current scene
//			if ((GameState)curSceneID == GameState.Lobby)
//				LowoUN.Business.UI.UIScene_Lobby.instance.OnExit();
//
			return true;
		}
	}
}