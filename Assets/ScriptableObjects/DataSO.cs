using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "DataSO", menuName = "ScriptableObjects/DataSO")]

public class DataSO : ScriptableObject
{
    public int startLevel = 5;
    public float startSpeed = 5f;
    public float itemTime = 2.5f;
    public int coin = 999999;
    public int increseSpeedCost = 500;
    public int increseLevelCost = 500;
    public int increseItemCost = 500;
    public int currentLevel = 1;
    public int skinIndex = 0;
    public int chooseIndex;
    public List<Sprite> foodSprites = new List<Sprite>();
    public List<DataSprite> skins = new List<DataSprite>();
    public List<string> names = new List<string>
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
        "Ivan", "Athena", "Hugo", "Gabi", "Mario", "Lia",
        "Nico", "Tiff", "Pablo", "Steph", "Max", "Kayla", "Ramon", "Angie", "Daisy",
        "Vicente", "Paige", "Victor", "Vanessa", "Agustin", "Addy", "Jade", "Nadia",
        "Dante", "Lorelei", "Emilio", "Joy", "Gonzalo", "Ruth", "Jesus", "Jas", "Joaquin", "Selena",
        "Lorenzo", "Sky", "Marcelo", "Veron", "Mariano", "Faith", "Max", "Hope", "Rob", "Harmony",
        "Salvador", "Trinity", "Thomas", "Summer", "Faker", "Gumayusi", "Messi", "Ricon", "Richa", "Yasuo", "Zeros", "Trungdz",
        "Pepe"
    };
    public List<float> SnakeUIPosX = new List<float>();
    public List<float> SnakeUIRotateZ = new List<float>();

}
[System.Serializable]
public class DataSprite
{
    public List<Sprite> dataSprite = new List<Sprite>();
    public int cost = 5000;
    public bool isBought = false;
    public bool isSelected = false;
}
