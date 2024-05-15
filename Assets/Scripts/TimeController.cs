using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;
public class TimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private DataSO data;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI top1Name;
    [SerializeField] private TextMeshProUGUI top1Score;
    [SerializeField] private TextMeshProUGUI top2Name;
    [SerializeField] private TextMeshProUGUI top2Score;
    [SerializeField] private TextMeshProUGUI top3Name;
    [SerializeField] private TextMeshProUGUI top3Score;
    //[SerializeField] private TextMeshProUGUI top4Name;
    //[SerializeField] private TextMeshProUGUI top4Score;
    //[SerializeField] private TextMeshProUGUI top5Name;
    //[SerializeField] private TextMeshProUGUI top5Score;
    [SerializeField] private TextMeshProUGUI playerRank;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject arrow;

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
            top1Name.text = RankingController.top1name.text;
            top1Score.text = RankingController.top1score.text;
            top2Name.text = RankingController.top2name.text;
            top2Score.text = RankingController.top2score.text;
            top3Name.text = RankingController.top3name.text;
            top3Score.text = RankingController.top3score.text;
            playerName.text = RankingController.playerName.text;
            playerRank.text = RankingController.playerRank.text;
            playerScore.text = RankingController.playerScore.text;
            scoreText.text = HeadController.level.ToString();
            RankingController.TurnOffText();
            HeadController.TurnOffLevelText();
            arrow.SetActive(false);
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
