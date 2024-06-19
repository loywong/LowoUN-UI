namespace LowoUN.Util.Log {
	public enum Enum_LvColor {
		//		eLogLevelAll   = 0Xff0000,// default
		eLogLevelInfo = 0X92ff00, // Info
		eLogLevelWarn = 0Xffff00, // Warning
		eLogLevelError = 0Xff00b9, // Error
		eLogLevelFatal = 0Xff0000, // Forbidden!!! //Fatal

		eLogLevelDebug = 0X00abff, // Debug
	}

	public static class LvColor {
		public static string GetColor (Enum_LvColor e) {
			return System.Convert.ToInt32 (e).ToString ("X2");
		}
	}
}