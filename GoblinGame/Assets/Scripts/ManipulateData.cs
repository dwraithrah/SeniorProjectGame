using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System; 
using System.IO;
using System.Diagnostics;

public class ManipulateData : MonoBehaviour {

	//public variables for determined baseline
	public float baseICI;
	public float baseBVP;
	public float baseTMP;
	public float baseGSR;

	//private variables to manipulate the data 
	private float iciSum;
	private float bvpSum;
	private float tmpSum;
	private float gsrSum;

	//iterator 
	private int iterate;

	//way to save file name
	private string savefilename = "name" + DateTime.UtcNow.ToString("dd_mm_yyyy_hh_mm_ss") + ".txt";

	//variable for timer for how often to check for messages
	//set to check for updates from empatica every five seconds
	private float timeRate = 20f;
	private float timer;

	//ensure this instance persists if game over
	public static ManipulateData Instance;

	//ensures this instance remains upon game reload but any others are destroyed
	void Awake(){
		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		//start timer
		timer = Time.time + timeRate;

		//set iterate to 0
		iterate = 1;

		//set the sums to 0 
		iciSum = 0;
		bvpSum = 0;
		tmpSum = 0;
		gsrSum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//if 20 seconds has passed
		if(Time.time >= timer){
			GameObject mainCamera = GameObject.Find("OverHeadCamera");
			ConnectToEmpatica4 cameraScript = mainCamera.GetComponent<ConnectToEmpatica4>();

			//get data from empatica
			float ici = cameraScript.fIBI;
			float bvp = cameraScript.fBVP;
			float tmp = cameraScript.fSTM;
			float gsr = cameraScript.fGSR;

			//add the sum values up
			iciSum += ici;
			bvpSum += bvp;
			tmpSum += tmp;
			gsrSum += gsr;

			//get the average
			baseICI = iciSum/iterate;
			baseBVP = bvpSum/iterate;
			baseTMP = tmpSum/iterate;
			baseGSR = gsrSum/iterate;

			//pump out data to a file
			using(StreamWriter sw = File.AppendText(savefilename)){
				sw.WriteLine (ici + "    " + bvp + "    " + tmp + "    " + gsr);
				sw.WriteLine(baseICI + "    " + baseBVP + "    " + baseTMP + "    " + baseGSR);
				sw.WriteLine (" ");
			
			}

			//reset the timer 
			timer = Time.time + timeRate;

			//up iterate
			iterate ++;
		}
	}
}
