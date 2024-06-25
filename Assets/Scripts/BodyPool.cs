using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPool : MonoBehaviour
{
    public static BodyPool instance;
    public List<GameObject> bodyObjects = new List<GameObject>();
    private int amountPool = 0;

    [SerializeField] private GameObject bodyPrefabs;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < amountPool; ++i)
        {
            GameObject obj = Instantiate(bodyPrefabs);
            obj.transform.SetParent(gameObject.transform);
            obj.SetActive(false);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < bodyObjects.Count; i++)
        {
            if (!bodyObjects[i].activeInHierarchy)
            {
                return bodyObjects[i];
            }
        }
        return null;
    }
    void Update()
    {
        
    }
}
