using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{

    private float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("up"))
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("down"))
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("right"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("left"))
        {
            pos.x -= speed * Time.deltaTime;
        }


        transform.position = pos;
    }
}
