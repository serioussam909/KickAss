using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().enabled=false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider other) {
    
	if (other.gameObject.tag == "enemy"||other.gameObject.tag == "player")
		{
			Damageable npc = other.gameObject.GetComponent<Damageable>();
		npc.Damage(99999);
		}
			
		
	 
	}
}
