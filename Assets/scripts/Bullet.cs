using UnityEngine;
using System.Collections;

public class Bullet : BulletTemp {
	
	
	// Use this for initialization
	void Start () {
	GetComponent<Renderer>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(this.transform.position, shooter.transform.position);
		if(distance<100)
		{
			GetComponent<Renderer>().enabled=false;
		}
		else
		{
			GetComponent<Renderer>().enabled=true;
		}
		if(distance>1000)
		{
			Destroy(this.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "enemy")
		{
			Damageable npc = collision.gameObject.GetComponent<Damageable>();
			npc.Damage(25);
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
