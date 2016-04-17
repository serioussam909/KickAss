using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	// Use this for initialization
	OTSound music;
	void Start () {
		music = new OTSound("BG");
		music.Idle();
		music.Volume(0.1f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
