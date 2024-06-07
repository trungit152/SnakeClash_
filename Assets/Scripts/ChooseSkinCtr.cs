using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseSkinCtr : MonoBehaviour
{
    [SerializeField] private GameObject buyBtn;
    [SerializeField] private GameObject selectBtn;
    [SerializeField] private GameObject selectedBtn;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private DataSO data;
    private void Start()
    {
        CheckSelect();
        coinText.text = data.coin.ToString();
    }
    public void BuyBtn()
    {
        if (!data.skins[data.skinIndex].isBought)
        {
            data.coin -= data.skins[data.skinIndex].cost;
            coinText.text = data.coin.ToString();
            data.skins[data.skinIndex].isBought = true;
            buyBtn.SetActive(false);
            selectBtn.SetActive(true);
        }
    }
    public void SelectSkinBtn()
    {
        if (data.skins[data.skinIndex].isBought && !data.skins[data.skinIndex].isSelected)
        {
            data.chooseIndex = data.skinIndex;
            for (int i = 0; i < data.skins.Count; i++)
            {
                data.skins[i].isSelected = false;
            }
            selectBtn.SetActive(false);
            selectedBtn.SetActive(true);
            data.skins[data.skinIndex].isSelected = true;
        }
    }
    public void CheckSelect()
    {
        if (data.skins[data.skinIndex].isSelected && data.skins[data.skinIndex].isBought)
        {
            buyBtn.SetActive(false);
            selectBtn.SetActive(false);
            selectedBtn.SetActive(true);
        }
        else if (!data.skins[data.skinIndex].isBought)
        {
            buyBtn.SetActive(true);
            costText.text = data.skins[data.skinIndex].cost.ToString();
            selectBtn.SetActive(false);
            selectedBtn.SetActive(false);
        }
        else
        {
            buyBtn.SetActive(false);
            selectBtn.SetActive(true);
            selectedBtn.SetActive(false);
        }
    }
    public void ShowChooseSkin()
    {
        data.skinIndex = data.chooseIndex;
    }
}
