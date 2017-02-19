using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDebug : MonoBehaviour {

    public Text debugText;
    public AudioSource myAudio;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        string newDebugText = "";

        //AudioSourceの再生位置
        newDebugText += "再生時間 " + Music.MusicalTime + "\n";
        newDebugText += "再生時間 " + Music.MusicalTimeBar + "\n";
        newDebugText += "TimeSample" + Music.TimeSamples + "\n";
        //BPM90
        newDebugText += Music.Just.ToString() + "\n";

        debugText.text = newDebugText;
	}
}
