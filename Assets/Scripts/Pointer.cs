using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeMonkey.Utils;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject img;
    private GameObject targetPos;
    private RectTransform pointerRectTransform;
    private float angle;
    public Camera mainCamera;
    private ItemSpawn itemSpawn;
    private ItemSpawn ItemSpawn
    {
        get
        {
            if (itemSpawn == null)
            {
                itemSpawn = GameObject.Find("FoodController").GetComponent<ItemSpawn>();
            }
            return itemSpawn;
        }
        set
        {
            itemSpawn = value;
        }
    }

    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }
    private void Start()
    {
        if (ItemSpawn.enemies.Count > 0) 
        {
            for (int i = 0; i < ItemSpawn.enemies.Count; i++)
            {
            }
        }
    }

    private void Update()
    {
        if(playerHead != null && targetPos != null)
        {
            Vector3 toPos = targetPos.transform.position;
            Vector3 fromPos = playerHead.transform.position;
            Vector3 res = new Vector3(toPos.x - fromPos.x, 0, toPos.z - fromPos.z);
            angle = 360 - Mathf.Atan2(res.x, res.z) * Mathf.Rad2Deg;
            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
            if (mainCamera != null)
            {
                if(CheckViewPos(targetPos.transform.position, -0.1f))
                {
                    img.SetActive(false);
                }
                else
                {
                    img.SetActive(true);   
                }
            }
        }
    }
    private bool CheckViewPos(Vector3 pos, float buffer = 0.1f)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(pos);
        if (viewportPoint.x > -buffer && viewportPoint.x < 1 + buffer && viewportPoint.y > -buffer && viewportPoint.y < 1 + buffer && viewportPoint.z > 0)
        {
            return true;
        }
        return false;
    }
    public void SetKing(GameObject king)
    { 
        targetPos = king;
    }
    public void GetKingPosition()
    {
        
    }
}
