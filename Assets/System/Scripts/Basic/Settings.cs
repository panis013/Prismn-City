using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour2 {
	public static Vector3 MapUnitStandardScale = new Vector3(100f,100f,100f);
	public static Vector3 MapUnitStandardPosition = new Vector3(0f,0f,10f);
	public static float FirstLayerZ = 100f;
	public static float ThirdLayerZ =-100f;
	public static int EditorUnitShowMaxAmount = 4;
	public static int MaxLayers = 4;
	public enum Layers{
		First = 0,
		Second = 1,
		Third = 2,
		Event = 3
	}
	public struct Tags{
		public static string MapUnit = "MapUnit";
		public static string MapContentSet = "MapContentSet";
		public static string Map = "Map";
	}
	public struct Paths
	{
		public static string MapUnitPath = "Assets/MapPrefabs/MapUnits";
		public static string MapPath = "Assets/Data/Map";
	}

	public static Vector3 GetRenderPosition(Vector3 originPosition,Settings.Layers layer,int height){
		float z;
		Map currentmap = GameController.CurrentMap;
		float length = currentmap.UnitLength;
		float miny = currentmap.UnitCounts.y * length + currentmap.OriginPoint.y;
		float maxy = currentmap.OriginPoint.y;
		float rate = (originPosition.y - height*length - miny) / (maxy - miny);
		float dis = Settings.FirstLayerZ - Settings.ThirdLayerZ;
		if (layer == Settings.Layers.First) {
			z = rate * dis *-1f + Settings.ThirdLayerZ + dis;
			return new Vector3 (originPosition.x, originPosition.y, z);
		} else if (layer == Settings.Layers.Second) {
			z = rate * dis*-1f+ Settings.ThirdLayerZ;
			return new Vector3 (originPosition.x, originPosition.y, z);
		} else if (layer == Settings.Layers.Third) {
			z = rate * dis*-1f + Settings.ThirdLayerZ - dis;
			return new Vector3 (originPosition.x, originPosition.y, z);
		}
		return originPosition;
	}
}
