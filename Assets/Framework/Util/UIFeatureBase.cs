namespace LowoUN.Module.UI
{
	public abstract class UIFeatureBase
	{
		private bool hasInit = false;

		public UIFeatureBase () {
			if (!hasInit) {
				hasInit = true;
//				_instance = this;
			}
			else {
				throw new System.Exception(" ====== LowoUN ===> !!!!!! Forbid to instantiate the Global Feature class: " + this + " repeatedly");
			}
		}

		public abstract void OnStart();
		//public virtual void OnUpdate(){}
		public abstract void OnEnd();
		public virtual void OnReset(){}
	}
}