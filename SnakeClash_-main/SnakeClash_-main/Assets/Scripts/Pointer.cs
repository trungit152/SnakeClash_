using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeMonkey.Utils;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject playerHead;
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

    public GameObject mid;

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
            if (mid != null && mainCamera != null)
            {
                GameObject img = gameObject.transform.GetChild(0).gameObject;
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(targetPos.transform.position);
                if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && viewportPoint.z > 0)
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
    public void SetKing(GameObject king)
    { 
        targetPos = king;
    }
    public void GetKingPosition()
    {
        
    }
}
