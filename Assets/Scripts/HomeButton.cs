using NodeCanvas.Tasks.Actions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button skinBtn;

    private void Awake()
    {
        playBtn.onClick.AddListener(PlayClick);
        skinBtn.onClick.AddListener(SkinClick);
    }
    
    private void SkinClick()
    {
        SoundController.instance.PlaySFX(SoundController.instance.clickSFX);
        SceneManager.LoadScene("SkinScene");
    }

    private void PlayClick()
    {
        SoundController.instance.PlaySFX(SoundController.instance.clickSFX);
        SceneManager.LoadScene("InGameScene");
        Time.timeScale = 0f;
    }
    

    void Start()
    {
        BackgroundMusic.instance.BackMusicVolume(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
