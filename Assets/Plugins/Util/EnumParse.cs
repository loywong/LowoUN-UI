using UnityEngine;

namespace LowoUN.Util
{
	public static class EnumParse
	{
		public static int GetEnumID (string name, System.Type e)
		{
			foreach (int intValue in System.Enum.GetValues(e)) {
				if(name == System.Enum.GetName(e, intValue))
					return intValue;
			}

			return -2147483648;//int range(-2147483648～+2147483647)
		}
	}
}