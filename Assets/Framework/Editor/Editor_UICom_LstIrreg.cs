using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;
using LowoUN.Util;

namespace LowoUN.Module.UI.Com {
    [CustomEditor(typeof(UILst_Irreg))]
	public class Editor_UICom_LstIrreg : Editor {
		private SerializedObject   obj;

		private SerializedProperty scrollView;
//		private SerializedProperty isDynamic;

//		private SerializedProperty isUseStaticPos;

		private SerializedProperty isU;
//		private SerializedProperty rows;
		private SerializedProperty uOffset;
		private SerializedProperty isV;
//		private SerializedProperty columns;
		private SerializedProperty vOffset;
		private SerializedProperty itemPanelType;
		private SerializedProperty itemPanelPrefab;
//		private SerializedProperty itemWidth;
//		private SerializedProperty itemHeight;
//		private SerializedProperty offsetEdge;

//		private SerializedProperty posStaticList;

		private SerializedProperty gameObjList;
//		private SerializedProperty isStillShow;
//		private SerializedProperty isArrange4Static;
		private SerializedProperty isURoll4Static;
		private SerializedProperty isVRoll4Static;

//		private SerializedProperty btnPrev;
//		private SerializedProperty btnNext;
//		private SerializedProperty showNum;


//		private SerializedProperty embedLst;
//		private SerializedProperty embedOffset;

//		private SerializedProperty isUCenterAlign;
		private SerializedProperty isPosByFocus;


		private SerializedProperty classID;
		private SerializedProperty typeIdx;
		private string className = "";
		private static Dictionary<string, List<string>> cachePanelClassDict = null;
		private static Dictionary<string, List<string>> cachePanelClassDictReverse = null;
		private static Dictionary<string, Pair<List<string>, List<bool>>> cachePanelAssetDict = null;

		private UILst_Irreg _target;
		public void OnEnable() {
			if (_target == null)
				_target = (UILst_Irreg)target;

			obj = new SerializedObject (target);

			scrollView             = obj.FindProperty("scrollView");

//			isDynamic              = obj.FindProperty("isDynamic");

//			isUseStaticPos         = obj.FindProperty("isUseStaticPos");

			isU                    = obj.FindProperty("isU");
//			rows                   = obj.FindProperty("rows");
			uOffset                = obj.FindProperty("uOffset");
			isV                    = obj.FindProperty("isV");
//			columns                = obj.FindProperty("columns");
			vOffset                = obj.FindProperty("vOffset");
			itemPanelType          = obj.FindProperty("itemPanelType");
			itemPanelPrefab        = obj.FindProperty("itemPanelPrefab");
//			itemWidth              = obj.FindProperty("itemWidth");
//			itemHeight             = obj.FindProperty("itemHeight");
//			offsetEdge             = obj.FindProperty("offsetEdge");

//			posStaticList          = obj.FindProperty("posStaticList");

			gameObjList            = obj.FindProperty("gameObjList");
//			isStillShow            = obj.FindProperty("isStillShow");
//			isArrange4Static       = obj.FindProperty("isArrange4Static");
			isURoll4Static         = obj.FindProperty("isURoll4Static");
			isVRoll4Static         = obj.FindProperty("isVRoll4Static");

//			isUCenterAlign         = obj.FindProperty("isUCenterAlign");
			isPosByFocus           = obj.FindProperty("isPosByFocus");

			classID = obj.FindProperty("classID");
			typeIdx = obj.FindProperty("typeIdx");
			typeIdx.stringValue = System.Enum.GetName(typeof(UIPanelType), itemPanelType.intValue);

			CachePanelAsset();

			if (cachePanelClassDict.ContainsKey(typeIdx.stringValue))
				className = cachePanelClassDict[typeIdx.stringValue][0];
		}

		private int GetIDByIdx (string idx) {
			foreach (int intValue in System.Enum.GetValues(typeof(UIPanelType))) {
				//Debug.Log ("intValue : " + intValue.ToString());
				//Debug.Log ("stringValue : " + System.Enum.GetName(typeof(UIPanelType), intValue));
				if(idx == System.Enum.GetName(typeof(UIPanelType), intValue))
					return intValue;
			}

			return 0;
		}
		//private string GetIdxByID (int id) {
		//	return System.Enum.GetName(typeof(UIPanelType), id);
		//}
		private void CheckScrollView () {
			if (scrollView.objectReferenceValue == null) {
				Transform go = _target.transform.parent.parent;
				if (go != null && go.GetComponent<ScrollRect> () != null) {
					scrollView.objectReferenceValue = _target.transform.parent.parent.gameObject;

					if(go.gameObject.GetComponent<UIScrollFixed> () == null)
						go.gameObject.AddComponent<UIScrollFixed> ();
				}
			}
		}

		public override void OnInspectorGUI() {
			obj.Update ();

			GUILayout.BeginVertical("box");
			CheckScrollView ();
			EditorGUILayout.PropertyField(scrollView);

			EditorGUI.indentLevel += 1;
			 
			{
				//-------------------------------- panel class ---------------------------------
				className = EditorGUILayout.EnumPopup("Panel Class", (UIPanelClass)System.Enum.Parse(typeof(UIPanelClass), className)).ToString();

				if (cachePanelClassDictReverse.ContainsKey(className)) {
					int idx = cachePanelClassDictReverse[className].IndexOf(typeIdx.stringValue);

					idx = Mathf.Min( Mathf.Max(0, idx), cachePanelClassDictReverse[className].Count - 1);
					int idx_new = EditorGUILayout.Popup("Panel Type", idx, cachePanelClassDictReverse[className].ToArray());
					if (idx_new != idx) {
						typeIdx.stringValue = cachePanelClassDictReverse[className][idx_new];
					}
					//EditorGUILayout.BeginHorizontal("box");
					//EditorGUILayout.SelectableLabel(cachePanelClassDictReverse[className][idx_new], GUILayout.Height(EditorGUIUtility.singleLineHeight));
					//EditorGUILayout.EndHorizontal();
				}

				//-------------------------------- panel type ---------------------------------
				itemPanelType.intValue = GetIDByIdx(typeIdx.stringValue);
				//EditorGUILayout.PropertyField (itemPanelType);

				string enumName = itemPanelType.enumNames[itemPanelType.enumValueIndex];
				if (itemPanelType.enumValueIndex > 0 && cachePanelAssetDict.ContainsKey(enumName)) {
					List<string> names = cachePanelAssetDict[enumName].first;
					if (names.Count > 0) {
						int i = names.FindIndex(x => string.Equals(x, itemPanelPrefab.stringValue));
						if (i < 0) {
							i = 0;
						}
						i = EditorGUILayout.Popup( "Item Panel Prefab", i, names.ToArray());
						itemPanelPrefab.stringValue = names[i];
					}
					else {
						itemPanelPrefab.stringValue = "";
						EditorGUILayout.LabelField("No prefab definde for " + enumName);
					}
				}
				else {
					itemPanelPrefab.stringValue = enumName;
				}

				//set position style ---------------------------------------------------------
				GUILayout.BeginVertical ("box");

				{
					GUILayout.BeginVertical ("box");
					if (!isU.boolValue && !isV.boolValue) {
						EditorGUILayout.PropertyField (isU);
						EditorGUILayout.PropertyField (isV);
					}

					if (isU.boolValue) {
						EditorGUILayout.PropertyField (isU);
						EditorGUI.indentLevel += 1;
//						EditorGUILayout.PropertyField (rows);
						EditorGUILayout.PropertyField (uOffset);
						EditorGUI.indentLevel -= 1;
					}

					if (isV.boolValue) {
						EditorGUILayout.PropertyField (isV);
						EditorGUI.indentLevel += 1;
//						EditorGUILayout.PropertyField (columns);
						EditorGUILayout.PropertyField (vOffset);
						EditorGUI.indentLevel -= 1;
					}
					if (scrollView.objectReferenceValue != null) {
						_target.transform.parent.parent.GetComponent<ScrollRect> ().horizontal = isU.boolValue;
						_target.transform.parent.parent.GetComponent<ScrollRect> ().vertical = isV.boolValue;
					}

					GUILayout.EndVertical ();
				}
				GUILayout.EndVertical ();
			}
			EditorGUI.indentLevel -= 1;

			GUILayout.EndVertical();

			//if (isDynamic.boolValue && scrollView.objectReferenceValue == null) {
//			if (scrollView.objectReferenceValue == null) {
//				GUILayout.BeginVertical("box");
//				GUILayout.Label ("Align for horizontal center row!");
//				EditorGUILayout.PropertyField (isUCenterAlign);
//				GUILayout.EndVertical ();
//			}

			if (scrollView.objectReferenceValue != null) {
				GUILayout.BeginVertical("box");
				GUILayout.Label("Set for lst con pos update!");
				EditorGUILayout.PropertyField (isPosByFocus);
				GUILayout.EndVertical ();
			}

			obj.ApplyModifiedProperties ();
		}

		private void CachePanelAsset () {
			//cache panel class ------------------------------------------------
			cachePanelClassDict = new Dictionary<string, List<string>>();
			cachePanelClassDictReverse = new Dictionary<string, List<string>>();

			foreach (FieldInfo field in UIPanelType.None.GetType().GetFields()) {
				object[] objs = field.GetCustomAttributes(typeof(UIPrefabDesc), true);
				if (objs != null && objs.Length > 0)
				{
					List<string> classNames = new List<string>();
					foreach (UIPrefabDesc desc in objs)
					{
						string name = desc.prefabClass.ToString();
						if (!classNames.Contains(name))
							classNames.Add(name);

						if (!cachePanelClassDictReverse.ContainsKey(name)) {
							cachePanelClassDictReverse[name] = new List<string>(){"None"};
						}
						cachePanelClassDictReverse[name].Add(field.Name);
					}
					cachePanelClassDict[field.Name] = classNames;
				}
			}

			if (!cachePanelClassDict.ContainsKey("None")) {
				cachePanelClassDict["None"] = new List<string> { "None" };
			}

			if (!cachePanelClassDictReverse.ContainsKey("None")) {
				cachePanelClassDictReverse["None"] = new List<string> { "None" };
			}

			//cache panel prefab ------------------------------------------------
			cachePanelAssetDict = new Dictionary<string, Pair<List<string>, List<bool>>>();

			foreach (FieldInfo field in UIPanelType.None.GetType().GetFields ()) {
				object[] objs = field.GetCustomAttributes(typeof(UIPrefabDesc), true);
				if (objs != null && objs.Length > 0) {
					List<string> prefabs = new List<string>();
					List<bool> isdialogue = new List<bool>();
					foreach(UIPrefabDesc desc in objs) {
						prefabs.Add(desc.prefabName);
						isdialogue.Add(desc.isDialog);
					}
					cachePanelAssetDict[field.Name] = new Pair<List<string>, List<bool>>(prefabs, isdialogue);
				}
			}
		}
	}
}