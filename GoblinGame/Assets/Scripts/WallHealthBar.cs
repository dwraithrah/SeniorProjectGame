using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WallHealthBar : MonoBehaviour {

	private int maxHP = 10;
	public int currentHP = 10;
	Slider healthBar;

	//source component
	private AudioSource source;

	//audio clip for smashing door
	public AudioClip doorSmash;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("WallHealthBar");
		healthBar = temp.GetComponent<Slider> ();

		//initialize audiosource
		source = GetComponent<AudioSource>();
	}
		
	
	// Update is called once per frame
	void Update () {
		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController>();
		currentHP = gameControllerScript.wallHealth;
		healthBar.value = currentHP;

		//checks to see if hit points went down. if it did it plays door smash sound
		if (currentHP < maxHP) {
			maxHP -= 1; //lowers max hit points value to match current hit points

			source.PlayOneShot (doorSmash); //plays the smash door audio
		}

		//if health bar = 0, reload level
		if (currentHP == 0)
			Application.LoadLevel (Application.loadedLevel);
	}
}
