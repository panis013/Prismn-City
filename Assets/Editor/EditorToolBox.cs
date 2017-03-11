using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorToolBox{
	[MenuItem("Tool/Transform sprite to MapUnits &t")]
	public static void SpriteToMapUnits(){
		string[] s = Selection.assetGUIDs;
		foreach (string ss in s) {
			string path = AssetDatabase.GUIDToAssetPath (ss);
			UnityEngine.Object mapSprite = AssetDatabase.LoadAssetAtPath<Sprite> (path);
			if (mapSprite.GetType () != typeof(UnityEngine.Sprite)) {
				continue;
			}
			string mapName = ((Sprite)mapSprite).name;
			UnityEngine.Object[] ass = AssetDatabase.LoadAllAssetsAtPath (path);
			foreach (UnityEngine.Object uo in ass) {
				if (uo.GetType () == typeof(Texture2D)) {
					continue;
				}
				Sprite sp = uo as Sprite;
				string name = sp.name;
				//string name = mapName+"_"+ uo.GetInstanceID ().ToString();
				GameObject newUnit = new GameObject (name);
				Rigidbody2D newR2 = newUnit.AddComponent<Rigidbody2D> ();
				BoxCollider2D newB2 = newUnit.AddComponent<BoxCollider2D> ();
				MapUnit newMu = newUnit.AddComponent<MapUnit> ();
				newMu.UnitSprite = sp;
				newMu.preSet ();
				GameObject newPre = PrefabUtility.CreatePrefab (Settings.Paths.MapUnitPath+"/"+name+".prefab",newUnit);
				//AssetDatabase.ImportAsset(("Asset/MapPrefabs/MpaUnits"+name);
			}
		}
	}
	[MenuItem("Tool/Transform sprite to MapUnits &t",true)]
	static bool ValidateSpriteToMapUnits(){
		return Selection.assetGUIDs.Length>0;
	}
}
