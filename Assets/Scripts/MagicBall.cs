using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBall : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Text player1text;
    public Text player2text;
    private int count = 0;
    private GameObject magicPotion;
    public GameObject _plane;

    // Start is called before the first frame update
    void Start()
    {
        player1text.text = "I am " + player1.tag;
        player2text.text = "I am " + player2.tag;
        magicPotion = this.gameObject;
        Mesh planeMesh = _plane.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        // size in pixels
        Debug.Log(bounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("change");
        if (other.gameObject.tag == "Prey" && other.gameObject.name == "Player1") 
        {
            // Debug.Log("!!!");
            player1.tag = "Predator";
            player2.tag = "Prey";
            player1text.text = "I am " + player1.tag;
            player2text.text = "I am " + player2.tag;
            count++;
            StartCoroutine(RespwanBall());
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Prey" && other.gameObject.name == "Player2")
        {
            // Debug.Log("???");
            player2.gameObject.tag = "Predator";
            player1.tag = "Prey";
            player1text.text = "I am " + player1.tag;
            player2text.text = "I am " + player2.tag;
            count++;
            StartCoroutine(RespwanBall());
            Destroy(gameObject);
        }
        //if (count == 1)
        //{
        //    Destroy(gameObject);
        //}
    }

    IEnumerator RespwanBall()
    {
        // Destroy(gameObject);
        var position = new Vector3(Random.Range(-5, 5), 0.82f, Random.Range(-5, 5));
        Instantiate(magicPotion, position, Quaternion.identity);
        yield return null;
    }

}
