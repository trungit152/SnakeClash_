using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SnakeSkinCtr : MonoBehaviour
{
    public List<GameObject> bodies;
    public GameObject head;
    public GameObject tail;
    public DataSO data;
    private void Awake()
    {
        if(data.skins.Count == 0) 
        {
            for (int i = 0; i < 5; i++)
            {

                string path = "Assets/Gameplay/skins/skin " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    List<Sprite> skin = new List<Sprite>();
                    data.skins.Add(skin);
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].Add(sprite);
                }
            }
            //add than skin
            for (int i = 0; i < 5; i++)
            {
                string path = "Assets/Gameplay/skins/than " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].Add(sprite);
                }
                else for (int j = 0; j < 10; j++)
                    {
                        string path2 = "Assets/Gameplay/skins/than " + (i + 1).ToString() + "." + (j + 1).ToString() + ".png";
                        if (File.Exists(path2))
                        {
                            Sprite sprite2 = AssetDatabase.LoadAssetAtPath<Sprite>(path2);
                            data.skins[i].Add(sprite2);
                        }
                        else break;
                    }
            }
            //add duoi skin
            for (int i = 0; i < 5; i++)
            {
                string path = "Assets/Gameplay/skins/duoi " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].Add(sprite);
                }
            }
        }

    }
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject body = GameObject.Find("body" + i.ToString());
            bodies.Add(body);
        }
        for (int i = 0; i < 6;)
        {
            int bodyPath = data.skins[data.skinIndex].Count - 2;
            for (int j = 1; j <= bodyPath; j++)
            {
                if (i < 6)
                {
                    Image skin = bodies[i].GetComponent<Image>();
                    skin.sprite = data.skins[data.skinIndex][j];
                    i++;
                }
            }
        }
        Image headSkin = head.GetComponent<Image>();
        headSkin.sprite = data.skins[data.skinIndex][0];

        Image tailSkin = tail.GetComponent<Image>();
        tailSkin.sprite = data.skins[data.skinIndex][data.skins[data.skinIndex].Count - 1];
    }

    public void LoadNextSkin()
    {
        if (data.skinIndex < 4)
        {
            data.skinIndex += 1;
        }
        for (int i = 0; i < 6;)
        {
            int bodyPath = data.skins[data.skinIndex].Count - 2;

            for (int j = 1; j <= bodyPath; j++)
            {
                if(i < 6)
                {
                    Image skin = bodies[i].GetComponent<Image>();
                    skin.sprite = data.skins[data.skinIndex][j];
                    i++;
                }
            }
        }
        Image headSkin = head.GetComponent<Image>();
        headSkin.sprite = data.skins[data.skinIndex][0];

        Image tailSkin = tail.GetComponent<Image>();
        tailSkin.sprite = data.skins[data.skinIndex][data.skins[data.skinIndex].Count - 1];
    }

    public void LoadPreviousSkin()
    {
        if (data.skinIndex > 0)
        {
            data.skinIndex -= 1;
        }
        for (int i = 0; i < 6;)
        {
            int bodyPath = data.skins[data.skinIndex].Count - 2;

            for (int j = 1; j <= bodyPath; j++)
            {
                if (i < 6)
                {
                    Image skin = bodies[i].GetComponent<Image>();
                    skin.sprite = data.skins[data.skinIndex][j];
                    i++;
                }
            }
        }
        Image headSkin = head.GetComponent<Image>();
        headSkin.sprite = data.skins[data.skinIndex][0];

        Image tailSkin = tail.GetComponent<Image>();
        tailSkin.sprite = data.skins[data.skinIndex][data.skins[data.skinIndex].Count - 1];
    }

}
