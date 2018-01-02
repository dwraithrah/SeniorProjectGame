using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

	public int currentHP = 5;
	private int maxHealth = 5;
	Slider healthBar;

	//audio source
	private AudioSource source;

	//audio clip for scream
	public AudioClip jessScream;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("PlayerHealthBar");
		healthBar = temp.GetComponent<Slider> ();

		//initialize audio
		source = GetComponent<AudioSource>();
	}


	// Update is called once per frame
	void Update () {

		if (currentHP < maxHealth) {
			source.PlayOneShot (jessScream);
			maxHealth = currentHP;
		}
		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController>();
		currentHP = gameControllerScript.playerHealth;
		healthBar.value = currentHP;
	}
}
