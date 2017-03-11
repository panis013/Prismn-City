using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor( typeof(MapData) )]
public class MapDataEditor : Editor2 {
	MapData mapdata;
	public override void OnInspectorGUI(){
		mapdata = target as MapData;
		base.OnInspectorGUI ();

		if (GUILayout.Button ("读取") == true) {
			Selection.activeObject = mapdata.LoadData ();
		}
	}
}
