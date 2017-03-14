using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour2 {
	public enum DamegeType
	{
		normal,
		light
	}
	public UnitType unitType;
	public bool BulletTakable = true;
	public float health;
	public float Armor = 0f;// 敌人护甲 减免受到的伤害（1/1+0.08*护甲）
	//单位是Rigidbody2D物体，通过相关函数控制运动


	public float LastHitDmg=0f;
	public Vector3 LastHitFromPos;
	public GameUnit LastHitFrom;


	public int height = 0;
	void Start(){
		gameObject.AddComponent<GamePosition> ().Init (Settings.GameUnitDefaultLayer,height);
	}




	//Functions
	public void getDamage(float dmg,DamegeType dt,Vector3 fromPos,GameUnit from){
		dmg = Settings.getFixedDmg (dmg,Armor);
		health -= dmg;
		LastHitDmg = dmg;
		LastHitFrom = from;
		LastHitFromPos = fromPos;
		if (unitType != UnitType.Player) {
			AI ai = gameObject.GetComponent<AI> ();
			if (ai!= null) {
				switch (dt) {
				case DamegeType.normal:
					ai.OnHit ();
					break;
				case DamegeType.light:
					ai.OnLightHit ();
					break;
				}
			}
		}
		if (health < 0f) {
			die ();
		}
	}
	//inside Actions
	public void die(){
		//die
		Destroy(gameObject,0.1f);
	}


	public static int UnitTypeAmount = 5;
	public enum UnitType{
		Player = 0,
		NPC = 1,
		Enemy = 2,
		Bullet = 3,
		Object = 4
	}
}
