using UnityEngine;
using System.Collections;

public class TopScript : MonoBehaviour
{
    /***************实用函数****************/
	public static Vector3 setDirection(Vector3 v, float angle)
    {
        float m = v.magnitude;
        return Angle2V(angle) * m;
    }
	public static Vector3 VectorRotate(Vector3 v, float angle)
    {
        float angle1 = Vector2A(v);
        return Angle2V(angle1 + angle) * v.magnitude;
    }
	public static Quaternion Angle2R(float r)
    {
        return Quaternion.Euler(0, 0, r);
    }
	public static float Vector2A(Vector3 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
	public static Quaternion Vector2R(Vector3 v)
    {
        return Angle2R(Vector2A(v));
    }
	public static Vector3 Angle2V(float a)
    {
        a = getBaseDeg(a);
        Vector3 v = Vector3.right;
        if (a == 90f)
        {
            return Vector3.up;
        }
        else if (a == 270f)
        {
            return Vector3.down;
        }
        else
        {
           
            v.x *= Mathf.Sign(Mathf.Cos(Mathf.Deg2Rad * a));
            v.y = Mathf.Tan(a * Mathf.Deg2Rad) * v.x;
            v.Normalize();
            return v;
        }
    }
	public static float getBaseDeg(float a)
    {
        while (a > 360f || a < 0f)
        {
            if (a > 360f)
            {
                a -= 360f;
            }
            else
            {
                a += 360f;
            }
        }
        return a;
    }
	public static float getRandom(float min, float max)
    {
        return min + Random.value * (max - min);
    }
	public static int getIntRandom(int min, int max)
    {
        int i = (int)(getRandom((float)min, (float)(max + 0.999999f)));
        return i;
    }
	public static int getSignRandom()
    {
        int i = getIntRandom(0, 1);
        if (i == 1)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
	public static Vector3 getRandomV(float min, float max)
    {
        Vector3 v = Angle2V(getRandom(min, max));
        return v;
    }
	public static float getPlayerAngle(Vector3 position)
	{
		Vector3 v = GameObject.FindGameObjectWithTag("Player").transform.position - position;
		return Vector2A(v);
	}
	public static Vector3 getPlayerAngleV3(Vector3 position)
	{
		Vector3 v = GameObject.FindGameObjectWithTag("Player").transform.position - position;
		return v;
	}
	public static float GetPlayerDistance(GameObject thisgo)
	{
		Vector3 v = GameObject.FindGameObjectWithTag("Player").transform.position - thisgo.transform.position;
		return v.magnitude;
	}
}
