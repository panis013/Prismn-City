using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour2 {
	GameObject player;
	public Vector3 notFollowAimPos;
	Vector3 mousePos;
	[Range(0f,12f)]
	public float lerpRate;
	[Range(0f,1f)]
	public float mouseRate;
	public float mouseBorder;
	public bool follow = true;
	void Start(){
		setPlayerFollowed ();
	}
	void Update(){
		mousePos = GameController.MousePosition;
		setPlayerFollowed ();
		if (player != null && follow == true) {
			LinerLerpFollow ();
		} else if (follow == false) {
			LinerLerpNotFollow ();
		}
	}
	public void setPlayerFollowed(){
		player = GameController.CurrentPlayer;
	}
	void LinerLerpFollow(){
		float z = gameObject.transform.position.z;
		Vector3 CameraPos = gameObject.transform.position;
		CameraPos.z = 0f;
		Vector3 PlayerPos = player.transform.position;
		Vector3 PMDis = mousePos - PlayerPos;
		PMDis = PMDis.normalized * Mathf.Clamp (PMDis.magnitude, 0f, mouseBorder);
		PlayerPos = PlayerPos + PMDis * mouseRate;
		PlayerPos.z = 0f;
		Vector3 newPos = Vector3.Lerp (CameraPos, PlayerPos, lerpRate*Time.deltaTime);
		newPos.z = z;
		gameObject.transform.position = newPos;
		notFollowAimPos = newPos;
	}
	void LinerLerpNotFollow(){
		float z = gameObject.transform.position.z;
		Vector3 CameraPos = gameObject.transform.position;
		CameraPos.z = 0f;
		Vector3 newPos = Vector3.Lerp (CameraPos, notFollowAimPos, lerpRate*Time.deltaTime);
		newPos.z = z;
		gameObject.transform.position = newPos;
	}
}
