using UnityEngine;
using System.Collections;

public class ElfBehavior : MonoBehaviour {

	//speed variable to decide how fast elf moves
	public int speed = 2;

	//health of the player
	public int health = 5;

	private AudioSource source;
	public AudioClip clubSwing;

	//animation variable
	private Animation anim;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animation> ();
	}

	void Update(){
		//create object of game controller to access the wall health
		GameObject gameController = GameObject.Find("GameController");
		//create reference to player script to access game controller's script
		GameController gameControllerScript = gameController.GetComponent<GameController>();
		//set health to the health set in game controller
		health = gameControllerScript.playerHealth;

		//destroy player game object if health = 0, then reset game
		if (health <= 0) {
			Destroy (this.gameObject);
			Application.LoadLevel (Application.loadedLevel);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 position; 
		float degree;
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			degree = transform.eulerAngles.y - speed;
			position = DeterminePosition (0f, 3f, degree);
			if (degree > 275 || degree < 90) {
				transform.rotation = Quaternion.Euler (0, degree, 0);
			}
			transform.position = position;

		}

		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			degree = transform.eulerAngles.y + speed;
			position = DeterminePosition (0f, 3f, degree); 
			if (degree > 275 || degree < 90) {
				transform.rotation = Quaternion.Euler (0, degree, 0);
			}

			transform.position = position;
		}

		//if space bar is hit
		if (Input.GetKeyDown (KeyCode.Space)&& (!anim.isPlaying)) {
			
			anim.Play ("Fighting"); 
			source.PlayOneShot (clubSwing);

		}


	}


	Vector3 DeterminePosition(float center, float radius, float degree){
		Vector3 position;
		if (degree> 275 || degree < 90) {
			position.x = center + radius * Mathf.Sin (degree * Mathf.Deg2Rad);
			position.y = .5f;
			position.z = center + radius * Mathf.Cos (degree * Mathf.Deg2Rad);
			return position;
		} else {
			return transform.position;
		}
	}

}