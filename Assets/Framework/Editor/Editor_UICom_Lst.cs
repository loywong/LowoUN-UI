using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;
using LowoUN.Util;

namespace LowoUN.Module.UI.Com {
    [CustomEditor(typeof(UIList))]
	public class Editor_UICom_Lst : Editor {
		//Layout ///////////////////////////////////////////////////////////////
		private enum UIEnum_LstTyp {
			None,
			General,
			Static,
			Mid,
			Irreg,
			Fold,
		}
		private UIList uilst;
		private RectTransform lstRT;
		private UIEnum_LstTyp _lsttyp;
		private UIEnum_LstTyp lsttyp {
			get { return _lsttyp;}
			set { 
				_lsttyp = value;

				if (value == UIEnum_LstTyp.General) {
					lstRT.pivot = new Vector2 (0, 1);
					lstRT.anchorMax = new Vector2 (0, 1);
					lstRT.anchorMin = new Vector2 (0, 1);
				}
			}
		}
		////////////////////////////////////////////////////////////////////////

		private SerializedObject   obj;

		private SerializedProperty scrollView;
		private bool isDynamic;

		private SerializedProperty isUseStaticPos;

		private SerializedProperty isU;
		private SerializedProperty rows;
		private SerializedProperty uOffset;
		private SerializedProperty isV;
		private SerializedProperty columns;
		private SerializedProperty vOffset;
		private SerializedProperty itemPanelType;
		private SerializedProperty itemPanelPrefab;
		private SerializedProperty itemWidth;
		private SerializedProperty itemHeight;
		private SerializedProperty offsetEdge;

		private SerializedProperty posStaticList;

		//private SerializedProperty gameObjList;
		//private SerializedProperty isStillShow;
//		private SerializedProperty isArrange4Static;
		private SerializedProperty isURoll4Static;
		private SerializedProperty isVRoll4Static;
		//private SerializedProperty isInteractive;
		//private SerializedProperty isNeedToggleBtnEvent;

		private SerializedProperty btnPrev;
		private SerializedProperty btnNext;
		private SerializedProperty showNum;


		private SerializedProperty embedLst;
		private SerializedProperty embedOffset;

		private SerializedProperty isUCenterAlign;
		private SerializedProperty isPosByFocus;

		private SerializedProperty interval4Anim;
		private SerializedProperty delay4Anim;
		private SerializedProperty group4Anim;

		//HACK
		private SerializedProperty isForceReloadItems;

		private SerializedProperty classID;
		private SerializedProperty typeIdx;
		private string className = "";
		private static Dictionary<string, List<string>> cachePanelClassDict = null;
		private static Dictionary<string, List<string>> cachePanelClassDictReverse = null;
		private static Dictionary<string, Pair<List<string>, List<bool>>> cachePanelAssetDict = null;

		private UIList _target;
		public void OnEnable() {
			uilst = target as UIList;
			lstRT = uilst.GetComponent<RectTransform> ();

			if (_target == null)
				_target = (UIList)target;

			obj = new SerializedObject (target);

			scrollView             = obj.FindProperty("scrollView");

			// isDynamic              = obj.FindProperty("isDynamic");

			isUseStaticPos         = obj.FindProperty("isUseStaticPos");

			isU                    = obj.FindProperty("isU");
			rows                   = obj.FindProperty("rows");
			uOffset                = obj.FindProperty("uOffset");
			isV                    = obj.FindProperty("isV");
			columns                = obj.FindProperty("columns");
			vOffset                = obj.FindProperty("vOffset");
			itemPanelType          = obj.FindProperty("itemPanelType");
			itemPanelPrefab        = obj.FindProperty("itemPanelPrefab");
			itemWidth              = obj.FindProperty("itemWidth");
			itemHeight             = obj.FindProperty("itemHeight");
			offsetEdge             = obj.FindProperty("offsetEdge");

			posStaticList          = obj.FindProperty("posStaticList");

			// gameObjList            = obj.FindProperty("gameObjList");
			// isStillShow            = obj.FindProperty("isStillShow");
			// isArrange4Static       = obj.FindProperty("isArrange4Static");
			isURoll4Static         = obj.FindProperty("isURoll4Static");
			isVRoll4Static         = obj.FindProperty("isVRoll4Static");

			btnPrev                = obj.FindProperty("btnPrev");
			btnNext                = obj.FindProperty("btnNext");
			showNum                = obj.FindProperty("showNum");

			embedLst               = obj.FindProperty("embedLst");
			embedOffset            = obj.FindProperty("embedOffset");

			isUCenterAlign         = obj.FindProperty("isUCenterAlign");
			isPosByFocus           = obj.FindProperty("isPosByFocus");

			/////////////////////////////////////////////////////////////
			/// animation
			interval4Anim          = obj.FindProperty("interval4Anim");
			delay4Anim             = obj.FindProperty("delay4Anim");
			group4Anim             = obj.FindProperty("group4Anim");

			isForceReloadItems     = obj.FindProperty("isForceReloadItems");

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
			lsttyp = (UIEnum_LstTyp)EditorGUILayout.EnumPopup ("Layout", lsttyp);

			obj.Update ();

			GUILayout.BeginVertical("box");
			GUILayout.Label("Choose to set for a static or dynamic list");
			//EditorGUILayout.PropertyField(isDynamic);
			GUILayout.EndVertical();

			GUILayout.BeginVertical("box");
			CheckScrollView ();
			EditorGUILayout.PropertyField(scrollView);

			EditorGUI.indentLevel += 1;

			//if (!isDynamic.boolValue) {
			//	if (scrollView.objectReferenceValue != null) {
			//		GUILayout.BeginVertical("box");
			//		//if (!isURoll4Static.boolValue && !isVRoll4Static.boolValue) {
			//		//	EditorGUILayout.PropertyField (isURoll4Static);
			//		//	EditorGUILayout.PropertyField (isVRoll4Static);
			//		//}
			//		//if (isURoll4Static.boolValue) {
			//			EditorGUILayout.PropertyField (isURoll4Static);
			//		//}
			//		//if (isVRoll4Static.boolValue) {
			//			EditorGUILayout.PropertyField (isVRoll4Static);

			//		_target.transform.parent.parent.GetComponent<ScrollRect> ().horizontal = isURoll4Static.boolValue;
			//		_target.transform.parent.parent.GetComponent<ScrollRect> ().vertical = isVRoll4Static.boolValue;

			//		//}
			//		GUILayout.EndVertical();
			//	}


				////EditorGUILayout.PropertyField (gameObjList);
				//Editor_Layout_UILst.ShowGeneral(gameObjList, true, true, 0);
				//EditorGUILayout.PropertyField(isStillShow);


				//TODO [LowoUN-UI]: arrange items for the static list
//				EditorGUILayout.PropertyField(isArrange4Static);
//				if(isArrange4Static.boolValue) {
//					GUILayout.BeginVertical("box");
//					if (!isU.boolValue && !isV.boolValue) {
//						EditorGUILayout.PropertyField (isU);
//						EditorGUILayout.PropertyField (isV);
//					}
//
//					if (isU.boolValue) {
//						EditorGUILayout.PropertyField (isU);
//						EditorGUI.indentLevel += 1;
//						EditorGUILayout.PropertyField (rows);
//						EditorGUILayout.PropertyField (uOffset);
//						EditorGUI.indentLevel -= 1;
//					}
//
//					if (isV.boolValue) {
//						EditorGUILayout.PropertyField (isV);
//						EditorGUI.indentLevel += 1;
//						EditorGUILayout.PropertyField (columns);
//						EditorGUILayout.PropertyField (vOffset);
//						EditorGUI.indentLevel -= 1;
//					}
//
//					GUILayout.EndVertical();
//				}
//			}
//			else 
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

				EditorGUILayout.PropertyField (itemWidth);
				EditorGUILayout.PropertyField (itemHeight);

				EditorGUILayout.PropertyField (isUseStaticPos);

				if (isUseStaticPos.boolValue) {
					Editor_Layout_UILst.ShowGeneral(posStaticList, true, true, 0);
				}
				else {
					GUILayout.BeginVertical ("box");
					if (!isU.boolValue && !isV.boolValue) {
						EditorGUILayout.PropertyField (isU);
						EditorGUILayout.PropertyField (isV);
					}

					if (isU.boolValue) {
						EditorGUILayout.PropertyField (isU);
						EditorGUI.indentLevel += 1;
						EditorGUILayout.PropertyField (rows);
						EditorGUILayout.PropertyField (uOffset);
						EditorGUI.indentLevel -= 1;
					}

					if (isV.boolValue) {
						EditorGUILayout.PropertyField (isV);
						EditorGUI.indentLevel += 1;
						EditorGUILayout.PropertyField (columns);
						EditorGUILayout.PropertyField (vOffset);
						EditorGUI.indentLevel -= 1;
					}
					if (scrollView.objectReferenceValue != null) {
						_target.transform.parent.parent.GetComponent<ScrollRect> ().horizontal = isU.boolValue;
						_target.transform.parent.parent.GetComponent<ScrollRect> ().vertical = isV.boolValue;
					}

					EditorGUILayout.PropertyField (offsetEdge);

					GUILayout.EndVertical ();
				}
				GUILayout.EndVertical ();
			}
			EditorGUI.indentLevel -= 1;

			GUILayout.EndVertical();

			//if (isDynamic.boolValue && scrollView.objectReferenceValue != null) {
			if (scrollView.objectReferenceValue != null) {
				GUILayout.BeginVertical ("box");
				GUILayout.Label ("Set for check the next invisible item");
				EditorGUILayout.PropertyField (btnPrev);
				EditorGUILayout.PropertyField (btnNext);
				EditorGUILayout.PropertyField (showNum);
				GUILayout.EndVertical ();

				GUILayout.BeginVertical ("box");
				GUILayout.Label ("!!! Special structure");
				EditorGUILayout.PropertyField (embedLst);
				EditorGUILayout.PropertyField (embedOffset);
				GUILayout.EndVertical ();
			}


			//if (isDynamic.boolValue && scrollView.objectReferenceValue == null) {
			if (scrollView.objectReferenceValue == null) {
				GUILayout.BeginVertical("box");
				GUILayout.Label ("Align for horizontal center row!");
				EditorGUILayout.PropertyField (isUCenterAlign);
				GUILayout.EndVertical ();
			}

			if (scrollView.objectReferenceValue != null) {
				GUILayout.BeginVertical("box");
				GUILayout.Label("Set for lst con pos update!");
				EditorGUILayout.PropertyField (isPosByFocus);
				GUILayout.EndVertical ();
			}

			GUILayout.BeginVertical("box");
			GUILayout.Label ("Set for item animation!");
			EditorGUILayout.PropertyField (interval4Anim);
			EditorGUILayout.PropertyField (delay4Anim);
			EditorGUILayout.PropertyField (group4Anim);
			GUILayout.EndVertical ();

			EditorGUILayout.PropertyField (isForceReloadItems);

			obj.ApplyModifiedProperties ();
		}

		private void CachePanelAsset () {
			//cache panel class ------------------------------------------------
			cachePanelClassDict = new Dictionary<string, List<string>> ();
			cachePanelClassDictReverse = new Dictionary<string, List<string>> ();

			foreach (FieldInfo field in UIPanelType.None.GetType().GetFields())	{
				object[] objs = field.GetCustomAttributes(typeof(UIPrefabDesc), true);
				if (objs != null && objs.Length > 0) {
					List<string> classNames = new List<string>();
					foreach (UIPrefabDesc desc in objs) {
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
			cachePanelAssetDict = new Dictionary<string, Pair<List<string>, List<bool>>> ();

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