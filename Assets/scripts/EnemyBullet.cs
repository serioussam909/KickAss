using UnityEngine;
using System.Collections;

public class EnemyBullet : BulletTemp {
//public GameObject shooter;
	//public GameObject flash;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(shooter!=null)
		{
			float distance = Vector3.Distance(this.transform.position, shooter.transform.position);
			if(distance>1000)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			//in case the shooter is dead - remove bullet after 10 seconds
			Destroy(this.gameObject,10.0f);
		}
	}
	bool colliding=false;
	
	void OnCollisionStay(Collision collision)
	{
		if(!colliding)
		{
			colliding=true;
						
			if (collision.gameObject.tag == "player")
			{
				Damageable npc = collision.gameObject.GetComponent<Damageable>();
				npc.Damage(20);
			}
			 // destroys the bullet
			GameObject  flashClone  = (GameObject )Instantiate(flash, this.transform.position, this.transform.rotation);
			OTSprite spr=flashClone.GetComponent<OTSprite>();
			Vector3 Pos=transform.position;
			spr.position=new Vector2(Pos.x,Pos.y);
				Destroy(this.gameObject);
			Destroy (flashClone,0.25f);
		}
	}
}
