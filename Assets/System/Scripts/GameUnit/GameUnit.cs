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
	public float health;
	public float Armor = 0f;// 敌人护甲 减免受到的伤害（1/1+0.08*护甲）
	//单位是Rigidbody2D物体，通过相关函数控制运动








	//Functions
	public void getDamege(float dmg,DamegeType dt,Vector3 fromPos,GameUnit from){
		health -= dmg;
		if (unitType != UnitType.Player) {
			AI ai = gameObject.GetComponent<AI> ();
			if (ai!= null) {
				switch (dt) {
				case DamegeType.normal:
					ai.OnHit (dmg, fromPos, from);
					break;
				case DamegeType.light:
					ai.OnLightHit (dmg, fromPos, from);
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
