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
    public const int ButtonAudio = 0, ThemeAudio = 2 , EndGame = 1;
 
    public void PlayAudio(int id)
    {
        audioSource.PlayOneShot(audio[id]);
    }
    public void PlayAudio(int id, float vol)
    {
        audioSource.PlayOneShot(audio[id], vol);
    }

    public void PlayThemeAudio()
    {
        bool status = GetComponent<AudioSource>().isPlaying;
        if (status == false)
        {
            GetComponent<AudioSource>().clip =(audio[ThemeAudio]);
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
       
    }
}
