using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource BGM1;
    public AudioSource BGM2;
    public AudioSource BGM3;
    public AudioSource BGM4;

    int status = 0;

    // Start is called before the first frame update
    void Start()
    {
        Reset();    
    }

    // Update is called once per frame
    void Update()
    {
        if (status == 0) {
            if (!BGM1.isPlaying && !BGM2.isPlaying) {
                BGM2.Play();
            }
        } else if (status == 1){
            if (!BGM2.isPlaying && !BGM3.isPlaying) {
                BGM3.Play();
            }
        } 
    }

    public void Reset() {
        status = 0;
        if (BGM2.isPlaying) {
            BGM2.Stop();
        }
        if (BGM3.isPlaying) {
            BGM3.Stop();
        }
        if (BGM4.isPlaying) {
            BGM4.Stop();
        }
        BGM1.PlayDelayed(2f);
    }

    public void Climax() {
        status = 1;
    }

    public void Victory(){
        status = 2;
        if (BGM1.isPlaying) {
            BGM1.Stop();
        }
        if (BGM2.isPlaying) {
            BGM2.Stop();
        }
        if (BGM3.isPlaying) {
            BGM3.Stop();
        }
        BGM4.Play();
    }
}
