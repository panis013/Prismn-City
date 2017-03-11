using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour2 {
	public UnitType unitType;
	public float health;









	//Functions
	public void getDamege(float dmg){
		health -= dmg;
		if (health < 0f) {
			die ();
		}
	}
	//inside Actions
	public void die(){
		//die
		Destroy(gameObject,1f);
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
