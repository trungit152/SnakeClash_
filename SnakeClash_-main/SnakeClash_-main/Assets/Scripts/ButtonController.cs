using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TextMeshProUGUI increseLevelText;
    [SerializeField] private TextMeshProUGUI increseSpeedText;
    [SerializeField] private TextMeshProUGUI increseItemText;
    [SerializeField] private Text coinText;
    [SerializeField] DataSO data;

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
    private void Start()
    {
        increseLevelText.text = "Cost: " + data.increseLevelCost.ToString();
        increseSpeedText.text = "Cost: " + data.increseSpeedCost.ToString();
        increseItemText.text = "Cost: " + data.increseItemCost.ToString();
        coinText.text = "Coin: " + data.coin.ToString();
    }
    public void PlayClick()
    {
        Time.timeScale = 1.0f;
        HeadController.SetStat();
        startPanel.SetActive(false);
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
            coinText.text = "Coin: " + data.coin.ToString();
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
            coinText.text = "Coin: " + data.coin.ToString();
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
            coinText.text = "Coin: " + data.coin.ToString();
        }

    }
}
