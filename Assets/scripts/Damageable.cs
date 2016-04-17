using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

	public float health=100;
	public bool Active=true;
	public bool blinking = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public virtual void Damage(int damage)
	{
		
		health-=damage;
		if(!blinking)
		{
			StartCoroutine(Blink());
		}
	}
	
	public IEnumerator Blink()
	{
		blinking=true;
		OTSprite sprite= GetComponent<OTSprite>();
		
		for(int i=0;i<2;i++)
		{
			
		sprite.tintColor= new Color(150,0,0);
		 yield return new WaitForSeconds(0.1f);
		sprite.tintColor= new Color(255,255,255);

		 yield return new WaitForSeconds(0.1f);	
		}
		blinking=false;
		
	}
	
	
	public  virtual void Die()
	{
	}
}
