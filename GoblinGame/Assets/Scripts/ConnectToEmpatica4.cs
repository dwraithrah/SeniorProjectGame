using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;



public class ConnectToEmpatica4 : MonoBehaviour {
	//the name of the connection, not required but better for overview if you have more than 1 connections running
	public string conName = "Localhost";

	//ip/address of the server, 127.0.0.1 is for your own computer
	public string conHost = "127.0.0.1";

	//port for the server, make sure to unblock this in your router firewall if you want to allow external connections
	public int conPort = 28000;

	//a true/false variable for connection status
	public bool socketReady = false;

	//needed to create connection to server
	public TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	Stopwatch myStopWatch;

	//for sending and receiving messages
	private string serverMsg;	
	public string msgToServer;

	//series of boolean variables used for flags to automate connection and retrieval of data
	//flag to indicate device conection status
	private bool deviceConnected = false;

	//flag to indicate good connection to server
	private bool goodConnection = false;

	//flag to indicate device id has been obtained
	private bool deviceID = false;

	//flag to indicate if data to be logged to file
	private bool logToFile = false;

	//message received boolean
	private bool messageReceived = false;
	//message sent boolean
	private bool messageSent = false;
	//subscription on boolean
	private bool subscriptionOn = false;
	//data received boolean
	private bool dataReceived = false;
	//to see if connection event happened
	private bool connectionEventHappened = false;
	//change the final subscription messge
	private bool wasSubscribed = false;
	//in case data was received instead of subscription message
	private bool eDataReceived = false;

	//other booleans needed
	//galvanicSkinResponseMessageSent set to true. It will be first message sent
	private bool galvanicSkinResponseMessageSent = false;
	private bool bloodVolumePulseMessageSent = false;
	private bool interBeatIntervalMessageSent = false;
	private bool skinTemperatureMessageSent = false;

	//public values that will receive the data from the empatica
	private string galvanicSkinResponse;
	private string bloodVolumePulse;
	private string interBeatInterval;
	private string skinTemperatureMessage;

	//float values
	public float fGSR;
	public float fBVP;
	public float fIBI;
	public float fSTM;

	//this variable will hold the device ID
	private string deviceIDent;

	//ensure this instance persists if game over
	public static ConnectToEmpatica4 Instance2;

	//ensures this instance remains upon game reload but any others are destroyed
	void Awake(){
		if (Instance2 == null) {
			DontDestroyOnLoad (gameObject);
			Instance2 = this;
		} else if (Instance2 != this) {
			Destroy (gameObject);
		}
	}


	// Use this for initialization
	void Start () {
		//set up the connection
		SetupSocket();
	}

	//try to initiate connection
	public void SetupSocket() {
		try {
			mySocket = new TcpClient(conHost, conPort);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
			mySocket.NoDelay = true;
			myStopWatch = Stopwatch.StartNew();
			UnityEngine.Debug.Log("Connection Successful");	
		}
		catch (Exception e) {
			UnityEngine.Debug.Log("Socket error:" + e);
		}
	}

	//send message to server
	public void WriteSocket(string theLine) {
		if (!socketReady)
			return;
		String tmpString = theLine + "\r\n";
		theWriter.Write(tmpString);
		theWriter.Flush();
		UnityEngine.Debug.Log ("Sent Message");
		messageSent = true;
	}

	//read message from server
	public string ReadSocket() {
		String result = "";
		if (!socketReady)
			return result;
		if (theStream.DataAvailable) {
			Byte[] inStream = new Byte[mySocket.SendBufferSize];
			theStream.Read(inStream, 0, inStream.Length);
			result += System.Text.Encoding.UTF8.GetString(inStream).TrimEnd('\0');

		}
		return result;
	}

	//disconnect from the socket
	public void CloseSocket() {
		if (!socketReady)
			return;
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}

	//keep connection alive, reconnect if connection lost
	public void MaintainConnection(){
		if(!theStream.CanRead) {
			SetupSocket();
		}
	}

	//socket reading script	
	string SocketResponse() {		
		string serverSays = ReadSocket();		
		if (serverSays != "") {			
			UnityEngine.Debug.Log("[SERVER]" + serverSays);
			messageReceived = true;
		}
		return serverSays;
	}

	//send message to the server	
	public void SendToServer(string str) {		
		WriteSocket(str);		
		UnityEngine.Debug.Log ("[CLIENT] -> " + str);		
	}

	//will check to see if we have a good connection to server and will try to reconnect if not
	public void ConnectToServerCheck(){
		string message = "";
		//if message has not been sent
		if (messageSent == false) {
			//call the SendSocketMessage function, sending the message to check
			UnityEngine.Debug.Log ("Sending message to check for good connection");
			WriteSocket ("server_status");
		}
		//now that the message has been sent, will wait to receive a message
		if (messageReceived == false) {
			message = SocketResponse ();
		}

		//once message received is true, will check the message
		if (messageReceived == true) {
			//split up the message
			String[] tempString = message.Split(' ');

			tempString [2] = "OK";

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2];

			UnityEngine.Debug.Log ("Reconstructed message is : " + message);

			//checks to see if the message is what we need it to be
			if (message.Equals ("R system_status OK", StringComparison.Ordinal)) {
				//if server status is okay, set goodConnection to true
				goodConnection = true;
				deviceID = true;
				deviceIDent = "90BFA7";
				//debug message saying we have a good connection
				UnityEngine.Debug.Log ("We are connected!");
				messageReceived = false;
				messageSent = false;
				connectionEventHappened = false;
			} else {
				//set booleans to resend connection
				messageReceived = false;
				messageSent = false;
			}
		}
	}//end ConnectToServerCheck

	public void DeviceConnectedCheck(){
		string message = "";
		//call send socket message to send message to server to connect to device
		if(messageSent == false){
			UnityEngine.Debug.Log ("Sending message to connect to device");
			WriteSocket("device_connect " + deviceIDent);
		}

		//if message received is false, keep listening
		if (messageReceived == false){
			message = SocketResponse();
		}

		if (messageReceived == true) {
			//split up the message
			String[] tempString = message.Split(' ');

			tempString [2] = "OK";

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2];

			UnityEngine.Debug.Log ("Reconstructed message is : " + message);

			//check to see if message received is a good connection
			if (message.Equals ("R device_connect OK", StringComparison.Ordinal)) {
				//let us know device connection is good
				UnityEngine.Debug.Log ("We are connected to " + deviceIDent);
				//set device connected to true
				deviceConnected = true;
				messageReceived = false;
				messageSent = false;
			}
			//otherwise try to send socket message again
			else {
				messageReceived = false;
				messageSent = false;
			}
		}
	}//end DeviceConnectedCheck

	//this function will subscribe to galvanic skin response stream. get the data.  store the data in a file. Then unsubscribe
	public void GalvanicSkinResponse(){
		string message = "";
		string finalMessage = "";
		if (wasSubscribed == false)
			finalMessage = "OK";
		else
			finalMessage = "OFF";

		//first, send message to subscribe
		if(messageSent == false){
			UnityEngine.Debug.Log ("Sending message to subscribe to gsr");
			WriteSocket ("device_subscribe gsr ON");
		}//end send message

		//if message is not received, keep checking for message
		if (messageReceived == false) {
			message = SocketResponse ();
		}

		if(messageReceived == true && subscriptionOn == false){
			//split up the message
			String[] tempString = message.Split(' ');

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2] + " " + finalMessage;
			UnityEngine.Debug.Log ("Reconstructed Message is : " + message);

			//once subscription message is received, will check to make sure subscription is good
			if(message.Equals("R device_subscribe gsr OK", StringComparison.Ordinal)|| tempString[0].Equals("E4_Gsr", StringComparison.Ordinal)){
				if (tempString [0].Equals ("E4_Gsr", StringComparison.Ordinal)) {
					eDataReceived = true;
					UnityEngine.Debug.Log ("Converting GSR data to float");
					fGSR = SplitData (message);
				}
				subscriptionOn = true;
				messageReceived = false;
				wasSubscribed = true;
				UnityEngine.Debug.Log ("We are subscribed to gsr");
			}
			else if(message.Equals("R device_subscribe gsr OFF", StringComparison.Ordinal)){
				messageReceived = false;
				messageSent = false;
				galvanicSkinResponseMessageSent = true;
				wasSubscribed = false;
				eDataReceived = false;
			}
			else{
				//start the process over again
				messageReceived = false;
			}
		}
			
		//once the subscription is on, if we get a message, we will do all the storing things
		if (messageReceived == true && subscriptionOn == true) {
			UnityEngine.Debug.Log ("The message received is: " + message);
			if (eDataReceived == true) {
				//now unsubscribe
				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe gsr OFF");
				subscriptionOn = false;
				messageReceived = false;
				wasSubscribed = true;
			}
			else {
				wasSubscribed = true;

				UnityEngine.Debug.Log ("Converting GSR data to float");
				fGSR = SplitData (message);

				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe gsr OFF");

				subscriptionOn = false;
				messageReceived = false;
			}
		} 
	}//end GalvanicSkinResponse

	//this function will subscribe to interbeat interval stream. get the data.  store the data in a file. Then unsubscribe
	public void InterBeatInterval(){
		string message = "";
		string finalMessage = "";
		if (wasSubscribed == false)
			finalMessage = "OK";
		else
			finalMessage = "OFF";

		//first, send message to subscribe
		if(messageSent == false){
			UnityEngine.Debug.Log ("Sending message to subscribe to ibi");
			WriteSocket ("device_subscribe ibi ON");
		}//end send message

		//if message is not received, keep checking for message
		if (messageReceived == false) {
			message = SocketResponse ();
		}

		if(messageReceived == true && subscriptionOn == false){
			//split up the message
			String[] tempString = message.Split(' ');

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2] + " " + finalMessage;
			UnityEngine.Debug.Log ("Reconstructed Message is : " + message);

			//once subscription message is received, will check to make sure subscription is good
			if(message.Equals("R device_subscribe ibi OK", StringComparison.Ordinal)|| tempString[0].Equals("E4_Hr", StringComparison.Ordinal)){
				if (tempString [0].Equals ("E4_Ibi", StringComparison.Ordinal)) {
					eDataReceived = true;
					UnityEngine.Debug.Log ("Converting IBI data to float");
					fIBI = SplitData (message);
				}
				subscriptionOn = true;
				messageReceived = false;
				wasSubscribed = true;
				UnityEngine.Debug.Log ("We are subscribed to ibi");
			}
			else if(message.Equals("R device_subscribe ibi OFF", StringComparison.Ordinal)){
				messageReceived = false;
				messageSent = false;
				interBeatIntervalMessageSent = true;
				wasSubscribed = false;
				eDataReceived = false;
			}
			else{
				//start the process over again
				messageReceived = false;
			}
		}

		//once the subscription is on, if we get a message, we will do all the storing things
		if (messageReceived == true && subscriptionOn == true) {
			UnityEngine.Debug.Log ("The message received is: " + message);
			if (eDataReceived == true) {
				//now unsubscribe
				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe ibi OFF");
				subscriptionOn = false;
				messageReceived = false;
				wasSubscribed = true;
			}
			else {
				wasSubscribed = true;

				UnityEngine.Debug.Log ("Converting IBI data to float");
				fIBI = SplitData (message);

				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe ibi OFF");

				subscriptionOn = false;
				messageReceived = false;
			}
		} 
	}//end InterBeatInterval

	//this function will subscribe to skin temperature stream. get the data.  store the data in a file. Then unsubscribe
	public void SkinTemperature(){
		string message = "";
		string finalMessage = "";
		if (wasSubscribed == false)
			finalMessage = "OK";
		else
			finalMessage = "OFF";

		//first, send message to subscribe
		if(messageSent == false){
			UnityEngine.Debug.Log ("Sending message to subscribe to tmp");
			WriteSocket ("device_subscribe tmp ON");
		}//end send message

		//if message is not received, keep checking for message
		if (messageReceived == false) {
			message = SocketResponse ();
		}

		if(messageReceived == true && subscriptionOn == false){
			//split up the message
			String[] tempString = message.Split(' ');

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2] + " " + finalMessage;
			UnityEngine.Debug.Log ("Reconstructed Message is : " + message);

			//once subscription message is received, will check to make sure subscription is good
			if(message.Equals("R device_subscribe tmp OK", StringComparison.Ordinal)|| tempString[0].Equals("E4_Temperature", StringComparison.Ordinal)){
				if (tempString [0].Equals ("E4_Temperature", StringComparison.Ordinal)) {
					eDataReceived = true;
					UnityEngine.Debug.Log ("Converting TMP data to float");
					fSTM = SplitData (message);
				}
				subscriptionOn = true;
				messageReceived = false;
				wasSubscribed = true;
				UnityEngine.Debug.Log ("We are subscribed to tmp");
			}
			else if(message.Equals("R device_subscribe tmp OFF", StringComparison.Ordinal)){
				messageReceived = false;
				messageSent = false;
				skinTemperatureMessageSent = true;
				wasSubscribed = false;
				eDataReceived = false;
			}
			else{
				//start the process over again
				messageReceived = false;
			}
		}

		//once the subscription is on, if we get a message, we will do all the storing things
		if (messageReceived == true && subscriptionOn == true) {
			UnityEngine.Debug.Log ("The message received is: " + message);
			if (eDataReceived == true) {
				//now unsubscribe
				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe tmp OFF");
				subscriptionOn = false;
				messageReceived = false;
				wasSubscribed = true;
			}
			else {
				wasSubscribed = true;

				UnityEngine.Debug.Log ("Converting TMP data to float");
				fSTM = SplitData (message);

				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe tmp OFF");

				subscriptionOn = false;
				messageReceived = false;
			}
		} 
	}//end SkinTemperature

	//this function will subscribe to blood volume pulse stream. get the data.  store the data in a file. Then unsubscribe
	public void BloodVolumePulse(){
		string message = "";
		string finalMessage = "";
		if (wasSubscribed == false)
			finalMessage = "OK";
		else
			finalMessage = "OFF";

		//first, send message to subscribe
		if(messageSent == false){
			UnityEngine.Debug.Log ("Sending message to subscribe to bvp");
			WriteSocket ("device_subscribe bvp ON");
		}//end send message

		//if message is not received, keep checking for message
		if (messageReceived == false) {
			message = SocketResponse ();
		}

		if(messageReceived == true && subscriptionOn == false){
			//split up the message
			String[] tempString = message.Split(' ');

			//reconstruct it
			message = tempString [0] + " " + tempString [1] + " " + tempString [2] + " " + finalMessage;
			UnityEngine.Debug.Log ("Reconstructed Message is : " + message);

			//once subscription message is received, will check to make sure subscription is good
			if(message.Equals("R device_subscribe bvp OK", StringComparison.Ordinal)|| tempString[0].Equals("E4_Bvp", StringComparison.Ordinal)){
				if (tempString [0].Equals ("E4_Bvp", StringComparison.Ordinal)){
					eDataReceived = true;
					UnityEngine.Debug.Log ("Converting BVP data to float");
					fBVP = SplitData (message);
				}
				subscriptionOn = true;
				messageReceived = false;
				wasSubscribed = true;
				UnityEngine.Debug.Log ("We are subscribed to bvp");
			}
			else if(message.Equals("R device_subscribe bvp OFF", StringComparison.Ordinal)){
				messageReceived = false;
				messageSent = false;
				bloodVolumePulseMessageSent = true;
				wasSubscribed = false;
				eDataReceived = false;
			}
			else{
				//start the process over again
				messageReceived = false;
			}
		}

		//once the subscription is on, if we get a message, we will do all the storing things
		if (messageReceived == true && subscriptionOn == true) {
			UnityEngine.Debug.Log ("The message received is: " + message);
			if (eDataReceived == true) {
				//now unsubscribe
				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe bvp OFF");
				subscriptionOn = false;
				messageReceived = false;
				wasSubscribed = true;
			}
			else {
				wasSubscribed = true;

				UnityEngine.Debug.Log ("Converting BVP data to float");
				fBVP = SplitData (message);

				UnityEngine.Debug.Log ("Sending unsubscription message");
				WriteSocket ("device_subscribe bvp OFF");

				subscriptionOn = false;
				messageReceived = false;
			}
		} 
	}//end BloodVolumePulse
	
	// Update is called once per frame
	void Update () {
		//if we have not established that the connection is okay, do this.
		if (goodConnection == false) {
			ConnectToServerCheck ();
		}//end of connecttoservercheck if statement

		//once the connection is established, will now connect to device
		else if (goodConnection == true && deviceConnected == false) {
			DeviceConnectedCheck ();
		}//end of deviceconnected check else if statement

		//we are connected to the device, now to get data for galvanic skin response
		else if (goodConnection == true && deviceConnected == true && galvanicSkinResponseMessageSent == false) {
			GalvanicSkinResponse ();
		}//end of getting data for galvanic skin response

		//now getting data for inter beat interval
		else if (goodConnection == true && deviceConnected == true && galvanicSkinResponseMessageSent == true && interBeatIntervalMessageSent == false) {
			InterBeatInterval ();
		} //end getting data for interbeat interval

		//now getting data for skin temperature
		else if (goodConnection == true && deviceConnected == true && galvanicSkinResponseMessageSent == true && interBeatIntervalMessageSent == true && skinTemperatureMessageSent == false) {
			SkinTemperature ();
		}//end getting data for skin temperature

		//now getting data for pulse
		else if (goodConnection == true && deviceConnected == true && galvanicSkinResponseMessageSent == true && interBeatIntervalMessageSent == true && skinTemperatureMessageSent == true && bloodVolumePulseMessageSent == false) {
			BloodVolumePulse ();
		}//end getting data for pulse

		//now resetting all flags and starting the process over again
		else if (goodConnection == true && deviceConnected == true && galvanicSkinResponseMessageSent == true && interBeatIntervalMessageSent == true && skinTemperatureMessageSent == true && bloodVolumePulseMessageSent == true) {
			galvanicSkinResponseMessageSent = false;
			interBeatIntervalMessageSent = false;
			skinTemperatureMessageSent = false;
			bloodVolumePulseMessageSent = false;
			messageSent = false;
			messageReceived = false;
			subscriptionOn = false;
			UnityEngine.Debug.Log ("All streams are reset, restarting the pattern to get streams");
			UnityEngine.Debug.Log ("Current float values");
			UnityEngine.Debug.Log ("BVP: " + fBVP + " IBI: " + fIBI + " TMP: " + fSTM + " GSR: " + fGSR);
		}
	}

	//this message will take the received streams and convert from string to a float value
	public float ConvertToFloat(String toConvert){
		float someNum = float.Parse(toConvert);
		return someNum;
	}//end converttofloat

	public float SplitData(string dataToSplit){
		//split up the message
		String[] tempString = dataToSplit.Split(' ');
		String[] tempString2;

		//if data that slipped through was not float value
		if(tempString[0].Equals("R", StringComparison.Ordinal)){
			//set message received to false so we'll get another message
			messageReceived = false;
			eDataReceived = false;
			return 0.0f;
		}
		else{
			//reconstruct it
			dataToSplit = tempString [2];

			tempString2 = dataToSplit.Split ('E');



			dataToSplit = tempString2 [0];
		
			UnityEngine.Debug.Log ("Reconstructed Message is : " + dataToSplit);



			//call function to convert it to a float
			float dataValue = ConvertToFloat (dataToSplit);

			UnityEngine.Debug.Log ("Converted float response = " + dataValue);

			return dataValue;
		}

	}//end SplitData
}
