namespace LowoUN
{
	/// <summary>
	/// Settings for current project business
	/// </summary>
	public static class Global 
	{
		public enum Enum_GameState
		{
			None = -1,
			Login = 0,
			World,
			Battle,

			Test = 999,
		}
	}
}