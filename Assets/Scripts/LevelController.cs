using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    // 0 : Player1 Win; 1: Player2 Win
    public static int isWin = -1;
    private bool hasCollide;
    public Text Player1text;
    public Text Player2text;

    // Start is called before the first frame update
    void Start()
    {
        hasCollide = PlayerCollide.hasCollide;
    }

    // Update is called once per frame
    void Update()
    {
        isWin = PlayerCollide.isWin;
        Debug.Log(isWin);
        // Debug.Log(isWin);
        if (hasCollide)
        {
            Debug.Log("collide");
        }
         // Debug.Log(isWin);
        if (player1.transform.position.y <= -3)
        {
            isWin = 1;
        }
        if (player2.transform.position.y <= -3)
        {
            isWin = 0;
        }
        if (isWin == 1)
        {
            Player1text.text = "You Lose :(";
            Player2text.text = "You Win !!";
            StartCoroutine(reset());
        }
        if (isWin == 0)
        {
            Player2text.text = "You Lose :(";
            Player1text.text = "You Win !!";
            StartCoroutine(reset());
        }
    }

    private IEnumerator reset()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameLogicBase");
    }
}
