using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class TestPlayerControl : MonoBehaviour {
	[TextArea]
	[Tooltip("??")]
	public string Name;
	public float Speed = 800f;
	public Map testmap;
	void Start () {
		gameObject.AddComponent<GamePosition> ().Init (Settings.Layers.Second,0);
	}
	void FixedUpdate () {
		Move ();
	}
	void Move(){
		Vector2 velocity = (Input.GetAxis ("Horizontal") * Vector2.right + Input.GetAxis ("Vertical") * Vector2.up).normalized*Speed;
		Rigidbody2D r2 = gameObject.GetComponent<Rigidbody2D> ();
		r2.velocity = velocity;
	}
	public void Update(){
		
	}
}
