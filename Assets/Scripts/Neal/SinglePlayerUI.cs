using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerUI : MonoBehaviour
{
    public GameObject DeathScreen;
    public Text Instruction;
    public Text CountDown;
    public Image[] Heart;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    int health;

    const string GoPotion = "Look for the magic potion and become the hunter!";
    const string HuntDown = "You are the hunter now and Hunt others down!";
    const string RunAway = "Run away from the hunter and survive!";

    public void Reset() {
        foreach (Image h in Heart) {
            h.sprite = FullHeart;
        }
        DeathScreen.SetActive(false);
        health = 3;
        UpdateInstruction(1);
        UpdateCountDown("");
    }

    public void DecreaseHealth() {
        health--;
        Heart[health].sprite = EmptyHeart;
        if (health == 0) {
            DeathScreen.SetActive(true);
        }
    }

    public void UpdateInstruction(int status) {
        switch(status) {
            case 1:
                Instruction.text = GoPotion;
                break;
            case 2:
                Instruction.text = HuntDown;
                break;
            case 3:
                Instruction.text = RunAway;
                break;
            default:
                break;
        }
    }

    public void UpdateCountDown(string time) {
        CountDown.text = time;
    }
    
}
