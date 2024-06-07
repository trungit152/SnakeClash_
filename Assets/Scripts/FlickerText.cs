using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class FlickerText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tapText;
    void Update()
    {

    }
    public void Show()
    {
        tapText.text = null;
        Debug.Log("Ok");
    }
    public void Hide()
    {
        tapText.text = "Tap to play";
    }
}
