using UnityEngine;
using System.Collections;

public class MeleeBullet : BulletTemp {

	// Use this for initialization
	void Start () {
	//renderer.enabled=false;
		Destroy(this.gameObject,1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(this.transform.position, shooter.transform.position);
		if(distance<100)
		{
		//	renderer.enabled=false;
		}
		else
		{
		//	renderer.enabled=true;
		}
		
		
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "enemy")
		{
			Damageable npc = collision.gameObject.GetComponent<Damageable>();
			npc.Damage(550);
		}
		if (collision.gameObject.tag  == "enemyBullet")
		{
			Destroy(collision.gameObject);
		}	 
	}
}
