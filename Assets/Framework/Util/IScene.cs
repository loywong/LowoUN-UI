namespace LowoUN.Module.UI 
{
	public interface IScene
	{
		void OnEnter ();
		void OnExit ();
		bool IsMainMenuDefaultPanel (int typ);
	}
}