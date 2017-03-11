using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor( typeof(MapContentSet) )]
public class MapSetMaker : Editor2{
	SerializedProperty MapSet;
	//SerializedProperty Size;
	SerializedProperty NewSet;
	int selectedSet;
	void OnEnable(){
		MapSet = serializedObject.FindProperty ("MapSet");
		//Size = MapSet.FindPropertyRelative ("Array.size");
		NewSet = serializedObject.FindProperty ("NewSet");
		selectedSet = 0;
	}
	public override void OnInspectorGUI(){
		serializedObject.Update ();
		EditorGUILayout.PropertyField (MapSet, true);
		EditorGUILayout.LabelField ("地图块组预览");
		int setAmount = MapSet.arraySize;
		Texture[] images = new Texture[setAmount];
		for (int i = 0; i < setAmount; i++) {
			GameObject g = (GameObject)MapSet.GetArrayElementAtIndex (i).objectReferenceValue;
			MapUnit mu = g.GetComponent<MapUnit> ();
			if (mu == null) {
				MapSet.DeleteArrayElementAtIndex (i);
				Debug.LogError ("存在不合法的地图块,已删除");
				continue;
			}
			images [i] = ToolBox.ConvertSpriteToTexture (g.GetComponent<MapUnit> ().UnitSprite);
		}
		this.selectedSet = GUILayout.SelectionGrid (this.selectedSet, images, Settings.EditorUnitShowMaxAmount);
		if (GUILayout.Button ("删除") == true) {
			DeleteElementInArrayE (MapSet, selectedSet);
			/*MapSet.MoveArrayElement (MapSet.arraySize - 1, selectedSet);
			MapSet.arraySize--;*/
		}
		EditorGUILayout.HelpBox ("删除中间元素将可能打乱地图块顺序,请谨慎操作",MessageType.Info);
		if (GUILayout.Button ("清空") == true) {
			MapSet.ClearArray ();
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("拖入地图块以加入:");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (NewSet);
		if (EditorGUI.EndChangeCheck () == true) {
			GameObject newg = (GameObject)NewSet.objectReferenceValue;
			if (newg.GetComponents<MapUnit> ().Length != 1) {
				Debug.LogError ("不合法的地图块组");
				NewSet.objectReferenceValue = null;
			} else {
				/*MapSet.arraySize++;
				MapSet.GetArrayElementAtIndex (MapSet.arraySize - 1).objectReferenceValue = newg;*/
				AddElementForArrayE (MapSet, newg);
				NewSet.objectReferenceValue = null;
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}