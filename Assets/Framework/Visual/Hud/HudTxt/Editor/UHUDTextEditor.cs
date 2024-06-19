using UnityEditor;

namespace LowoUN.Module.UI.HUDText {
	//[CustomEditor(typeof(UHUDText))]
	public class UHUDTextEditor : Editor {
		//		private UHUDText m_Target = null;
		//
		//	    public void OnEnable()
		//	    {
		//	        if (m_Target == null)
		//	        {
		//				m_Target = (UHUDText)target;
		//	        }
		//	    }
		//
		//	    public override void OnInspectorGUI()
		//	    {
		//	        GUI.enabled = false;
		//	        GUILayout.BeginVertical("box");
		//	        GUILayout.Label("Reference transform where texts are instantiated");
		//	        GUI.enabled = true;
		//	        m_Target.CanvasParent = EditorGUILayout.ObjectField("Canvas Parent", m_Target.CanvasParent, typeof(Transform), true) as Transform;
		//	        m_Target.TextConPrefab = EditorGUILayout.ObjectField("TextCon Prefab", m_Target.TextConPrefab, typeof(GameObject), true) as GameObject;
		//	        GUI.enabled = false;
		//	        GUILayout.EndVertical();
		//	        GUILayout.BeginVertical("box");
		//	        GUILayout.Label("Customization");
		//	        GUI.enabled = true;
		//	        //this.m_Target.FadeSpeed = EditorGUILayout.Slider("Fade Speed", this.m_Target.FadeSpeed, 0, 50);
		//	        //this.m_Target.FloatingSpeed = EditorGUILayout.Slider("Floating Speed", this.m_Target.FloatingSpeed, 0, 100);
		//	        //this.m_Target.HideDistance = EditorGUILayout.Slider("Hide Distance", this.m_Target.HideDistance, 0, 500);
		//	        //this.m_Target.MaxViewAngle = EditorGUILayout.Slider("Max View Angle", this.m_Target.MaxViewAngle, 0, 180);
		//	        m_Target.FontSize = EditorGUILayout.IntField("Default Font Size", m_Target.FontSize);
		//	        m_Target.DestroyTextOnDeath = EditorGUILayout.ToggleLeft("Destroy Text On Death", m_Target.DestroyTextOnDeath);
		//	        GUI.enabled = false;
		//	        GUILayout.EndVertical();
		//	    }
	}
}