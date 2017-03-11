using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class MapContentSet : ScriptableObject{
	public GameObject[] MapSet;
	public GameObject NewSet;
	public GameObject getUnit(int i){
		return (GameObject)MapSet [i];
	}
	//Tools
	public Texture[] GetSetImages(){
		Texture[] images = new Texture[MapSet.Length];
		for (int i = 0; i < MapSet.Length; i++) {
			GameObject g = (GameObject)MapSet[i];
			MapUnit mu = g.GetComponent<MapUnit> ();
			if (mu == null) {
				Debug.LogError ("存在不合法的地图块");
				continue;
			}
			images [i] = ToolBox.ConvertSpriteToTexture (g.GetComponent<MapUnit> ().UnitSprite);
		}
		return images;
	}
}