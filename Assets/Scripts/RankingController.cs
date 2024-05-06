using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class RankingController : MonoBehaviour
{
    public TextMeshProUGUI top1Text;
    public TextMeshProUGUI top2Text;
    public TextMeshProUGUI top3Text;
    public TextMeshProUGUI playerRankText;
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
            "Jos�", "Avery", "Gabriel", "Scarlett", "Aaron", "Aria", "Samuel", "Nora", "Victor", "Riley",
            "Alexander", "Evelyn", "Mason", "Abigail", "Elijah", "Leah", "Benjamin", "Aaliyah", "Angel", "Audrey",
            "Adam", "Maya", "Lucian", "Penelope", "Luke", "Victoria", "Juan Pablo", "Natalie", "Josef", "Alice",
            "Felix", "Katherine", "Matias", "Elizabeth", "Isaac", "Aria", "Roman", "Samantha", "Matteo", "Sarah",
            "Maximilian", "Aurora", "Felipe", "Camila", "Leo", "Eva", "Gustavo", "Lillian", "Joaquin", "Anna",
            "Oscar", "Hailey", "Rafael", "Julia", "Cristian", "Clara", "Tomas", "Bella", "Raul", "Nina",
            "Andres", "Lila", "Fernando", "Hazel", "Javier", "Ellie", "Martin", "Stella", "Marco", "Savannah",
            "Alonso", "Ruby", "Pedro", "Zara", "Emilio", "Alexa", "Eduardo", "Lydia", "Hector", "Josephine",
            "Julio", "Gabriella", "Ricardo", "Rebecca", "Manuel", "Ariana", "Esteban", "Valentina", "Jorge", "Diana",
            "Guillermo", "Juliana", "Ignacio", "Gianna", "Ram�n", "Alexandra", "Sergio", "Daniela", "Armando", "Caroline",
            "Alejo", "Delilah", "Andr�", "Mariana", "Bruno", "Luciana", "C�sar", "Amaya", "Fabian", "Angela",
            "Ivan", "Athena", "Hugo", "Gabrielle", "Juan Jos�", "Isabelle", "Juli�n", "Valerie", "Mario", "Lia",
            "Nicol�s", "Tiffany", "Pablo", "Stephanie", "Patricio", "Kayla", "Ramon", "Angelina", "Santiago Jos�", "Daisy",
            "Vicente", "Paige", "Victor Hugo", "Vanessa", "Agust�n", "Adeline", "�lvaro", "Jade", "Dante", "Nadia",
            "Diego Alejandro", "Lorelei", "Emiliano", "Joy", "Gonzalo", "Ruth", "Jes�s", "Jasmine", "Joaqu�n", "Selena",
            "Lorenzo", "Skylar", "Marcelo", "Veronica", "Mariano", "Faith", "Maximiliano", "Hope", "Roberto", "Harmony",
            "Salvador", "Trinity", "Tom�s Jos�", "Summer", "Faker", "Gumayusi", "Messi", "Ricon", "Richa", "Yasuo", "Zeros",
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
                    top1Text.text = "Top 1 - " + enemyCollide.enemyName + ": " + enemyCollide.level.ToString();
                    top1Text.color = Color.black;
                }
                else if (i == 1)
                {
                    top2Text.text = "Top 2 - " + enemyCollide.enemyName + ": " + enemyCollide.level.ToString();
                    top2Text.color = Color.black;
                }
                else
                {
                    top3Text.text = "Top 3 - " + enemyCollide.enemyName + ": " + enemyCollide.level.ToString();
                    top3Text.color = Color.black;
                }
                playerRankText.text = "Top " + (enemiesRank.IndexOf(playerHead) + 1) + " - Player: " + player.level;
                playerRankText.color = Color.blue;
            }
            else
            {
                playerRankText.text = null;
                if (enemiesRank.IndexOf(playerHead) == 0)
                {
                    top1Text.text = "Top 1 - Player: " + player.level;
                    top1Text.color = Color.blue;
                    EnemyCollide enemy1 = enemiesRank[1].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    EnemyCollide enemy2 = enemiesRank[2].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    top2Text.text = "Top 2 - " + enemy1.enemyName + ": " + enemy1.level.ToString();
                    top2Text.color = Color.black;
                    top3Text.text = "Top 3 - " + enemy2.enemyName + ": " + enemy2.level.ToString();
                    top3Text.color = Color.black;
                }
                else if (enemiesRank.IndexOf(playerHead) == 1)
                {
                    top2Text.text = "Top 2 - Player: " + player.level;
                    top2Text.color = Color.blue;
                    EnemyCollide enemy1 = enemiesRank[0].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    EnemyCollide enemy2 = enemiesRank[2].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    top1Text.text = "Top 1 - " + enemy1.enemyName + ": " + enemy1.level.ToString();
                    top1Text.color = Color.black;
                    top3Text.text = "Top 3 - " + enemy2.enemyName + ": " + enemy2.level.ToString();
                    top3Text.color = Color.black;
                }
                else if (enemiesRank.IndexOf(playerHead) == 2)
                {
                    top3Text.text = "Top 3 - Player: " + player.level;
                    top3Text.color = Color.blue;
                    EnemyCollide enemy1 = enemiesRank[0].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    EnemyCollide enemy2 = enemiesRank[1].transform.GetChild(0).gameObject.GetComponent<EnemyCollide>();
                    top1Text.text = "Top 1 - " + enemy1.enemyName + ": " + enemy1.level.ToString();
                    top1Text.color = Color.black;
                    top2Text.text = "Top 2 - " + enemy2.enemyName + ": " + enemy2.level.ToString();
                    top2Text.color = Color.black;
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
        top1Text.text = null;
        top2Text.text = null;
        top3Text.text = null;
        playerRankText.text = null;
    }
    public void SetText(string text1, string text2, string text3, string text4)
    {
        text1 = top1Text.text;
        text2 = top2Text.text;
        text3 = top3Text.text;
        text4 = playerRankText.text;
        Debug.Log("da gan");
    }
}