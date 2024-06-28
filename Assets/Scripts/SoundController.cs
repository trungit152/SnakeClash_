using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    [Header("-----------------Audio Source----------------")]
    [SerializeField] AudioSource SFXSource;

    [Header("-----------------Audio Clip----------------")]
    public AudioClip backMusic;
    public AudioClip clickSFX;
    public AudioClip coinSFX;
    public AudioClip countDownSFX;
    public AudioClip eatSFX;
    public AudioClip loseSFX;
    public AudioClip magniteSFX;
    public AudioClip levelUpSFX;
    public AudioClip winSFX;
    public AudioClip zoomSFX;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        //musicSource.loop = true;
    }
    void Start()
    {
        //musicSource.clip = backMusic;
        //musicSource.Play();
    }

    //public void PlayBackMusic()
    //{
    //    musicSource.clip = backMusic;
    //    musicSource.Play();
    //}
    //public void BackMusicVolume(float volume)
    //{
    //    musicSource.volume = volume;
    //}
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
