using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class TimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private DataSO data;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI top1Text;
    [SerializeField] private TextMeshProUGUI top2Text;
    [SerializeField] private TextMeshProUGUI top3Text;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject rankingPanel;

    private float time;
    private string minute, second;
    private int targetLevel;

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
    void Start()
    {
 
        targetLevel = 200 + data.currentLevel*200;
        targetText.text = "Boss level: " + targetLevel.ToString();
        time = 80;
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }
    void Update()
    {
        ShowTime();
        if(time < 0) 
        {
            time = 0;
            MinimapController.HideMinimap();
            inGameUI.SetActive(false);
            top1Text.text = RankingController.top1Text.text;
            top2Text.text = RankingController.top2Text.text;
            top3Text.text = RankingController.top3Text.text;
            playerRankText.text = RankingController.playerRankText.text;
            scoreText.text = "Your Score: " + HeadController.level;
            RankingController.TurnOffText();
            HeadController.TurnOffLevelText();
            data.coin += HeadController.level;
            Time.timeScale = 0f;
            if(HeadController.level < targetLevel)
            {
                losePanel.SetActive(true);
                rankingPanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(true);
                rankingPanel.SetActive(true);
                data.currentLevel += 1;
            }

        }
    }
    private void ShowTime()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        minute = ((time / 60) < 10) ? "0" + ((int)time / 60).ToString() : ((int)time / 60).ToString();
        second = ((time % 60) < 10) ? "0" + ((int)time % 60).ToString() : ((int)time % 60).ToString();
        timeText.text = minute + ":" + second;
    }
}
