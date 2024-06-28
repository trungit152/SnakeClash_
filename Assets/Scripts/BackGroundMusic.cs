using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance = null;
    public AudioSource backMusicAudio;
    public static BackgroundMusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void BackMusicVolume(float volume = 1f)
    {
        backMusicAudio.volume = volume;
    }
}