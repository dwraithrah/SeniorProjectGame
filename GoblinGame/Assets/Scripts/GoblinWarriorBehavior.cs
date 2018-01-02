using UnityEngine;
using System.Collections;

public class GoblinWarriorBehavior : MonoBehaviour {
	public float speed = 10f;

	//invulnerability variable
	private bool invulnerable = false;

	//animation component
	private Animation animate;

	//sounds tagged to goblin warrior
	private AudioSource source;
	public AudioClip clubSwing;
	public AudioClip clubHit;
	public AudioClip running;



	// Use this for initialization
	void Start () {
		//get the current game controller base speed level
		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find ("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController> ();

		//goblin warroir speed will be set to base speed. He's vicious enough without having to go as fast as Kamikaze Goblin.
		speed = gameControllerScript.baseSpeed;

		source = GetComponent<AudioSource> ();
		animate = GetComponent<Animation> ();
		animate ["Run"].wrapMode = WrapMode.Loop;
		animate.Play ("Run");
	}

	// Update is called once per frame
	void Update () {
		//set the speed at which the goblin will move
		float translate = Time.deltaTime * speed;

		//set the target towards which the target will move. In this case the 'wall'
		Vector3 target = new Vector3 (0f, .5f, -2f);

		//Will move the target in a straight line towards its target.
		transform.position = Vector3.MoveTowards (transform.position, target, translate);

		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find ("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController> ();


		//destroys the goblin once it gets off-screen.
		if (transform.position.z < -1) {
			Destroy (this.gameObject);
			//reduce the wall health in game controller's script
			gameControllerScript.wallHealth -= 2;
		}
	}

	//function that will trigger if collision happens
	IEnumerator OnCollisionEnter(Collision c){


		//if the object struck is the player
		if (c.gameObject.tag == "Player") {

			if (!invulnerable) {
				//create object of game controller to access the wall health
				GameObject gameController = GameObject.Find ("GameController");
				//create reference to player script to access game controller's script
				GameController gameControllerScript = gameController.GetComponent<GameController> ();
				animate.Play ("Fighting");
				source.PlayOneShot(clubSwing);
				source.PlayOneShot (clubHit);
				animate.PlayQueued ("Run");

				//reduce player's health in game controller
				gameControllerScript.playerHealth = gameControllerScript.playerHealth - 2;

				//turn on invulnerability 
				invulnerable = true;
				yield return new WaitForSeconds (3);
				invulnerable = false;

			}


		}


	}
}
