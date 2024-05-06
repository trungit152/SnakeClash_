using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class TimeController : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private DataSO data;
    [SerializeField] private TextMeshProUGUI targetText;

    private float time;
    private string minute, second;
    private int targetLevel;

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
    void Start()
    {
 
        targetLevel = 200 + data.currentLevel*200;
        targetText.text = "Boss level: " + targetLevel.ToString();
        time = 90;
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }
    void Update()
    {
        ShowTime();
        if(time < 0) 
        {
            time = 0;
            Time.timeScale = 0f;
            if(HeadController.level < targetLevel)
            {
                losePanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(true);
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
