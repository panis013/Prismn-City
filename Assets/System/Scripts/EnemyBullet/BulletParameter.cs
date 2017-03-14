using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BulletParameter : ScriptableObject
{
    public enum Ctrltype1
    {
        变化到,
        增加,
        减少,

    }
    //存储弹幕需要的变量
    /************************发射器需要使用的****************************/
    public BulletData.BulletType bulletType=0;
    public BulletData.LaucherType laucherType=0;
    public string rotation = "0";//弹幕朝向（角度）
    public string speed = "0";//弹幕速度
    public string amount = "0";
    public string length = "0";//LaunchHorizon
    public string midAngle = "0";//LaunchABarrier,LaunchRandom
    public string spreadAngle = "360";//LaunchABarrier,LaunchRandom
    public string distance = "0";//发射点距弹幕中心点距离（半径）
    public string minspeed = "0";//LaunchALine
    public string maxspeed = "0";//LaunchALine

    public Vector3 offset=Vector3.zero;//发射点距弹幕中心点偏移量(向量) 用给LaunchABullet则是发射位置
    public Vector3 velocity = Vector3.zero;///LaunchABullet


    /************************弹幕需要使用的****************************/

    public string AccX = "0";
    public string AccY = "0";
    public string MSpeed = "0";
    public string RotateSpeed = "0";
    public string RotateAcc = "0";
    public string Rotation = "0";
    public string Lifetime = "5";
    public string JudgeSize = "0.04";
    public string alpha = "1";


    public bool isRemoveable = true;//效果消弹
    public bool IsRemovedOut = true;//出屏消弹
    public bool isAwake = true;
    public bool IsForward = true;

    public int[] Counter=new int[10];//可用的计数器
    public float ans;
    // 变量

    //发射器事件组
    [System.Serializable]
    public struct LauncherCtrl
    {
        //从【某个时间】开始 （经过【某段时间】）使得【目标变量】改变【目标数值】
        //从【某个时间】开始 每【某段时间】 使得【目标变量】改变【目标数值】
        //从【某个时间】开始 调用某个内置方法
        public string startTime;
        public string continuousTime;
        public string Expr;
        public Ctrltype1 AddtionalInfo;
        public string number;
        public string note;
    }










}
