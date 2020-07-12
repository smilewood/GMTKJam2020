using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomAudioClip : MonoBehaviour
{
    public AudioSource source;
    public List<AudioClip> clips;

    // Use this for initialization
    void Start()
    {
        if(source is null)
        {
            source = gameObject.GetComponent<AudioSource>();
        }
        source.loop = false;
    }

    public void Play()
    {
        if (clips.Count == 0) return;
        source.clip = clips[Random.Range(0, clips.Count)];
        source.Play();
    }
}
