using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Module.UI.Com {
	[CustomEditor (typeof (UIScrollFixed))]
	public class Editor_UICom_ScrollFixed : Editor {

		private SerializedObject obj;

		//private SerializedProperty isDynamic;
		private SerializedProperty isShowScrollbar;
		//private SerializedProperty listCon;

		UIScrollFixed _target;
		public void OnEnable () {
			//if (_target == null)
			//	_target = (UIScrollFixed)target;
			_target = (UIScrollFixed) target;

			obj = new SerializedObject (target);

			isShowScrollbar = obj.FindProperty ("isShowScrollbar");
			//listCon       = obj.FindProperty("listCon");

			if (_target != null && _target.transform.GetComponent<Image> () != null)
				_target.transform.GetComponent<Image> ().raycastTarget = false;
		}

		public override void OnInspectorGUI () {
			obj.Update ();

			GUILayout.BeginVertical ("box");
			EditorGUILayout.PropertyField (isShowScrollbar);
			//GUILayout.Label(">>> Assign the list");
			//EditorGUILayout.PropertyField(listCon);
			GUILayout.EndVertical ();

			obj.ApplyModifiedProperties ();
		}
	}
}