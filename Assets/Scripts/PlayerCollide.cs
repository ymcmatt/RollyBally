using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCollide : MonoBehaviour
{
    // 0 : Player1 Win; 1: Player2 Win
    public static int isWin = -1;
    public static bool hasCollide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Predator")
        {
            isWin = 1;
            Debug.Log(isWin);
            //hasCollide = true;
            //Player1text.text = "You Lose :(";
            //Player2text.text = "You Win !!";
            //StartCoroutine(reset());
        }
        else if (collision.gameObject.tag == "Prey")
        {
            isWin = 0;
            Debug.Log(isWin);
            //hasCollide = true;
            //Player2text.text = "You Lose :(";
            //Player1text.text = "You Win !!";
            //StartCoroutine(reset());
        }
    }

    private IEnumerator reset()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameLogicBase");
    }
}
