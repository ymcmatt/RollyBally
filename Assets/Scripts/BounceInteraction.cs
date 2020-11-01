using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Prey" || other.gameObject.tag == "Predator")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.Impulse);
            // other.attachedRigidbody.AddForce(0f, 11f, 0f);
        }
    }
}
