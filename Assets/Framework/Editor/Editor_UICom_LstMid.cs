using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;
using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.UI.Com {
	[CustomEditor (typeof (UILst_Mid))]
	public class Editor_UICom_LstMid : Editor {

		private SerializedObject obj;

		private SerializedProperty isU;
		//		private SerializedProperty rows;
		private SerializedProperty uOffset;
		private SerializedProperty isV;
		//		private SerializedProperty columns;
		private SerializedProperty vOffset;
		private SerializedProperty itemPanelType;
		private SerializedProperty itemPanelPrefab;
		private SerializedProperty itemWidth;
		private SerializedProperty itemHeight;

		private SerializedProperty interval4Anim;
		private SerializedProperty delay4Anim;
		//		private SerializedProperty group4Anim;

		private SerializedProperty classID;
		private SerializedProperty typeIdx;
		private string className = "";
		private static Dictionary<string, List<string>> cachePanelClassDict = null;
		private static Dictionary<string, List<string>> cachePanelClassDictReverse = null;
		private static Dictionary<string, Pair<List<string>, List<bool>>> cachePanelAssetDict = null;

		private UILst_Mid _target;
		public void OnEnable () {
			if (_target == null)
				_target = (UILst_Mid) target;

			obj = new SerializedObject (target);

			isU 			= obj.FindProperty ("isU");
			// rows			= obj.FindProperty("rows");
			uOffset 		= obj.FindProperty ("uOffset");
			isV 			= obj.FindProperty ("isV");
			// columns		= obj.FindProperty("columns");
			vOffset 		= obj.FindProperty ("vOffset");
			itemPanelType 	= obj.FindProperty ("itemPanelType");
			itemPanelPrefab = obj.FindProperty ("itemPanelPrefab");
			itemWidth 		= obj.FindProperty ("itemWidth");
			itemHeight 		= obj.FindProperty ("itemHeight");

			/////////////////////////////////////////////////////////////
			/// animation
			interval4Anim 	= obj.FindProperty ("interval4Anim");
			delay4Anim 		= obj.FindProperty ("delay4Anim");
			// group4Anim		= obj.FindProperty("group4Anim");

			classID = obj.FindProperty ("classID");
			typeIdx = obj.FindProperty ("typeIdx");
			typeIdx.stringValue = System.Enum.GetName (typeof (UIPanelType), itemPanelType.intValue);

			CachePanelAsset ();

			if (cachePanelClassDict.ContainsKey (typeIdx.stringValue))
				className = cachePanelClassDict[typeIdx.stringValue][0];
		}

		private int GetIDByIdx (string idx) {
			foreach (int intValue in System.Enum.GetValues (typeof (UIPanelType))) {
				//Debug.Log ("intValue : " + intValue.ToString());
				//Debug.Log ("stringValue : " + System.Enum.GetName(typeof(UIPanelType), intValue));
				if (idx == System.Enum.GetName (typeof (UIPanelType), intValue))
					return intValue;
			}

			return 0;
		}

		public override void OnInspectorGUI () {
			obj.Update ();

			GUILayout.BeginVertical ("box");

			//-------------------------------- panel class ---------------------------------
			className = EditorGUILayout.EnumPopup ("Panel Class", (UIPanelClass) System.Enum.Parse (typeof (UIPanelClass), className)).ToString ();

			if (cachePanelClassDictReverse.ContainsKey (className)) {
				int idx = cachePanelClassDictReverse[className].IndexOf (typeIdx.stringValue);

				idx = Mathf.Min (Mathf.Max (0, idx), cachePanelClassDictReverse[className].Count - 1);
				int idx_new = EditorGUILayout.Popup ("Panel Type", idx, cachePanelClassDictReverse[className].ToArray ());
				if (idx_new != idx)
					typeIdx.stringValue = cachePanelClassDictReverse[className][idx_new];
			}

			//-------------------------------- panel type ---------------------------------
			itemPanelType.intValue = GetIDByIdx (typeIdx.stringValue);
			//EditorGUILayout.PropertyField (itemPanelType);

			string enumName = itemPanelType.enumNames[itemPanelType.enumValueIndex];
			if (itemPanelType.enumValueIndex > 0 && cachePanelAssetDict.ContainsKey (enumName)) {
				List<string> names = cachePanelAssetDict[enumName].first;
				if (names.Count > 0) {
					int i = names.FindIndex (x => string.Equals (x, itemPanelPrefab.stringValue));
					if (i < 0) {
						i = 0;
					}
					i = EditorGUILayout.Popup ("Item Panel Prefab", i, names.ToArray ());
					itemPanelPrefab.stringValue = names[i];
				} else {
					itemPanelPrefab.stringValue = "";
					EditorGUILayout.LabelField ("No prefab definde for " + enumName);
				}
			} else {
				itemPanelPrefab.stringValue = enumName;
			}

			//set position style ---------------------------------------------------------
			GUILayout.BeginVertical ("box");

			EditorGUILayout.PropertyField (itemWidth);
			EditorGUILayout.PropertyField (itemHeight);

			GUILayout.BeginVertical ("box");
			if (!isU.boolValue && !isV.boolValue) {
				EditorGUILayout.PropertyField (isU);
				EditorGUILayout.PropertyField (isV);
			}

			if (isU.boolValue) {
				EditorGUILayout.PropertyField (isU);
				EditorGUI.indentLevel += 1;
				//				EditorGUILayout.PropertyField (rows);
				EditorGUILayout.PropertyField (uOffset);
				EditorGUI.indentLevel -= 1;
			}

			if (isV.boolValue) {
				EditorGUILayout.PropertyField (isV);
				EditorGUI.indentLevel += 1;
				//				EditorGUILayout.PropertyField (columns);
				EditorGUILayout.PropertyField (vOffset);
				EditorGUI.indentLevel -= 1;
			}

			GUILayout.EndVertical ();
			GUILayout.EndVertical ();
			GUILayout.EndVertical ();

			GUILayout.BeginVertical ("box");
			GUILayout.Label ("Set for item animation!");
			EditorGUILayout.PropertyField (interval4Anim);
			EditorGUILayout.PropertyField (delay4Anim);
			//			EditorGUILayout.PropertyField (group4Anim);
			GUILayout.EndVertical ();

			obj.ApplyModifiedProperties ();
		}

		private void CachePanelAsset () {
			//cache panel class ------------------------------------------------
			cachePanelClassDict = new Dictionary<string, List<string>> ();
			cachePanelClassDictReverse = new Dictionary<string, List<string>> ();

			foreach (FieldInfo field in UIPanelType.None.GetType ().GetFields ()) {
				object[] objs = field.GetCustomAttributes (typeof (UIPrefabDesc), true);
				if (objs != null && objs.Length > 0) {
					List<string> classNames = new List<string> ();
					foreach (UIPrefabDesc desc in objs) {
						string name = desc.prefabClass.ToString ();
						if (!classNames.Contains (name))
							classNames.Add (name);

						if (!cachePanelClassDictReverse.ContainsKey (name)) {
							cachePanelClassDictReverse[name] = new List<string> () { "None" };
						}
						cachePanelClassDictReverse[name].Add (field.Name);
					}
					cachePanelClassDict[field.Name] = classNames;
				}
			}
			if (!cachePanelClassDict.ContainsKey ("None")) {
				cachePanelClassDict["None"] = new List<string> { "None" };
			}
			if (!cachePanelClassDictReverse.ContainsKey ("None")) {
				cachePanelClassDictReverse["None"] = new List<string> { "None" };
			}

			//cache panel prefab ------------------------------------------------
			cachePanelAssetDict = new Dictionary<string, Pair<List<string>, List<bool>>> ();

			foreach (FieldInfo field in UIPanelType.None.GetType ().GetFields ()) {
				object[] objs = field.GetCustomAttributes (typeof (UIPrefabDesc), true);
				if (objs != null && objs.Length > 0) {
					List<string> prefabs = new List<string> ();
					List<bool> isdialogue = new List<bool> ();
					foreach (UIPrefabDesc desc in objs) {
						prefabs.Add (desc.prefabName);
						isdialogue.Add (desc.isDialog);
					}
					cachePanelAssetDict[field.Name] = new Pair<List<string>, List<bool>> (prefabs, isdialogue);
				}
			}
		}
	}
}