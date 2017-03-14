using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGun : TopScript {
    public float Radius = 50f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePositionOnScreen = Input.mousePosition;
        mousePositionOnScreen.z = screenPosition.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        Vector3 v = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = v + (mousePositionInWorld - v).normalized * Radius;
    }
}
