using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePosition : MonoBehaviour {
	Settings.Layers layer;
	int height;
	public void Init(Settings.Layers layer,int height){
		this.layer = layer;
		this.height = height;
	}
	public void Update(){
		gameObject.transform.position = Settings.GetRenderPosition (gameObject.transform.position, layer,height);
	}

}
