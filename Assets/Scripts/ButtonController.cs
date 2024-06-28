using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TextMeshProUGUI increseLevelText;
    [SerializeField] private TextMeshProUGUI increseSpeedText;
    [SerializeField] private TextMeshProUGUI increseItemText;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] DataSO data;
    [SerializeField] private Button btnRestart;

    [SerializeField] private Image magniteImg;
    [SerializeField] private Image magniteBg;
    [SerializeField] private TextMeshProUGUI magniteCdText;
    private float magniteCd;

    [SerializeField] private Image zoomOutImg;
    [SerializeField] private Image zoomOutBg;
    [SerializeField] private TextMeshProUGUI zoomOutCdText;
    private float zoomOutCd;

    [SerializeField] private Image growImg;
    [SerializeField] private Image growBg;
    [SerializeField] private TextMeshProUGUI growCdText;
    private float growCd;

    private bool isStarted = false;


    MinimapController minimapController;
    MinimapController MinimapController
    {
        get
        {
            if (minimapController == null)
            {
                minimapController = GameObject.Find("MinimapCamera").GetComponent<MinimapController>();
            }
            return minimapController;
        }
        set
        {
            minimapController = value;
        }
    }
    private HeadController headController;
    private HeadController HeadController
    {
        get
        {
            if (headController == null)
            {
                headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
            }
            return headController;
        }
        set
        {
            headController = value;
        }
    }
    private RankingController rankingController;
    private RankingController RankingController
    {
        get
        {
            if (rankingController == null)
            {
                rankingController = GameObject.Find("RankingController").gameObject.GetComponent<RankingController>();
            }
            return rankingController;
        }
        set
        {
            rankingController = value;
        }
    }

    private void Awake()
    {
        btnRestart.onClick.AddListener(PlayAgainClick);

    }
    private void Start()
    {
        increseLevelText.text = "Cost: " + data.increseLevelCost.ToString();
        increseSpeedText.text = "Cost: " + data.increseSpeedCost.ToString();
        increseItemText.text = "Cost: " + data.increseItemCost.ToString();
        coinText.text = data.coin.ToString();
    }
    private void Update()
    {
        SkillCdUpdate();
        if (!isStarted && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            isStarted = true;
            Time.timeScale = 1.0f;
            inGameUI.SetActive(true);
            HeadController.SetStat();
            startPanel.SetActive(false);
            MinimapController.ShowMinimap();
        }
    }

    private void SkillCdUpdate()
    {
        if (magniteCd > 0)
        {
            magniteBg.color = Color.gray;
            magniteImg.color = Color.gray;
            magniteCd -= Time.deltaTime;
            magniteCdText.text = Mathf.RoundToInt(magniteCd).ToString();
        }
        else if (magniteCd < 0)
        {
            magniteBg.color = Color.white;
            magniteImg.color = Color.white;
            magniteCd = 0;
            magniteCdText.text = null;
        }

        if (zoomOutCd > 0)
        {
            zoomOutBg.color = Color.gray;
            zoomOutImg.color = Color.gray;
            zoomOutCd -= Time.deltaTime;
            zoomOutCdText.text = Mathf.RoundToInt(zoomOutCd).ToString();
        }
        else if (zoomOutCd < 0)
        {
            zoomOutBg.color = Color.white;
            zoomOutImg.color = Color.white;
            zoomOutCd = 0;
            zoomOutCdText.text = null;
        }

        if (growCd > 0)
        {
            growBg.color = Color.gray;
            growImg.color = Color.gray;
            growCd -= Time.deltaTime;
            growCdText.text = Mathf.RoundToInt(growCd).ToString();
        }
        else if (growCd < 0)
        {
            growBg.color = Color.white;
            growImg.color = Color.white;
            growCd = 0;
            growCdText.text = null;
        }
    }
    
    public void GoToPlayClick()
    {
        SoundController.instance.PlaySFX(SoundController.instance.clickSFX);
        SceneManager.LoadScene("InGameScene");
    }
    public void PlayClick()
    {
        Time.timeScale = 1.0f;
        inGameUI.SetActive(true);
        HeadController.SetStat();
        startPanel.SetActive(false);
        MinimapController.ShowMinimap();
    }
    public void PlayAgainClick()
    {
        SoundController.instance.PlaySFX(SoundController.instance.clickSFX);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void IncreseSpeedClick()
    {
        if(data.coin >= data.increseSpeedCost)
        {
            data.startSpeed += 0.1f;
            data.increseSpeedCost += 1000;
            data.coin -= data.increseSpeedCost;
            increseSpeedText.text = "Cost: " + data.increseSpeedCost.ToString();
            coinText.text = data.coin.ToString();
        }
    }
    public void IncreseLevelClick()
    {
        if(data.coin >= data.increseLevelCost)
        {
            data.increseLevelCost += 2000;
            data.startLevel += 1;
            data.coin -= data.increseLevelCost;
            increseLevelText.text = "Cost: " + data.increseLevelCost.ToString();
            coinText.text = data.coin.ToString();
        }

    }
    public void IncreseItemClick()
    {
        if(data.coin >= data.increseItemCost)
        {
            data.increseItemCost += 1000;
            data.itemTime += 0.1f;
            data.coin -= data.increseItemCost;
            increseItemText.text = "Cost: " + data.increseItemCost.ToString();
            coinText.text = data.coin.ToString();
        }
    }
    public void HomeButtonClick()
    {
        SoundController.instance.PlaySFX(SoundController.instance.clickSFX);
        SceneManager.LoadScene("HomeScene");
    }
    public void IncreseRadius()
    {
        SoundController.instance.PlaySFX(SoundController.instance.magniteSFX);
        if (data.coin >= 1000 && magniteCd == 0)
        {
            SoundController.instance.PlaySFX(SoundController.instance.coinSFX);
            magniteCd = 5f;
            HeadController.Magnite();
            data.coin -= 1000;
            coinText.text = data.coin.ToString();
        }
    }

   
    public void Grow()
    {
        SoundController.instance.PlaySFX(SoundController.instance.levelUpSFX);
        if (data.coin >= 1000 && growCd == 0)
        {
            SoundController.instance.PlaySFX(SoundController.instance.coinSFX);
            growCd = 0.5f;
            for (int i = 0; i < 7; i++)
            {
                HeadController.LevelUp();
            }
            data.coin -= 1000;
            coinText.text = data.coin.ToString();
        }
    }

    public void ZoomOutBtn()
    {
        SoundController.instance.PlaySFX(SoundController.instance.zoomSFX);
        if (data.coin >= 1000 && zoomOutCd == 0)
        {
            SoundController.instance.PlaySFX(SoundController.instance.coinSFX);
            zoomOutCd = 6f;
            HeadController.ZoomOut();
            data.coin -= 1000;
            coinText.text = data.coin.ToString();
        }
    }
}
