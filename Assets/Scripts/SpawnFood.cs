using System.Collections.Generic;
using UnityEngine;
public class SpawnFood : MonoBehaviour
{
    [SerializeField] private Sprite foodImg;
    [SerializeField] private int size = 115;
    [SerializeField] private GameObject foodPrefabs;
    [SerializeField] private GameObject fullFood;

    public static SpawnFood instance;
    public List<GameObject> foods;
    private System.Random rand;
    private float spawnCd = 0.08f;

    //private EnemyMove2 enemyMove2;
    //private EnemyMove2 EnemyMove2
    //{
    //    get
    //    {
    //        if (enemyMove2 == null)
    //        {
    //            enemyMove2 = GameObject.Find("Enemy").transform.GetChild(0).GetComponent<EnemyMove2>();
    //        }
    //        return enemyMove2;
    //    }
    //    set
    //    {
    //        enemyMove2 = value;
    //    }
    //}
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foods = new List<GameObject>();
        for (int i = 0; i < fullFood.transform.childCount; i++)
        {
            foods.Add(fullFood.transform.GetChild(i).gameObject);
        }
        rand = new System.Random();
    }
    private void Spawn()
    {
        if(spawnCd > 0 )
        {
            spawnCd -= Time.deltaTime;
        }
        else
        {         
            GameObject food = FoodPool.instance.GetPooledObject();
            if(food != null)
            {
                float radius = rand.Next(20, 30) / 10;
                Vector3 randPos = new Vector3(rand.Next(-size / 2, size / 2) + (float)rand.NextDouble(), 
                    foodPrefabs.transform.localScale.x, rand.Next(-size / 2, size / 2) + (float)rand.NextDouble());
                food.transform.position = randPos;
                food.transform.localScale = new Vector3(radius,radius,radius);
                food.GetComponent<FoodController>().SetSprite(foodImg);
                food.SetActive(true);
                spawnCd = 0.08f;
            }
        }
    }

    private void Update()
    {
         Spawn();
    }
}
