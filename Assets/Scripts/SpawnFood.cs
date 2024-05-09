using System.Collections.Generic;
using UnityEngine;
public class SpawnFood : MonoBehaviour
{
    [SerializeField] private int size = 175;
    [SerializeField] private GameObject foodPrefabs;
    [SerializeField] private GameObject fullFood;

    public static SpawnFood instance;
    public List<GameObject> foods;
    private System.Random rand;
    private float spawnCd = 0.05f;

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
        if(spawnCd > 0)
        {
            spawnCd -= Time.deltaTime;
        }
        else
        {
            float radius = rand.Next(20, 30) / 10;
            foodPrefabs.transform.localScale = new Vector3(radius, radius, radius);
            Vector3 randPos = new Vector3(rand.Next(-size / 2, size / 2) + (float)rand.NextDouble(), foodPrefabs.transform.localScale.x, rand.Next(-size / 2, size / 2) + (float)rand.NextDouble());
            GameObject food = Instantiate(foodPrefabs, randPos, Quaternion.identity);
            AddFood(food);           
            //EnemyMove2.AddNearFood(food);
            food.transform.SetParent(fullFood.transform);
            spawnCd = 0.05f;
        }
    }
    private void Update()
    {
        if(foods.Count < 200)
        {
            Spawn();
        }
    }
    public void AddFood(GameObject food)
    {
        foods.Add(food);
    }

    public void RemoveFood(GameObject food)
    {
        foods.Remove(food);
    }
}
