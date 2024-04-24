using UnityEngine;
using System.Collections;
using Cinemachine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private DataSO data;
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 newFollowOffset;

    private float changeDuration = 2f;

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
    void Start()
    {
        newFollowOffset = new Vector3(0, 30, 0);
    }
    void Update()
    {

    }
    public void CameraUp()
    {
        newFollowOffset += new Vector3(0, 18, 0);
        if (virtualCamera != null)
        {
            StartCoroutine(CameraUpSmooth(newFollowOffset));
        }
    }
    IEnumerator CameraUpSmooth(Vector3 targetFollowOffset)
    {
        float elapsedTime = 0f;
        Vector3 initialFollowOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        while (elapsedTime < changeDuration)
        {
            Vector3 newFollowOffset = Vector3.Lerp(initialFollowOffset, targetFollowOffset, elapsedTime / changeDuration);
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newFollowOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = targetFollowOffset;
    }

}