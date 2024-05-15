using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseSkinText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private DataSO data;
    void Start()
    {
        coinText.text = data.coin.ToString();
        costText.text = data.skins[data.skinIndex].cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
