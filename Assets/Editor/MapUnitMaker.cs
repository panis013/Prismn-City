using UnityEngine;
using UnityEditor;
using System;
[CustomEditor( typeof(MapUnit) )]
public class MapUnitMaker : Editor2 {
	MapUnit mu;
	SerializedProperty UnitSprite;
	SerializedProperty Height;
	void OnEnable(){
		UnitSprite = serializedObject.FindProperty ("UnitSprite");
		Height = serializedObject.FindProperty ("height");
		mu = target as MapUnit;
	}
	public override void OnInspectorGUI(){
		mu = target as MapUnit;
		serializedObject.Update ();
		MapUnit g = (MapUnit)serializedObject.targetObject;
		if(GUILayout.Button(new GUIContent("重设(设置比例和Tag)"))){
			mu.preSet ();
		}
		EditorGUILayout.LabelField ("地图块");
		EditorGUILayout.PropertyField (UnitSprite);
		EditorGUILayout.PropertyField (Height);
		serializedObject.ApplyModifiedProperties();
		g.gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)UnitSprite.objectReferenceValue;
		if (mu.coordinate != null) {
			EditorGUILayout.LabelField ("X:" + mu.coordinate.x + " Y:" + mu.coordinate.y);
		}
	}
}
