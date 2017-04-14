using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip[] audio;

	// Use this for initialization
	
    public void play(int clipInArray)
    {
        gameObject.GetComponent<AudioSource>().clip = audio[clipInArray];
        gameObject.GetComponent<AudioSource>().Play();
    }
}
