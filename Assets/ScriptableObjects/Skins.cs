using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/Skins")]

public class Skins : ScriptableObject
{
    public List<Sprite> skins;
}