using UnityEngine;
using System.Collections.Generic;

namespace LowoUN.Util
{
	public static class Extensions {
		public static int ShowInt(this float f){
			return Mathf.RoundToInt (f);
		}
		public static float ShowFloat(this float f){
			return Mathf.Round (f * 100) / 100f;
		}

		public static string CorrectRichText (this string s) {
			return s.Replace("\\n","\n");
		}

		//dictionary
	    public static TValue Val<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
	    {
	//		#if UNITY_EDITOR
	//		if (!dict.ContainsKey (key)) {
	//			Debug.Log (string.Format("====== LowoUN ===> Dict[Get] can't find value by key type:{0}, id{1}", key.GetType().ToString(), key.ToString()));
	//		}
	//		#endif
			
	        return dict.ContainsKey(key) ? dict[key] : defaultValue;
	    }

		public static bool Remov<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
		{
			if (!dict.ContainsKey (key)) {
				
	//			#if UNITY_EDITOR
	//			Debug.Log (string.Format ("====== LowoUN ===> Dict[Remove] can't find value by key type:{0}, id{1}", key.GetType().ToString(), key.ToString()));
	//			#endif

				return false;
			} else {
				dict.Remove (key);
			}
				
			return true;
		}
	}
}