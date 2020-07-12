using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combataudioplayer : MonoBehaviour
{

    public AudioSource sourceA, sourceB;
    public List<AudioClip> clips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!sourceA.isPlaying)
        {
            sourceA.clip = clips[Random.Range(0, clips.Count)];
            sourceA.PlayDelayed(Random.Range(0, 0.1f));
        }
        if (!sourceB.isPlaying)
        {
            sourceB.clip = clips[Random.Range(0, clips.Count)];
            sourceB.PlayDelayed(Random.Range(0, 0.1f));
        }
    }
}
