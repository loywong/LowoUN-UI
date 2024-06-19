using UnityEngine;

namespace LowoUN.Util {
    public static class Maths {
        //		public static bool isOutDate (System.DateTime given, System.DateTime tar) {
        //			int Compare = given.CompareTo(tar);
        //
        //			return !(Compare > 0);
        //		}

        /// <summary>
        /// 3维中如何计算两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float TwoPointDistance3D (Vector3 p1, Vector3 p2) {

            float i = Mathf.Sqrt ((p1.x - p2.x) * (p1.x - p2.x) +
                (p1.y - p2.y) * (p1.y - p2.y) +
                (p1.z - p2.z) * (p1.z - p2.z));

            return i;
        }

        /// <summary>
        /// 2维中如何计算两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float TwoPointDistance2D (Vector2 p1, Vector2 p2) {

            float i = Mathf.Sqrt ((p1.x - p2.x) * (p1.x - p2.x) +
                (p1.y - p2.y) * (p1.y - p2.y));

            return i;
        }
    }
}