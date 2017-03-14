using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
	public static Map CurrentMap;
	public BulletData bd; 
	public int CurrentGun = 1;
	private int Prisms = 7;
	public void GunSwitchUp()
	{
		if (CurrentGun == Prisms)
		{
			CurrentGun = 1;
		}
		else if (CurrentGun< Prisms)
		{
			CurrentGun++;
		}

	}
	public void GunSwitchDown()
	{
		if (CurrentGun == 1)
		{
			CurrentGun = Prisms;
		}
		else if (CurrentGun > 1)
		{
			CurrentGun--;
		}

	}
	public void GunSwitchNum(int num)
	{
		if (num <= Prisms) CurrentGun = num;
	}
	void Start ()
	{
		SetResolution ();
	}

	void Update ()
	{
		
	}

	///设置
	void SetResolution ()
	{
		Resolution[] reslution = Screen.resolutions; 
		int standard_width;
		int standard_height;
		/*
		standard_width = reslution[reslution.Length - 1].width;  
		standard_height = reslution[reslution.Length - 1].height;
		*/
		standard_width = 1600;
		standard_height = 1100;
		Screen.SetResolution (Convert.ToInt32 (standard_width), Convert.ToInt32 (standard_height), false); 
	}
}
