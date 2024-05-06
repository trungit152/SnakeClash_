using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeTest : MonoBehaviour
{
    [SerializeField] GameObject foodPrefabs;
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject food = Instantiate(foodPrefabs, RandomPos(), Quaternion.identity);
            food.transform.localScale = RandomScale();
            Debug.Log("ok1");
        }
        Destroy(gameObject);
    }

    private Vector3 RandomPos()
    {
        System.Random rand = new System.Random();
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-50, 50) / 100f, transform.position.y, transform.position.z + Random.Range(-50, 50) / 100f);
        return pos;
    }
    private Vector3 RandomScale()
    {
        System.Random rand = new System.Random();
        float rd = Random.Range(-400, 100) / 1000f;
        Vector3 scale = new Vector3(transform.localScale.x + rd, transform.localScale.y + rd, transform.localScale.z + rd);
        return scale;
    }
}

