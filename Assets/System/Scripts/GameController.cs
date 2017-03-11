using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
	public static Map CurrentMap;
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
