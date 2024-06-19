using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.UI.HudAction {
	[CustomEditor (typeof (UHUDAction))]
	public class UHUDActionEffectEditor : Editor {
		private UHUDAction m_Target = null;

		public void OnEnable () {
			if (this.m_Target == null) {
				this.m_Target = (UHUDAction) target;
			}
		}

		public override void OnInspectorGUI () {
			GUI.enabled = false;
			GUILayout.BeginVertical ("box");
			GUILayout.Label ("Reference transform where texts are instantiated");
			GUI.enabled = true;
			this.m_Target.CanvasParent = EditorGUILayout.ObjectField ("Canvas Parent", this.m_Target.CanvasParent, typeof (Transform), true) as Transform;
			this.m_Target.ImagePrefab = EditorGUILayout.ObjectField ("Image Prefab", this.m_Target.ImagePrefab, typeof (GameObject), true) as GameObject;
			GUI.enabled = false;
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ("box");
			GUILayout.Label ("Customization");
			GUI.enabled = true;
			this.m_Target.FadeSpeed = EditorGUILayout.Slider ("Fade Speed", this.m_Target.FadeSpeed, 0, 50);
			this.m_Target.FloatingSpeed = EditorGUILayout.Slider ("Floating Speed", this.m_Target.FloatingSpeed, 0, 100);
			this.m_Target.HideDistance = EditorGUILayout.Slider ("Hide Distance", this.m_Target.HideDistance, 0, 500);
			this.m_Target.MaxViewAngle = EditorGUILayout.Slider ("Max View Angle", this.m_Target.MaxViewAngle, 0, 180);
			this.m_Target.DestroyTextOnDeath = EditorGUILayout.ToggleLeft ("Destroy Text On Death", this.m_Target.DestroyTextOnDeath);
			GUI.enabled = false;
			GUILayout.EndVertical ();
		}
	}
}