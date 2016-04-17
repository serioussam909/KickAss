using UnityEngine;
using System.Collections;

public class Npc : Damageable {
	
	CharacterController controller;
	public GameObject weapon;
	public OTAnimatingSprite character;
	public GameObject flash;
	public GameObject bloodSplash;
	public GameObject explosion;
	
	OTSound shot;
	// Use this for initialization
	void Start () {
	//	Destroy (this.gameObject);
	//	return;
		shot = new OTSound("shot_enemy");
		shot.Idle();
		shot.Volume(0.05f);
		controller = GetComponent<CharacterController>();
		character=GetComponent<OTAnimatingSprite>();
		
		
		foreach(Transform child   in transform){
    	if(child.gameObject.tag == "weapon"){
	    		weapon=child.gameObject;
	    	}
			
			if(child.gameObject.tag == "blood"){
	    		bloodSplash=child.gameObject;
	    	}
			if(child.gameObject.tag == "explosion"){
	    		explosion=child.gameObject;
	    	}
    	}
		
		
		foreach(Transform child   in weapon.transform){
    	if(child.gameObject.tag == "flash"){
    		flash=child.gameObject;
    	}
		}
	
		bloodSplash.GetComponent<Renderer>().enabled=false;
		explosion.GetComponent<Renderer>().enabled=false;
		flash.GetComponent<Renderer>().enabled=false;
		float speedVariation = Random.Range(0,200) ;
		Charspeed+=speedVariation;
	}
	
	string currAnim="idle";
	float Charspeed  = 100.0f;
	float jumpSpeed  = 400.0f;
	float gravity  = 480.0f;
		float posMod=1;
	private Vector3 moveDirection  = Vector3.zero;
	// Update is called once per frame
	
	float hmovement=0;
	float vmovement=0;
	bool jump=false;
	bool fire=false;
	float distance;
	float playerDirection=-1;
	  GameObject player;
	void Update () {
	 //move to player if close enough
	if(Active)
		{
		player = GameObject.FindWithTag("player");
		Player pl=player.GetComponent<Player>();
		distance = Vector3.Distance(this.transform.position, player.transform.position);
		if(pl.Active)
		{
			if(distance<700)
			{
				fire=true;
			}
			else
			{
				fire=false;
			}
			
			
			if(Mathf.Abs (transform.position.y-player.transform.position.y)<400)
				{
			if(distance<800&&distance>500)
			{
				if(this.transform.position.x>player.transform.position.x)
				{
					hmovement=-1;
					playerDirection=-1;
				}
				else
				{
					hmovement=1;
					playerDirection=1;
				}
			}
			else
			if(distance<=400)
			{
				if(this.transform.position.x>player.transform.position.x)
				{
					hmovement=1;
					playerDirection=-1;
				}
				else
				{
					hmovement=-1;
					playerDirection=1;
				}
			}
			else
			{
				hmovement=0;
			}
				}
				else
			{
				hmovement=0;
			}
				
		}
		else
		{
			hmovement=0;
			vmovement=0;
			fire=false;
		}
		}
		else
		{
			hmovement=0;
			vmovement=0;
			fire=false;
		}
		UpdateMovement ();
		if(health<=0)
		{
			Die ();
		}
	
	}
	void UpdateMovement()
	{
		if(Active)
		{
			OTSprite wspr=weapon.GetComponent<OTSprite>();
			OTSprite fspr=flash.GetComponent<OTSprite>();
		
			
		if (controller.isGrounded) {  
        // We are grounded, so recalculate
        // move direction directly from axes
        moveDirection = new Vector3(hmovement, 0,
                                0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= Charspeed;
      if(Mathf.Abs(hmovement)>0.1)
			{
			if(currAnim!="run")
			{
				currAnim="run";
					
					if(playerDirection!=hmovement)
					{
				character.PlayBackward("run");
			
					}
					else{
				character.Play("run");
					}
						character.speed=0.6f;
			}
			}
			else
			{
				if(currAnim!="idle")
			{
				currAnim="idle";
				
				character.Play("idle");
				
				character.speed=0.2f;
					
			}
			}
			
        if (jump) {
            moveDirection.y = jumpSpeed;
        }
			wspr.position=new Vector2(wspr.position.x,0.22f);
			
    }
		else
		{
		
			
			wspr.position=new Vector2(wspr.position.x,0.12f);
			
			    moveDirection.x =hmovement* Charspeed;
			
		}
			float weaponInput=vmovement;
		
		float rotation=0;
		float rotMod=0;
		
		if(moveDirection.x>=0.1)
		{
			posMod=1;
			
			
						
		}
		else if(moveDirection.x<=-0.1)
		{
			
			posMod=-1;
			
		}
		
		
		bool flip=false;
		if(posMod>0)
		{
			
			
			
			if(hmovement==0)
			{
				flip=false;
			}
		
		
		}
		else
		{
			
			if(hmovement==0)
			{
				flip=true;
			}
				
		
		}
		if(distance<1000)
		{
			weapon.transform.LookAt(player.transform);
			if(playerDirection>0)
				{
					flip=false;
					weapon.transform.eulerAngles=new Vector3(0,0,-weapon.transform.eulerAngles.x);
					rotMod=weapon.transform.eulerAngles.z;
					
			}
			else
			{
				flip=true;
				weapon.transform.eulerAngles=new Vector3(180,0,180-weapon.transform.eulerAngles.x);
		
			rotMod=180-weapon.transform.eulerAngles.z;
				
			}
		}
		character.flipHorizontal=flip;
		
		
		 if (fire&&canFire) {
			
             StartCoroutine(Fire(rotMod));
        }
		}

    // Apply gravity
    moveDirection.y -= gravity * Time.deltaTime;
    
    // Move the controller
    controller.Move(moveDirection * Time.deltaTime);
		
		
	}
	
	private IEnumerator Fire(float rotation)
	{	
		canFire = false;
		Vector3 Pos=flash.transform.position;
		shot.Play();
		GameObject  bulletClone  = (GameObject )Instantiate(bullet, Pos, transform.rotation);
    	flash.GetComponent<Renderer>().enabled=true;
		BulletTemp bulletscr=bulletClone.GetComponent<BulletTemp>();
		bulletscr.shooter=this.gameObject;
		OTSprite spr=bulletClone.GetComponent<OTSprite>();
		spr.position=new Vector2(Pos.x,Pos.y);
		spr.rotation=rotation;
		Vector3 force = new Vector3(1,0,0);
		CharacterController charc=GetComponent<CharacterController>();
		CapsuleCollider coll=GetComponent<CapsuleCollider>();
		
		Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), charc);
		if(coll!=null)
		{
			Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), coll);
		}
		
    	force= Quaternion.Euler(0, 0, rotation) * force;
		bulletClone.GetComponent<Rigidbody>().AddForce(force* bulletSpeed);
    	yield return new WaitForSeconds(0.1f);
	    
		flash.GetComponent<Renderer>().enabled=false;
		
	    yield return new WaitForSeconds(fireDelay);
	    canFire = true;
		
			
		
		
	}
	
	public  void Die()
	{
		if(Active)
		{
	
			bloodSplash.GetComponent<Renderer>().enabled=true;
			explosion.GetComponent<Renderer>().enabled=true;
			OTAnimatingSprite splash=bloodSplash.GetComponent<OTAnimatingSprite>();
			OTAnimatingSprite explosions=explosion.GetComponent<OTAnimatingSprite>();
			splash.Play("splash");
			explosions.Play("flash2");
			
			weapon.GetComponent<Renderer>().enabled=false;
			flash.GetComponent<Renderer>().enabled=false;
			
			this.GetComponent<Renderer>().enabled=false;
			Active=false;
			Destroy(this.gameObject,0.3f);
		}
	}
	

	
	
	
	bool canFire=true;
	float fireDelay = 1f;
	public GameObject  bullet;
 	float bulletSpeed = 10.0f;
}
