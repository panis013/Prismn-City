using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGun : MonoBehaviour {
    public float ClearInterval = 2f;
    public float DelayInterval = 0.01f;
    public int EffectiveTime = 1;
    GameObject gc;
    private float t = 0;
    private float t2 = 0;
    private int counter1;
    private int counter2;
    private bool isDelay = false;
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController");
        counter1 = 0;
        counter2 = 0;
    }
    void FixedUpdate()
    {
        t += Time.fixedDeltaTime;
        SwitchByKeyBoard();
        if (isDelay)
        {
            t2 += Time.fixedDeltaTime;
            if(t2>= DelayInterval)
            {
                t2 = 0;
                isDelay = false;
            }
        }
        else
        {
            SwitchByScroll();
        }
        if(t>= ClearInterval)
        {
            counter1 = 0;
            counter2 = 0;
            t = 0;
        }

    }
    void SwitchByScroll()
    {
           
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
         {
            counter1++;
            //counter2 = 0;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            counter2++;
            //counter1 = 0;
        }
        if (counter1 >= EffectiveTime)
        {
            isDelay = true;
            gc.GetComponent<GameController>().GunSwitchDown();
            counter1 = 0;
        }
        if (counter2 >= EffectiveTime)
        {
            isDelay = true;
            gc.GetComponent<GameController>().GunSwitchUp();
            counter2 = 0;
        }
    }
    void SwitchByKeyBoard()
    {
        GameController GC = gc.GetComponent<GameController>();
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            GC.GunSwitchNum(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            GC.GunSwitchNum(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            GC.GunSwitchNum(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            GC.GunSwitchNum(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            GC.GunSwitchNum(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            GC.GunSwitchNum(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            GC.GunSwitchNum(7);
        }

    }
}
