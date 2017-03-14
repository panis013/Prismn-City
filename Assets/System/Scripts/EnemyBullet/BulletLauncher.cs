using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLauncher : TopScript
{
    public GameObject Bullet;
    private GameObject BulletClone;
    private GameObject dmk;
    private GameObject[] dmks;
    Vector3 position;

    //弹幕发射器 
    public void Launch(BulletParameter BP)
    {
        float rotation = analysis(BP.rotation);
        float speed = analysis(BP.speed);
        int amount = (int)analysis(BP.amount);
        float length = analysis(BP.length);
        float midAngle = analysis(BP.midAngle);
        float spreadAngle = analysis(BP.spreadAngle);
        float distance = analysis(BP.distance);
        float minspeed = analysis(BP.minspeed);
        float maxspeed = analysis(BP.maxspeed);
        Vector2 Acc;
        Acc.x = analysis(BP.AccX);
        Acc.y = analysis(BP.AccY);
        float MSpeed = analysis(BP.MSpeed);
        float RotateSpeed = analysis(BP.RotateSpeed);
        float RotateAcc = analysis(BP.RotateAcc);
        float Rotation = analysis(BP.Rotation);
        float Lifetime = analysis(BP.Lifetime);
        float JudgeSize = analysis(BP.JudgeSize);
        float alpha = analysis(BP.alpha);

        
        if (BP.laucherType == BulletData.LaucherType.ABullet)
        {
            dmk = LaunchABullet(BP.bulletType,rotation,BP.offset,BP.velocity);
        }

        if(BP.laucherType == BulletData.LaucherType.Barrier)
        {
            dmks = LaunchABarrier(BP.bulletType, rotation, BP.offset, speed, amount, midAngle, spreadAngle, distance);
        }
        else if (BP.laucherType == BulletData.LaucherType.Random)
        {
            dmks = LaunchRandom(BP.bulletType, BP.offset,amount,midAngle, spreadAngle, distance,speed);
        }
        else if (BP.laucherType == BulletData.LaucherType.Line)
        {
            dmks = LaunchALine(BP.bulletType, BP.offset, amount, midAngle, distance, minspeed, maxspeed);
        }
        else if (BP.laucherType == BulletData.LaucherType.Horizon)
        {
            dmks = LaunchHorizon(BP.bulletType, BP.offset, amount, midAngle, distance, length, speed);
        }
        else
        {
            Debug.Log("参数出现问题 未发射");
            return;
        }
        //给弹幕附加属性
     if (BP.laucherType == BulletData.LaucherType.ABullet)
        {
            Bullet temp = dmk.GetComponent<Bullet>();
            temp.isRemoveable = BP.isRemoveable;
            temp.IsRemovedOut = BP.IsRemovedOut;
            temp.isAwake = BP.isAwake;
            temp.IsForward = BP.IsForward;
            temp.Acc = Acc;
            temp.MSpeed = MSpeed;
            temp.Rotation = Rotation;
            temp.RotateAcc = RotateAcc;
            temp.RotateSpeed = RotateSpeed;
            temp.Lifetime = Lifetime;
            temp.JudgeSize = JudgeSize;
            temp.alpha = alpha;
            temp.Counter = BP.Counter;
        }
        else
        {
            foreach(GameObject Dmk in dmks)
            {
                Bullet temp = Dmk.GetComponent<Bullet>();
                temp.isRemoveable = BP.isRemoveable;
                temp.IsRemovedOut = BP.IsRemovedOut;
                temp.isAwake = BP.isAwake;
                temp.IsForward = BP.IsForward;
                temp.Acc = Acc;
                temp.MSpeed = MSpeed;
                temp.Rotation = Rotation;
                temp.RotateAcc = RotateAcc;
                temp.RotateSpeed = RotateSpeed;
                temp.Lifetime = Lifetime;
                temp.JudgeSize = JudgeSize;
                temp.alpha = alpha;
                temp.Counter = BP.Counter;
            }
        }
        
    }
    public void FixedUpdate()
    {
        position = gameObject.GetComponent<Rigidbody2D>().position;


    }
    /************************基础发射函数****************************/
    public GameObject Create(BulletData.BulletType type, float rotation, Vector3 pos)
    {   

        Quaternion rot = Angle2R(rotation);
        BulletClone = (GameObject)Instantiate(Bullet, pos, rot);
        BulletClone.GetComponent<SpriteRenderer>().sprite = BulletData.instance.GetSprite(type);
		BulletClone.GetComponent<Bullet> ().setOwner (gameObject);
        return BulletClone;

    }
    public void setToward(GameObject dmk)
    {
        Vector3 v = GameObject.FindGameObjectWithTag("Player").transform.position - dmk.transform.position;
        v.Normalize();
        float m = dmk.GetComponent<Bullet>().velocity.magnitude;
        dmk.GetComponent<Bullet>().velocity = v * m;
    }

    /************************内置发射函数****************************/
    public GameObject LaunchABullet(BulletData.BulletType type, float rotation, Vector3 position, Vector3 velocity)
    {
            
        GameObject Dmk = Create(type, rotation, position);
        Dmk.GetComponent<Bullet>().velocity = velocity;
        return Dmk;
    }
    public GameObject[] LaunchABarrier(BulletData.BulletType type, float rotation, Vector3 offset, float speed, int amount, float midAngle, float spreadAngle, float distance)
    {   //数量 发射角度范围（0-360为整圈） 实际发射点距中心的距离 发射一圈
        GameObject[] dmks = new GameObject[amount];
        float minAngle = midAngle - (spreadAngle / 2);
        float maxAngle = midAngle + (spreadAngle / 2);
        float intervalAngle;//计算每颗之间的夹角
        if ((maxAngle - minAngle) % 360f == 0f)
        {
            intervalAngle = (maxAngle - minAngle) / (amount);
        }
        else
        {
            if (amount != 1 && amount != 0)
            {
                intervalAngle = (maxAngle - minAngle) / (amount - 1);
            }
            else
            {
                intervalAngle = 360f;
            }
        }
        Vector3 direction1=Vector3.zero;
        if (amount != 1 && amount != 0)
            direction1 = Angle2V(minAngle);
        else
        {
            if (amount == 1)
            {
                direction1 = Angle2V(midAngle);
            }
        }
        for (int i = 0; i < amount; i++)
        {
            Vector3 p = position + offset + direction1 * distance;
            Vector3 sp = direction1 * speed;
            float localR = Vector2A(direction1);
            dmks[i]=LaunchABullet(type, localR, p, sp);
            direction1 = VectorRotate(direction1, intervalAngle);
        }
        return dmks;
    }
    public GameObject[] LaunchRandom(BulletData.BulletType type, Vector3 offset, int amount, float midAngle, float spreadAngle, float distance, float speed)
    {
        GameObject[] dmks = new GameObject[amount];
        float minAngle = midAngle - (spreadAngle / 2);
        float maxAngle = midAngle + (spreadAngle / 2);
        for (int i = 0; i < amount; i++)
        {
            float angle1 = getRandom(minAngle, maxAngle);
            Vector3 direction1 = Angle2V(angle1);
            Vector3 p = position + offset+ direction1 * distance;
            Vector3 sp = direction1 * speed;
            float localR = Vector2A(direction1);
            GameObject dmk = LaunchABullet(type, localR, p, sp);
            dmks[i] = dmk;
        }
        return dmks;
    }
    public GameObject[] LaunchALine(BulletData.BulletType type, Vector3 offset, int amount, float midAngle, float distance, float minspeed, float maxspeed)
    {
        GameObject[] dmks = new GameObject[amount];
        float addspeed = (maxspeed - minspeed) / (amount - 1);
        Vector3 direction1 = Angle2V(midAngle);
        Vector3 p = position + offset + direction1 * distance;
        for (int i = 0; i < amount; i++)
        {
            Vector3 sp = direction1 * minspeed;
            minspeed += addspeed;
            float localR = Vector2A(direction1);
            GameObject dmk = LaunchABullet(type, localR, p, sp);
            dmks[i] = dmk;
        }
        return dmks;
    }
    public GameObject[] LaunchHorizon(BulletData.BulletType type, Vector3 offset, int amount, float midAngle, float distance, float length, float speed)
    {
        GameObject[] dmks = new GameObject[amount];
        float interval = (length) / (amount - 1);
        Vector3 direction1 = Angle2V(midAngle + 90f);
        Vector3 direction2 = Angle2V(midAngle);
        Vector3 baseposition = position + offset - direction1 * (length / 2f);
        for (int i = 0; i < amount; i++)
        {
            Vector3 p = baseposition + direction2 * distance;
            Vector3 sp = direction2 * speed;
            baseposition += direction1 * interval;
            float localR = Vector2A(direction2);
            GameObject dmk = LaunchABullet(type, localR, p, sp);
            dmks[i] = dmk;
        }
        return dmks;
    }
    /************************表达式解析函数****************************/
    float analysis(string tar)
    {

        // 函数  
        tar = delSpace(tar);
        if (CheckGrammar(tar))
        {
            return parse(tar);
        }
        else
        {
            Debug.Log("语法错误");
            return 0;
        }
    }
    string delSpace(string tar)
    {
        string NewStr = "";
        for (int i = 0; i < tar.Length; i++)
        {
            if (tar[i] != ' ')
                NewStr += tar[i];
        }
        return NewStr;

    }
    bool CheckGrammar(string tar)
    {
        Stack<int> ParaStack = new Stack<int>();
        //括号是否对齐
        for (int i = 0; i < tar.Length; i++)
        {
            if (tar[i] == '(')
            {
                ParaStack.Push(1);
            }
            if (tar[i] == ')')
            {
                if (ParaStack.Count != 0)
                {
                    ParaStack.Pop();
                }
                else
                {
                    Debug.Log("括号没有对齐；有多余的右括号");
                    return false;
                }
            }
        }
        if (ParaStack.Count != 0)
        {
            Debug.Log("括号没有对齐；有多余的左括号");
            return false;
        }
        // ----------------------------
        //
        return true;
    }
    float parse(string tar)
    {
        //分隔括号
        if (string.IsNullOrEmpty(tar))
        {
            return 0;
        }
        int startIndex = tar.LastIndexOf("(");
        if (startIndex != -1)
        {
            int endIndex = tar.IndexOf(")", startIndex);
            int len = endIndex - startIndex - 1;
            float d = parse(tar.Substring(startIndex + 1, len));
            return parse(tar.Substring(0, startIndex) + d + tar.Substring(endIndex + 1));
        }

        //加减入栈递归
        int index = tar.IndexOf("+");
        if (index != -1)
        {
            return parse(tar.Substring(0, index)) + parse(tar.Substring(index + 1));
        }
        index = tar.LastIndexOf("-");
        if (index != -1)
        {
            return parse(tar.Substring(0, index)) - parse(tar.Substring(index + 1));
        }
        //进行乘除运算（已经没有加减）
        index = tar.IndexOf("*");
        if (index != -1)
        {
            return parse(tar.Substring(0, index)) * parse(tar.Substring(index + 1));
        }
        index = tar.LastIndexOf("/");
        if (index != -1)
        {
            return parse(tar.Substring(0, index)) / parse(tar.Substring(index + 1));
        }
        //至此，剩余单个数字，函数，或变量
        //变量 变量名 函数 函数名[函数值1,函数值2]...
        float temp;
        if (tar.IndexOf("[") != -1)
        {
            return FuntionAnal(tar);
        }

        else if (float.TryParse(tar, out temp))
        {
            return temp;
        }
        else
        {
            return VariableAnal(tar);
        }
    }
    float FuntionAnal(string tar)
    {
        int index = tar.IndexOf("[");
        int endindex = tar.IndexOf("]");
        if (endindex == -1)
        {
            Debug.Log("表达式函数缺少右方括号" + this.name);
            return 0;
        }
        string funtionName = tar.Substring(0, index);


        //rand[a,b] 取a,b之间的随机数（float） 若a,b都是整数 返回整形随机数
        if (string.Equals(funtionName, "rand"))
        {
            string[] Variables = tar.Substring(index + 1, (endindex - index - 1)).Split(',');
            if (Variables.Length != 2)
            {
                Debug.Log("表达式函数rand[]参数错误" + this.name);
                return 0;
            }
            int a, b;
            if(int.TryParse((Variables[0]),out a)&& int.TryParse((Variables[1]), out b))
                return getIntRandom(int.Parse(Variables[0]), int.Parse(Variables[1]));
            else
                return getRandom(float.Parse(Variables[0]), float.Parse(Variables[1]));

        }
        else if (string.Equals(funtionName, "sin"))
        {
            string[] Variables = tar.Substring(index + 1, (endindex - index - 1)).Split(',');
            if (Variables.Length != 1)
            {
                Debug.Log("表达式函数sin[]参数错误" + this.name);
                return 0;
            }
            return Mathf.Sin(float.Parse(Variables[0]));
        }
        else if (string.Equals(funtionName, "cos"))
        {
            string[] Variables = tar.Substring(index + 1, (endindex - index - 1)).Split(',');
            if (Variables.Length != 1)
            {
                Debug.Log("表达式函数cos[]参数错误" + this.name);
                return 0;
            }
            return Mathf.Cos(float.Parse(Variables[0]));
        }
        else
        {
            Debug.Log("表达式有非法的函数名称" + this.name);
            return 0;
        }
    }
    float VariableAnal(string tar)
    {
        if (string.Equals(tar, "player"))
        {
            return getPlayerAngle(position);
        }

        else
        {
            Debug.Log("表达式有非法的变量名称" + this.name);
            return 0;
        }
    }
}
