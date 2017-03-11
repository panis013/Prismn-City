using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class MapData : ScriptableObject {
	public MapContentSet ms;
	public string MapName;
	public float UnitLength;
	public Vector2 OriginPoint;
	public Map.TypeUnitCounts UnitCounts;
	public MapContentSet MapContentSet;
	public GameObject[] ContentInMap;
	public void SaveData(Map map){
		MapName = map.MapName;
		UnitLength = map.UnitLength;
		OriginPoint = map.OriginPoint;
		UnitCounts = map.UnitCounts;
		MapContentSet = map.MapContentSet;
		ContentInMap = (GameObject[])map.ContentInMap.Clone ();
	}
	public GameObject LoadData(){
		GameObject newMap = new GameObject ();
		newMap.name = MapName;
		Map mapComponent = newMap.AddComponent<Map> ();
		mapComponent.Init (this);
		return newMap;
	}
}
