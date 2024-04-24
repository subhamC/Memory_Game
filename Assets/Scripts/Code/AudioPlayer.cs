using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to play audio
public class AudioPlayer : GenericSingletonClass<AudioPlayer>
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audio;

    private static float vol = 1;

 
    public void PlayAudio(int id)
    {
        audioSource.PlayOneShot(audio[id]);
    }
    public void PlayAudio(int id, float vol)
    {
        audioSource.PlayOneShot(audio[id], vol);
    }


}
