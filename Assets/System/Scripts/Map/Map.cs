using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[AddComponentMenu("Game/Map")]
public class Map : MonoBehaviour2 {
	public string MapName = "new map";
	[Tooltip("网格边长")]
	public float UnitLength = 100f;
	[Tooltip("在视图中生成地图块时的原点")]
	public Vector2 OriginPoint = Vector2.zero;
	[Serializable]
	public struct TypeUnitCounts
	{
		public int x;
		public int y;
	}
	[Tooltip("网格数量")]
	public TypeUnitCounts UnitCounts;
	[Tooltip("使用的地图块组")]
	public MapContentSet MapContentSet;
	///地图的内容(地图块序列) 
	public GameObject[] ContentInMap;
	public Coordinate coordinate = new Coordinate();
	//Tools
	public Coordinate GetCoordinate(Vector2 v2){
		Coordinate coor = new Coordinate ();
		Vector2 newV2 = new Vector2 (v2.x - OriginPoint.x, v2.y - OriginPoint.y);
		coor.x = (int)(newV2.x / UnitLength);
		coor.y = (int)(newV2.y / UnitLength);
		return coor;
	}
	public Coordinate GetCoordinate(Vector3 v3){
		Coordinate coor = new Coordinate ();
		Vector2 v2 = new Vector2 (v3.x, v3.y);
		Vector2 newV2 = new Vector2 (v2.x - OriginPoint.x, v2.y - OriginPoint.y);
		coor.x = Mathf.FloorToInt(newV2.x / UnitLength);
		coor.y = Mathf.FloorToInt (newV2.y / UnitLength);
		return coor;
	}
	public bool InMap(Coordinate c){
		if (c.x < 0 || c.x >= UnitCounts.x || c.y < 0 || c.y >= UnitCounts.y) {
			return false;
		}
		return true;
	}
	public Vector3 GetWorldPositionCenterOfCoordinate(Coordinate tc){
		return new Vector3 ((tc.x  + 0.5f) * UnitLength, (tc.y + 0.5f) * UnitLength);
	}
	public GameObject GetAUnit(Settings.Layers layer,int x,int y){
		return ContentInMap [GetMapdataIndex(layer,x,y)];
	}
	public void SetAUnit(Settings.Layers layer,int x,int y,GameObject go){
		ContentInMap [GetMapdataIndex(layer,x,y)] = go;
	}
	public GameObject[] NewMap(int x,int y){
		ContentInMap = new GameObject[Settings.MaxLayers*x*y];
		return ContentInMap;
	}
	///按行优先排列(从左到右依次增1,一行结束到邻接的下面一行继续)
	public int GetMapdataIndex(Settings.Layers layer,int x,int y){
		int unitsPerLayer =UnitCounts.x*UnitCounts.y;
		return (int)layer * unitsPerLayer + x + y * UnitCounts.x;
	}
	public Coordinate GetMapdataCoordinate(int index){
		int unitsPerLayer =UnitCounts.x*UnitCounts.y;
		int layerInt = Mathf.FloorToInt((float)index/(float)unitsPerLayer);
		Settings.Layers layer = ((Settings.Layers)Enum.GetValues(typeof(Settings.Layers)).GetValue(layerInt));
		index -= layerInt * unitsPerLayer;
		int y = Mathf.FloorToInt((float)index/(float)UnitCounts.x);
		int x = index % UnitCounts.x;
		Coordinate coor = new Coordinate (x, y);
		coor.layer = layer;
		return coor;
	}
	public void Clear(){
		for (int z = 0; z < ContentInMap.Length; z++) {
			ContentInMap [z] = null;
		}
	}

	public GameObject CreateAUnit(Settings.Layers layer,int x,int y){
		GameObject go = GetAUnit (layer, x, y);
		if (go == null) {
			return null;
		}
		GameObject go2 = GameObject.Instantiate (go, gameObject.transform);
		Coordinate ct = new Coordinate ();
		ct.x = x;
		ct.y = y;
		go2.transform.position = GetWorldPositionCenterOfCoordinate (ct);
		go2.GetComponent<MapUnit> ().init (this, layer, x, y);
		return go2;
	}
	public GameObject CreateAUnit(int index){
		GameObject go = ContentInMap [index];
		if (go == null) {
			return null;
		}
		GameObject go2 = GameObject.Instantiate (go, gameObject.transform);
		Coordinate ct = GetMapdataCoordinate (index);
		go2.transform.parent = gameObject.transform;
		go2.transform.position = GetWorldPositionCenterOfCoordinate (ct);
		go2.GetComponent<MapUnit> ().init (this, ct.layer, ct.x, ct.y);
		return go2;
	}
	public void CreateMapByIndex(){
		GameObject[] MUs = GameObject.FindGameObjectsWithTag (Settings.Tags.MapUnit);
		foreach (GameObject g in MUs) {
			Destroy (g);
		}
		for (int z = 0; z < ContentInMap.Length; z++) {
			CreateAUnit (z);
		}
	}
	//Logics
	void Awake(){
		Init ();
	}
	void Start(){
		Test ();
		CreateMapByIndex ();
	}
	void Update(){
		//CreateMapByIndex ();
	}
	public void Init(){
		GameController.CurrentMap = this;
		CreateMapByIndex ();
	}
	public void Init(MapData mapdata){
		MapName = mapdata.MapName;
		UnitLength = mapdata.UnitLength;
		UnitCounts = mapdata.UnitCounts;
		OriginPoint = mapdata.OriginPoint;
		MapContentSet = mapdata.MapContentSet;
		ContentInMap = mapdata.ContentInMap;
		GameController.CurrentMap = this;
		CreateMapByIndex ();
	}
	void Test(){
		TestPlayerControl tpc = GameObject.FindGameObjectWithTag ("Player").GetComponent<TestPlayerControl> ();
		tpc.testmap = this;
	}
}
