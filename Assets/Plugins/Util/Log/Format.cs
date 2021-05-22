namespace LowoUN.Util.Log
{
	public static class Format
	{
		private static readonly string ui = "====== LowoUN-UI ===> ";
		private static readonly string module = "====== LowoUN / Module / ";
		private static readonly string util = "====== LowoUN / Util";

		public static string UI () {
			return ui; 
		}
		public static string Module (string mName) {
			return module + mName + " ===> "; 
		}
		public static string Util (string uName = null) {
			if(string.IsNullOrEmpty(uName))
				return util + " ===> "; 
			else
				return util + " / "+ uName + " ===> "; 
		}
		public static string Business (string author) {
			return string.Format("====== {0} ===> ", author); 
		}
	}
}

