using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FoodPool : MonoBehaviour
{
    public static FoodPool instance;
    private List<GameObject> foodPools = new List<GameObject>();
    private int amountPool = 250;

    [SerializeField] private GameObject foodPrefabs;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < amountPool; i++)
        {
            GameObject food = Instantiate(foodPrefabs);
            food.SetActive(false);
            food.transform.SetParent(transform);
            foodPools.Add(food);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < foodPools.Count; i++)
        {
            if (!foodPools[i].activeInHierarchy)
            {
                return foodPools[i];
            }
        }
        return null;
    }
    void Update()
    {
        
    }
}
