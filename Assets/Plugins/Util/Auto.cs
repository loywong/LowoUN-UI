using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LowoUN.Util {
	public static class Auto {
		public static List<T> CreateFeatures<T> () {
			//use class
			var types = System.AppDomain.CurrentDomain.GetAssemblies ()
				.SelectMany (a => a.GetTypes ()
					.Where (t => t.BaseType == typeof (T)))
				.ToArray ();

			var fs = new List<T> ();
			for (int i = 0; i < types.Length; i++) {
#if UNITY_EDITOR
				Debug.Log ("====== LowoUN ===> " + types[i].Name);
#endif

				var f = (T) Activator.CreateInstance (types[i]);
				fs.Add (f);
			}

			return fs;
		}
	}
}