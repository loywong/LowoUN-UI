using System;
using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;
using UnityEditor;
using UnityEngine;

namespace LowoUN.Module.UI.Com {
	public static class Editor_Layout_UILst {
		[Flags]
		public enum EditorListOption {
			None = 0,
			ListSize = 1,
			ListLabel = 2,
			ElementLabels = 4,
			Default = ListSize | ListLabel | ElementLabels,
			NoElementLabels = ListSize | ListLabel
		}

		public enum UIHolderListType {
			ObjectList,
			EventList,
			MotionList,
			SystemBuildIn,
		}

		private static GUIContent
		insertContent = new GUIContent ("+", "duplicate this point"),
			deleteContent = new GUIContent ("-", "delete this point"),
			noContent = GUIContent.none;

		private static GUILayoutOption
		buttonWidth = GUILayout.MaxWidth (30f),
			lableMinWidth = GUILayout.Width (60f),
			enumNameMinWidth = GUILayout.MinWidth (120f),
			gameobjMinWidth = GUILayout.MinWidth (90f),
			isExpandWidth = GUILayout.ExpandWidth (true);

		public static void ShowGeneral (SerializedProperty list, bool showListSize = true, bool isForceExpanded = false, int indent = 1) {
			EditorGUILayout.PropertyField (list);
			EditorGUI.indentLevel += indent;

			if (isForceExpanded) {
				list.isExpanded = true;
			}

			if (list.isExpanded) {
				if (showListSize) {
					ModifyLength (list);
				}
				for (int i = 0; i < list.arraySize; i++) {
					EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i));
				}
			}
			EditorGUI.indentLevel -= indent;
		}

		private static int curObjListCount = 0;
		private static int curEvtListCount = 0;
		//private static List<string> nameList;
		private static List<string> objNameList;
		private static List<string> evtNameList;
		public static void Show (SerializedProperty list, bool showListSize = true, UIHolderListType type = UIHolderListType.SystemBuildIn, int panelTypeID = -1, bool isForceExpanded = true, bool showListLabel = false) { //EditorListOption options = EditorListOption.Default
			//bool showListLabel = (options & EditorListOption.ListLabel) != 0;
			//bool showListSize2 = (options & EditorListOption.ListSize) != 0;

			//			if (showListLabel) {
			//				EditorGUILayout.PropertyField (list);
			//				EditorGUI.indentLevel += 1;
			//			}

			list.isExpanded = isForceExpanded;
			if (list.isExpanded) {
				if (showListSize) {
					ModifyLength (list);
				}

				if (type != UIHolderListType.SystemBuildIn) {
					if (type == UIHolderListType.ObjectList) {
						objNameList = GetObjectsNameList ((UIPanelType) panelTypeID);
						if (objNameList != null) {
							curObjListCount = list.arraySize;
							list.arraySize = objNameList.Count;

							ResetForList (list, type, panelTypeID, curObjListCount, isForceExpanded);
						} else {
#if UNITY_EDITOR
							Debug.LogWarning ("====== LowoUN-UI ===> no object Names enum defined for the panel type: " + ((UIPanelType) panelTypeID).ToString ());
#endif
						}
					} else if (type == UIHolderListType.EventList) {
						evtNameList = GetEventsNameList ((UIPanelType) panelTypeID);
						if (evtNameList != null) {
							curEvtListCount = list.arraySize;
							list.arraySize = evtNameList.Count;

							ResetForList (list, type, panelTypeID, curEvtListCount, isForceExpanded);
						} else {
#if UNITY_EDITOR
							Debug.LogWarning ("====== LowoUN-UI ===> no event Names enum defined for the panel type: " + ((UIPanelType) panelTypeID).ToString ());
#endif
						}
					}
				}
			}

			//if (showListLabel) 
			//	EditorGUI.indentLevel -= 1;

		}

		private static void ResetForList (SerializedProperty list, UIHolderListType type, int panelTypeID, int curListCount, bool isForceExpanded = true) {
			for (int i = 0; i < list.arraySize; i++) {

				//if (showListLabel) {
				//	EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i));
				//}

				SerializedProperty item = list.GetArrayElementAtIndex (i); // this is a UIObject instance
				item.isExpanded = isForceExpanded;
				//Debug.LogError ("item.type:" + item.type);
				//Debug.LogError ("item.GetType():" + item.GetType());
				//Debug.Log ("item.propertyType:" + item.propertyType);
				//Debug.LogError ("item.type:" + item.enumNames);

				//if (item.FindPropertyRelative ("obj").objectReferenceValue != null)
				//	Debug.Log ("====== LowoUN-UI ===> list game object:" + item.FindPropertyRelative ("obj").objectReferenceValue.ToString ());

				if (type != UIHolderListType.SystemBuildIn) {
					//if (item.isExpanded) {

					if (type == UIHolderListType.ObjectList) {
						if (i >= curListCount)
							item.FindPropertyRelative ("obj").objectReferenceValue = null;
						ShowObjectListType (item, panelTypeID, i);
					} else if (type == UIHolderListType.EventList) {
						if (i >= curListCount)
							item.FindPropertyRelative ("obj").objectReferenceValue = null;
						ShowEventListType (item, panelTypeID, i);
					}
					//else if(type == UIHolderListType.MotionList)
					//	ShowMotionListType (item);

					//}
				}
			}
		}

		private static void ShowObjectListType (SerializedProperty item /*a UIObject instance*/ , int panelTypeID = -1, int idx = -1) {
			if (objNameList == null || objNameList.Count == 0)
				return;
			else {
				if (string.IsNullOrEmpty (objNameList[idx]) || objNameList[idx] == "None")
					return;
			}

			item.FindPropertyRelative ("id").intValue = idx; //SetObjectEnum(item, (UIPanelIns)panelTypeID);

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginHorizontal ("box");
			EditorGUILayout.LabelField (objNameList[idx], enumNameMinWidth, isExpandWidth);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ("box");
			//GUILayout.Label ("object instance");
			EditorGUILayout.PropertyField (item.FindPropertyRelative ("obj"), noContent, gameobjMinWidth, isExpandWidth);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndHorizontal ();
		}

		private static int tempEnumInt;
		private static void ShowEventListType (SerializedProperty item, int panelTypeID = -1, int idx = -1) //string eventName = ""
		{
			if (evtNameList == null || evtNameList.Count == 0)
				return;
			else {
				//if (evtNameList [idx] == null || evtNameList [idx] == "None")
				if (string.IsNullOrEmpty (evtNameList[idx]) || evtNameList[idx] == "None")
					return;
			}

			//Debug.LogError ("++++++++++ : " + UIHandlerHub.instance.uiEventsEnumDict [(UIPanelIns)panelTypeID].GetType ());
			//item.FindPropertyRelative ("_eventID").intValue = (int)tempEventEnum<UIHandlerHub.instance.uiEventsEnumDict [(UIPanelIns)panelTypeID].GetType ()>(item);//, panelTypeID
			item.FindPropertyRelative ("_eventID").intValue = idx; //SetEventEnum(item, (UIPanelIns)panelTypeID);
			//item.FindPropertyRelative ("_eventID").intValue = UIHandlerHub.instance.SetInspectorEventEnum(item, (UIPanelIns)panelTypeID);

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginHorizontal ("box");
			//GUILayout.Label ("object instance");
			EditorGUILayout.LabelField (evtNameList[idx], enumNameMinWidth, isExpandWidth);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ("box");
			EditorGUILayout.PropertyField (item.FindPropertyRelative ("obj"), noContent, gameobjMinWidth, isExpandWidth);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndHorizontal ();
		}

		//		private static List<string> objNameList;
		//		private static List<string> evtNameList;
		private static Type t = typeof (UIHandler);
		private static List<string> GetObjectsNameList (UIPanelType panelTypeID) //SerializedProperty item,
		{
			List<string> objNameList = new List<string> ();

			//object[] parameters;
			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (ObjsAtt4UIInspector att in info.GetCustomAttributes (typeof (ObjsAtt4UIInspector), false)) {
					if (panelTypeID == att.uiPanelType) {
						//parameters = new object[]{ item };

						if (info != null)
							objNameList = info.Invoke (UIHandler.instance, null) as List<string>;
					}
				}
			}

			return objNameList;
		}

		private static List<string> GetEventsNameList (UIPanelType panelTypeID) //SerializedProperty item, 
		{
			List<string> evtNameList = new List<string> ();

			foreach (MethodInfo info in t.GetMethods ()) {
				foreach (EvtsAtt4UIInspector att in info.GetCustomAttributes (typeof (EvtsAtt4UIInspector), false)) {
					if (panelTypeID == att.uiPanelType) {
						if (info != null)
							evtNameList = info.Invoke (UIHandler.instance, null) as List<string>;
					}
				}
			}

			return evtNameList;
		}

		//		private static void ShowMotionListType (SerializedProperty item)
		//		{
		//			EditorGUI.indentLevel += 1;
		//			EditorGUILayout.BeginHorizontal ();
		//			EditorGUILayout.LabelField ("state", lableMinWidth);
		//			EditorGUILayout.PropertyField (item.FindPropertyRelative ("state"), noContent);
		//			EditorGUILayout.EndHorizontal ();
		//
		//			EditorGUILayout.BeginHorizontal ();
		//			EditorGUILayout.LabelField ("type", lableMinWidth);
		//			EditorGUILayout.PropertyField (item.FindPropertyRelative ("type"), noContent);
		//			EditorGUILayout.EndHorizontal ();
		//			EditorGUI.indentLevel -= 1;
		//		}

		private static void ModifyLength (SerializedProperty list) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Length", lableMinWidth);
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"), noContent);

			if (GUILayout.Button (insertContent, EditorStyles.miniButtonMid, buttonWidth)) {
				list.InsertArrayElementAtIndex (list.arraySize);
			}
			if (GUILayout.Button (deleteContent, EditorStyles.miniButtonRight, buttonWidth)) {
				list.DeleteArrayElementAtIndex (list.arraySize - 1);
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
}