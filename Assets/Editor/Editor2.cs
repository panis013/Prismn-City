using UnityEngine;
using UnityEditor;
using System;

public class Editor2 : Editor {
	public static string aLine = "-----------------------------------------------------------------------------------------";
	protected void BanSceneSelection(){
		Event e = Event.current;
		int controlID = GUIUtility.GetControlID (FocusType.Passive);
		if (e.type == EventType.Layout) {
			HandleUtility.AddDefaultControl (controlID);
		}
	}
	protected void ProgressBar (float value, string label) {
		Rect rect = GUILayoutUtility.GetRect (18, 18,"TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}
	protected void Slider (Rect position,SerializedProperty property,float leftValue,float rightValue,GUIContent label) {
		label = EditorGUI.BeginProperty (position, label, property);

		EditorGUI.BeginChangeCheck ();
		var newValue = EditorGUI.Slider (position, label, property.floatValue, leftValue, rightValue);
		if (EditorGUI.EndChangeCheck ())
			property.floatValue = newValue;

		EditorGUI.EndProperty ();
	}
	///zero-based index
	protected void DeleteElementInArrayE(SerializedProperty array, int index){
		if (array.isArray == false||array.arraySize<=index) {
			Debug.LogError ("非法的参数");
			return;
		}
		for (int i = index; i < array.arraySize - 1; i++) {
			array.MoveArrayElement (i+1, i);
		}
		array.arraySize--;
	}
	///zero-based index
	protected void InsertElementInArrayE(SerializedProperty array,int index,UnityEngine.Object insertObject){
		if (array.isArray == false||array.arraySize<=index) {
			Debug.LogError ("非法的参数");
			return;
		}
		array.arraySize++;
		for (int i = array.arraySize-2; i >= index; i--) {
			array.MoveArrayElement (i, i+1);
		}
		array.GetArrayElementAtIndex (index).objectReferenceValue = insertObject;
	}
	protected void AddElementForArrayE(SerializedProperty array,UnityEngine.Object insertObject){
		if (array.isArray == false||array.arraySize<=0) {
			Debug.LogError ("非法的参数");
			return;
		}
		array.arraySize++;
		array.GetArrayElementAtIndex (array.arraySize-1).objectReferenceValue = insertObject;
	}
}
