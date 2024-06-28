using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utilities.Common;
using static Cinemachine.DocumentationSortingAttribute;
public class TimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
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
    [SerializeField] private GameObject inGameRanking;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject top4Frame;
    [SerializeField] private TextMeshProUGUI timeOutText;

    private float time;
    private bool checkCount;
    private bool checkEnd;
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
        //BackgroundMusic.instance.BackMusicVolume(0.5f);
        checkCount = true;
        checkEnd = true;
        timeOutText.SetActive(false);
        targetLevel = 100 + data.currentLevel*100;
        targetText.text = "Boss level: " + targetLevel.ToString();
        time = 80;
    }
    private void PlayCountDownSFX()
    {
        if (checkCount)
        {
            SoundController.instance.PlaySFX(SoundController.instance.countDownSFX);
            checkCount = false;
        }
    }
    private void PlayEndGameSFX()
    {
        if (checkEnd)
        {
            time = 0;
            RankingController.Sort();
            RankingController.UpdateText();
            MinimapController.HideMinimap();
            inGameUI.SetActive(false);
            top1Name.text = RankingController.top1name.text;
            top1Score.text = RankingController.top1score.text;
            top1Name.color = RankingController.top1name.color;
            top1Score.color = RankingController.top1score.color;

            top2Name.text = RankingController.top2name.text;
            top2Score.text = RankingController.top2score.text;
            top2Name.color = RankingController.top2name.color;
            top2Score.color = RankingController.top2score.color;

            top3Name.text = RankingController.top3name.text;
            top3Score.text = RankingController.top3score.text;
            top3Name.color = RankingController.top3name.color;
            top3Score.color = RankingController.top3score.color;

            playerName.text = RankingController.playerName.text;
            playerRank.text = RankingController.playerRank.text;
            playerScore.text = RankingController.playerScore.text;
            playerName.color = RankingController.playerName.color;
            playerRank.color = RankingController.playerRank.color;
            playerScore.color = RankingController.playerScore.color;
            if (playerName.text == null)
            {
                top4Frame.SetActive(false);
            }

            scoreText.text = HeadController.level.ToString();
            RankingController.TurnOffText();
            HeadController.TurnOffLevelText();
            inGameRanking.SetActive(false);
            arrow.SetActive(false);
            data.coin += HeadController.level;
            Time.timeScale = 0f;
            if (HeadController.level < targetLevel)
            {
                SoundController.instance.PlaySFX(SoundController.instance.loseSFX);
                statusText.text = "FAILED";
                statusText.color = Color.red;
                rankingPanel.SetActive(true);
            }
            else
            {
                SoundController.instance.PlaySFX(SoundController.instance.winSFX);
                statusText.text = "VICTORY";
                statusText.color = Color.green;
                rankingPanel.SetActive(true);
                data.currentLevel += 1;
            }
            timeOutText.SetActive(false);
            checkEnd = false;
        }
    }
    void Update()
    {
        ShowTime();
        if(time < 5 && time > 0.001)
        {
            PlayCountDownSFX();
            timeOutText.text = timeText.text;
            timeOutText.SetActive(true);
        }
        if(time < 0) 
        {
            PlayEndGameSFX();
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
