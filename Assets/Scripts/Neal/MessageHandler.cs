using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.Playables;
using UnityEngine.Video;

public class MessageHandler : MonoBehaviour
{
    public Player[] PlayerObjects;

	public UIManager ui;
	public BGMManager BGM;
	public GameObject EndScene;
	public GameObject BattleScene;
	public level cutscene;
	public VideoPlayer video;

    int PlayerNum;

	// Status of the game
	// 0: titleScreen
	// 1: In Battle
	// 2: Game Over
	int Status;
	public int inGamePlayer;
	public bool hasHunter;

	public float ReadyDuration = 6f;
	float ReadyTimer;
	public AudioSource Ready;
	public AudioSource CountDown1;
	public AudioSource CountDown2;
	public AudioSource Death;

	public Dictionary<int, Player> players = new Dictionary<int, Player> (); 

	void Awake () {
		PlayerNum = 0;
		Reset();
		AirConsole.instance.onMessage += OnMessage;		
		AirConsole.instance.onReady += OnReady;		
		AirConsole.instance.onConnect += OnConnect;		
	}

	void OnReady(string code){
		//Since people might be coming to the game from the AirConsole store once the game is live, 
		//I have to check for already connected devices here and cannot rely only on the OnConnect event 
		List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
		foreach (int deviceID in connectedDevices) {
			AddNewPlayer (deviceID);
		}
	}

	void OnConnect (int device){
		AddNewPlayer (device);
	}

	private void AddNewPlayer(int deviceID){

		if (players.ContainsKey (deviceID)) {
			return;
		}

        if (PlayerNum < PlayerObjects.Length) {
            players.Add(deviceID, PlayerObjects[PlayerNum]);
            PlayerNum++;
			ui.UpdatePlayerStatus(PlayerNum, "Connected");
        }
	}

	void OnMessage (int from, JToken data){

		//When I get a message, I check if it's from any of the devices stored in my device Id dictionary
		if (players.ContainsKey (from) && data["action"] != null) {
			//TitleScreen
			if (Status == 0) {
				if (data ["action"].ToString () == "shake") {
					if (players[from].ToggleReady()) {
						Ready.Play();
						ui.UpdatePlayerStatus(players[from].playerIndex, "Ready");
					} else {
						ui.UpdatePlayerStatus(players[from].playerIndex, "Connected");
					}				
				}
			//In Battle
			} else if (Status == 1) {
				switch (data ["action"].ToString ()) {
				case "motion":
					if (data ["motion_data"] != null) {
						if (data ["motion_data"] ["x"].ToString() != "") {
							Vector2 abgAngles = new Vector3 ((float)data ["motion_data"] ["beta"], (float)data ["motion_data"] ["gamma"]);
							players[from].AngleInput (abgAngles);
						}
					}
					break;

				case "swipe":
					if ((float)data["vector"]["y"] < 0) {
						players[from].TurnLeft();
					} else {
						players[from].TurnRight();
					}
					break;

				default:
					break;
				}		
			}         
        }
	}

	void Update(){
		if (Status == 0) {
			bool AllReady = PlayerObjects[0].isReady && PlayerObjects[1].isReady && PlayerObjects[2].isReady;// && PlayerObjects[3].isReady;
			
			/*foreach(Player p in PlayerObjects) {
				if (!p.isReady) {
					AllReady = false;
				}
			}*/

			//temporary test line
			//AllReady = PlayerObjects[0].isReady;
			if (AllReady) {
				
				int preTime = (int)ReadyTimer;
				ReadyTimer -= Time.deltaTime;
				int afterTime = (int)ReadyTimer;
				if (preTime != afterTime) {
					if (afterTime > 0) {
						CountDown1.Play();
					} else {
						CountDown2.Play();
					}	
				}
				if (ReadyTimer <= 0) {
					//video.Play();
					Status = 1;
					ui.gameObject.SetActive(false);
					foreach(Player p in PlayerObjects) {
						p.gameObject.SetActive(true);
						p.Reset();
						p.inGame = true;
					}
					BGM.Reset(); 
				}
				ui.UpdateTimer("" + (int)ReadyTimer);
				
			} else {
				ReadyTimer = ReadyDuration;
				ui.UpdateTimer("");
			}
		// Battle Scene
		} else if (Status == 1) {
			if (inGamePlayer == 2) {
				BGM.Climax();
			}
			if (inGamePlayer == 1) {
				Status = 2;
				GameObject[] potions = GameObject.FindGameObjectsWithTag("potion");
				foreach (GameObject p in potions) {
					Destroy(p);
				}
				BGM.Victory();
				BattleScene.SetActive(false);
				EndScene.SetActive(true);
				cutscene.StartTimeline();
			}
		// End Scene
		} 
	}

	void OnDestroy () {
		if (AirConsole.instance != null) {
			AirConsole.instance.onMessage -= OnMessage;		
			AirConsole.instance.onReady -= OnReady;		
			AirConsole.instance.onConnect -= OnConnect;		
		}
	}

	void Reset() {
		BGM.Reset();
		BattleScene.SetActive(true);
		EndScene.SetActive(false);
		ReadyTimer = ReadyDuration;      
		Status = 0;
		inGamePlayer = 4;
		hasHunter = false;
		foreach(Player p in PlayerObjects) {
			p.inGame = false;
		}
		ui.gameObject.SetActive(true);
		ui.UpdateTimer("");
	}

	public void CutSceneEnd(){
		Status = 0;
		Reset();
	}
}
