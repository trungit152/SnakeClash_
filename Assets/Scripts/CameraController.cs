using UnityEngine;
using System.Collections;
using Cinemachine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private DataSO data;
    public CinemachineVirtualCamera virtualCamera;
    public Camera mainCamera;
    public float camSize;

    private HeadController headController;

    private HeadController HeadController
    {
        get
        {
            if (headController == null)
            {
                headController = GameObject.Find("PlayerHead").GetComponent<HeadController>();
            }
            return headController;
        }
        set
        {
            headController = value;
        }
    }

    MinimapController minimapController;
    MinimapController MinimapController
    {
        get
        {
            if (minimapController == null)
            {
                minimapController = GameObject.Find("MinimapCamera").GetComponent<MinimapController>();
            }
            return minimapController;
        }
        set
        {
            minimapController = value;
        }
    }
    void Start()
    {
        camSize = mainCamera.orthographicSize; 
    }
    public void CameraUp(float delta)
    {
        StartCoroutine(ChangeValueOverTime(camSize, camSize+ delta, 1f));        
    }
    public IEnumerator ChangeValueOverTime(float a, float b, float t)
    {
        float elapsedTime = 0f;
        while (elapsedTime < t)
        {
            camSize = Mathf.Lerp(a, b, elapsedTime / t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camSize = b;
        mainCamera.orthographicSize = camSize;

    }
}