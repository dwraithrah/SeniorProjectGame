using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//game object for instatiating clones of elf and goblin
	public GameObject TempElf;
	public GameObject TempGoblin;
	public GameObject GoblinWarrior;
	public bool des;
	public bool despa;
	public bool despair; 


	//the health of the 'wall' that player is trying to protect
	public int wallHealth = 10;
	//the player's health
	public int playerHealth = 5;

	//used to keep track of when to instatiate a goblin
	private float timer;
	//the number of seconds between instatiation of a goblin
	public float timeRate = 5f;

	//iterate will be used to keep track of how many goblins have spawned. Once it reaches a certain point, the difficulty level will go up
	private int iterate = 0;

	//difficulty level
	public int difficulty = 0;

	//boolean variables to decide when to spawn certain enemies
	private bool spawnKamikazeGoblin = false;
	private bool spawnGoblinWarrior = false;
	private bool moreWarrior = false; //boolean value to decide which to spawn more of, warrior or kamikaze goblin
	private int spawnM; 
	//enemy base speed
	public float baseSpeed = 1f;

	//mod value used to determine when a particular spawn object spawns
	private int modValue = 0;

	// Use this for initialization
	void Start () {
		//instantiate the player
		Instantiate(TempElf);
		//start timer
		timer = Time.time + timeRate;
	}



	// Update is called once per frame	
	void Update () {
		print (difficulty);
		//if escape key is pressed, end game
		if (Input.GetKey ("escape"))
			Application.Quit ();
		
		//checkDifficulty ();
		difficultyMode(); 
		//if 5 second passes, create new goblin
		if (timer < Time.time) {

			for (int i = 0; i < spawnM; i++) {
			//create random degree
				float j = i; 
			float degree = RandomDegree();

			//determine position
			Vector3 position = DetermineSpawnPosition(0, 10, degree);

			//determine rotation
			float degreeRotation = DetermineRotation(degree);
				//baseSpeed= baseSpeed -j/5;
				//print (baseSpeed);
			//if statements to determine which goblin spawns

				print ("yup"); 
				if (!spawnGoblinWarrior) { //if goblin warrior is not set to spawn, simply spawn a temp goblin
					//create instance of kamikaze goblin
					Instantiate (TempGoblin, new Vector3 (position.x, position.y, position.z), Quaternion.Euler (0, degreeRotation, 0));
				} else if ((spawnGoblinWarrior) && (!moreWarrior)) {//if goblin warrior is set to spawn, but not more than kamikaze goblin
					if (iterate % modValue == 0) {//if the modValue is 0, then spawn goblin warrior
						Instantiate (GoblinWarrior, new Vector3 (position.x, position.y, position.z), Quaternion.Euler (0, degreeRotation, 0));
					} else {//else spawn kamikaze goblin
						Instantiate (TempGoblin, new Vector3 (position.x, position.y, position.z), Quaternion.Euler (0, degreeRotation, 0));
					}
				} else if ((spawnGoblinWarrior) && (moreWarrior)) {//if goblin warriors are spawning and we want more of them than kamikaze goblins
					if (iterate % modValue == 0) {//if the modValue is 0, then spawn kamikaze goblin
						Instantiate (TempGoblin, new Vector3 (position.x, position.y, position.z), Quaternion.Euler (0, degreeRotation, 0));
					} else {//else spawn goblin warrior
						Instantiate (GoblinWarrior, new Vector3 (position.x, position.y, position.z), Quaternion.Euler (0, degreeRotation, 0));
					}
				}

			}
			//reset the timer 
			timer = Time.time + timeRate;

			iterate += 1;
		}

		//reset the timer 

		//once ten enemies have spawned, difficulty will go up
		if (iterate == 10) {
			//if difficulty is not already at 10, difficulty goes up
			if (difficulty < 10) {
				difficulty += 1;
			}
			//else difficulty stays at 10
			else
				difficulty = 10;

			//iterate always resets to 0
			iterate = 0;
		}
	}

	//this function will determine at which degree point the goblin will spawn at
	float RandomDegree (){
		float degree = Random.Range(60, -60);

		//ensure that depending on value, degree spawn points will be set
		if ((degree <= 60) && (degree > 45))
			degree = 60;
		else if ((degree <= 45) && (degree > 30))
			degree = 45;
		else if ((degree <= 30) && (degree > 15))
			degree = 30;
		else if ((degree <= 15) && (degree > 0))
			degree = 15;
		else if ((degree <= 0) && (degree > -15))
			degree = 0;
		else if ((degree <= -15) && (degree > -30))
			degree = -15;
		else if ((degree <= -30) && (degree > -45))
			degree = -30;
		else if ((degree <= -45) && (degree > -60))
			degree = -45;
		else
			degree = -60;

		return degree;
	}

	//this value will take the determined random angle and convert it into x,y,z coordinates and store it in a vector object
	Vector3 DetermineSpawnPosition(float center, float radius, float degree){
		Vector3 position;
		position.x = center + radius * Mathf.Sin(degree * Mathf.Deg2Rad);
		position.y = .5f;
		position.z = center + radius * Mathf.Cos(degree * Mathf.Deg2Rad);
		return position;
	}

	//this value will determine the amount to rotate the facing of the spawned goblin based on where it spawns
	float DetermineRotation(float degree){
		if ((degree <= 60) && (degree > 45))
			degree = 180 + 60;
		else if ((degree <= 45) && (degree > 30))
			degree = 180 + 45;
		else if ((degree <= 30) && (degree > 15))
			degree = 180 + 30;
		else if ((degree <= 15) && (degree > 0))
			degree = 180 + 15;
		else if ((degree <= 0) && (degree > -15))
			degree = 180;
		else if ((degree <= -15) && (degree > -30))
			degree = 180 - 15;
		else if ((degree <= -30) && (degree > -45))
			degree = 180 - 30;
		else if ((degree <= -45) && (degree > -60))
			degree = 180 - 45;
		else
			degree = 180 - 60;

		return degree;
	}

	void checkDifficulty(){
		//the following if statements will decide certain difficulty parameters
		if (difficulty == 0) {  //easiest difficulty, slow kamikaze goblin, no goblin warriors
			baseSpeed = 1;      //speed set to lowest
			spawnKamikazeGoblin = true; //kamikaze goblin will spawn
			spawnGoblinWarrior = false;  //warrior goblin will not spawn
			moreWarrior = false;
		} else if (difficulty == 1) { //faster kamikaze goblin, still no goblin warrior
			baseSpeed = 2;  //up the speed a little
			spawnKamikazeGoblin = true;  //kamikaze goblin will spawn
			spawnGoblinWarrior = false;  //goblin warrior will not spawn
			moreWarrior = false;

		} else if (difficulty == 2) { //much faster kamikaze goblin, still no goblin warrior
			baseSpeed = 4;  
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = false;
			moreWarrior = false;
		} else if (difficulty == 3) {// kamikaze goblin slows down, warrior goblin added, will spawn a couple of times
			baseSpeed = 1;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = false;
		} else if (difficulty == 4) {//faster kamikaze, faster goblin warrior, will spawn goblin warrior 3 times
			baseSpeed = 2;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = false;
			modValue = 3; //as iterate goes up, mod 3 will == 0 at 3, 6, 9, so 3 warrior spawns
		} else if (difficulty == 5) {//fastest kamikaze/warrior speed, goblin warrior spawns 3 times
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = false;
			modValue = 3;
		} else if(difficulty == 6) {//more even kamikaze/warrior spawn, faster spawn rate
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = false;
			timeRate = 4; //faster spawn rate
			modValue = 2; //as iterate goes up mod 2 == 0 occurs at 2, 4, 6, 8, 10, so 5 spawn times
		} else if(difficulty == 7){//kamikaze only spawns 3 times
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			modValue = 3;
		} else if(difficulty == 8){//kamikaze only spawns 3 times, faster spawn rate
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			timeRate = 3;
			modValue = 3;
		} else if(difficulty == 9){//kamikaze only spawns 2 times
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			modValue = 5;
		} else if(difficulty == 10){//kamikaze only spawns one time, fastest spawn rate
			baseSpeed = 4;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			timeRate = 2;
			modValue = 9;
		}

	}

	void easyDifficulty(){
	
		//the following if statements will decide certain difficulty parameters
		 if (difficulty == 0) { //faster kamikaze goblin, still no goblin warrior
			baseSpeed = 1;  //up the speed a little
			spawnKamikazeGoblin = true;  //kamikaze goblin will spawn
			spawnGoblinWarrior = false;  //goblin warrior will not spawn
			moreWarrior = false;
			timeRate = 3; 
		} else if (difficulty == 1) { //much faster kamikaze goblin, still no goblin warrior
			baseSpeed = 1.7f;  
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = false;
			moreWarrior = false;
			timeRate = 2.5f; 
		}  else if (difficulty == 2) {//faster kamikaze/warrior speed, goblin warrior is introduced
			baseSpeed = 2;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = false;
			modValue = 1;
			timeRate = 2.3f; 
		} else if(difficulty == 3){//faster kamikaze/warrior speed, two warrior, faster spawn
			baseSpeed = 2.7f;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			modValue = 2;
			timeRate = 2.2f; 
		} else if(difficulty == 4){//kamikaze only spawns 2 times
			baseSpeed = 3;
			spawnKamikazeGoblin = true;
			spawnGoblinWarrior = true;
			moreWarrior = true;
			timeRate = 2f;
			modValue = 2;
		} 
	}

	void hardDifficulty(){
		if (difficulty == 0) {  //easiest difficulty, slow kamikaze goblin, no goblin warriors
			baseSpeed = 1;     
			spawnKamikazeGoblin = true; 
			spawnGoblinWarrior = false;  
			moreWarrior = false;
			spawnM = 1; 
			timeRate = 2; 
		}

		if (difficulty == 1) {  
			baseSpeed = 2;     
			spawnKamikazeGoblin = true; 
			spawnGoblinWarrior = true;  
			moreWarrior = false;
			spawnM = 1; 
			timeRate = 2; 
			modValue = 1; 

		}

		if (difficulty == 2) {
			baseSpeed = 3;     
			spawnKamikazeGoblin = true; 
			spawnGoblinWarrior = true;  
			moreWarrior = false;
			spawnM = 1; 
			modValue = 1; 
			timeRate = 1.7f;
		}
		if (difficulty == 3) {
			baseSpeed = 3;      
			spawnKamikazeGoblin = true; 
			spawnGoblinWarrior = true;  
			moreWarrior = true;
			spawnM = 2; 
			modValue = 2; 
			timeRate = 1.4f;
		}
		if (difficulty == 4){
		baseSpeed = 4;      
		spawnKamikazeGoblin = true; 
		spawnGoblinWarrior = true;  
		moreWarrior = true;
		modValue = 3;
			timeRate = 1.4f; 
		spawnM = 2; }
	}

	void difficultyMode(){
		//easy
		if (des) {
			spawnM = 1;
			easyDifficulty(); 
		} 
		//End of Easy
		//hard
		else if (despa) {
			hardDifficulty ();
		} 
		//End of Hard
		//Dynamic
		else if (despair) {
		} //End of Dynamic

		//calls original checkDiffculty function if nothing is true 
		else {
			checkDifficulty ();
			spawnM = 1; 
		}
	
	}
	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds (time); 
	}

}
