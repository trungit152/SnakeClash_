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
        SceneManager.LoadScene("SkinScene");
    }

    private void PlayClick()
    {
        SceneManager.LoadScene("InGameScene");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
