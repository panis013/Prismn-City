using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor( typeof(GameEvent) )]
public class GameEventMaker : Editor2 {
	public static string Parameter = "Parameter";
	SerializedProperty GEtype;
	SerializedProperty para;
	SerializedProperty Wait;
	void OnEnable(){
		GEtype = serializedObject.FindProperty ("functionType");
		Wait = serializedObject.FindProperty ("Wait");
	}
	public override void OnInspectorGUI(){
		EditorGUILayout.PropertyField (GEtype);
		serializedObject.ApplyModifiedProperties();
		string[] paranames = GEtype.enumNames;
		int paraIndex = GEtype.enumValueIndex;
		string paraname = paranames [paraIndex] + Parameter;
		para = serializedObject.FindProperty (paraname);
		if (para != null) {
			if (EditorGUILayout.PropertyField (para)) {
				int childrenCount = para.CountInProperty();
				para = serializedObject.FindProperty (paraname);
				para.Next (true);
				EditorGUI.indentLevel++;
				for (int i = 1; i < childrenCount; i++) {
					EditorGUILayout.PropertyField (para);
					para.Next (false);
				}
				EditorGUI.indentLevel--;
			}
		}
		EditorGUILayout.PropertyField (Wait);
		serializedObject.ApplyModifiedProperties();
	}
}
