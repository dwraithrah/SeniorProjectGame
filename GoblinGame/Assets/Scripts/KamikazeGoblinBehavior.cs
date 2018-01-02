using UnityEngine;
using System.Collections;

public class KamikazeGoblinBehavior : MonoBehaviour {

	public float speed = 10f;

	//invulnerability variable
	private bool invulnerable = false;



	// Use this for initialization
	void Start () {
		//speed will be based off of base speed. kamikaze goblin runs faster so will always be base speed +1

		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find ("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController> ();

		//set the speed for this instance of goblin
		speed = gameControllerScript.baseSpeed + 1;

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
		if (transform.position.z <= -1) {
				Destroy (this.gameObject);
				//reduce the wall health in game controller's script
				gameControllerScript.wallHealth -= 1;
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

				//reduce player's health in game controller
				gameControllerScript.playerHealth = gameControllerScript.playerHealth - 1;

				//turn on invulnerability 
				invulnerable = true;
				yield return new WaitForSeconds (3);
				invulnerable = false;

			}

	
		}


	}

}
