using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BulletData : ScriptableObject
{
    public static BulletData instance;
    public Sprite[] DmkSprite;
    public void OnEnable()
    {
        BulletData.instance = this;
    }
    public Sprite GetSprite(BulletData.BulletType BT)
    {
        return DmkSprite[(int)BT];
    }
    public Sprite GetSprite(int BT)
    {
        return DmkSprite[BT];
    }
    public enum BulletType
    {
        弹幕1 =0,
        另一个弹幕 = 1

    }
    public enum LaucherType
    {
        ABullet = 0,
        Barrier = 1,
        Random = 2,
        Line = 3,
        Horizon = 4
    }
    //
    
    public enum BulletPatameterType
    {

    }
}
