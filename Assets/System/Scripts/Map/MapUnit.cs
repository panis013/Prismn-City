using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("Game/MapUnit")]
public class MapUnit : MonoBehaviour2 {
	[Tooltip("地图块使用的图像,会自动设置到渲染器上")]
	public Sprite UnitSprite;
	public int height;
	public Map parent;
	public Coordinate coordinate;
	public Settings.Layers layer;
	public void init(Map map,Settings.Layers layer,int x,int y){
		parent = map;
		coordinate = new Coordinate (x, y);
		this.layer = layer;
		gameObject.AddComponent<GamePosition> ().Init (layer,height);
	}
	public void preSet(){
		gameObject.transform.localScale = Settings.MapUnitStandardScale;
		gameObject.tag = Settings.Tags.MapUnit;
		//gameObject.transform.transform.position = ToolBox.MapUnitStandardPosition;
	}
	public void Update(){
		
	}
}
