using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using LowoUN.Business.UI;

namespace LowoUN.Module.UI.Com 
{
	[CustomEditor(typeof(UIHolder))]
	public class Editor_UIHolder : Editor {
		//Layout ///////////////////////////////////////////////////////////////
		private enum UIEnum_HolderTyp {
			None,
			General,
			Item_Lst,
			Item_Son,
			Item_General,
		}
		private UIHolder uiholder;
		private RectTransform uiholderRT;
		private UIEnum_HolderTyp _holdertyp;
		private UIEnum_HolderTyp holdertyp{
			get{ return _holdertyp;}
			set{ 
				_holdertyp = value;

				if (value == UIEnum_HolderTyp.Item_Lst || value == UIEnum_HolderTyp.Item_Son) {
					uiholderRT.pivot = new Vector2 (0.5f, 0.5f);
					uiholderRT.anchorMax = new Vector2 (0, 1);
					uiholderRT.anchorMin = new Vector2 (0, 1);
				} else if (value == UIEnum_HolderTyp.General|| value == UIEnum_HolderTyp.Item_General) {
					uiholderRT.pivot = new Vector2 (0.5f, 0.5f);
					uiholderRT.anchorMin = new Vector2 (0, 0);
					uiholderRT.anchorMax = new Vector2 (1, 1);
					uiholderRT.sizeDelta = Vector2.zero;
					uiholderRT.anchoredPosition = Vector2.zero;
				}
			}
		}
		////////////////////////////////////////////////////////////////////////

		private SerializedObject obj;
		private SerializedProperty classID;
		private SerializedProperty typeID;
		private SerializedProperty typeIdx;
		private SerializedProperty objectList;
		private SerializedProperty eventList;
        //private SerializedProperty motionList;

        private string className = "";
        private static Dictionary<string, List<string>> cachePanelClassDict = null;
        private static Dictionary<string, List<string>> cachePanelClassDictReverse = null;

		public void OnEnable()
		{
			uiholder = target as UIHolder;
			uiholderRT = uiholder.GetComponent<RectTransform> ();

			obj = new SerializedObject (target);

//			holdertyp = obj.FindProperty("holdertyp");
			classID = obj.FindProperty("classID");

			typeID = obj.FindProperty("typeID");
			objectList = obj.FindProperty("objectList");
			eventList = obj.FindProperty("eventList");
			//motionList = obj.FindProperty("motionList");

			typeIdx = obj.FindProperty("typeIdx");
			typeIdx.stringValue = System.Enum.GetName(typeof(UIPanelType), typeID.intValue);
            //Debug.Log ("holder string value : " + typeIdx.stringValue);

            CachePanelAsset();

            if (cachePanelClassDict.ContainsKey(typeIdx.stringValue))
                className = cachePanelClassDict[typeIdx.stringValue][0];
		}

		private int GetIDByIdx (string idx) {
			foreach (int intValue in System.Enum.GetValues(typeof(UIPanelType))) {
				//Debug.Log ("intValue : " + intValue.ToString());
				//Debug.Log ("stringValue : " + System.Enum.GetName(typeof(UIPanelIns), intValue));

				if(idx == System.Enum.GetName(typeof(UIPanelType), intValue))
					return intValue;
			}

			return 0;
		}

		public override void OnInspectorGUI()
		{
			//this._target.typeID = (UIPanelIns)EditorGUILayout.EnumPopup("UI Panel Type", this._target.typeID);
			holdertyp = (UIEnum_HolderTyp)EditorGUILayout.EnumPopup ("Layout", holdertyp);

			obj.Update ();

			//EditorGUILayout.EnumPopup("UI Panel Type", this._target.typeID);
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.Label("UI Panel Type");
            GUILayout.Label("Instance ID: " + obj.FindProperty("insID").intValue);
            GUILayout.EndHorizontal();

            
			//EditorGUILayout.PropertyField(typeID);
            //EditorGUILayout.EnumPopup("Panel Class", )
            className = EditorGUILayout.EnumPopup("Panel Class", (UIPanelClass)System.Enum.Parse(typeof(UIPanelClass), className)).ToString();

            if (cachePanelClassDictReverse.ContainsKey(className))
            {
                int idx = cachePanelClassDictReverse[className].IndexOf(typeIdx.stringValue);

                idx = Mathf.Min( Mathf.Max(0, idx), cachePanelClassDictReverse[className].Count - 1);
                int idx_new = EditorGUILayout.Popup("Panel Type", idx, cachePanelClassDictReverse[className].ToArray());
                if (idx_new != idx)
                {
                    typeIdx.stringValue = cachePanelClassDictReverse[className][idx_new];
                }
            }

            typeID.intValue = GetIDByIdx(typeIdx.stringValue);
			//Debug.Log ("UIHolder typeID.stringValue : " + typeID.intValue);
			//typeIdx.stringValue = ((UIPanelIns)typeID.intValue).ToString ();
			GUILayout.EndVertical();

			//don't use enumValueIndex property
			if (typeID.intValue != 0) {
				GUILayout.BeginVertical("box");
				GUILayout.Label("Objects");
				Editor_Layout_UILst.Show(objectList, false, Editor_Layout_UILst.UIHolderListType.ObjectList, typeID.intValue);//true, false
				GUILayout.EndVertical();

				GUILayout.BeginVertical("box");
				GUILayout.Label("Events");
				Editor_Layout_UILst.Show(eventList, false, Editor_Layout_UILst.UIHolderListType.EventList, typeID.intValue);//true, false
				GUILayout.EndVertical();

				//UIListLayoutEditor.Show(motionList, true, UIListLayoutEditor.UIHolderListType.MotionList, typeID.intValue, false, true);
			}

			obj.ApplyModifiedProperties ();
		}


        private void CachePanelAsset()
        {
            cachePanelClassDict = new Dictionary<string, List<string>>();
            cachePanelClassDictReverse = new Dictionary<string, List<string>>();

            foreach (FieldInfo field in UIPanelType.None.GetType().GetFields())
            {
                object[] objs = field.GetCustomAttributes(typeof(UIPrefabDesc), true);
                if (objs != null && objs.Length > 0)
                {
                    List<string> classNames = new List<string>();
                    foreach (UIPrefabDesc desc in objs)
                    {
                        string name = desc.prefabClass.ToString();
                        if (!classNames.Contains(name))
                            classNames.Add(name);

                        if (!cachePanelClassDictReverse.ContainsKey(name))
                        {
							cachePanelClassDictReverse[name] = new List<string>(){"None"};
                        }
                        cachePanelClassDictReverse[name].Add(field.Name);
                    }
                    cachePanelClassDict[field.Name] = classNames;
                }
            }
            if (!cachePanelClassDict.ContainsKey("None"))
            {
                cachePanelClassDict["None"] = new List<string> { "None" };
            }
            if (!cachePanelClassDictReverse.ContainsKey("None"))
            {
                cachePanelClassDictReverse["None"] = new List<string> { "None" };
            }
        }
	}
}