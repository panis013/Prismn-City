using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class Actions : ScriptableObject {
    public bool Over = false;//flag for if is over
    public BulletParameter[] BPs;
    public float AttackInterval = 0.5f;
    public float SlowMovespeed = 300f;
    private float time;
    public enum ActionType
    {
        idle,
        walk,
        damaged,
        run,
        attack,
        blasting_off
    }
    public ActionType funtionType;
    private AI OwnerAI;
    //判断条件  
    public bool run()
    {
        
        if (funtionType == ActionType.idle)
        {
            Move(0);
        }
        if (funtionType == ActionType.walk)
        {
            Move(1);
        }
        if (funtionType == ActionType.damaged)
        {
            Damaged();
        }
        else if (funtionType == ActionType.attack)
        {
            Attack(BPs[0]);
            Move(0);
        }
        return true;
    }

    public void setOwner(AI owner)
    {
        OwnerAI = owner;
        
    }

    void Attack(BulletParameter BP)//泛用的攻击函数 传入一个BulletParameter给BulletLauncher的发射函数
    {
        OwnerAI.gameObject.GetComponent<BulletLauncher>().Launch(BP);
    }
    void Move(int type)
    {
        Rigidbody2D enemy = OwnerAI.gameObject.GetComponent<Rigidbody2D>();
        //0 break
        if (type == 0)
        {
            enemy.velocity = Vector2.zero;
        }
        if (type == 1)
        {
			enemy.velocity = SlowMovespeed * TopScript.getPlayerAngleV3(OwnerAI.gameObject.transform.position).normalized;
        }
    }
    void Damaged()
    {
		GameUnit enemy = OwnerAI.gameObject.GetComponent<GameUnit>();      
		float armor = enemy.Armor;
        float damage = OwnerAI.LastHitPower * (1 - (armor) * 0.08f / (1 + 0.08f * (armor)));
		//enemy.getDamege (damage);
    }
    
    

}
