using System.Collections.Generic;
using UnityEngine;

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
    public List<List<Sprite>> skins = new List<List<Sprite>>();
    public List<DataSprite> skins2 = new List<DataSprite>();
    public List<string> names = new List<string>
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
}
[System.Serializable]
public class DataSprite
{
    public List<Sprite> dataSprite = new List<Sprite>();
}
