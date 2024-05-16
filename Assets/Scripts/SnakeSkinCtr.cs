#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
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
        if (data.skins.Count == 0)
        {
#if UNITY_EDITOR
            for (int i = 0; i < 5; i++)
            {
                string path = "Assets/Gameplay/skins/skin " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    DataSprite skin = new DataSprite();
                    data.skins.Add(skin);
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].dataSprite.Add(sprite);
                }
            }

            // Add body skin
            for (int i = 0; i < 5; i++)
            {
                string path = "Assets/Gameplay/skins/than " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].dataSprite.Add(sprite);
                }
                else
                {
                    for (int j = 0; j < 10; j++)
                    {
                        string path2 = "Assets/Gameplay/skins/than " + (i + 1).ToString() + "." + (j + 1).ToString() + ".png";
                        if (File.Exists(path2))
                        {
                            Sprite sprite2 = AssetDatabase.LoadAssetAtPath<Sprite>(path2);
                            data.skins[i].dataSprite.Add(sprite2);
                        }
                        else break;
                    }
                }
            }

            // Add tail skin
            for (int i = 0; i < 5; i++)
            {
                string path = "Assets/Gameplay/skins/duoi " + (i + 1).ToString() + ".png";
                if (File.Exists(path))
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    data.skins[i].dataSprite.Add(sprite);
                }
            }
#endif
        }
    }

    void Start()
    {
        InitializeBodies();

        if (data.skins.Count > 0)
        {
            UpdateSkins();
        }
    }

    private void InitializeBodies()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject body = GameObject.Find("body" + i.ToString());
            if (body != null)
            {
                bodies.Add(body);
            }
        }
    }

    private void UpdateSkins()
    {
        if (data.skins.Count > 0 && data.skinIndex < data.skins.Count)
        {
            for (int i = 0; i < 6;)
            {
                int bodyPath = Mathf.Min(data.skins[data.skinIndex].dataSprite.Count - 2, 6);
                for (int j = 1; j <= bodyPath; j++)
                {
                    if (i < bodies.Count)
                    {
                        Image skin = bodies[i].GetComponent<Image>();
                        skin.sprite = data.skins[data.skinIndex].dataSprite[j];
                        i++;
                    }
                }
            }

            Image headSkin = head.GetComponent<Image>();
            headSkin.sprite = data.skins[data.skinIndex].dataSprite[0];

            Image tailSkin = tail.GetComponent<Image>();
            tailSkin.sprite = data.skins[data.skinIndex].dataSprite[data.skins[data.skinIndex].dataSprite.Count - 1];
        }
    }

    public void LoadNextSkin()
    {
        if (data.skinIndex < data.skins.Count - 1)
        {
            data.skinIndex++;
        }
        UpdateSkins();
    }

    public void LoadPreviousSkin()
    {
        if (data.skinIndex > 0)
        {
            data.skinIndex--;
        }
        UpdateSkins();
    }
}
