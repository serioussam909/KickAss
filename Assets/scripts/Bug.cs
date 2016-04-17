using UnityEngine;
using System.Collections;

public class Bug : Damageable {

		CharacterController controller;
	
	public OTAnimatingSprite character;
	string currAnim="idle";
		public GameObject bloodSplash;
	// Use this for initialization
	void Start () {
	//	Destroy (this.gameObject);
	//	return;
	controller = GetComponent<CharacterController>();
		character=GetComponent<OTAnimatingSprite>();
	foreach(Transform child   in transform){
    	if(child.gameObject.tag == "blood"){
    		bloodSplash=child.gameObject;
    	}
    }
		bloodSplash.GetComponent<Renderer>().enabled=false;
		float speedVariation = Random.Range(0,300) ;
		Charspeed+=speedVariation;
	}
	float Charspeed  = 150.0f;
	float jumpSpeed  = 400.0f;
	float gravity  = 480.0f;
		float posMod=1;
	private Vector3 moveDirection  = Vector3.zero;
	// Update is called once per frame
	
	float movement=0;
	bool jump=false;
	bool fire;
	void Update () {
	 //move to player if close enough
		  GameObject player = GameObject.FindWithTag("player");
		Player pl=player.GetComponent<Player>();
		float distance = Vector3.Distance(this.transform.position, player.transform.position);
		
			if(distance<700&&distance>40&&Mathf.Abs (transform.position.y-player.transform.position.y)<200)
			{
				if(this.transform.position.x>player.transform.position.x)
				{
					movement=-1;
				}
				else
				{
					movement=1;
				}
				if(currAnim!="walk")
				{
					currAnim="walk";
					character.Play("walk");
				character.speed=2;
				}
			}
			else
			{
				if(currAnim!="idle")
				{
					currAnim="idle";
					character.Play("idle");
					character.speed=1;
				}
				
				movement=0;
			}
		
		if(!pl.Active)
		{
			
				if(currAnim!="idle")
				{
					currAnim="idle";
					character.Play("idle");
					character.speed=1;
				}
			movement=0;
		}
		
		UpdateMovement ();
		if(health<=0)
		{
		Die ();
		}
	
	}
	void UpdateMovement()
	{
			if (controller.isGrounded) {  
        // We are grounded, so recalculate
        // move direction directly from axes
        moveDirection = new Vector3(movement, 0,
                                0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= Charspeed;
      
        if (jump) {
            moveDirection.y = jumpSpeed;
        }
    }
		else
		{
			    moveDirection.x =movement* Charspeed;
			
		}
		
		if(moveDirection.x>=0.1)
		{
			posMod=1;
			character.flipHorizontal=false;
			
		}
		else if(moveDirection.x<=-0.1)
		{
			
			character.flipHorizontal=true;
			posMod=-1;
		}
		
		

    // Apply gravity
    moveDirection.y -= gravity * Time.deltaTime;
    
    // Move the controller
    controller.Move(moveDirection * Time.deltaTime);
		
		
		
	}

	IEnumerator Bite (ControllerColliderHit hit)
	{
		if(canBite)
		{
			
			if (hit.gameObject.tag == "player")
			{
			
			canBite=false;
				
				Damageable npc = hit.gameObject.GetComponent<Damageable>();
				npc.Damage(25);
					
			    yield return new WaitForSeconds(0.5f);
				canBite=true;
			}
		}
	}
	
	//bite player on collision
		
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(Active)
		{
			StartCoroutine(Bite (hit));
		}
	}
	bool canBite=true;
	

	
	public  void Die()
	{
		if(Active)
		{
	
			bloodSplash.GetComponent<Renderer>().enabled=true;
			OTAnimatingSprite splash=bloodSplash.GetComponent<OTAnimatingSprite>();
			splash.Play("splash");
			this.GetComponent<Renderer>().enabled=false;
			Active=false;
			Destroy(this.gameObject,0.3f);
		}
	}


	
	

}
