using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class level : MonoBehaviour
{
    public PlayableDirector timeline;
    public MessageHandler MH;

    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
        timeline.played += Director_Played;
        timeline.stopped += Director_Stopped;
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        MH.CutSceneEnd();
    }

    private void Director_Played(PlayableDirector obj)
    {
       
    }

    public void StartTimeline()
    {
        timeline.Play();
    }
}
