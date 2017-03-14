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
		endType = serializedObject.FindProperty ("endType");
		eventList = serializedObject.FindProperty ("eventList");
	}
	int selectedEvent;
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
		EditorGUILayout.PropertyField (triggerType);
		EditorGUILayout.PropertyField (endType);
		GameEventMaker gem = null;
		GUILayout.Label (aLine);
		if (GUILayout.Button ("Add new gameevent")) {
			GameEvent newGe = (GameEvent)GameEvent.CreateInstance (typeof(GameEvent));
			AddElementForArrayE (eventList, newGe);
			selectedEvent = eventList.arraySize-1;
			serializedObject.ApplyModifiedProperties ();
			return;
		}
		if (eventList.arraySize > 0) {
			GameEvent[] ges = new GameEvent[eventList.arraySize];
			string[] funNames = new string[eventList.arraySize];
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Insert new gameevent")) {
				GameEvent newGe = (GameEvent)GameEvent.CreateInstance (typeof(GameEvent));
				InsertElementInArrayE (eventList, selectedEvent, newGe);
				serializedObject.ApplyModifiedProperties ();
				return;
			}
			GUILayout.Space (10f);
			if (GUILayout.Button ("Delete gameevent")) {
				DeleteElementInArrayE (eventList, selectedEvent);
				selectedEvent = Mathf.Clamp (selectedEvent, 0, eventList.arraySize-1);
				serializedObject.ApplyModifiedProperties ();
				return;
			}
			EditorGUILayout.EndHorizontal ();
			GUILayout.Space (20f);
			GUILayout.Label ("EventList");
			for (int i = 0; i < eventList.arraySize; i++) {
				GameEvent ge = (GameEvent)eventList.GetArrayElementAtIndex (i).objectReferenceValue;
				ges [i] = ge;
				SerializedProperty sGe = eventList.GetArrayElementAtIndex (i);
				GameEventMaker tempGem = (GameEventMaker)CreateEditor ((GameEvent)sGe.objectReferenceValue);
				SerializedProperty funcType = tempGem.serializedObject.FindProperty ("functionType");
				string[] paranames = funcType.enumNames;
				int paraIndex = funcType.enumValueIndex;
				string paraname = paranames [paraIndex] + GameEventMaker.Parameter;
				SerializedProperty para = tempGem.serializedObject.FindProperty (paraname);
				funNames [i] = paranames [paraIndex];
				if (para != null) {
					funNames [i] += formatName (para);
				} else {
					funNames [i] += "[";
				}
				bool isWait = ge.Wait;
				if (isWait) {
					funNames [i] += "Wait]";
				} else {
					funNames [i] += "]";
				}
			}
			selectedEvent = GUILayout.SelectionGrid (selectedEvent, funNames, 1);
			gem = (GameEventMaker)CreateEditor (ges [selectedEvent]);

		}
		GUILayout.Label (aLine);
		if (gem != null) {
			gem.OnInspectorGUI ();
		}
		serializedObject.ApplyModifiedProperties ();
	}
	public static string formatName(SerializedProperty sp){
		if (sp == null) {
			Debug.Log ("NullSP");
			return "";
		}
		if (sp.hasChildren == false) {
			Debug.Log ("NoStruct");
			return "";
		}
		SerializedProperty tempsp = sp.Copy ();
		SerializedPropertyType spt;
		string newName = "[";
		int childCount = tempsp.CountInProperty ();
		tempsp = sp.Copy ();
		tempsp.Next (true);
		for (int i = 1; i < childCount; i++) {
			string tempName = tempsp.name + ":";
			string end = "][";
			spt = tempsp.propertyType;
			switch (spt) {
			case SerializedPropertyType.Float:
				tempName += tempsp.floatValue.ToString () + end;
				break;
			case SerializedPropertyType.Integer:
				tempName += tempsp.intValue.ToString()+ end;
				break;
			case SerializedPropertyType.Boolean:
				tempName += tempsp.boolValue.ToString()+ end;
				break;
			case SerializedPropertyType.String:
				tempName += tempsp.stringValue+ end;
				break;
			case SerializedPropertyType.Color:
				tempName += tempsp.colorValue.ToString ()+ end;
				break;
			case SerializedPropertyType.Enum:
				tempName += tempsp.enumNames [tempsp.enumValueIndex]+ end;
				break;
			case SerializedPropertyType.ObjectReference:
				tempName += tempsp.objectReferenceValue.name+ end;
				break;
			case SerializedPropertyType.Vector2:
				tempName += tempsp.vector2Value.ToString ()+ end;
				break;
			default:
				tempName = "";
				break;
			}
			newName += tempName;
			tempsp.Next (false);
		}
		return newName;
	}
}
