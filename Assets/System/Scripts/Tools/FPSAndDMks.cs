using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSAndDMks : MonoBehaviour2 {
	public float FPSTiming = 0.25f;
	float FPSTimer;
	int count;
	float fps;
	void Start(){
		float FPSTimer = Time.realtimeSinceStartup;
	}
	void Update () {
		count++;
		float FPSCurrentTimer = Time.realtimeSinceStartup;
		if (FPSCurrentTimer > FPSTiming + FPSTiming) {
			fps = (float)count / (FPSCurrentTimer - FPSTimer);
			FPSTimer = FPSCurrentTimer;
			count = 0;
		}
		Text t = gameObject.GetComponent<Text> ();
		t.text = "DMKs:" + GameObject.FindGameObjectsWithTag ("EnemyBullet").Length.ToString () + "FPS:" + fps.ToString ();
	}
}
