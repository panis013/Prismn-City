using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameUnitCheck{
	public bool[] UnitTypeCheck = new bool[GameUnit.UnitTypeAmount];
	public bool check(GameUnit.UnitType ut){
		return UnitTypeCheck [(int)ut];
	}
	public GameUnitCheck(bool Player,bool NPC,bool Enemy,bool Bullet,bool Object){
		UnitTypeCheck = new bool[GameUnit.UnitTypeAmount];
		UnitTypeCheck [0] = Player;
		UnitTypeCheck [1] = NPC;
		UnitTypeCheck [2] = Enemy;
		UnitTypeCheck [3] = Bullet;
		UnitTypeCheck [4] = Object;
	}
	public GameUnitCheck (){
		UnitTypeCheck = new bool[GameUnit.UnitTypeAmount];
	}
}
