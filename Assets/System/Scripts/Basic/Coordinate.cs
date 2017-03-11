using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Coordinate :UnityEngine.Object {
	public Settings.Layers layer;
	public int x;
	public int y;
	public Coordinate(){
		x = 0;
		y = 0;
	}
	public Coordinate(int x,int y){
		this.x = x;
		this.y = y;
	}
	public Coordinate Clone(){
		return new Coordinate (this.x, this.y);
	}
}
