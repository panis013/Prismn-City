using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : TopScript {
	[HideInInspector]
	public GameUnit owner;
	public float Power = 1f;//analyze.
	public GameUnit.DamegeType damegeType;

	public BulletData.BulletType Type;
    public bool isRemoveable = true;//效果消弹
    public bool IsRemovedOut = true;//出屏消弹
    public bool isAwake = true;
    public bool IsForward = true;
    public Vector3 velocity = Vector3.zero;
    public Vector3 Acc = Vector3.zero;
    public float MSpeed = 0f;//最速度
    public float RotateSpeed;
    public float RotateAcc;
    public float Rotation;
    public float Lifetime=3f;
    public float JudgeSize = 0.04f;
    public float alpha = 1f;
    public int[] Counter;//可用的计数器
    float t;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        t = Time.fixedDeltaTime;
        OutBoundCheck();
        AlphaCheck();
        LifeMinus();
        CheckSize();
        Calc();   
        Move();


    }
    void Calc( ) //计算velocity和Rotation
    {
        bool isAcc = true;
        Vector3 V = velocity;
        float MS = MSpeed;
        //最速度影响
        if (MS != 0f)
        {
            if (MS > 0)
            {
                if (V.magnitude >= MS)
                {
                    isAcc = false;
                    velocity *= MS / velocity.magnitude;
                }
                if(V.magnitude < MS)
                {
                    isAcc = false;
                    velocity *= -MS / velocity.magnitude;
                }
            }
        }
        else
        {
            if (Vector3.Dot(V, V + Acc * t) < 0f)
            {
                isAcc = false;
                velocity = Vector3.zero;
                V = velocity;
            }
        }
        //加速度
        if(isAcc) {
            velocity += Acc * t;
        }
        //角速度
        RotateSpeed += RotateAcc * t;
        //旋转
        if(IsForward && velocity != Vector3.zero)
        {
            Rotation= Vector2A(V);
        }
        else
        {
            Rotation = RotateSpeed * t;
        }

    }
     void Move()
    {
        Rigidbody2D r2 = gameObject.GetComponent<Rigidbody2D>();
        r2.velocity = velocity;
        r2.rotation = Rotation;
    }
    void CheckSize()
    {
        CircleCollider2D C2 = gameObject.GetComponent<CircleCollider2D>();
        C2.radius = JudgeSize;
    }
    void LifeMinus()
    {
        Lifetime -= t;
        if (Lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OutBoundCheck()
    {
        if (IsRemovedOut)
        {

        }
    }
    void AlphaCheck()
    {
        if (alpha < 0.9)
        {
            isAwake = false;
        }
        if (alpha >= 0.9)
        {
            isAwake = true;
        }
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = alpha;
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    void ChangeSize(float Size,float changeSpeed)
    {
        if (changeSpeed == 0f)
        {
            JudgeSize = Size;
        }
      
    }
    void ChangeSizeOverTime(float Size, float time)
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAwake)
        {
			GameUnit aim = collider.gameObject.GetComponent<GameUnit>();
			GameUnit self = gameObject.GetComponent<GameUnit> ();
			if (owner == null || aim.unitType != self.unitType) {
				aim.getDamege (Power, damegeType, gameObject.transform.position, owner);
			}
			Die ();
        }
    }
	void Die(){
		Destroy(this.gameObject);
	}
	public void setOwner(GameObject go){
		GameUnit gu = go.GetComponent<GameUnit> ();
		if (gu != null) {
			owner = gu;
		}
	}
	public void setOwner(GameUnit gu){
		owner = gu;
	}
}
