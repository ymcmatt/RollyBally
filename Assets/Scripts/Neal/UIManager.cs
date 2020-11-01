using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text[] PlayerStatus;
    public GameObject[] PlayerImage;
    public Text Timer;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < PlayerStatus.Length; i++) {
            PlayerStatus[i].text = "Player " + (i+1) + ":\nNot Connected";
        }
        Timer.text = "";
    }

    public void UpdatePlayerStatus(int PlayerIndex, string status) {
        PlayerStatus[PlayerIndex - 1].text = "Player " + PlayerIndex + ":\n" + status;
        PlayerImage[PlayerIndex - 1].SetActive(true);
    }

    public void UpdateTimer(string time) {
        Timer.text = time;
    }
}
