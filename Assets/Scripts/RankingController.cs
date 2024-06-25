using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using System.Drawing;

public class RankingController : MonoBehaviour
{
    public TextMeshProUGUI top1name;
    public TextMeshProUGUI top1score;
    public TextMeshProUGUI top2name;
    public TextMeshProUGUI top2score;
    public TextMeshProUGUI top3name;
    public TextMeshProUGUI top3score;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI playerRank;
    public GameObject top4Frame;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private DataSO data;
    public List<GameObject> enemiesRank;

    private int randEnemy;
    private int randEnemy2;
    private float chooseEnemyToCheatTime = 3f;
    private float updateInterval = 1f;
    private float lastUpdateTime = 0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        data.names = new List<string>
        {
            "John", "Mary", "Luis", "Sophia", "Ahmed", "Anna", "Santi", "Elena", "Hiroshi", "Yuki",
            "Juan", "Emily", "Carlos", "Olivia", "Mohamed", "Mia", "Alejo", "Isabel", "Youssef", "Emma",
            "Liam", "Ava", "Mateo", "Charli", "Miguel", "Amelia", "Ethan", "Sophie", "Enzo", "Ella",
            "Sebas", "Chloe", "Diego", "Lily", "Lucas", "Grace", "Omar", "Zoe", "Julian", "Luna",
            "Adrian", "Harper", "David", "Sofia", "Daniel", "Madison", "Chris", "Layla", "Nicolas", "Hannah",
            "Jose", "Avery", "Gabriel", "Scarlet", "Aaron", "Aria", "Samuel", "Nora", "Victor", "Riley",
            "Alexa", "Evelyn", "Mason", "Abigail", "Elijah", "Leah", "Ben", "Aaliyah", "Angel", "Audrey",
            "Adam", "Maya", "Lucian", "Penelo", "Luke", "Vicky", "Pablo", "Natalie", "Josef", "Alice",
            "Felix", "Kat", "Matias", "Liz", "Isaac", "Roman", "Matteo", "Sarah",
            "Max", "Aurora", "Felipe", "Camila", "Leo", "Eva", "Gus", "Lilly", "Joaquin", "Anna",
            "Oscar", "Hailey", "Rafael", "Julia", "Cris", "Clara", "Tomas", "Bella", "Raul", "Nina",
            "Andres", "Lila", "Fernando", "Hazel", "Javier", "Ellie", "Martin", "Stella", "Marco", "Savannah",
            "Alonso", "Ruby", "Pedro", "Zara", "Emilio", "Alexa", "Eduardo", "Lydia", "Hector", "Josie",
            "Julio", "Gabby", "Ricky", "Becky", "Manuel", "Ariana", "Esteban", "Mario", "Jorge", "Diana",
            "Trung", "Julie", "Ignacio", "Gianna", "Mai", "Sandra", "Sergio", "Daniela", "Arman", "Carol",
            "Alejo", "Lilah", "Andre", "Maria", "Bruno", "Luciana", "Cesar", "Amaya", "Fabian", "Angela",
            "Ivan", "Athena", "Hugo", "Gabi", "Mario", "Lia", "Trungdz",
            "Nico", "Tiff", "Pablo", "Steph", "Max", "Kayla", "Ramon", "Angie", "Daisy",
            "Vicente", "Paige", "Victor", "Vanessa", "Agustin", "Addy", "Jade", "Nadia",
            "Dante", "Lorelei", "Emilio", "Joy", "Gonzalo", "Ruth", "Jesus", "Jas", "Joaquin", "Selena",
            "Lorenzo", "Sky", "Marcelo", "Veron", "Mariano", "Faith", "Max", "Hope", "Rob", "Harmony",
            "Salvador", "Trinity", "Thomas", "Summer", "Faker", "Gumayusi", "Messi", "Ricon", "Richa", "Yasuo", "Zeros",
            "Pepe"
        };
        enemiesRank.Add(playerHead);
    }

    // Update is called once per frame
    private void Update()
    {
        if (chooseEnemyToCheatTime > 0)
        {
            chooseEnemyToCheatTime -= Time.deltaTime;
        }
        if (Time.time - lastUpdateTime > updateInterval)
        {
            Sort();
            UpdateText();
            lastUpdateTime = Time.time;
        }
    }
    public void UpdateText()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerHead != null)
            {
                HeadController player = playerHead.GetComponent<HeadController>();
                if (enemiesRank.IndexOf(playerHead) > 2)
                {
                    top4Frame.SetActive(true);
                    EnemyCollide enemyCollide = enemiesRank[i].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    if (i == 0)
                    {
                        top1name.text = enemyCollide.enemyName;
                        top1score.text = enemyCollide.level.ToString();
                        top1name.color = Color.white;
                        top1score.color = Color.white;
                    }
                    else if (i == 1)
                    {
                        top2name.text = enemyCollide.enemyName;
                        top2score.text = enemyCollide.level.ToString();
                        top2name.color = Color.white;
                        top2score.color = Color.white;
                    }
                    else
                    {
                        top3name.text = enemyCollide.enemyName;
                        top3score.text = enemyCollide.level.ToString();
                        top3name.color = Color.white;
                        top3score.color = Color.white;
                    }
                    playerName.text = "Player";
                    playerScore.text = player.level.ToString();
                    playerRank.text = (enemiesRank.IndexOf(playerHead) + 1).ToString();
                    playerName.color = Color.blue;
                    playerScore.color = Color.blue;
                    playerRank.color = Color.blue;

                }
                else
                {
                    top4Frame.SetActive(false);
                    //if (enemiesRank.IndexOf(playerHead) != 3)
                    //{
                    //    EnemyCollide top4 = enemiesRank[3].gameObject.GetComponent<EnemyCollide>();
                    //    playerRank.text = "4";
                    //    playerName.text = top4.name;
                    //    playerScore.text = top4.level.ToString();
                    //}
                    playerRank.text = null;
                    playerName.text = null;
                    playerScore.text = null;
                    if (enemiesRank.IndexOf(playerHead) == 0)
                    {
                        top1name.text = "Player";
                        top1name.color = Color.blue;
                        top1score.text = player.level.ToString();
                        top1score.color = Color.blue;
                        EnemyCollide enemy1 = enemiesRank[1].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        EnemyCollide enemy2 = enemiesRank[2].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        top2name.text = enemy1.enemyName;
                        top2score.text = enemy1.level.ToString();
                        top2name.color = Color.white;
                        top2score.color = Color.white;
                        top3name.text = enemy2.enemyName;
                        top3score.text = enemy2.level.ToString();
                        top3name.color = Color.white;
                        top3score.color = Color.white;
                    }
                    else if (enemiesRank.IndexOf(playerHead) == 1)
                    {
                        top2name.text = "Player";
                        top2name.color = Color.blue;
                        top2score.text = player.level.ToString();
                        top2score.color = Color.blue;
                        EnemyCollide enemy1 = enemiesRank[0].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        EnemyCollide enemy2 = enemiesRank[2].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        top1name.text = enemy1.enemyName;
                        top1score.text = enemy1.level.ToString();
                        top1name.color = Color.white;
                        top1score.color = Color.white;
                        top3name.text = enemy2.enemyName;
                        top3score.text = enemy2.level.ToString();
                        top3name.color = Color.white;
                        top3score.color = Color.white;
                    }
                    else if (enemiesRank.IndexOf(playerHead) == 2)
                    {
                        top3name.text = "Player";
                        top3name.color = Color.blue;
                        top3score.text = player.level.ToString();
                        top3score.color = Color.blue;
                        EnemyCollide enemy1 = enemiesRank[0].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        EnemyCollide enemy2 = enemiesRank[1].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                        top1name.text = enemy1.enemyName;
                        top1score.text = enemy1.level.ToString();
                        top1name.color = Color.white;
                        top1score.color = Color.white;
                        top2name.text = enemy2.enemyName;
                        top2score.text = enemy2.level.ToString();
                        top2name.color = Color.white;
                        top2score.color = Color.white;
                    }
                }
            }
        }
    }
    public void Sort()
    {
        for (int i = 0; i < 3; i++)
        {
            int max = 0;
            for (int j = i; j < enemiesRank.Count; j++)
            {
                if (enemiesRank[j] != null && enemiesRank[j] != playerHead)
                {
                    EnemyCollide enemyCollide = enemiesRank[j].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    if (enemyCollide.level > max)
                    {
                        max = enemyCollide.level;
                        GameObject temp = enemiesRank[j];
                        enemiesRank.RemoveAt(j);
                        enemiesRank.Insert(i, temp);
                    }
                }
            }
        }
        if (playerHead != null)
        {
            HeadController player = playerHead.GetComponent<HeadController>();
            for (int i = 0; i < enemiesRank.Count; i++)
            {
                if (enemiesRank[i] != playerHead)
                {
                    EnemyCollide enemyCollide = enemiesRank[i].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    if (player.level > enemyCollide.level)
                    {
                        enemiesRank.RemoveAt(enemiesRank.IndexOf(playerHead));
                        enemiesRank.Insert(i, playerHead);
                        break;
                    }
                }
            }
        }
    }
    public void AddToRankedList(GameObject enemy)
    {
        enemiesRank.Add(enemy);
    }
    public void RemoveFromRankedList(GameObject enemy)
    {
        enemiesRank.Remove(enemy);
    }
    public void TurnOffText()
    {
        top1name.text = null;
        top2name.text = null;
        top3name.text = null;
        top1score.text = null;
        top3score.text = null;
        top2score.text = null;
        playerRank.text = null;
        playerScore.text = null;
        playerName.text = null;
    }
    public void EnemyCheat(int headLevel)
    {
        if (chooseEnemyToCheatTime <= 0)
        {
            randEnemy = Random.Range(0, 10);
            randEnemy2 = Random.Range(0, 10);
            while (randEnemy == enemiesRank.IndexOf(playerHead) || randEnemy2 == enemiesRank.IndexOf(playerHead))
            {
                randEnemy = Random.Range(0, 10);
                while (randEnemy2 == randEnemy)
                {
                    randEnemy2 = Random.Range(0, 10);
                }
            }

            EnemyCollide enemy1 = enemiesRank[randEnemy].transform.GetChild(0).GetComponent<EnemyCollide>();
            Cheat(enemy1, headLevel);

            EnemyCollide enemy2 = enemiesRank[randEnemy2].transform.GetChild(0).GetComponent<EnemyCollide>();
            Cheat(enemy2, headLevel);

            chooseEnemyToCheatTime = 3f;
        }

    }
    public void Cheat(EnemyCollide enemy, int headLevel)
    {
        int delta = headLevel - enemy.level + Random.Range(-30, 50);
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);
        if (delta > 0 && delta < 800)
        {
            if (viewportPoint.x > -0.3f && viewportPoint.x < 1+ 0.3f && viewportPoint.y > -0.3f && viewportPoint.y < 1+ 0.3f && viewportPoint.z > 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < delta; i++)
                {
                    enemy.Eat();
                }
            }
        }
    }
}