using UnityEngine;
using System.Collections;

public class Player : Damageable {
	
	CharacterController controller;
	public GameObject weapon;
	public GameObject flash;
	public ParticleSystem shells;
	public OTAnimatingSprite character;
	public OTAnimatingSprite meleeRenderer;
	public GUIText healthDisplay;
	public GUIText powerDisplay;
	public float power = 1000;
	string currAnim="idle";
	// Use this for initialization
	OTSound shot;
	void Start () {
		
		shot = new OTSound("shot");
		shot.Idle();
		shot.Volume(0.1f);
		controller = GetComponent<CharacterController>();
		character=GetComponent<OTAnimatingSprite>();
		meleeRenderer.GetComponent<Renderer>().enabled=false;
		flash.GetComponent<Renderer>().enabled=false;
	}
	float Charspeed  = 400.0f;
	float jumpSpeed  = 600.0f;
	float gravity  = 480.0f;
		float posMod=1;
	float hmovement=0;
	float vmovement=0;
	
	private Vector3 moveDirection  = Vector3.zero;
	// Update is called once per frame
	
	void FixedUpdate () {
		if(Active)
		{
			if(power<1000)
			{
				power+=6;
			}
			healthDisplay.text="HEALTH: ";
			healthDisplay.text+=health.ToString();
			powerDisplay.text="POWER: ";
			powerDisplay.text+=power.ToString();
		}
		
	}
	
	void Update () {
		if(Active)
		{
			hmovement=	 Input.GetAxis("Horizontal");
			vmovement=Input.GetAxis("Vertical");
			if(Input.GetAxis("HorD")!=0)
			{
				hmovement=Input.GetAxis("HorD");
			}
			if(Input.GetAxis("VerD")!=0)
			{
				vmovement=Input.GetAxis("VerD");
				if(Mathf.Abs(hmovement)==1&&Mathf.Abs(vmovement)==1)
				{
					
					vmovement/=2;
				}
			}
			if(Input.GetButton("ShootUp"))
			{
				vmovement=1;
			}
			if(Input.GetButton("ShootDown"))
			{
				vmovement=-1;
			}
		}
		else
		{
			vmovement=0;
			hmovement=0;
			moveDirection = new Vector3(hmovement, 0,
                                0);
		}
		UpdateMovement();
		if(health<=0&&Active)
		{
		StartCoroutine(Die());
		}
		
		if(CanRestart)
		{
			if(Input.anyKey)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		
		}
		
	}
	
	private void UpdateMovement()
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
					character.Play("run");
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
			
        if (Input.GetButton ("Jump")) {
            moveDirection.y = jumpSpeed;
        }
			wspr.position=new Vector2(wspr.position.x,0.22f);
			
    }
		else
		{
			if(currAnim!="jump")
			{
				currAnim="jump";
				character.Play("jump");
				character.speed=0.6f;
			}
			
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
	
		if(posMod>0)
		{
			character.flipHorizontal=false;
			meleeRenderer.flipHorizontal=false;
			rotMod=rotation=90*weaponInput;
			weapon.transform.eulerAngles=new Vector3(0,0,rotation);
		}
		else
		{
			character.flipHorizontal=true;
		meleeRenderer.flipHorizontal=true;
			
			rotation=90*weaponInput +180;
			rotMod=-90*weaponInput +180;
			
			weapon.transform.eulerAngles=new Vector3(180,0,rotation);
		}
		//wspr.rotation=rotation;
		
		if (Input.GetButton ("Fire1"))
		{
			if(shells.isStopped)
			{
				shells.Play();
			}
		}
		else
		{
			if(!shells.isStopped)
			{
				shells.Stop();
			}
				
		}
		
		if(Input.GetButton ("Melee")&&canMelee&&power>=1000) {
			 StartCoroutine(Melee());
		}	
			
		if (Input.GetButton ("Fire1")&&canFire) {
			
             StartCoroutine(Fire(rotMod));
        }
		
		}
    // Apply gravity
    moveDirection.y -= gravity * Time.deltaTime;
    
    // Move the controller
    controller.Move(moveDirection * Time.deltaTime);
		
		if(Active)
		{
		OT.view.position=new Vector2(transform.position.x,transform.position.y);
		}
	}
	
	public bool canMelee=true;
	
	private IEnumerator Melee()
	{
		canFire=false;
		canMelee=false;
		power=0;
		weapon.GetComponent<Renderer>().enabled=false;
		flash.GetComponent<Renderer>().enabled=false;
		meleeRenderer.GetComponent<Renderer>().enabled=true;
		GetComponent<Renderer>().enabled=false;
			currAnim="melee";
			meleeRenderer.Play("melee");
			meleeRenderer.speed=0.5f;
		yield return new WaitForSeconds(0.75F);
		if(Active)
		{
			Vector3 Pos=transform.position;
			GameObject  bulletClone  = (GameObject )Instantiate(meleeBullet, Pos, transform.rotation);
			MeleeBullet bulletscr=bulletClone.GetComponent<MeleeBullet>();
			
			bulletscr.shooter=this.gameObject;
			OTSprite spr=bulletClone.GetComponent<OTSprite>();
			spr.position=new Vector2(Pos.x,Pos.y);
			if(posMod>0)
			{
				spr.flipHorizontal=false;
			}
			else
			{
					spr.flipHorizontal=true;
			
			}
			
			Vector3 force = new Vector3(posMod,0,0);
			CharacterController charc=GetComponent<CharacterController>();
			CapsuleCollider coll=GetComponent<CapsuleCollider>();
			
			Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), charc);
			if(coll!=null)
			{
				Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), coll);
			}
			bulletClone.GetComponent<Rigidbody>().AddForce(force* bulletSpeed*16000);
			
			
			meleeRenderer.GetComponent<Renderer>().enabled=false;
		    this.GetComponent<Renderer>().enabled=true;
			
		
			canFire = true;
			
			weapon.GetComponent<Renderer>().enabled=true;
			//yield return new WaitForSeconds(1.5f);
		    canMelee=true;
		}
		
	}
	
	private IEnumerator Fire(float rotation)
	{
	  	canFire = false;
		Vector3 Pos=flash.transform.position;
		if(!shot.isPlaying)
		{
		shot.Play();
		}
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
    
		
	    yield return new WaitForSeconds(fireDelay);
	    canFire = true;
		flash.GetComponent<Renderer>().enabled=false;
			
		
		
	}
	bool CanRestart=false;
	public IEnumerator Die()
	{
		if(Active)
		{
	
		
		Active=false;
			if(currAnim!="death")
			{
				currAnim="death";
				character.looping=false;
				character.Play("death");
				character.speed=0.6f;
				shells.Stop();
			}
				weapon.GetComponent<Renderer>().enabled=false;
				flash.GetComponent<Renderer>().enabled=false;
				GetComponent<Renderer>().enabled=true;
				meleeRenderer.GetComponent<Renderer>().enabled=false;
				
				hmovement=0;
				vmovement=0;
			
		yield return new WaitForSeconds(0.5f);	
				healthDisplay.text="YOU DIED - PRESS ANY KEY TO RESTART";
		CanRestart=true;
		}	
	}
	
	 public float pushPower = 2.0F;
   /* void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
        body.velocity = pushDir * pushPower;
    }
	*/
	
	
	bool canFire=true;
	float fireDelay = 0.25f;
	public GameObject bullet;
 	public GameObject meleeBullet;
	float bulletSpeed = 10000.0f;
}
