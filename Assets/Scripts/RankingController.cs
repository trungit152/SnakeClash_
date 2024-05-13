using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

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
    [SerializeField] private GameObject playerHead;
    [SerializeField] private DataSO data;

    public List<GameObject> enemiesRank;

    private float updateInterval = 1f;
    private float lastUpdateTime = 0f;

    void Start()
    {
        data.names = new List<string>
        {
            "John", "Mary", "Luis", "Sophia", "Ahmed", "Anna", "Santiago", "Elena", "Hiroshi", "Yuki",
            "Juan", "Emily", "Carlos", "Olivia", "Mohammed", "Mia", "Alejandro", "Isabella", "Youssef", "Emma",
            "Liam", "Ava", "Mateo", "Charlotte", "Miguel", "Amelia", "Ethan", "Sophie", "Enzo", "Ella",
            "Sebastian", "Chloe", "Diego", "Lily", "Lucas", "Grace", "Omar", "Zoe", "Julian", "Luna",
            "Adrian", "Harper", "David", "Sofia", "Daniel", "Madison", "Christian", "Layla", "Nicolas", "Hannah",
            "José", "Avery", "Gabriel", "Scarlett", "Aaron", "Aria", "Samuel", "Nora", "Victor", "Riley",
            "Alexander", "Evelyn", "Mason", "Abigail", "Elijah", "Leah", "Benjamin", "Aaliyah", "Angel", "Audrey",
            "Adam", "Maya", "Lucian", "Penelope", "Luke", "Victoria", "Juan Pablo", "Natalie", "Josef", "Alice",
            "Felix", "Katherine", "Matias", "Elizabeth", "Isaac", "Aria", "Roman", "Samantha", "Matteo", "Sarah",
            "Maximilian", "Aurora", "Felipe", "Camila", "Leo", "Eva", "Gustavo", "Lillian", "Joaquin", "Anna",
            "Oscar", "Hailey", "Rafael", "Julia", "Cristian", "Clara", "Tomas", "Bella", "Raul", "Nina",
            "Andres", "Lila", "Fernando", "Hazel", "Javier", "Ellie", "Martin", "Stella", "Marco", "Savannah",
            "Alonso", "Ruby", "Pedro", "Zara", "Emilio", "Alexa", "Eduardo", "Lydia", "Hector", "Josephine",
            "Julio", "Gabriella", "Ricardo", "Rebecca", "Manuel", "Ariana", "Esteban", "Valentina", "Jorge", "Diana",
            "Guillermo", "Juliana", "Ignacio", "Gianna", "Ramón", "Alexandra", "Sergio", "Daniela", "Armando", "Caroline",
            "Alejo", "Delilah", "André", "Mariana", "Bruno", "Luciana", "César", "Amaya", "Fabian", "Angela",
            "Ivan", "Athena", "Hugo", "Gabrielle", "Juan José", "Isabelle", "Julián", "Valerie", "Mario", "Lia",
            "Nicolás", "Tiffany", "Pablo", "Stephanie", "Patricio", "Kayla", "Ramon", "Angelina", "Santiago José", "Daisy",
            "Vicente", "Paige", "Victor Hugo", "Vanessa", "Agustín", "Adeline", "Álvaro", "Jade", "Dante", "Nadia",
            "Diego Alejandro", "Lorelei", "Emiliano", "Joy", "Gonzalo", "Ruth", "Jesús", "Jasmine", "Joaquín", "Selena",
            "Lorenzo", "Skylar", "Marcelo", "Veronica", "Mariano", "Faith", "Maximiliano", "Hope", "Roberto", "Harmony",
            "Salvador", "Trinity", "Tomás José", "Summer", "Faker", "Gumayusi", "Messi", "Ricon", "Richa", "Yasuo", "Zeros",
            "Pepe"
        };
    enemiesRank.Add(playerHead); 
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            Sort();
            UpdateText();
            lastUpdateTime = Time.time;
        }
    }
    private void UpdateText()
    {
        for (int i = 0; i < 3; i++)
        {
            HeadController player = playerHead.GetComponent<HeadController>();
            if (enemiesRank.IndexOf(playerHead) > 2)
            {
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
                    top2score.color= Color.blue;
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
                    top3score.text= player.level.ToString();
                    top3score.color= Color.blue;
                    EnemyCollide enemy1 = enemiesRank[0].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    EnemyCollide enemy2 = enemiesRank[1].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    top1name.text = enemy1.enemyName;
                    top1score.text = enemy1.level.ToString();
                    top1name.color = Color.white;
                    top1score.color = Color.white;
                    top2name.text = enemy1.enemyName;
                    top2score.text = enemy1.level.ToString();
                    top2name.color = Color.white;
                    top2score.color = Color.white;
                }
            }
        }
    }
    private void Sort()
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
}
