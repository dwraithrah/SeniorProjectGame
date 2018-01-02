using UnityEngine;
using System.Collections;

public class SwordBehavior : MonoBehaviour {

	public bool attack = false;
	private double rotateSpeed = 5;

	//keeping score
	public int scoreValue = 0;

	//for goblin death scream
	private AudioSource source;
	public AudioClip deathGrunt;

	public AudioClip clubHit;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		attack = false;
	}

	void FixedUpdate(){
		if (Input.GetKey (KeyCode.Space)) {
			attack = true;

		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("The player has hit");
		if(other.gameObject.CompareTag("Enemy") && (attack == true)){
			source.PlayOneShot (clubHit, 1.0f);
			Debug.Log("Hit");
			source.PlayOneShot (deathGrunt, 1.0f);
			Destroy (other.gameObject);
			ScoreManager.score += scoreValue = 10;
		}
	}


}

