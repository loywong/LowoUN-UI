using System.Collections.Generic;
using LowoUN.Util;
using LowoUN.Business.UI;

namespace LowoUN.Module.UI
{
	public abstract class UILogicBase : IRestart
	{
		protected Dictionary<int, List<int>> scenePreloadPanels = new Dictionary<int, List<int>> ();
		protected Dictionary<int, List<int>> sceneDefaultPanels = new Dictionary<int, List<int>> ();

		private List<UIFeatureBase> features = new List<UIFeatureBase>();

		protected UILogicBase() {
			var d = UIAsset.instance.GetSceneDefault (); 
			foreach (var item in d) 
				sceneDefaultPanels [EnumParse.GetEnumID(item.Key, typeof(Global.Enum_GameState))] = item.Value.ConvertAll<int>(i=>EnumParse.GetEnumID(i, typeof(UIPanelType)));
		}

		virtual protected void HandleBasicEvent (bool isAdd){
			if (isAdd) {
				//for scene
				//UINotifyMgr.Register<int>(UIScene.instance.EnterScene, "*_UIScene-Enter");
				//UINotifyMgr.Register(UIScene.instance.ExitScene, "*_UIScene-Exit");
			}
			else {
				//UINotifyMgr.Remove<int>(UIScene.instance.EnterScene);
				//UINotifyMgr.Remove(UIScene.instance.ExitScene);
			}
		}
		virtual public void OnStart () {
			//1
			UIScene.instance.OnInit(sceneDefaultPanels, scenePreloadPanels);
			//2
			UIHub.instance.OnStart();
			//3
			StartUIFeatures();
			//4
			HandleBasicEvent(true);
		}
		virtual public void OnUpdate (){
			//2
			UIHub.instance.OnUpdate();
		}
		virtual public void OnReset () {
			//3
			ResetUIFeatures();
			//1
			ResetUIScene();
		}
		virtual public void OnEnd (){
			//4
			HandleBasicEvent(false);
			//3
			EndUIFeatures();
			//2
			UIHub.instance.OnEnd();
			//1
			UIScene.instance.OnEnd();
		}

		private void StartUIFeatures (){
			features = Auto.CreateFeatures<UIFeatureBase> ();
			features.ForEach (i=>i.OnStart());
		}
		private void EndUIFeatures (){
			features.ForEach (i=>i.OnEnd());
		}
		private void ResetUIFeatures (){
			features.ForEach (i=>i.OnReset());
		}

		private void ResetUIScene (){}
	}
}