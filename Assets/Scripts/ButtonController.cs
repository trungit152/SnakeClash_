using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

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
    public void GoToPlayClick()
    {
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
        SceneManager.LoadScene("HomeScene");
    }
    public void IncreseRadius()
    {
        HeadController.Magnite();
    }
    public void Grow()
    {
        HeadController.LevelUp();
    }
}
