using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/DataSO")]

public class DataSO : ScriptableObject
{
    public int startLevel = 5;
    public float startSpeed = 8f;
    public float itemTime = 2.5f;
    public int coin = 9999999;
    public int increseSpeedCost = 500;
    public int increseLevelCost = 500;
    public int increseItemCost = 500;
    public int currentLevel = 1;
    public int skinIndex = 0;
    public int chooseIndex;
    public List<DataSprite> skins = new List<DataSprite>();
    public List<string> names = new List<string>
        {
            "John", "Mary", "Luis", "Sophia", "Ahmed", "Anna", "Santiago", "Elena", "Hiroshi", "Yuki",
            "Juan", "Emily", "Carlos", "Olivia", "Mohammed", "Mia", "Alejandro", "Isabella", "Youssef", "Emma",
            "Liam", "Ava", "Mateo", "Charlotte", "Miguel", "Amelia", "Ethan", "Sophie", "Enzo", "Ella",
            "Sebasti", "Chloe", "Diego", "Lily", "Lucas", "Grace", "Omar", "Zoe", "Julian", "Luna",
            "Adrian", "Harper", "David", "Sofia", "Daniel", "Madison", "Christian", "Layla", "Nicolas", "Hannah",
            "Jose", "Avery", "Gabriel", "Scarlett", "Aaron", "Aria", "Samuel", "Nora", "Victor", "Riley",
            "Alexa", "Evelyn", "Mason", "Abigail", "Elijah", "Leah", "Benjamin", "Aaliyah", "Angel", "Audrey",
            "Adam", "Maya", "Lucian", "Penelope", "Luke", "Victoria", "Pablo", "Natalie", "Josef", "Alice",
            "Felix", "Katherine", "Matias", "Elizabeth", "Isaac", "Aria", "Roman", "Saman", "Matteo", "Sarah",
            "Maximilian", "Aurora", "Felipe", "Camila", "Leo", "Eva", "Gustavo", "Lillian", "Joaquin", "Anna",
            "Oscar", "Hailey", "Rafael", "Julia", "Cristian", "Clara", "Tomas", "Bella", "Raul", "Nina",
            "Andres", "Lila", "Fernando", "Hazel", "Javier", "Ellie", "Martin", "Stella", "Marco", "Savannah",
            "Alonso", "Ruby", "Pedro", "Zara", "Emilio", "Alexa", "Eduardo", "Lydia", "Hector", "Josephine",
            "Julio", "Gabriella", "Ricardo", "Rebecca", "Manuel", "Ariana", "Esteban", "Valentin", "Jorge", "Diana",
            "Trung", "Juliana", "Ignacio", "Gianna", "Mai", "Alexandra", "Sergio", "Daniela", "Armando", "Caroline",
            "Alejo", "Delilah", "André", "Mariana", "Bruno", "Luciana", "Cesar", "Amaya", "Fabian", "Angela",
            "Ivan", "Athena", "Hugo", "Gabrielle", "Juan José", "Isabelle", "Julián", "Valerie", "Mario", "Lia",
            "Nicolás", "Tiffany", "Pablo", "Stephanie", "Patricio", "Kayla", "Ramon", "Angelina", "Santiagoé", "Daisy",
            "Vicente", "Paige", "Victor Hugo", "Vanessa", "Agustín", "Adeline", "alvaro", "Jade", "Dante", "Nadia",
            "Diego", "Lorelei", "Emiliano", "Joy", "Gonzalo", "Ruth", "Jesús", "Jasmine", "Joaquin", "Selena",
            "Lorenzo", "Skylar", "Marcelo", "Veronica", "Mariano", "Faith", "Max", "Hope", "Roberto", "Harmony",
            "Salvador", "Trinity", "Thomas", "Summer", "Faker", "Gumayusi", "Messi", "Ricon", "Richa", "Yasuo", "Zeros",
            "Pepe"
        };
}
[System.Serializable]
public class DataSprite
{
    public List<Sprite> dataSprite = new List<Sprite>();
    public int cost = 5000;
    public bool isBought = false;
    public bool isSelected = false;
}
