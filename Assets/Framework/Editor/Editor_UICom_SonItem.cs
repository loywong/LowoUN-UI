using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;
using LowoUN.Util;
using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.UI.Com {
	[CustomEditor (typeof (UISonItem))]
	public class Editor_UICom_SonItem : Editor {
		private SerializedObject obj;

		private SerializedProperty itemPanelType;
		private SerializedProperty itemPanelPrefab;
		private SerializedProperty itemWidth;
		private SerializedProperty itemHeight;

		private SerializedProperty classID;
		private SerializedProperty typeIdx;
		private string className = "";
		private static Dictionary<string, List<string>> cachePanelClassDict = null;
		private static Dictionary<string, List<string>> cachePanelClassDictReverse = null;
		private static Dictionary<string, Pair<List<string>, List<bool>>> cachePanelAssetDict = null;

		private UISonItem _target;
		public void OnEnable () {
			if (_target == null)
				_target = (UISonItem) target;

			obj = new SerializedObject (target);

			itemPanelType = obj.FindProperty ("itemPanelType");
			itemPanelPrefab = obj.FindProperty ("itemPanelPrefab");
			itemWidth = obj.FindProperty ("itemWidth");
			itemHeight = obj.FindProperty ("itemHeight");

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

			EditorGUI.indentLevel += 1;

			//-------------------------------- panel class ---------------------------------
			className = EditorGUILayout.EnumPopup ("Panel Class", (UIPanelClass) System.Enum.Parse (typeof (UIPanelClass), className)).ToString ();

			if (cachePanelClassDictReverse.ContainsKey (className)) {
				int idx = cachePanelClassDictReverse[className].IndexOf (typeIdx.stringValue);

				idx = Mathf.Min (Mathf.Max (0, idx), cachePanelClassDictReverse[className].Count - 1);
				int idx_new = EditorGUILayout.Popup ("Panel Type", idx, cachePanelClassDictReverse[className].ToArray ());
				if (idx_new != idx) {
					typeIdx.stringValue = cachePanelClassDictReverse[className][idx_new];
				}
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
					EditorGUILayout.LabelField ("No prefab define for " + enumName);
				}
			} else {
				itemPanelPrefab.stringValue = enumName;
			}

			//set position style ---------------------------------------------------------
			GUILayout.BeginVertical ("box");

			EditorGUILayout.PropertyField (itemWidth);
			EditorGUILayout.PropertyField (itemHeight);

			GUILayout.EndVertical ();
			EditorGUI.indentLevel -= 1;

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