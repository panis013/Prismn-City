using UnityEngine;
using UnityEditor;
using System;
[CustomEditor( typeof( Map ) )]
public class MapMaker : Editor2{
	bool showMap = true;
	bool showAllLayers = false;
	int selectedSet;
	bool AreaSDKeyDown = false;
	Coordinate AreaSDPastCoor = new Coordinate();
	Map t;
	Settings.Layers selectedLayer = Settings.Layers.First;
	SerializedProperty UnitLength;
	SerializedProperty UnitCountX;
	SerializedProperty UnitCountY;
	SerializedProperty MapContentSet;
	SerializedProperty MapName;
	MapContentSet mapContentSet;
	void OnEnable(){
		t = target as Map;
		GameController.CurrentMap = t;
		UnitLength = serializedObject.FindProperty ("UnitLength");
		UnitCountX = serializedObject.FindProperty ("UnitCounts.x");
		UnitCountY = serializedObject.FindProperty ("UnitCounts.y");
		MapContentSet = serializedObject.FindProperty ("MapContentSet");
		mapContentSet = ((MapContentSet)MapContentSet.objectReferenceValue);
		MapName = serializedObject.FindProperty ("MapName");
		Refresh ();
	}
	public override void OnInspectorGUI(){
		serializedObject.Update ();
		t = target as Map;
		EditorGUILayout.LabelField ("地图网格参数");
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (UnitLength);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PropertyField (UnitCountX);
		EditorGUILayout.PropertyField (UnitCountY);
		EditorGUILayout.EndHorizontal ();
		EditorGUI.indentLevel--;

		EditorGUILayout.LabelField ("地图内容参数");
		EditorGUI.indentLevel++;
		MapContentSet tempGO = (MapContentSet)MapContentSet.objectReferenceValue;
		if (GUILayout.Button ("新建")==true) {
			t.ContentInMap = t.NewMap (UnitCountX.intValue, UnitCountY.intValue);
			Refresh ();
		}
		if (t.ContentInMap == null) {
			return;
		}
		//检查变更的地图块组是否具有相应的组件
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (MapContentSet);
		if (EditorGUI.EndChangeCheck ()) {
			/*
			MapContentSet ms = (MapContentSet)MapContentSet.objectReferenceValue;
			if (ms==null) {
				Debug.LogError ("不合法的地图块组");
				MapContentSet.objectReferenceValue = tempGO;
			}
			*/
		}
		serializedObject.ApplyModifiedProperties ();
		if (t.MapContentSet ==null) {
			return;
		}
		EditorGUI.indentLevel--;

		EditorGUILayout.LabelField ("层数");
		EditorGUI.indentLevel++;
		EditorGUI.BeginChangeCheck();
		selectedLayer = (Settings.Layers)EditorGUILayout.EnumPopup (selectedLayer);
		if (EditorGUI.EndChangeCheck ()) {
			Refresh ();
		}
		EditorGUI.indentLevel--;

		EditorGUI.BeginChangeCheck ();
		showMap = GUILayout.Toggle (showMap, "显示地图");
		if (EditorGUI.EndChangeCheck ()) {
			ClearMapUnits ();
			if (showMap == false) {
				ClearMapUnits ();
			} else {
				if (showAllLayers == false) {
					ShowUnitsInLayer (selectedLayer);
				} else {
					for (int i = 0; i < Settings.MaxLayers; i++) {
						ShowUnitsInLayer ((Settings.Layers)Enum.GetValues(typeof(Settings.Layers)).GetValue(i));
					}
				}
			}
		}
		EditorGUI.BeginChangeCheck ();
		showAllLayers = GUILayout.Toggle (showAllLayers, "显示所有层");
		if (EditorGUI.EndChangeCheck ()) {
			ClearMapUnits ();
			if (showMap == true) {
				if (showAllLayers == true) {
					for (int i = 0; i < Settings.MaxLayers; i++) {
						ShowUnitsInLayer ((Settings.Layers)Enum.GetValues(typeof(Settings.Layers)).GetValue(i));
					}
				} else {
					ShowUnitsInLayer (selectedLayer);
				}
			}
		}

		EditorGUI.indentLevel++;

		EditorGUI.indentLevel--;
		EditorGUILayout.LabelField ("按S放置,按D删除,按住X以矩形放置,按住C以矩形删除");
		Texture[] images = ((MapContentSet)(MapContentSet.objectReferenceValue)).GetSetImages ();
		this.selectedSet = GUILayout.SelectionGrid (this.selectedSet, images, Settings.EditorUnitShowMaxAmount);
		EditorGUI.indentLevel++;
		if (GUILayout.Button ("清空") == true) {
			t.Clear ();
			Refresh ();
		}
		EditorGUI.indentLevel--;
		EditorGUILayout.PropertyField (MapName);
		serializedObject.ApplyModifiedProperties ();
		if (GUILayout.Button ("保存")) {
			SaveMapData (t.MapName);
		}
		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI( )
	{
		t = target as Map;
		if( t == null||t.ContentInMap==null)
			return;
		int x, y;
		x = t.UnitCounts.x;
		y = t.UnitCounts.y;
		float length = t.UnitLength;
		Vector3 OriginPoint = new Vector3 (t.OriginPoint.x, t.OriginPoint.y);
		Vector3 p1,p2;
		//绘制地图网格
		Handles.color = Color.blue;
		for (int i = 0;i <= x;i++) {//横向
			p1 = new Vector3(i*length,0f)+OriginPoint;
			p2 = new Vector3 (i * length, y * length) + OriginPoint;
			Handles.DrawLine (p1, p2);
		}
		for (int j = 0;j <= y;j++) {//纵向
			p1 = new Vector3(0f,j*length)+OriginPoint;
			p2 = new Vector3 (x * length, j * length) + OriginPoint;
			Handles.DrawLine (p1, p2);
		}
		//获取鼠标地图坐标
		Coordinate tc = MouseCoordinateInMap ();
		Vector3 MouseCenter = t.GetWorldPositionCenterOfCoordinate (tc);
		//处理键盘事件
		Event keyboardEventForPrintMap = Event.current;
		Coordinate printAimCoor = fixedCoordinate (tc);
		if (t.InMap (MouseCoordinateInMap ()) == true) {
			//按下S放置一个地图块
			if (keyboardEventForPrintMap.type == EventType.KeyDown && keyboardEventForPrintMap.keyCode == KeyCode.S) {
				Coordinate ct = MouseCoordinateInMap ();
				GameObject aimgo = getSelectedGO ();
				if (aimgo != null) {
					t.SetAUnit (selectedLayer, ct.x, ct.y, aimgo);
					Refresh ();
				}
				//按下D删除一个地图块
			} else if (keyboardEventForPrintMap.type == EventType.KeyDown && keyboardEventForPrintMap.keyCode == KeyCode.D) {
				Coordinate ct = MouseCoordinateInMap ();
				t.SetAUnit (selectedLayer, ct.x, ct.y, null);
				Refresh ();
			}
		}
		//区域刷
		if (keyboardEventForPrintMap.type == EventType.KeyDown && keyboardEventForPrintMap.keyCode == KeyCode.X) {
			if (AreaSDKeyDown == false) {
				AreaSDKeyDown = true;
				AreaSDPastCoor = fixedCoordinate (tc);
			}
			keyboardEventForPrintMap.Use ();
		} else if (keyboardEventForPrintMap.type == EventType.KeyUp && keyboardEventForPrintMap.keyCode == KeyCode.X) {
			if (AreaSDKeyDown == true) {
				AreaSDKeyDown = false;
				AreaPrint (selectedLayer, AreaSDPastCoor, printAimCoor, getSelectedGO ());
				Refresh ();
			}
		}
		//区域删除
		if (keyboardEventForPrintMap.type == EventType.KeyDown && keyboardEventForPrintMap.keyCode == KeyCode.C) {
			if (AreaSDKeyDown == false) {
				AreaSDKeyDown = true;
				AreaSDPastCoor = fixedCoordinate (tc);
			}
			keyboardEventForPrintMap.Use ();
		} else if (keyboardEventForPrintMap.type == EventType.KeyUp && keyboardEventForPrintMap.keyCode == KeyCode.C) {
			if (AreaSDKeyDown == true) {
				AreaSDKeyDown = false;
				AreaPrint (selectedLayer, AreaSDPastCoor, printAimCoor, null);
				Refresh ();
			}
		}
		//绘制选中框
		Handles.color = Color.red;
		if (t.InMap (tc) == true && AreaSDKeyDown == false) {
			Handles.DrawWireCube (MouseCenter, Vector3.one * t.UnitLength);
		} else if (AreaSDKeyDown == true) {
			int x1, x2, y1, y2;
			x1 = AreaSDPastCoor.x;
			x2 = printAimCoor.x;
			y1 = AreaSDPastCoor.y;
			y2 = printAimCoor.y;
			if (x1 < x2) {
				int t = x1;
				x1 = x2;
				x2 = t;
			}
			if (y1 < y2) {
				int t = y1;
				y1 = y2;
				y2 = t;
			}
			Vector3 areaCenter = new Vector3 ((float)(x1 + x2 + 1) / 2f * t.UnitLength+t.OriginPoint.x, (float)(y1 + y2 + 1) / 2f * t.UnitLength+t.OriginPoint.y);
			Vector3 areaSize = new Vector3 ((float)(x1 - x2+1) * t.UnitLength, (float)(y1 - y2+1) * t.UnitLength);
			Handles.DrawWireCube (areaCenter,areaSize);
		}
		HandleUtility.Repaint ();
		BanSceneSelection ();
	}
	//Tools
	public void ClearMapUnits(){
		GameObject[] MUs = GameObject.FindGameObjectsWithTag (Settings.Tags.MapUnit);
		foreach (GameObject g in MUs) {
			Editor.DestroyImmediate (g);
		}
	}
	public void ShowUnitsInLayer(Settings.Layers layer){
		//测试版,以后修改map的储存方式再修改
		for (int x = 0;x< t.UnitCounts.x; x++) {
			for (int y = 0; y < t.UnitCounts.y; y++) {
				CreateAUnit (layer, x, y);
			}
		}
	}
	public void Refresh (){
		ClearMapUnits ();
		if (t == null)
			return;
		if (showMap == true) {
			if (showAllLayers == false) {
				ShowUnitsInLayer (selectedLayer);
			} else {
				for (int i = 0; i < Settings.MaxLayers; i++) {
					ShowUnitsInLayer ((Settings.Layers)Enum.GetValues(typeof(Settings.Layers)).GetValue(i));
				}
			}
		}
	}
	public GameObject getSelectedGO(){
		GameObject aimgo = mapContentSet.MapSet [selectedSet] as GameObject;
		return aimgo;
	}
	public GameObject CreateAUnit(Settings.Layers layer,int x,int y){
		GameObject go = t.GetAUnit (layer, x, y);
		if (go == null) {
			return null;
		}
		GameObject go2 = GameObject.Instantiate (go, t.gameObject.transform);
		Coordinate ct = new Coordinate ();
		ct.x = x;
		ct.y = y;
		MapUnit mu = go2.GetComponent<MapUnit> ();
		Vector3 newPos = t.GetWorldPositionCenterOfCoordinate (ct);
		newPos = Settings.GetRenderPosition (newPos, layer,mu.height);
		go2.transform.position = newPos;
		go2.GetComponent<MapUnit> ().init (t, layer, x, y);
		return go2;
	}
	public bool AreaPrint(Settings.Layers layer,Coordinate start,Coordinate end,GameObject aimgo){
		if ((t.InMap (start) && t.InMap (end)) == false) {
			return false;
		}
		int x1, x2, y1, y2;
		x1 = start.x;
		x2 = end.x;
		y1 = start.y;
		y2 = end.y;
		if (x1 < x2) {
			int t = x1;
			x1 = x2;
			x2 = t;
		}
		if (y1 < y2) {
			int t = y1;
			y1 = y2;
			y2 = t;
		}
		for (int xx = x2; xx <= x1; xx++) {
			for (int yy = y2; yy <= y1; yy++) {
				t.SetAUnit (layer, xx, yy, aimgo);
			}
		}
		return true;
	}
	public Coordinate fixedCoordinate(Coordinate coor){
		if (t.InMap (coor) == true)
			return coor.Clone ();
		int x, y;
		x = Mathf.Clamp (coor.x, 0, t.UnitCounts.x-1);
		y = Mathf.Clamp (coor.y, 0, t.UnitCounts.y-1);
		return new Coordinate (x, y);
	}
	public Coordinate MouseCoordinateInMap(){
		Vector3 mouseV3 = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition).origin;
		mouseV3 = new Vector3 (mouseV3.x, mouseV3.y,0f);
		Coordinate tc = t.GetCoordinate (mouseV3);
		return tc;
	}
	public void SaveMapData(string name){
		string path = Settings.Paths.MapPath +"/" + name + ".asset";
		AssetDatabase.DeleteAsset (path);
		MapData newMapData = MapData.CreateInstance<MapData> ();
		newMapData.SaveData (t);
		AssetDatabase.CreateAsset (newMapData, path);
		t.gameObject.name = name;
		Selection.activeObject = newMapData;
	}
}
