using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeTextCtr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private DataSO data;
    void Start()
    {
        coinText.text = data.coin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
