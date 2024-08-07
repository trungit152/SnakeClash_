using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] private GameObject speedUp;
    [SerializeField] private GameObject magnite;
    [SerializeField] private GameObject enemy;
    public Camera mainCamera;
    [SerializeField] private DataSO data;
    [SerializeField] private GameObject fullEnemy;

    public List<GameObject> enemies;
    public int kingLevel;
    //[SerializeField] private Camera cameraSite;
    public GameObject king;
    //private float cd = 10f;
    private float maxX;
    private float minX;
    private float minZ;
    private float maxZ;

    private Pointer poiter;
    private Pointer Pointer
    {
        get
        {
            if (poiter == null)
            {
                poiter = GameObject.Find("Arrow").GetComponent<Pointer>();
            }
            return poiter;
        }
        set
        {
            poiter = value;
        }
    }

    private RankingController rankingController;
    private RankingController RankingController
    {
        get
        {
            if (rankingController == null)
            {
                rankingController = GameObject.Find("RankingController").gameObject.GetComponent<RankingController>();
            }
            return rankingController;
        }
        set
        {
            rankingController = value;
        }
    }

    private void Awake()
    {
        enemies = new List<GameObject>();
        int t = 55;
        maxX = t;
        minX = -t;
        maxZ = t;
        minZ = -t;
        kingLevel = 0;
        for (int i = 0; i < 20; i++)
        {
            int level = Random.Range(3, 15);
            SpawnEnemy(level);
            if (level > kingLevel)
            {
                kingLevel = level;
                king = enemies[i].transform.GetChild(0).gameObject;
                Pointer.SetKing(king);
            }
        }
    }
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        king.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void SetKing2(GameObject king2)
    {
        Pointer.SetKing(king);
        king.transform.GetChild(1).gameObject.SetActive(false);
        king = king2;
        king.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void KingDie(GameObject currentKing)
    {
        kingLevel = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemyi = enemies[i].transform.GetChild(0).gameObject;
            EnemyCollide enemy = enemyi.GetComponent<EnemyCollide>();
            if (enemyi != currentKing && enemy.level > kingLevel)
            {
                SetKing2(enemyi);
            }
        }
    }
    private void Update()
    {
        //cd -= Time.deltaTime;
        //if (cd < 0)
        //{
        //    cd = 10f;
        //    Spawn();
        //}
    }

    private Vector3 RandomSpawnPos()
    {
        Vector3 pos = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(pos);
        while (viewportPoint.x > -0.2f && viewportPoint.x < 1 + 0.2f && viewportPoint.y > -0.2f && viewportPoint.y < 1 + 0.2f && viewportPoint.z > 0)
        {
            pos = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));
            viewportPoint = mainCamera.WorldToViewportPoint(pos);        
        }
        return pos;
    }
    //private void Spawn()
    //{
    //    int i = Random.Range(1, 2);
    //    if (i == 1)
    //    {
    //        GameObject.Instantiate(speedUp, RandomSpawnPos(), Quaternion.identity);
    //    }
    //    else
    //    {
    //        GameObject.Instantiate(magnite, RandomSpawnPos(), Quaternion.identity);
    //    }
    //}
    public void SpawnEnemy(int level)
    {
        //GameObject enemySpawn = Instantiate(enemy, RandomSpawnPos(), Quaternion.identity);
        //enemies.Add(enemySpawn);
        //RankingController.enemiesRank.Add(enemySpawn);  
        //EnemyCollide enemyCollide = enemy.transform.GetChild(0).GetComponent<EnemyCollide>();
        //enemyCollide.enemyName = data.names[Random.Range(0, data.names.Count - 1)];
        //enemyCollide.level = level;
        GameObject enemySpawn = Instantiate(enemy, Vector3.zero, Quaternion.identity);
        enemySpawn.transform.GetChild(0).transform.position = RandomSpawnPos();
        enemies.Add(enemySpawn);
        enemySpawn.transform.SetParent(fullEnemy.transform);
        RankingController.enemiesRank.Add(enemySpawn);
        EnemyCollide enemyCollide = enemySpawn.transform.GetChild(0).GetComponent<EnemyCollide>();
        enemyCollide.enemyName = data.names[Random.Range(0, data.names.Count - 1)];
        enemyCollide.level = level;
    }
}