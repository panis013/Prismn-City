using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox{
	public static Texture2D ConvertSpriteToTexture(Sprite sp){
		Texture2D t2 = new Texture2D ((int)sp.rect.width, (int)sp.rect.height);
		Color[] pixels = sp.texture.GetPixels ((int)sp.rect.x, (int)sp.rect.y, (int)sp.rect.width, (int)sp.rect.height);
		//Debug.Log ("1:" + (int)sp.rect.width + "/" + (int)sp.rect.height);
		//Debug.Log ("2:" + (int)sp.textureRect.width + "/" + (int)sp.textureRect.height);
		t2.SetPixels (pixels);
		t2.Apply ();

		return t2;
	}
	public static Sprite ConvertTextureToSprite(Texture2D t2){
		Rect r = new Rect (0, 0, t2.width, t2.height);
		Sprite sp = Sprite.Create (t2,r,Vector2.zero);
		sp.name = t2.name;
		return sp;
	}
	//math
}
