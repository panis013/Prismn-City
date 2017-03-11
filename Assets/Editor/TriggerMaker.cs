using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor( typeof(GameEventTrigger) )]
public class TriggerMaker : Editor2 {
	GameEvent geOnFocus;
	GameEventMaker editorForGeOnFocus;

	SerializedProperty unitTypeCheck;
	SerializedProperty triggerType;
	SerializedProperty endType;
	SerializedProperty eventList;

	void OnEnable(){
		unitTypeCheck = serializedObject.FindProperty ("gameUCheck.UnitTypeCheck");
		triggerType = serializedObject.FindProperty ("triggerType");
		triggerType = serializedObject.FindProperty ("endType");
		eventList = serializedObject.FindProperty ("eventList");
	}
	public override void OnInspectorGUI(){
		//base.OnInspectorGUI ();
		string[] gameUnitTypeNames = GameUnit.UnitType.GetNames(typeof(GameUnit.UnitType));
		for (int i = 0; i < GameUnit.UnitTypeAmount; i++) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (gameUnitTypeNames [i]);
			EditorGUILayout.PropertyField (unitTypeCheck.GetArrayElementAtIndex (i));
			EditorGUILayout.EndHorizontal ();
		}
		serializedObject.ApplyModifiedProperties ();
		GUILayout.Label (aLine);
	}
}
