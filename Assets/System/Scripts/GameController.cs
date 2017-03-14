using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{ 
	public static Map CurrentMap;
	public static GameObject CurrentPlayer{ get { return GameObject.FindGameObjectWithTag ("Player"); } }
	public static Vector3 MousePosition {
		get {
			Vector3 mousePositionOnScreen = Input.mousePosition;   
			mousePositionOnScreen.z = 0f;
			Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint (mousePositionOnScreen);
			mousePositionInWorld.z = 0f;
			return mousePositionInWorld;
		}
	}
	public BulletData bd; 
	public int CurrentGun = 1;
	private int Prisms = 7;
	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	///设置

	///工具
}
