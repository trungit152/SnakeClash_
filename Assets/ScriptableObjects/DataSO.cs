using UnityEngine;

[CreateAssetMenu(fileName ="DataSO", menuName = "ScriptableObjects/DataSO")] 

public class DataSO : ScriptableObject
{
    public int startLevel = 5;
    public float startSpeed = 10f;
    public float itemTime = 2.5f;
    public int coin = 9999999;
    public int increseSpeedCost = 500;
    public int increseLevelCost = 500;
    public int increseItemCost = 500;
    public int currentLevel = 1;
}
