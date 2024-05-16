using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Camera miniCamera;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject minimap;

    private  float cameraHeight = 30f;
    void Update()
    {
        if (player != null)
        {
            Vector3 newPosition = new Vector3(player.position.x, cameraHeight, player.position.z);
            transform.position = newPosition;
            transform.LookAt(player.position);
        }
    }

    public void ShowMinimap()
    {
        minimap.SetActive(true);
    }
    public void HideMinimap()
    {
        minimap.SetActive(false);
    }
    public void MiniCameraUp()
    {
        miniCamera.orthographicSize += 3f;
    }
}
